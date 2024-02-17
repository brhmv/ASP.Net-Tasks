using System.ComponentModel.DataAnnotations;

namespace TodoWebService.Models.DTOs.Todo
{
    public class CreateTodoItemRequest
    {
        [Required]
        [MinLength(5)]
        public string Text { get; set; } = string.Empty;

        [Required]
        public DateTime Deadline { get; set; } 
    }
}