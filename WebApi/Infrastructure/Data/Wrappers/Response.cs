using System.Net;

namespace WebApi.Infrastructure.Data.Wrappers;

public class Response<T>
{
    
    public T Data { get; set; }
    public int StatusCode { get; set; }
    public string Message { get; set; }

    public Response()
    {
        
    }
    public Response(T data)
    {
        Data = data;
        StatusCode = 200;
    }

    public Response(HttpStatusCode statusCode, string message)
    {
        StatusCode = (int)statusCode;
        Message = message;
    }
}