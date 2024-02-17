using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoWebService.Models.DTOs.Pagination;
using TodoWebService.Models.DTOs.Todo;
using TodoWebService.Providers;
using TodoWebService.Services.Todo;

namespace TodoWebService.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TodoController : ControllerBase
{
    private readonly ITodoService _todoService;
    private readonly IRequestUserProvider _provider;
    public TodoController(ITodoService todoService, IRequestUserProvider provider)
    {
        _todoService = todoService;
        _provider = provider;
    }

    [HttpGet("get/{id}")]
    public async Task<ActionResult<TodoItemDto>> Get(int id)
    {
        UserInfo? userInfo = _provider.GetUserInfo();
        var item = await _todoService.GetTodoItem(id, userInfo!);

        return item is not null
            ? item
            : NotFound();
    }

    [HttpDelete("delete/{id}")]
    public async Task<ActionResult<bool>> Delete(int id)
    {
        UserInfo? userInfo = _provider.GetUserInfo();
        return await _todoService.DeleteTodo(id, userInfo!);
    }

    [HttpPost("create")]
    public async Task<ActionResult<TodoItemDto>> Create([FromBody] CreateTodoItemRequest request)
    {
        UserInfo? userInfo = _provider.GetUserInfo();
        return await _todoService.CreateTodo(request, userInfo!);
    }

    [HttpPost("change-status")]
    public async Task<ActionResult<TodoItemDto>> ChangeStatus([FromBody] ChangeStatusRequest request)
    {
        UserInfo? userInfo = _provider.GetUserInfo();
        var item = await _todoService.ChangeTodoItemStatus(request, userInfo!);
        return item is not null ? item : NotFound();
    }

    [HttpGet("all")]
    public async Task<PaginatedListDto<TodoItemDto>?> All(PaginationRequest request, bool? isCompleted)
    {
        UserInfo? userInfo = _provider.GetUserInfo();
        var result = await _todoService.GetAll(request.Page, request.PageSize, isCompleted, userInfo!);
        return result is not null ? result : null;
    }
}