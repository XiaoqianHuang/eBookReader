using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadIt.Models
{
  public class ReadDocumentViewModel
  {
    public IEnumerable<Category> Categories { get; set; }

    public IEnumerable<Document> Documents { get; set; }

    public IEnumerable<Author> Authors { get; set; }

    public IEnumerable<Comment> Comments { get; set; }

    public IEnumerable<DocumentAuthor> DocumentAuthors { get; set; }

    public IEnumerable<DocumentCategory> DocumentCategories { get; set; } // all the records

    public Document Document { get; set; }
    public Comment Comment { get; set; } // new comment

    public string CurrentUserId { get; set; }

    public string CurrentUserName { get; set; }
    public string CurrentUserRole { get; set; }
    
  }
}
