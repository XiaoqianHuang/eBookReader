using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadIt.Models
{
  public class AuthorDetailViewModel
  {
    public IEnumerable<Author> Authors { get; set; }

    public IEnumerable<Document> Documents { get; set; }

    public Author Author { get; set; }

    public IEnumerable<DocumentAuthor> DocumentAuthors { get; set; }

    public string CurrentUserId { get; set; }
    
    public string CurrentUserRole { get; set; }
  }
}
