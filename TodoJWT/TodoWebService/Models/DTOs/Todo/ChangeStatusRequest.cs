namespace TodoWebService.Models.DTOs.Todo;

public class ChangeStatusRequest
{
    public int Id { get; set; }
    
    public bool IsCompeleted { get; set; }
}