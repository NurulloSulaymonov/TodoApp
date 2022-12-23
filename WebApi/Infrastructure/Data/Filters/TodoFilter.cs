namespace WebApi.Infrastructure.Data.Filters;

public class TodoFilter: PaginationFilter
{

    public string? Name { get; set; }
    public bool? SortAscending { get; set; }
    public bool? SortDescending { get; set; }

    public TodoFilter():base()
    {
        
    }
    public TodoFilter(int pageNumber, int pageSize) :base(pageNumber,pageSize)
    {
        
    }
    
    
}

