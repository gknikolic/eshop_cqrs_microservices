using BuildingBlocks.Messaging.Events;
using Catalog.API.Repositories;
using MassTransit;

namespace Catalog.API.CQRS.Commands.CreateProduct;

internal class CreateProductCommandHandler
    (ICatalogRepository repository, IPublishEndpoint publishEndpoint)
    : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        //create Product entity from command object
        //save to database
        //return CreateProductResult result               

        var product = new Product
        {
            Name = command.Name,
            Category = command.Category,
            Description = command.Description,
            ImageFile = command.ImageFile,
            Price = command.Price
        };

        await repository.AddProductAsync(product, cancellationToken);

        await publishEndpoint.Publish(product.Adapt<ProductCreatedEvent>(), cancellationToken);

        //return result
        return new CreateProductResult(product.Id);
    }
}
