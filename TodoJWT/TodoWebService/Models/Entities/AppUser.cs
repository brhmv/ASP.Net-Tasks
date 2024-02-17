using Microsoft.AspNetCore.Identity;

namespace TodoWebService.Models.Entities;

public class AppUser : IdentityUser
{
    public string? RefreshToken { get; set; } 
    
    public virtual ICollection<TodoItem> TodoItems { get; set; } = new List<TodoItem>();
}