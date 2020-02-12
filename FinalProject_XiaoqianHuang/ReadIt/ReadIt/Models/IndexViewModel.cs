using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReadIt.Models
{
  public class IndexViewModel
  {
    public IEnumerable<Category> Categories { get; set; }

    public IEnumerable<Document> Documents { get; set; }

    public IEnumerable<Author> Authors { get; set; }

    public IEnumerable<DocumentAuthor> DocumentAuthors { get; set; }

    public IEnumerable<DocumentCategory> DocumentCategories { get; set; } // all the records

    public Document Document { get; set; }

    [Required]
    [BindProperty]
    [CheckboxValidation(ErrorMessage = "Select at least 1 item")]
    public List<int> SelectedAuthors { get; set; } //id

    [Required]
    [BindProperty]
    [CheckboxValidation(ErrorMessage = "Select at least 1 item")]
    public List<int> SelectedCategories { get; set; } // new modifed record(documentid is the same)

    public string UploadUserId { get; set; }

    public string UploadUserName { get; set; }

    public string CurrentUserId { get; set; }

    public string CurrentUserName { get; set; }
    public string CurrentUserRole { get; set; }

    public DateTime LastUpdateTime { get; set; }

    public IFormFile uploadedfile { get; set; }
  }

  public class CheckboxValidationAttribute : ValidationAttribute
  {
    public override bool IsValid(object value)
    {
      List<int> instance = value as List<int>;
      if (instance!=null)
        return true;
      else
        return false;
    }
  }
}
