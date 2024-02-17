using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TodoWebService.Models.Entities;

namespace TodoWebService.Data;

public class TodoDbContext(DbContextOptions options) : IdentityDbContext<AppUser>(options)
{
    public DbSet<TodoItem> TodoItems => Set<TodoItem>();
}