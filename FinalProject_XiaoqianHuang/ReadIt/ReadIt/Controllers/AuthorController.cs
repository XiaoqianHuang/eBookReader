using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ReadIt.Data;
using ReadIt.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ReadIt.Controllers
{
    public class AuthorController : Controller
    {
    private readonly ApplicationDbContext context_;
    public AuthorController(ApplicationDbContext context)
    {
      context_ = context;
    }

    public IActionResult AuthorIndex()
    {
      if (User.IsInRole("Admin"))
      {
        return RedirectToAction("AuthorIndexAdmin");
      }
      return View(context_.Authors.ToList<Author>());
    }

    public IActionResult AuthorIndexAdmin()
    {
      if (User.IsInRole("Admin"))
      {
        return View(context_.Authors.ToList<Author>());
      }
      return RedirectToAction("AuthorIndex");
    }

    [Authorize(Roles = "Admin,User")]
    [HttpGet]
    public IActionResult AuthorCreate(int id)
    {
      var model = new Author();
      return View(model);
    }

    [HttpPost]
    public IActionResult AuthorCreate(int id, Author a)
    {
      context_.Authors.Add(a);
      context_.SaveChanges();
      return RedirectToAction("AuthorIndex");
    }

    [Authorize(Roles = "Admin,User")]
    [HttpGet]
    public IActionResult AuthorEdit(int? id)
    {
      if (id == null)
      {
        return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest);
      }
      Author author = context_.Authors.Find(id);
      if (author == null)
      {
        return StatusCode(StatusCodes.Status404NotFound);
      }
      return View(author);
    }

    [HttpPost]
    public IActionResult AuthorEdit(int? id, Author a)
    {
      if (id == null)
      {
        return StatusCode(StatusCodes.Status400BadRequest);
      }
      var author = context_.Authors.Find(id);
      if (author != null)
      {
        author.AuthorId = a.AuthorId;
        author.AuthorIntro = a.AuthorIntro;
        author.AuthorName = a.AuthorName;
        try
        {
          context_.SaveChanges();
        }
        catch (Exception)
        {
          // do nothing for now
        }
      }
      return RedirectToAction("AuthorIndex");
    }

    public ActionResult AuthorDetails(int? id)
    {
      if (id == null)
      {
        return StatusCode(StatusCodes.Status400BadRequest);
      }
      Author author = context_.Authors.Find(id);

      if (author == null)
      {
        return StatusCode(StatusCodes.Status404NotFound);
      }

      var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
      string currentrole;
      if (User.IsInRole("Admin"))
      {
        currentrole = "Admin";
      }
      else
        currentrole = "User";
      var documents = context_.Documents.ToList();
      var authors = context_.Authors.ToList();
      var documentauthors = context_.DocumentAuthors.ToList();
      var viewmodel = new AuthorDetailViewModel
      {
        Author = author,
        CurrentUserId = userId,
        CurrentUserRole = currentrole,
        Authors = authors,
        DocumentAuthors = documentauthors,
        Documents = documents
      };
      return View(viewmodel);
    }

    [Authorize(Roles = "Admin")]
    public IActionResult AuthorDelete(int? id)
    {
      if (id == null)
      {
        return StatusCode(StatusCodes.Status400BadRequest);
      }
      try
      {
        var author = context_.Authors.Find(id);
        if (author != null)
        {
          foreach (var record in context_.DocumentAuthors)
          {
            if (record.AuthorId == author.AuthorId)
              context_.DocumentAuthors.Remove(record);
          }
          context_.Remove(author);
          context_.SaveChanges();
        }
      }
      catch (Exception)
      {
        // nothing for now
      }
      return RedirectToAction("AuthorIndex");
    }
  }
}