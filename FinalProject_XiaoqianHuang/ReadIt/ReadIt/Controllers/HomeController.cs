using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ReadIt.Data;
using ReadIt.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Net.Http;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace ReadIt.Controllers
{
  public class HomeController : Controller
  {

    private readonly ApplicationDbContext context_;
    private const string sessionId_ = "SessionId";
    private readonly string baseUrl_= "https://localhost:44372/api/Files";
    public HomeController(ApplicationDbContext context)
    {
      context_ = context;
    }

    public IActionResult Index()
    {
      if (User.IsInRole("Admin"))
      {
        return RedirectToAction("IndexAdmin");
      }
      var currentuser = User.Identity.Name;
      var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
      var currentrole = "User";
      var documents = context_.Documents.ToList();
      var categories = context_.Categories.ToList();
      var authors = context_.Authors.ToList();
      var documentcategories = context_.DocumentCategories.ToList();
      var documentauthors = context_.DocumentAuthors.ToList();
      var viewmodel = new IndexViewModel
      {
        Categories = categories,
        Documents = documents,
        Authors = authors,
        DocumentAuthors = documentauthors,
        DocumentCategories = documentcategories,
        CurrentUserId = userId,
        CurrentUserName = currentuser,
        CurrentUserRole = currentrole
      };
      return View(viewmodel);
    }

    public IActionResult IndexAdmin()
    {
      if (User.IsInRole("Admin"))
      {
        var currentuser = User.Identity.Name;
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var currentrole = "Admin";
        var documents = context_.Documents.ToList();
        var categories = context_.Categories.ToList();
        var authors = context_.Authors.ToList();
        var documentcategories = context_.DocumentCategories.ToList();
        var documentauthors = context_.DocumentAuthors.ToList();
        var viewmodel = new IndexViewModel
        {
          Categories = categories,
          Documents = documents,
          Authors = authors,
          DocumentAuthors = documentauthors,
          DocumentCategories = documentcategories,
          CurrentUserId = userId,
          CurrentUserName = currentuser,
          CurrentUserRole = currentrole
        };
        return View(viewmodel);
      }
      return RedirectToAction("Index");
    }

    [HttpGet]
    [Authorize(Roles = "Admin,User")]
    public IActionResult CreateDocument(int id)
    {
      var currentuser = User.Identity.Name;
      var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
      var documents = context_.Documents.ToList();
      var categories = context_.Categories.ToList();
      var authors = context_.Authors.ToList();
      var documentcategories = context_.DocumentCategories.ToList();
      var documentauthors = context_.DocumentAuthors.ToList();
      var viewmodel = new IndexViewModel
      {
        Categories = categories,
        Documents = documents,
        Authors = authors,
        DocumentAuthors = documentauthors,
        DocumentCategories = documentcategories,
        CurrentUserId = userId,
        CurrentUserName = currentuser
      };
      return View(viewmodel);
    }

    [HttpPost]
    public async Task<IActionResult> CreateDocument(int id, IndexViewModel viewmodel)
    {
      if (ModelState.IsValid)
      {
        ViewBag.Result = "Form Submitted Successfully.";
        viewmodel.Document.UploadUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        viewmodel.Document.UploadUserName = User.Identity.Name;
        viewmodel.Document.LastUpdateTime = DateTime.Now;
        viewmodel.Document.StoragePath = viewmodel.uploadedfile.FileName;

        MultipartFormDataContent multiContent = new MultipartFormDataContent();
        string uploadpath = Directory.GetCurrentDirectory();
        uploadpath = uploadpath + "\\BookLibrary\\" + viewmodel.Document.StoragePath;
        string path = uploadpath;

        if (viewmodel.Document.StoragePath != null)
        {
          using (var stream = new FileStream(uploadpath, FileMode.Create))
          {
            await viewmodel.uploadedfile.CopyToAsync(stream);
          }

          // if type is mobi convert file here!
          string ext = Path.GetExtension(viewmodel.uploadedfile.FileName);
          if (ext == ".mobi" || ext == ".epub")
          {
            uploadpath = createEpub(path);
            byte[] data = System.IO.File.ReadAllBytes(uploadpath);
            ByteArrayContent bytes = new ByteArrayContent(data);
            multiContent.Add(bytes, "files", Path.GetFileName(uploadpath));
            HttpClient client = new HttpClient();
            await client.PostAsync(baseUrl_, multiContent);
            viewmodel.Document.StoragePath = Path.GetFileName(uploadpath);
            System.IO.File.Delete(uploadpath);
            System.IO.File.Delete(path);
          }
          else
          {
            byte[] data = System.IO.File.ReadAllBytes(uploadpath);
            ByteArrayContent bytes = new ByteArrayContent(data);
            string fileName = Path.GetFileName(viewmodel.Document.StoragePath);
            multiContent.Add(bytes, "files", fileName);
            HttpClient client = new HttpClient();
            await client.PostAsync(baseUrl_, multiContent);
            System.IO.File.Delete(uploadpath);
          }
        }
  
        context_.Documents.Add(viewmodel.Document);
        foreach (var newrecord in viewmodel.SelectedAuthors) // verify authority
        {
          DocumentAuthor temp = new DocumentAuthor();
          temp.AuthorId = newrecord;
          temp.DocumentId = viewmodel.Document.DocumentId;
          context_.DocumentAuthors.Add(temp);
        }
        foreach (var newrecord in viewmodel.SelectedCategories)
        {
          DocumentCategory temp = new DocumentCategory();
          temp.CategoryId = newrecord;
          temp.DocumentId = viewmodel.Document.DocumentId;
          context_.DocumentCategories.Add(temp);
        }
        context_.SaveChanges();
        return RedirectToAction("Index");
      }
      else
        ViewBag.Result = "Invalid Entries, Please Recheck.";
        return RedirectToAction("CreateDocument");
    }

    [HttpGet]
    public IActionResult EditDocument(int? id)
    {
      if (id == null)
      {
        return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest);
      }
      Document document = context_.Documents.Find(id);
      if (document == null)
      {
        return StatusCode(StatusCodes.Status404NotFound);
      }
      var currentuser = User.Identity.Name;
      var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
      var uploaduser = document.UploadUserName;
      var uploaduserid = document.UploadUserId;
      if(User.IsInRole("Admin") != true && userId != uploaduserid)
      {
        return RedirectToAction("Index");
      }
      var documents = context_.Documents.ToList();
      var categories = context_.Categories.ToList();
      var authors = context_.Authors.ToList();
      var documentcategories = context_.DocumentCategories.ToList();
      var documentauthors = context_.DocumentAuthors.ToList();
      var selectedauthors = new List<int>();
      foreach(var record in documentauthors)
      {
        if(record.DocumentId == document.DocumentId)
          selectedauthors.Add(record.AuthorId);
      }
      var selectedcategories = new List<int>();
      foreach (var record in documentcategories)
      {
        if (record.DocumentId == document.DocumentId)
          selectedcategories.Add(record.CategoryId);
      }

      var viewmodel = new IndexViewModel
      {
        Categories = categories,
        Documents = documents,
        Authors = authors,
        DocumentAuthors = documentauthors,
        DocumentCategories = documentcategories,
        Document = document,
        SelectedAuthors = selectedauthors,
        SelectedCategories = selectedcategories,
        CurrentUserId = userId,
        CurrentUserName = currentuser,
        UploadUserId = uploaduserid,
        UploadUserName = uploaduser
      };
      return View(viewmodel);
    }

    [HttpPost]
    public async Task<IActionResult> EditDocument(int? id, IndexViewModel viewmodel)
    {
      if (ModelState.IsValid)
      {
        if (id == null)
        {
          return StatusCode(StatusCodes.Status400BadRequest);
        }
        var document = context_.Documents.Find(id);
        var oldpath = document.StoragePath;
        if (document != null)
        {
          document.DocumentId = viewmodel.Document.DocumentId;
          document.DocumentIntro = viewmodel.Document.DocumentIntro;
          document.Authority = viewmodel.Document.Authority;
          document.StoragePath = viewmodel.Document.StoragePath;
          document.LastUpdateTime = DateTime.Now;
          if (viewmodel.uploadedfile != null)
          {
            viewmodel.Document.StoragePath = viewmodel.uploadedfile.FileName;
            document.StoragePath = viewmodel.uploadedfile.FileName;
          }
          foreach (var record in context_.DocumentAuthors) // remove old
          {
            if (record.DocumentId == document.DocumentId)
              context_.DocumentAuthors.Remove(record);
          }
          foreach (var record in viewmodel.SelectedAuthors) // add new
          {
            var newrecord = new DocumentAuthor();
            newrecord.DocumentId = document.DocumentId;
            newrecord.AuthorId = record;
            context_.DocumentAuthors.Add(newrecord);
          }
          foreach (var record in context_.DocumentCategories) // remove old
          {
            if (record.DocumentId == document.DocumentId)
              context_.DocumentCategories.Remove(record);
          }
          foreach (var record in viewmodel.SelectedCategories) // add new
          {
            var newrecord = new DocumentCategory();
            newrecord.DocumentId = document.DocumentId;
            newrecord.CategoryId = record;
            context_.DocumentCategories.Add(newrecord);
          }

          try
          { //path: ReadIt/ReadIt/xxx.pdf
            if(viewmodel.Document.StoragePath!= null)
            {
              HttpClient client = new HttpClient();
              //find old path
              HttpResponseMessage resp = await client.GetAsync(baseUrl_).ConfigureAwait(false);
              var files = new List<string>();
              var index = 0;
              if (resp.IsSuccessStatusCode)
              {
                var json = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);
                JArray jArr = (JArray)JsonConvert.DeserializeObject(json);
                foreach (var item in jArr)
                  files.Add(item.ToString());
              }
              for (int i = 0; i < files.Count; i++)
              {
                if (files[i] == viewmodel.Document.StoragePath)
                {
                  index = i;
                }
              }

              if (index != 0 && User.IsInRole("Admin"))
              {
                //delete old file
                await client.DeleteAsync(baseUrl_ + "/" + index).ConfigureAwait(false);
              }

              //create new file
              MultipartFormDataContent multiContent = new MultipartFormDataContent();
              string uploadpath = Directory.GetCurrentDirectory();
              uploadpath = uploadpath + "\\BookLibrary\\" + viewmodel.Document.StoragePath;
              string path = uploadpath;

              using (var stream = new FileStream(uploadpath, FileMode.Create))
              {
                await viewmodel.uploadedfile.CopyToAsync(stream);
              }

              // if type is mobi convert file here!
              string ext = Path.GetExtension(viewmodel.uploadedfile.FileName);
              if (ext == ".mobi" || ext == ".epub")
              {
                uploadpath = createEpub(path);
                document.StoragePath = Path.GetFileName(uploadpath);
                byte[] data = System.IO.File.ReadAllBytes(uploadpath);
                ByteArrayContent bytes = new ByteArrayContent(data);
                multiContent.Add(bytes, "files", Path.GetFileName(uploadpath));
                await client.PostAsync(baseUrl_, multiContent);
                System.IO.File.Delete(uploadpath);
                System.IO.File.Delete(path);
              }
              else
              {
                byte[] data = System.IO.File.ReadAllBytes(uploadpath);
                ByteArrayContent bytes = new ByteArrayContent(data);
                string fileName = Path.GetFileName(viewmodel.Document.StoragePath);
                multiContent.Add(bytes, "files", fileName);
                await client.PostAsync(baseUrl_, multiContent);
                System.IO.File.Delete(uploadpath);
              }
            }
          }
          catch
          {
            //fail uploading files!
          }
          try
          {
            context_.SaveChanges();
          }
          catch (Exception)
          {
            // do nothing for now
          }
        }
        return RedirectToAction("Index");
      }
      else
      {
        ViewBag.Result = "Invalid Entries, Please Recheck.";
        return RedirectToAction("EditDocument");
      }
  
    }
    
    public async Task<IActionResult> DownloadDocument(int? id)
    {
      if (id == null)
      {
        return StatusCode(StatusCodes.Status400BadRequest);
      }
      var document = context_.Documents.Find(id);
      if (document != null)
      {
        string filename = document.StoragePath;
        try
        { //path: ReadIt/ReadIt/xxx.pdf
          HttpClient client = new HttpClient();
          //find old path
          HttpResponseMessage resp = await client.GetAsync(baseUrl_).ConfigureAwait(false);
          var files = new List<string>();
          var index = 0;
          if (resp.IsSuccessStatusCode)
          {
            var json = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);
            JArray jArr = (JArray)JsonConvert.DeserializeObject(json);
            foreach (var item in jArr)
              files.Add(item.ToString());
          }
          for (int i = 0; i < files.Count; i++)
          {
            if (files[i] == document.StoragePath)
            {
              index = i;
            }
          }

          if (index != 0)
          {
            //download file
            string display = baseUrl_ + "/" + index;
          var resp2 = await client.GetAsync(baseUrl_ + "/" + index).ConfigureAwait(false);
          System.Net.Http.HttpContent content = resp2.Content; // actually a System.Net.Http.StreamContent instance but you do not need to cast as the actual type does not matter in this case

          string uploadpath = Directory.GetCurrentDirectory();
          uploadpath = uploadpath + "\\DownloadBook\\" + document.StoragePath;
          using (var file = System.IO.File.Create(uploadpath))
          { // create a new file to write to
            var contentStream = await content.ReadAsStreamAsync(); // get the actual content stream
            await contentStream.CopyToAsync(file); // copy that stream to the file stream
            await file.FlushAsync(); // flush back to disk before disposing
          }
        }
      }
        catch
      {
        //fail uploading files!
        //can verify: IsSuccessStatusCode
      }
    }
      return RedirectToAction("Index");
    }

    [HttpGet]
    public  IActionResult ReadDocument(int? id)
    {
      if (id == null)
      {
        return StatusCode(StatusCodes.Status400BadRequest);
      }
      Document document = context_.Documents.Find(id);

      if (document == null)
      {
        return StatusCode(StatusCodes.Status404NotFound);
      }

      var currentuser = User.Identity.Name;
      var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
      var uploaduser = document.UploadUserName;
      var uploaduserid = document.UploadUserId;
      string currentrole = null;
      if (User.IsInRole("Admin"))
        currentrole = "Admin";
      else if (User.IsInRole("User"))
        currentrole = "User";
      if (User.IsInRole("Admin") != true && userId != uploaduserid && document.Authority=="private")
      {
        return RedirectToAction("Index");
      }
      var documents = context_.Documents.ToList();
      var categories = context_.Categories.ToList();
      var authors = context_.Authors.ToList();
      var documentcategories = context_.DocumentCategories.ToList();
      var documentauthors = context_.DocumentAuthors.ToList();
      var comments = context_.Comments.ToList();
      var viewmodel = new ReadDocumentViewModel
      {
        Categories = categories,
        Documents = documents,
        Authors = authors,
        Comments = comments,
        DocumentAuthors = documentauthors,
        DocumentCategories = documentcategories,
        Document =document,
        CurrentUserId = userId,
        CurrentUserName = currentuser,
        CurrentUserRole = currentrole
      };
      return View(viewmodel);
    }

    [Authorize(Roles = "Admin,User")]
    [HttpGet]
    public IActionResult CommentList()
    {
      
      var currentuser = User.Identity.Name;
      var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
      string currentrole = null;
      if (User.IsInRole("Admin"))
        currentrole = "Admin";
      else if (User.IsInRole("User"))
        currentrole = "User";
      var documents = context_.Documents.ToList();
      var categories = context_.Categories.ToList();
      var authors = context_.Authors.ToList();
      var documentcategories = context_.DocumentCategories.ToList();
      var documentauthors = context_.DocumentAuthors.ToList();
      var comments = context_.Comments.ToList();
      var viewmodel = new ReadDocumentViewModel
      {
        Categories = categories,
        Documents = documents,
        Authors = authors,
        Comments = comments,
        DocumentAuthors = documentauthors,
        DocumentCategories = documentcategories,
        CurrentUserId = userId,
        CurrentUserName = currentuser,
        CurrentUserRole = currentrole
      };
      return View(viewmodel);
    }

    [HttpPost]
    public IActionResult CreateComment(int id, ReadDocumentViewModel viewmodel)
    {
      Comment newcomment = new Comment();
      newcomment.Content = viewmodel.Comment.Content;
      newcomment.CommentUserId = viewmodel.CurrentUserId;
      newcomment.CommentUserName = viewmodel.CurrentUserName;
      newcomment.DocumentId = viewmodel.Document.DocumentId;
      newcomment.PostedTime = DateTime.Now;
      context_.Comments.Add(newcomment);
      context_.SaveChanges();
      return RedirectToAction("ReadDocument", new { id = viewmodel.Document.DocumentId });
    }

    public IActionResult CommentDelete(int? id)
    {
      int documentid = 0 ;
      if (id == null)
      {
        return StatusCode(StatusCodes.Status400BadRequest);
      }
      try
      {
        var comment = context_.Comments.Find(id);
        if (comment != null)
        {
          documentid = comment.DocumentId.Value;
          context_.Remove(comment);
          context_.SaveChanges();
        }
      }
      catch (Exception)
      {
        // nothing for now
      }
      return RedirectToAction("ReadDocument", new {  Id = documentid });
    }

    public IActionResult DeleteDocument(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var document = context_.Documents.Find(id);
      if (document == null)
      {
        return NotFound();
      }

      var currentuser = User.Identity.Name;
      var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
      var uploaduser = document.UploadUserName;
      var uploaduserid = document.UploadUserId;
      if (User.IsInRole("Admin") != true && userId != uploaduserid)
      {
        return RedirectToAction("Index");
      }

      return View(document);
    }

    // POST
    [HttpPost, ActionName("DeleteDocument")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmedDocument(int? id)
    {
      if (id == null)
      {
        return StatusCode(StatusCodes.Status400BadRequest);
      }
      try
      {
        var document = context_.Documents.Find(id);
        if (document != null)
        {
          foreach(var record in context_.DocumentAuthors)
          {
            if (record.DocumentId == document.DocumentId)
              context_.DocumentAuthors.Remove(record);
          }
          foreach (var record in context_.DocumentCategories)
          {
            if (record.DocumentId == document.DocumentId)
              context_.DocumentCategories.Remove(record);
          }
          foreach(var record in context_.Comments)
        {
          if (record.DocumentId == document.DocumentId)
            context_.Comments.Remove(record);
        }
          context_.Remove(document);
          context_.SaveChanges();
        }
      }
      catch (Exception)
      {
        // nothing for now
      }
      return RedirectToAction("Index");
    }

    private string createEpub(string path)
    {
      string output = path + ".pdf";
      string arg = "ebook-convert" + " \"" + path + "\" \"" + output + "\"";
      try
      {
        string batpath ="C:\\Users\\Xiaoqian Huang\\Desktop\\FinalProject\\FinalProject_XiaoqianHuang\\ReadIt\\ReadIt\\BookLibrary\\run.bat";
        // Delete the file if it exists.
        if (System.IO.File.Exists(batpath))
        {
          // Note that no lock is put on the
          // file and the possibility exists
          // that another process could do
          // something with it between
          // the calls to Exists and Delete.
          System.IO.File.Delete(batpath);
        }

        // Create the file.
        using (FileStream fs = System.IO.File.Create(batpath, 1024))
        {
          Byte[] info = new UTF8Encoding(true).GetBytes(arg);
          // Add some information to the file.
          fs.Write(info, 0, info.Length);
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
      }
    ProcessStartInfo start = new ProcessStartInfo();

    //start.FileName = "C:\\Windows\\System32\\cmd.exe";
     start.FileName = "C:\\Users\\Xiaoqian Huang\\Desktop\\FinalProject\\FinalProject_XiaoqianHuang\\ReadIt\\ReadIt\\BookLibrary\\run.bat";
    //start.Arguments = arg;
     start.CreateNoWindow = false;
     start.RedirectStandardOutput = true;
     start.RedirectStandardInput = true;
     start.UseShellExecute = false;
     Process p = Process.Start(start);
     p.WaitForExit();
     p.Close();
     return output;
    }

    public IActionResult Privacy()
    {
      return View();
    }

    public IActionResult homeindex()
    {
      return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
  }
}
