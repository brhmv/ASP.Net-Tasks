using TodoWebService.Models.DTOs.Pagination;
using TodoWebService.Models.DTOs.Todo;
using TodoWebService.Providers;

namespace TodoWebService.Services.Todo;

public interface ITodoService
{
    Task<TodoItemDto?> GetTodoItem(int id, UserInfo info);
   
    Task<TodoItemDto> CreateTodo(CreateTodoItemRequest request, UserInfo info);
    
    Task<TodoItemDto> ChangeTodoItemStatus(ChangeStatusRequest request, UserInfo info);
    
    Task<bool> DeleteTodo(int id, UserInfo info);
    
    Task<PaginatedListDto<TodoItemDto>> GetAll(int page, int pageSize, bool? isCompleted, UserInfo info);
}