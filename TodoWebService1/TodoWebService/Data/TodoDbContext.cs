using Microsoft.EntityFrameworkCore;
using TodoWebService.Models.Entities;

namespace TodoWebService.Data
{
    public class TodoDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<TodoItem> TodoItems => Set<TodoItem>();
    }
}
