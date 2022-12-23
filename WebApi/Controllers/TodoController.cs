using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Infrastructure.Data.Dtos;
using WebApi.Infrastructure.Data.Filters;
using WebApi.Infrastructure.Data.Wrappers;
using WebApi.Infrastructure.Services;

namespace WebApi.Controllers;
[ApiController]
[Route("[controller]")]
// [Authorize]
public class TodoController
{
    private readonly TodoService _todoService;

    public TodoController(TodoService todoService)
    {
        _todoService = todoService;
    }
    
    [HttpGet("GetTodos")]
    public async Task<PaginationResponse<List<GetTodoDto>>> GetAllAsync([FromQuery]TodoFilter filter)
    {
        return await _todoService.GetAllAsync(filter);
    }
    

    [HttpGet("GetById")]
    [Authorize(Roles = "Admin")]
    public async Task<GetTodoDto> GetById(int id)
    {
        return await _todoService.GetByIdAsync(id);
    }

    [HttpPost("Insert")]
    public async Task<GetTodoDto> AddTodo([FromForm]AddTodoDto todo)
    {
        return await _todoService.AddTodo(todo);
    }
    [HttpPost("Update")]
    public async Task<GetTodoDto> Update([FromForm]AddTodoDto todo)
    {
        return await _todoService.Update(todo);
    }
    
}