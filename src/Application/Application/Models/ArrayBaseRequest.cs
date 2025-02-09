namespace Application.Application.Models;

public class ArrayBaseRequest
{
    public int PageIndex { get; set; }
    public int PageLength { get; set; }

    public ArrayBaseRequest()
    {
        PageLength = 10;
    }

    public ArrayBaseRequest(int pageIndex, int pageLength = 10)
    {
        PageIndex = pageIndex;
        PageLength = pageLength;
    }
}