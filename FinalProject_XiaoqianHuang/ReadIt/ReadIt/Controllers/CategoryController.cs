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
  public class CategoryController : Controller
  {
    private readonly ApplicationDbContext context_;
    public CategoryController(ApplicationDbContext context)
    {
      context_ = context;
    }

    public IActionResult CategoryIndex()
    {
      if (User.IsInRole("Admin"))
      {
        return RedirectToAction("CategoryIndexAdmin");
      }
      return View(context_.Categories.ToList<Category>());
    }

    public IActionResult CategoryIndexAdmin()
    {
      if (User.IsInRole("Admin"))
      {
        return View(context_.Categories.ToList<Category>());
      }
      return RedirectToAction("CategoryIndex");
    }

    [Authorize(Roles = "Admin,User")]
    [HttpGet]
    public IActionResult CategoryCreate(int id)
    {
      var model = new Category();
      return View(model);
    }
    
    [HttpPost]
    public IActionResult CategoryCreate(int id, Category c)
    {
      context_.Categories.Add(c);
      context_.SaveChanges();
      return RedirectToAction("CategoryIndex");
    }

    [Authorize(Roles = "Admin,User")]
    [HttpGet]
    public IActionResult CategoryEdit(int? id)
    {
      if (id == null)
      {
        return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest);
      }
      Category category = context_.Categories.Find(id);
      if (category == null)
      {
        return StatusCode(StatusCodes.Status404NotFound);
      }
      return View(category);
    }

    [Authorize(Roles = "Admin,User")]
    [HttpPost]
    public IActionResult CategoryEdit(int? id, Category c)
    {
      if (id == null)
      {
        return StatusCode(StatusCodes.Status400BadRequest);
      }
      var category = context_.Categories.Find(id);
      if (category != null)
      {
        category.CategoryId = c.CategoryId;
        category.CategoryName = c.CategoryName;
        category.CategoryIntro = c.CategoryIntro;
        try
        {
          context_.SaveChanges();
        }
        catch (Exception)
        {
          // do nothing for now
        }
      }
      return RedirectToAction("CategoryIndex");
    }

    public ActionResult CategoryDetails(int? id)
    {
      if (id == null)
      {
        return StatusCode(StatusCodes.Status400BadRequest);
      }
      Category category = context_.Categories.Find(id);

      if (category == null)
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
      var categories = context_.Categories.ToList();
      var documentcategories = context_.DocumentCategories.ToList();
      var viewmodel = new CategoryDetailViewModel
      {
        Category = category,
        CurrentUserId = userId,
        CurrentUserRole = currentrole,
        Categories = categories,
        DocumentCategories = documentcategories,
        Documents = documents
      };
      return View(viewmodel);
    }

    [Authorize(Roles = "Admin")]
    public IActionResult CategoryDelete(int? id)
    {
      if (id == null)
      {
        return StatusCode(StatusCodes.Status400BadRequest);
      }
      try
      {
        var category = context_.Categories.Find(id);
        if (category != null)
        {
          foreach (var record in context_.DocumentCategories)
          {
            if (record.CategoryId == category.CategoryId)
              context_.DocumentCategories.Remove(record);
          }
          context_.Remove(category);
          context_.SaveChanges();
        }
      }
      catch (Exception)
      {
        // nothing for now
      }
      return RedirectToAction("CategoryIndex");
    }
  }
}