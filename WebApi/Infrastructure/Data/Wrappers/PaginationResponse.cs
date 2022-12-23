namespace WebApi.Infrastructure.Data.Wrappers;

public class PaginationResponse<T> : Response<T>
{
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
    public int TotalPages { get; set; }
    public int TotalRecords { get; set; }

    public PaginationResponse(T data, int totalRecords, int pageSize, int pageNumber):base(data)
    {
        PageSize = pageSize;
        TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
        TotalRecords = totalRecords;
        PageNumber = pageNumber;
    }
   
}