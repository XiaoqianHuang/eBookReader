using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReadIt.Data;
using ReadIt.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ReadIt.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class FileItemsController : ControllerBase
  {
    private readonly ApplicationDbContext context_;

    public FileItemsController(ApplicationDbContext context)
    {
      context_ = context;

      if (context_.FileItems.Count() == 0)
      {
        context_.FileItems.Add(new FileItem { Name = "SeedFile", Path = "~/FileStore" });
        context_.SaveChanges();
      }
    }
    // GET: api/<controller>
    // ActionResults are automatially serialized to JSON and written into the body of the reply
    [HttpGet]
    public async Task<ActionResult<IEnumerable<FileItem>>> GetFileItems()
    {
      return await context_.FileItems.ToListAsync();
    }

    // GET api/<controller>/5
    [HttpGet("{id}")]
    public async Task<ActionResult<FileItem>> GetFileItem(long id)
    {
      var fileItem = await context_.FileItems.FindAsync(id);

      if(fileItem == null)
      {
        return NotFound();
      }
      return fileItem;
    }

    // POST api/<controller>
    [HttpPost]
    public async Task<ActionResult<FileItem>> PostFileItem(FileItem fileItem)
    {
      context_.FileItems.Add(fileItem);
      await context_.SaveChangesAsync();

      return CreatedAtAction(nameof(GetFileItem), new { id = fileItem.FileItemId }, fileItem);
    }

    // PUT api/<controller>/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody]string value)
    {
    }

    // DELETE api/<controller>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
  }
}
