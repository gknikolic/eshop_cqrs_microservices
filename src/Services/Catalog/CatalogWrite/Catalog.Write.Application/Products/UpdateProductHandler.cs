namespace Catalog.Write.Application.Products;
public record UpdateProductCommand(ProductDto ProductDto) : ICommand<UpdateProductResponse>;
public record UpdateProductResponse(bool IsUpdated, string Message);
public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.ProductDto).NotNull();
        RuleFor(x => x.ProductDto.Name).NotEmpty();
        RuleFor(x => x.ProductDto.Sku).NotEmpty();
        RuleFor(x => x.ProductDto.Price).NotEmpty().GreaterThanOrEqualTo(0);
        RuleFor(x => x.ProductDto.Category).NotEmpty();
        RuleFor(x => x.ProductDto.Color).NotEmpty();
    }
}
public class UpdateProductHandler(IApplicationDbContext context)
    : ICommandHandler<UpdateProductCommand, UpdateProductResponse>
{
    public async Task<UpdateProductResponse> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await context.Products.FirstOrDefaultAsync(x => x.Id == request.ProductDto.Id);
        if (product == null) { 
            return new UpdateProductResponse(false, "Product not found");
        }

        var category = await context.Categories.FirstOrDefaultAsync(x => x.Name == request.ProductDto.Category);
        if (category == null)
        {
            category = new Category(request.ProductDto.Category, string.Empty);
            await context.Categories.AddAsync(category, cancellationToken);
        }

        product.UpdateDetails(
            name: request.ProductDto.Name,
            description: request.ProductDto.Description,
            price: new Price(request.ProductDto.Price),
            categoryId: category.Id,
            color: (Color)Enum.Parse(typeof(Color), request.ProductDto.Color)
        );

        var updatedAttributes = request.ProductDto.Attributes.Where(x => product.Attributes.Any(y => y.Key == x.Name)).ToList();
        foreach (var attribute in updatedAttributes)
        {
            product.UpdateAttribute(attribute.Name, attribute.Value);
        }
        var removedAttributes = product.Attributes.Where(x => !request.ProductDto.Attributes.Any(y => y.Name == x.Key)).ToList();
        foreach (var attribute in removedAttributes)
        {
            product.RemoveAttribute(attribute.Key);
        }
        var newAttributes = request.ProductDto.Attributes.Where(x => !product.Attributes.Any(y => y.Key == x.Name)).ToList();
        foreach (var attribute in newAttributes)
        {
            product.AddAttribute(attribute.Name, attribute.Value);
        }

        product.AddDomainEvent(new ProductUpdatedEvent(product));

        await context.SaveChangesAsync(cancellationToken);

        return new UpdateProductResponse(true, "Product updated successfully");
    }
}
