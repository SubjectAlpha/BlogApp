using BlogApp.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Data
{
    public class BlogContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }

        public BlogContext(DbContextOptions<BlogContext> options): base(options) { }
    }
}
