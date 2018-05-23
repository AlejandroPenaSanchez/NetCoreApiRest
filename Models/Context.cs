using Microsoft.EntityFrameworkCore;

namespace ApiRestAlejandro.Models
{    
    public class Context : DbContext
    {
        public DbSet<TodoItem> TodoItems { get; set; }
        public Context(DbContextOptions<Context> options) : base(options)
        {

        }
    }
}