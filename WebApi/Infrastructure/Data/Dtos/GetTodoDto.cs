namespace WebApi.Infrastructure.Data.Dtos;

public class GetTodoDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string ImageName { get; set; }
}