namespace TodoWebService.Models.DTOs
{
    public record TodoItemDto(int Id, string Text, bool IsCompleted, DateTime CreatedTime,DateTime Deadline);
}
