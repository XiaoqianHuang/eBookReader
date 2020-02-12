using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReadIt.Models
{
  public class Category
  {
    public int CategoryId { get; set; }

    [Display(Name = "Category Name")]
    public string CategoryName { get; set; }

    [Display(Name = "Introduction")]
    public string CategoryIntro { get; set; }

    public virtual List<DocumentCategory> DocumentCategories { get; set; }
  }
}
