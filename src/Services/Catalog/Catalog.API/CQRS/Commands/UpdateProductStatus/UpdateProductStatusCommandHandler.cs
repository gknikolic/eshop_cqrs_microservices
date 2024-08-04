using Catalog.API.Repositories;
using MassTransit;

namespace Catalog.API.CQRS.Commands.UpdateProductStatus;

public class UpdateProductStatusCommandHandler(ICatalogRepository repository, IPublishEndpoint publishEndpoint)
    : ICommandHandler<UpdateProductStatusCommand>
{
    public async Task<Unit> Handle(UpdateProductStatusCommand request, CancellationToken cancellationToken)
    {
        var product = await repository.GetProductByIdAsync(request.Id, cancellationToken);
        if (product == null)
        {
            throw new ProductNotFoundException(request.Id);
        }

        product.IsActive = request.IsActive;
        await repository.UpdateProductAsync(product, cancellationToken);

        return Unit.Value;
    }
}
