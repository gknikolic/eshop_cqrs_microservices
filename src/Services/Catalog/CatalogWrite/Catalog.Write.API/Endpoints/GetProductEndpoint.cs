
using Catalog.Write.Application.Repositories;

namespace Catalog.Write.API.Endpoints;

public class GetProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/{id}", async (Guid id, IProductRepository productRepository) =>
        {
            try
            {
                var product = await productRepository.GetAsync(id);

                if (product == null)
                    return Results.NotFound();

                return Results.Ok(product);
            } catch(Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }

        });
    }
}
