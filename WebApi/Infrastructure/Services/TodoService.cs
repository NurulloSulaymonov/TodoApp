using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApi.Infrastructure.Data.Dtos;

namespace WebApi.Infrastructure.Services;

public class TodoService
{
    private readonly DataContext _context;
    private readonly IWebHostEnvironment _environment;
    private readonly IMapper _mapper;

    public TodoService(DataContext context, IWebHostEnvironment environment,IMapper mapper)
    {
        _context = context;
        _environment = environment;
        _mapper = mapper;
    }
    
    public async Task<List<GetTodoDto>> GetAllAsync()
    {
        return  _mapper.Map<List<Todo>,List<GetTodoDto>>(await _context.Todos.ToListAsync());
    }
    
    public async Task<GetTodoDto> GetByIdAsync(int id)
    {
        var find =  await _context.Todos.FindAsync(id);
        var response = new GetTodoDto()
        {
            Description = find.Description,
            Id = find.Id,
            Title = find.Title,
            ImageName = find.ImageName
        };
        return response;
    }

    public async Task<GetTodoDto> AddTodo(AddTodoDto todo)
    {
        var newTodo = _mapper.Map<Todo>(todo);
        //save file 
        newTodo.ImageName = await UploadFile(todo.Image);
        _context.Todos.Add(newTodo);
        await _context.SaveChangesAsync();

        return  _mapper.Map<GetTodoDto>(newTodo);
    }

    public async Task<GetTodoDto> Update(AddTodoDto todo)
    {
        // logic
        var find = await _context.Todos.FindAsync(todo.Id);
        find.Description = todo.Description;
        find.Title = todo.Title;

        if (todo.Image != null)
        {
            find.ImageName = await UpdateFile(todo.Image, find.ImageName);
        }
        return _mapper.Map<GetTodoDto>(find);
    }

    private async Task<string> UploadFile(IFormFile file)
    {
        if (file == null) return null;
        
        //create folder if not exists
        var path = Path.Combine(_environment.WebRootPath, "todo");
        if (Directory.Exists(path) == false) Directory.CreateDirectory(path);
        
        var filepath = Path.Combine(path, file.FileName);
        using (var stream = new FileStream(filepath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return file.FileName;
    }

    private async Task<string> UpdateFile(IFormFile file, string oldFileName)
    {
        //delete old image if exists
        var filepath = Path.Combine(_environment.WebRootPath, "todo", oldFileName);
        if(File.Exists(filepath) == true) File.Delete(filepath);
        
        var newFilepath = Path.Combine(_environment.WebRootPath, "todo", file.FileName);
        using (var stream = new FileStream(newFilepath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return file.FileName;

    }
    
   
}