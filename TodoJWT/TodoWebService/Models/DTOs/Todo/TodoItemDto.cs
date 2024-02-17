namespace TodoWebService.Models.DTOs.Todo
{
    public class TodoItemDto(int Id, string Text, bool IsCompleted, DateTime CreatedTime, DateTime Deadline)
    {
        public int Id { get; set; } = Id;

        public string Text { get; set; } = Text;

        public bool IsCompleted { get; set; } = IsCompleted;

        public DateTime CreatedTime { get; set; } = CreatedTime;

        public DateTime Deadline { get; set; } = Deadline;
    }
}