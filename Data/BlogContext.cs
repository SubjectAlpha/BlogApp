using BlogApp.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Data
{
    //This class sets up our tables in EntityFramework.
    public class BlogContext : DbContext
    {
        //Each DbSet represents a table in our SQL database.
        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }

        public BlogContext(DbContextOptions<BlogContext> options) : base(options) { }
    }
}
