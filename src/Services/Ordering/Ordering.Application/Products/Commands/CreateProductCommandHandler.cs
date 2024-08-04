
namespace Ordering.Application.Products.Commands;

public class CreateProductCommandHandler(IApplicationDbContext _context)
    : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = Product.Create(ProductId.Of(request.Id), request.Name, request.Price);

        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync(cancellationToken);

        return new CreateProductResult();
    }
}
