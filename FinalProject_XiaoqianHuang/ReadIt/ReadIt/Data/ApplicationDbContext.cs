using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ReadIt.Models;

namespace ReadIt.Data
{
  public class ApplicationDbContext : IdentityDbContext
  {
    public DbSet<Document> Documents { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<DocumentAuthor> DocumentAuthors { get; set; }
    public DbSet<DocumentCategory> DocumentCategories { get; set; }
    public DbSet<FileItem> FileItems { get; set; }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      modelBuilder.Entity<DocumentCategory>().HasKey(dc => new { dc.DocumentId, dc.CategoryId });
      modelBuilder.Entity<DocumentCategory>().HasOne(dc => dc.Document)
                                                                                   .WithMany(doc => doc.DocumentCategories)
                                                                                   .HasForeignKey(dc => dc.DocumentId);
      modelBuilder.Entity<DocumentCategory>().HasOne(dc => dc.Category)
                                                                       .WithMany(c => c.DocumentCategories)
                                                                       .HasForeignKey(dc => dc.CategoryId);

      modelBuilder.Entity<DocumentAuthor>().HasKey(da => new { da.DocumentId, da.AuthorId });
      modelBuilder.Entity<DocumentAuthor>().HasOne(da => da.Document)
                                                                                   .WithMany(doc => doc.DocumentAuthors)
                                                                                   .HasForeignKey(da => da.DocumentId);
      modelBuilder.Entity<DocumentAuthor>().HasOne(da => da.Author)
                                                                       .WithMany(c => c.DocumentAuthors)
                                                                       .HasForeignKey(da => da.AuthorId);
    }
  }
}
