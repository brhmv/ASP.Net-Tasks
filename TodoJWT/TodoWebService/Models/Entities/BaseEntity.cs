namespace TodoWebService.Models.Entities;

public class BaseEntity
{
    public int Id { get; set; }
  
    public DateTime CreatedTime { get; set; } = DateTime.Now;
    
    public DateTime UpdatedTime { get; set; } = DateTime.Now;
}