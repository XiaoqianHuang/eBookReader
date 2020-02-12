using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReadIt.Models
{
  public class Author
  {
      public int AuthorId { get; set; }

      [Display(Name = "Author Name")]
      public string AuthorName { get; set; }

      [Display(Name = "Introduction")]
      public string AuthorIntro { get; set; }

      public virtual List<DocumentAuthor> DocumentAuthors { get; set; }
  }
}
