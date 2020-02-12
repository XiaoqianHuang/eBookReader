using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReadIt.Models
{
  public class Comment
  {
    public int CommentId { get; set; }

    public string Content { get; set; }

    [Display(Name = "Time")]
    public DateTime PostedTime { get; set; }

    public int? DocumentId { get; set; }
    //public Document Document { get; set; }

    public string CommentUserId { get; set; }

    public string CommentUserName { get; set; } 
  }
}
