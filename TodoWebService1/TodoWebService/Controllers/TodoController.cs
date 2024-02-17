using Microsoft.AspNetCore.Mvc;
using TodoWebService.Models.DTOs;
using TodoWebService.Models.DTOs.Todo;
using TodoWebService.Services.Todo;

namespace TodoWebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly ITodoService _todoService;
        private readonly ILogger<TodoController> _logger;

        public TodoController(ITodoService todoService, ILogger<TodoController> logger)
        {
            _todoService = todoService;
            _logger = logger;
        }

        [HttpGet("get/{id}")]
        public async Task<ActionResult<TodoItemDto>> Get(int id)
        {
            try
            {
                var item = await _todoService.GetTodoItem(id);
                if (item != null)
                    return item;
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting todo item with id {Id}", id);
                return StatusCode(500, "An unexpected error occurred");
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult<TodoItemDto>> Create([FromBody] CreateTodoItemRequest request)
        {
            try
            {
                var createdItem = await _todoService.CreateTodo(request);
                return CreatedAtAction(nameof(Get), new { id = createdItem.Id }, createdItem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a todo item");
                return StatusCode(500, "An unexpected error occurred");
            }
        }

        [HttpPut("updateStatus/{id}")]
        public async Task<ActionResult<TodoItemDto>> UpdateStatus(int id, [FromBody] bool isCompleted)
        {
            try
            {
                var updatedItem = await _todoService.ChangeTodoItemStatus(id, isCompleted);
                if (updatedItem != null)
                    return updatedItem;
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating todo item status with id {Id}", id);
                return StatusCode(500, "An unexpected error occurred");
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var isDeleted = await _todoService.DeleteTodo(id);
                return isDeleted ? NoContent() : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting todo item with id {Id}", id);
                return StatusCode(500, "An unexpected error occurred");
            }
        }

        [HttpGet("getAll")]
        public async Task<ActionResult<List<TodoItemDto>>> GetAll([FromQuery] int page, [FromQuery] int pageSize)
        {
            try
            {
                var todoItems = await _todoService.GetAll(page, pageSize);
                return Ok(todoItems);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving todo items");
                return StatusCode(500, "An unexpected error occurred");
            }
        }
    }
}
