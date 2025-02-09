using Application.Application.Base.Queries;
using Application.Application.Customers.Dtos;
using Application.Application.Models;
using MediatR;

namespace Application.Application.Customers.Queries.Get;

public class GetCustomersQuery : BaseQuery, IRequest<ArrayBaseResponse<CustomerDto>>
{
    public string Name { get; set; }
    public string LastName { get; set; }
    public string Address { get; set; }
    public string PostalCode { get; set; }

    public GetCustomersQuery()
    {

    }

    public GetCustomersQuery(
        string name,
        string lastName,
        string address,
        string postalCode,
        int pageIndex,
        int pageLength) : base(pageIndex, pageLength)
    {
        Name = name;
        LastName = lastName;
        Address = address;
        PostalCode = postalCode;
    }
}