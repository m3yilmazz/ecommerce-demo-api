using Core.Domain.Base;
using Core.Domain.Errors.Exceptions;
using Core.Domain.Products;
using MediatR;

namespace Application.Application.Products.Commands.Delete;

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, bool>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.FindByIdAsync(request.Id) ??
           throw new NotFoundException($"There is no product with given {request.Id} ID.");

        _productRepository.Delete(product);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}