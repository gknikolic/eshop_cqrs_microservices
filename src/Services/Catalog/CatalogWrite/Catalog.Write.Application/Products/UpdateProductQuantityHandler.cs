namespace Catalog.Write.Application.Products;
public record UpdateProductQuantityCommand(Guid ProductId, int Quantity) : ICommand<bool>;
public class UpdateProductQuantityCommandValidator : AbstractValidator<UpdateProductQuantityCommand>
{
    public UpdateProductQuantityCommandValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.Quantity).GreaterThan(0);
    }
}
public class UpdateProductQuantityHandler(IApplicationDbContext context)
    : ICommandHandler<UpdateProductQuantityCommand, bool>
{
    public async Task<bool> Handle(UpdateProductQuantityCommand request, CancellationToken cancellationToken)
    {
        var product = await context.Products.FirstOrDefaultAsync(x => x.Id == request.ProductId);
        if (product == null)
        {
            throw new DomainException($"Product with ID {request.ProductId} not found");
        }

        var previousStock = product.Stock.Quantity;

        product.UpdateStock(request.Quantity);

        product.AddDomainEvent(new ProductUpdatedEvent(product));

        var stockChangedLog = new ProductStockChangeEvent(product.Id, request.Quantity, product.Stock.Quantity - previousStock >= 0 ? StockChangeReason.Added : StockChangeReason.Sold);
        context.ProductStockChangeEvents.Add(stockChangedLog);

        await context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
