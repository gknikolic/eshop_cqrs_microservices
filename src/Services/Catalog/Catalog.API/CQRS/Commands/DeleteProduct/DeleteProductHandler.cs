using Catalog.API.Repositories;
using MassTransit;

namespace Catalog.API.CQRS.Commands.DeleteProduct;

internal class DeleteProductCommandHandler
    (ICatalogRepository repository, IPublishEndpoint publishEndpoint)
    : ICommandHandler<DeleteProductCommand, DeleteProductResult>
{
    public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        await repository.DeleteProductAsync(command.Id, cancellationToken);

        //await publishEndpoint.Publish(new ProductDeletedEvent(command.Id), cancellationToken);

        return new DeleteProductResult(true);
    }
}
