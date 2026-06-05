
namespace Catalog.Write.Application.Products;

public record DeleteProductCommand(Guid ProductId, bool HardDelete = false) : ICommand<DeleteProductResult>;
public record DeleteProductResult(bool IsDeleted, string Message);
public class DeleteProductHandler(IApplicationDbContext context)
    : ICommandHandler<DeleteProductCommand, DeleteProductResult>
{
    public async Task<DeleteProductResult> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await context.Products.FirstOrDefaultAsync(p => p.Id == request.ProductId, cancellationToken);
        if (product == null)
        {
            return new DeleteProductResult(false, "Product not found");
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

        return new DeleteProductResult(true, "Product deleted");
    }
}
