using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadIt.Models
{
  public class CategoryDetailViewModel
  {
    public IEnumerable<Category> Categories { get; set; }

    public IEnumerable<Document> Documents { get; set; }

    public Category Category { get; set; }

    public IEnumerable<DocumentCategory> DocumentCategories { get; set; }

    public string CurrentUserId { get; set; }
    
    public string CurrentUserRole { get; set; }
  }
}
