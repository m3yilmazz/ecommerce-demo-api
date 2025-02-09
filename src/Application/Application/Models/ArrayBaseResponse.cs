namespace Application.Application.Models;

public class ArrayBaseResponse<T>
{
    public ICollection<T> Data { get; }
    public int TotalData { get; }
    public int PageLength { get; }
    public int PageIndex { get; }

    public ArrayBaseResponse(ICollection<T> data, int totalData, int pageLength, int pageIndex)
    {
        Data = data;
        TotalData = totalData;
        PageLength = pageLength;
        PageIndex = pageIndex;
    }
}