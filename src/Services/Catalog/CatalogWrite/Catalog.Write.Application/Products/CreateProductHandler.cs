using Catalog.Write.Application.Repositories;

namespace Catalog.Write.Application.Products;

public record CreateProductCommand(ProductDto ProductDto) : ICommand<CreateProductResult>;
public record CreateProductResult(bool Success, Guid? ProductId);
public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.ProductDto).NotNull();
        //RuleFor(x => x.ProductDto.Id).NotEmpty();
        RuleFor(x => x.ProductDto.Name).NotEmpty();
        RuleFor(x => x.ProductDto.Sku).NotEmpty();
        RuleFor(x => x.ProductDto.Price).NotEmpty().GreaterThanOrEqualTo(0);
        RuleFor(x => x.ProductDto.Category).NotEmpty();
        RuleFor(x => x.ProductDto.Color).NotEmpty();
        RuleFor(x => x.ProductDto.Attributes).NotNull();
        RuleForEach(x => x.ProductDto.Attributes).SetValidator(new ProductAttributeDtoValidator());
        RuleFor(x => x.ProductDto.Images).NotNull();
        RuleForEach(x => x.ProductDto.Images).SetValidator(new ProductImageDtoValidator());
    }
}

public class CreateProductHandler(IApplicationDbContext context, ICategoryRepository categoryRepository)
    : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.GetOrCreateAsync(request.ProductDto.Category);

        var product = new Product(
            id: Guid.NewGuid(),
            sku: new Sku(request.ProductDto.Sku),
            name: request.ProductDto.Name,
            description: request.ProductDto.Description,
            price: new Price(request.ProductDto.Price),
            color: (Color)Enum.Parse(typeof(Color), request.ProductDto.Color)
            );

        product.ChangeCategory(category);

        request.ProductDto.Images.ForEach(image =>
        {
            product.AddImage(image.FilePath, image.AltText, image.DisplayOrder ?? 1);
        });

        foreach (var attribute in request.ProductDto.Attributes)
        {
            product.AddAttribute(attribute.Name, attribute.Value);
        }

        product.AddDomainEvent(new ProductCreatedEvent(product));

        await context.Products.AddAsync(product, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return new CreateProductResult(true, product.Id);
    }
}
