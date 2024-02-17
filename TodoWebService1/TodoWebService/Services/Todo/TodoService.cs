using Microsoft.EntityFrameworkCore;
using TodoWebService.Data;
using TodoWebService.Models.DTOs;
using TodoWebService.Models.DTOs.Todo;
using TodoWebService.Models.Entities;

namespace TodoWebService.Services.Todo
{
    public class TodoService(TodoDbContext context) : ITodoService
    {
        private readonly TodoDbContext _context = context;

        public async Task<TodoItemDto> ChangeTodoItemStatus(int id, bool isCompleted)
        {
            try
            {
                var ItemToChange = await _context.TodoItems.FirstOrDefaultAsync(x => x.Id == id);
                if (ItemToChange != null)
                {
                    ItemToChange.IsCompleted = !ItemToChange.IsCompleted;
                    _context.TodoItems.Update(ItemToChange);
                    await _context.SaveChangesAsync();
                    return new TodoItemDto(ItemToChange.Id, ItemToChange.Text, ItemToChange.IsCompleted, ItemToChange.CreatedTime,ItemToChange.Deadline);
                }
                return null!;
            }
            catch (Exception) { return null!; }
        }

        public async Task<TodoItemDto> CreateTodo(CreateTodoItemRequest request)
        {
            try
            {
                var newTodo = new TodoItem()
                {
                    Text = request.Text,
                    IsCompleted = false,
                    Deadline=request.Deadline,
                    CreatedTime = DateTime.Now,
                    UpdatedTime = DateTime.Now,
                };

                await _context.TodoItems.AddAsync(newTodo);
                await _context.SaveChangesAsync();
                return new TodoItemDto(newTodo.Id, newTodo.Text, newTodo.IsCompleted, newTodo.CreatedTime,newTodo.Deadline);
            }
            catch (Exception) { return null!; }
        }

        public async Task<bool> DeleteTodo(int id)
        {
            try
            {
                var ItemToDelete = await _context.TodoItems.FirstOrDefaultAsync(i => i.Id == id);
                if (ItemToDelete is not null)
                {
                    _context.TodoItems.Remove(ItemToDelete!);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<TodoItemDto>> GetAll(int page, int pageSize)
        {
            var todoList = await _context.TodoItems.ToListAsync();

            var startIndex = page * pageSize;
            var endIndex = startIndex + pageSize;

            endIndex = Math.Min(endIndex, todoList.Count);

            var dtoList = new List<TodoItemDto>();

            for (int i = startIndex; i < endIndex; i++)
            {
                dtoList.Add(new TodoItemDto(
                    Id: todoList[i].Id,
                    Text: todoList[i].Text,
                    IsCompleted: todoList[i].IsCompleted,
                    CreatedTime: todoList[i].CreatedTime,
                    Deadline: todoList[i].Deadline
                ));
            }

            return dtoList;
        }

        public async Task<TodoItemDto?> GetTodoItem(int id)
        {
            var todoItem = await _context.TodoItems.FirstOrDefaultAsync(e => e.Id == id);

            return todoItem is not null
                ? new TodoItemDto(
                    Id: todoItem.Id,
                    Text: todoItem.Text,
                    IsCompleted: todoItem.IsCompleted,
                    Deadline: todoItem.Deadline,
                    CreatedTime: todoItem.CreatedTime)
                : null;
        }
    }
}