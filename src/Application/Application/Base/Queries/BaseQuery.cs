using Application.Application.Models;

namespace Application.Application.Base.Queries;

public abstract class BaseQuery : ArrayBaseRequest
{
    protected BaseQuery() { }

    protected BaseQuery(int pageIndex, int pageLength) : base(pageIndex, pageLength)
    {

    }
}