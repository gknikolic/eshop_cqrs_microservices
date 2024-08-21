
namespace Catalog.Write.Application.Products;

public record DeleteProductCommand(Guid ProductId, bool HardDelete = false) : ICommand<DeleteProductResponse>;
public record DeleteProductResponse(bool IsDeleted, string Message);
public class DeleteProductHandler(IApplicationDbContext context)
    : ICommandHandler<DeleteProductCommand, DeleteProductResponse>
{
    public async Task<DeleteProductResponse> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await context.Products.FirstOrDefaultAsync(p => p.Id == request.ProductId, cancellationToken);
        if (product == null)
        {
            return new DeleteProductResponse(false, "Product not found");
        }

        if (request.HardDelete)
        {
            context.Products.Remove(product);
        }
        else
        {
            product.Deactivate();
        }

        product.AddDomainEvent(new ProductDeletedEvent(product.Id));

        await context.SaveChangesAsync(cancellationToken);

        return new DeleteProductResponse(true, "Product deleted");
    }
}
