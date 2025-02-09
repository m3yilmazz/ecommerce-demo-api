namespace Application.Application.Models;

public class ObjectBaseResponse<T>
{
    public T Data { get; set; }
    public string Message { get; set; }

    public ObjectBaseResponse(T data, string message = null)
    {
        Data = data;
        Message = message;
    }
}