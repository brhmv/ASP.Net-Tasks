namespace ProductCrudEF.Models
{
    public class BaseEntity
    {
        public string Id { get; set; }
        
        public DateTime CreatedTime { get; set; } = DateTime.Now;

        public DateTime ModifiedTime { get; set; } = DateTime.Now;
    }
}