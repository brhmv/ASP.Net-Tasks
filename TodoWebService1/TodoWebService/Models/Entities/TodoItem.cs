namespace TodoWebService.Models.Entities
{
    public class TodoItem : BaseEntity
    {
        public string Text { get; set; } = string.Empty;

        public bool IsCompleted { get; set; }

        public DateTime CreatedTime { get; set; }

        public DateTime UpdatedTime { get; set; }

        public DateTime Deadline {  get; set; }
    }
}