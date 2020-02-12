using Microsoft.AspNetCore.Identity;
using ReadIt.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReadIt.Models
{
  public class Document
  {
    public int DocumentId { get; set; }

    [Required]
    public string Title { get; set; }

    public string Authority { get; set; }

    public string StoragePath { get; set; }

    [Required]
    [Display(Name = "Introduction")]
    public string DocumentIntro { get; set; }

    public virtual List<DocumentCategory> DocumentCategories { get; set; }

    public virtual List<DocumentAuthor> DocumentAuthors { get; set; }

    public virtual List<Comment> Comments { get; set; }

   
    public string UploadUserId { get; set; }

    [Display(Name = "Posted by")]
    public string UploadUserName { get; set; }

    [Display(Name = "Last Updated Time")]
    public DateTime LastUpdateTime { get; set; }
    //public IdentityUser User { get; set; } //????
  }

  public class DocumentCategory
  {
    public int DocumentId { get; set; }
    public Document Document { get; set; }

    public int CategoryId { get; set; }
    public Category Category { get; set; }
  }

  public class DocumentAuthor
  {
    public int DocumentId { get; set; }
    public Document Document { get; set; }

    public int AuthorId { get; set; }
    public Author Author { get; set; }
  }
  
}
