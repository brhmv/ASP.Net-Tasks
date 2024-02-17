using Microsoft.EntityFrameworkCore;
using System.Net.NetworkInformation;
using TodoWebService.Data;
using TodoWebService.Models.DTOs.Pagination;
using TodoWebService.Models.DTOs.Todo;
using TodoWebService.Models.Entities;
using TodoWebService.Providers;
using Microsoft.Extensions.Logging;

namespace TodoWebService.Services.Todo;

public class TodoService(TodoDbContext context, ILogger<TodoService> logger) : ITodoService
{
    ILogger<TodoService> _logger = logger;

    private readonly TodoDbContext _context = context;

    public async Task<TodoItemDto> ChangeTodoItemStatus(ChangeStatusRequest request, UserInfo info)
    {
        try
        {
            var todoItem = await _context.TodoItems
                .Where(t => t.UserId == info.Id)
                .FirstOrDefaultAsync(e => e.Id == request.Id);
            if (todoItem is not null)
            {
                todoItem.IsCompleted = request.IsCompeleted;
                todoItem.UpdatedTime = DateTime.Now;
                await _context.SaveChangesAsync();
                _logger.LogInformation("Changed status of todo item with ID {Id} for user {UserId}.", request.Id, info.Id);
                return new TodoItemDto(todoItem.Id, todoItem.Text, todoItem.IsCompleted, todoItem.CreatedTime, todoItem.Deadline);
            }
            else
            {
                _logger.LogWarning("Todo item with ID {Id} not found for user {UserId}.", request.Id, info.Id);
                return null!;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while changing status of todo item with ID {Id} for user {UserId}.", request.Id, info.Id);
            throw;
        }
    }

    public async Task<TodoItemDto> CreateTodo(CreateTodoItemRequest request, UserInfo info)
    {
        try
        {
            _logger.LogInformation("Creating a new todo item for user {UserId}. Text: {Text}", info.Id, request.Text);

            var todo = new TodoItem()
            {
                Text = request.Text,
                IsCompleted = false,
                UserId = info.Id
            };
            await _context.TodoItems.AddAsync(todo);
            await _context.SaveChangesAsync();

            var lastItem = await _context.TodoItems
                .Where(t => t.UserId == info.Id)
                .OrderBy(t => t.Id)
                .LastAsync();

            _logger.LogInformation("Created a new todo item with ID {Id} for user {UserId}.", lastItem.Id, info.Id);

            return new TodoItemDto(lastItem.Id, lastItem.Text, lastItem.IsCompleted, lastItem.CreatedTime, lastItem.Deadline);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating a new todo item for user {UserId}. Text: {Text}", info.Id, request.Text);
            throw;
        }
    }

    public async Task<bool> DeleteTodo(int id, UserInfo info)
    {
        try
        {
            _logger.LogInformation("Deleting todo item with ID {Id} for user {UserId}.", id, info.Id);

            var todoItem = await _context.TodoItems
                .Where(t => t.UserId == info.Id)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (todoItem != null)
            {
                _context.TodoItems.Remove(todoItem);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Deleted todo item with ID {Id} for user {UserId}.", id, info.Id);

                return true;
            }
            else
            {
                _logger.LogWarning("Todo item with ID {Id} not found for user {UserId}.", id, info.Id);
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while deleting todo item with ID {Id} for user {UserId}.", id, info.Id);
            throw;
        }
    }

    public async Task<PaginatedListDto<TodoItemDto>> GetAll(int page, int pageSize, bool? isCompleted, UserInfo info)
    {
        try
        {
            _logger.LogInformation("Getting all todo items for user {UserId}. Page: {Page}, PageSize: {PageSize}, IsCompleted: {IsCompleted}", info.Id, page, pageSize, isCompleted);

            IQueryable<TodoItem> query = _context.TodoItems
                .Where(t => t.UserId == info.Id)
                .AsQueryable();

            if (isCompleted.HasValue)
                query = query.Where(e => e.IsCompleted == isCompleted);

            var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            var totalCount = await query.CountAsync();

            _logger.LogInformation("Retrieved {Count} todo items for user {UserId}.", items.Count, info.Id);

            return new PaginatedListDto<TodoItemDto>(
                items.Select(e => new TodoItemDto(
                   Id: e.Id,
                    Text: e.Text,
                    IsCompleted: e.IsCompleted,
                    Deadline: e.Deadline,
                    CreatedTime: e.CreatedTime)
                ),
                new PaginationMeta(page, pageSize, totalCount)
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting all todo items for user {UserId}. Page: {Page}, PageSize: {PageSize}, IsCompleted: {IsCompleted}", info.Id, page, pageSize, isCompleted);
            throw;
        }
    }

    public async Task<TodoItemDto?> GetTodoItem(int id, UserInfo info)
    {
        try
        {
            var todoItem = await _context.TodoItems
          .Where(t => t.UserId == info.Id)
          .FirstOrDefaultAsync(e => e.Id == id);
            _logger.LogInformation("GetTodoItem method called with ID: {Id}", id);


            return todoItem is not null
                ? new TodoItemDto(
                    Id: todoItem.Id,
                    Text: todoItem.Text,
                    IsCompleted: todoItem.IsCompleted,
                    Deadline: todoItem.Deadline,
                    CreatedTime: todoItem.CreatedTime)
                : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting the todo item with ID: {Id}", id);
            throw;
        }    
    }
}