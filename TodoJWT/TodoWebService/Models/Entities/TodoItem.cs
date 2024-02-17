namespace TodoWebService.Models.Entities
{
    public class TodoItem : BaseEntity
    {
        public string Text { get; set; } = string.Empty;

        public bool IsCompleted { get; set; }

        public string UserId { get; set; } = null!;

        public virtual AppUser User { get; set; } = null!;

        public DateTime Deadline { get; set; }
    }
}