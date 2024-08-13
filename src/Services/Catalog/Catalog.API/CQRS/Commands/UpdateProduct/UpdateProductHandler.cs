using BuildingBlocks.Messaging.Events.InventoryEvents;
using Catalog.API.Repositories;
using MassTransit;

namespace Catalog.API.CQRS.Commands.UpdateProduct;
internal class UpdateProductCommandHandler
    (ICatalogRepository repository, IPublishEndpoint publishEndpoint)
    : ICommandHandler<UpdateProductCommand, UpdateProductResult>
{
    public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        var product = await repository.GetProductByIdAsync(command.Id, cancellationToken);

        if (product is null)
        {
            throw new ProductNotFoundException(command.Id);
        }

        product.Name = command.Name;
        product.Category = command.Category;
        product.Description = command.Description;
        product.ImageFile = command.ImageFile;
        product.Price = command.Price;

        var updatedProduct = await repository.UpdateProductAsync(product, cancellationToken);

        if (updatedProduct.Name != product.Name || updatedProduct.Price != product.Price)
        {
            await publishEndpoint.Publish(new ProductSpecsUpdatedEvent(updatedProduct.Id, updatedProduct.Name, updatedProduct.Price), cancellationToken);
        }

        return new UpdateProductResult(true);
    }
}
