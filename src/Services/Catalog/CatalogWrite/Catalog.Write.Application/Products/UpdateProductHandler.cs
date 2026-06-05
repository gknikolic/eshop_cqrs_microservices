using Catalog.Write.Application.Repositories;
using FluentValidation.Validators;
using Microsoft.Extensions.Logging;

namespace Catalog.Write.Application.Products;
public record UpdateProductCommand(ProductDto ProductDto) : ICommand<UpdateProductResult>;
public record UpdateProductResult(bool IsUpdated, string Message);
public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.ProductDto).NotNull();
        RuleFor(x => x.ProductDto.Id).NotEmpty();
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

public class ProductAttributeDtoValidator : AbstractValidator<ProductAttributeDto>
{
    public ProductAttributeDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Value).NotEmpty();
    }
}

public class ProductImageDtoValidator : AbstractValidator<ProductImageDto>
{
    public ProductImageDtoValidator()
    {
        RuleFor(x => x.FilePath).NotEmpty();
        //RuleFor(x => x.AltText).NotEmpty();
        //RuleFor(x => x.DisplayOrder).NotEmpty().GreaterThanOrEqualTo(0);
    }
}

public class UpdateProductHandler(IApplicationDbContext context, ICategoryRepository categoryRepository, IProductRepository productRepository, ILogger<UpdateProductHandler> logger)
    : ICommandHandler<UpdateProductCommand, UpdateProductResult>
{
    public async Task<UpdateProductResult> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetAsync(request.ProductDto.Id.Value);

        var category = await categoryRepository.GetOrCreateAsync(request.ProductDto.Category);

        if(IsChanged(request.ProductDto, product) == false)
        {
            return new UpdateProductResult(true, "No changes detected");
        }

        // create new version of product
        var newProduct = product.CreateNewVersion();

        // update product details
        newProduct.UpdateDetails(
            name: request.ProductDto.Name,
            description: request.ProductDto.Description,
            price: new Price(request.ProductDto.Price),
            categoryId: category.Id,
            color: (Color)Enum.Parse(typeof(Color), request.ProductDto.Color)
        );

        // update category
        if (category.Id != newProduct.Category.Id)
        {
            newProduct.ChangeCategory(category);
        }

        // update attributes
        var updatedAttributes = request.ProductDto.Attributes.Where(x => newProduct.Attributes.Any(y => y.Key == x.Name)).ToList();
        foreach (var attribute in updatedAttributes)
        {
            newProduct.UpdateAttribute(attribute.Name, attribute.Value);
        }
        var removedAttributes = newProduct.Attributes.Where(x => !request.ProductDto.Attributes.Any(y => y.Name == x.Key)).ToList();
        foreach (var attribute in removedAttributes)
        {
            newProduct.RemoveAttribute(attribute.Key);
        }
        var newAttributes = request.ProductDto.Attributes.Where(x => !newProduct.Attributes.Any(y => y.Key == x.Name)).ToList();
        foreach (var attribute in newAttributes)
        {
            newProduct.AddAttribute(attribute.Name, attribute.Value);
        }

        // update images
        var removedImages = newProduct.Images.Where(x => !request.ProductDto.Images.Any(y => y.FilePath == x.FilePath)).ToList();
        foreach (var image in removedImages)
        {
            newProduct.RemoveImage(image);
        }
        var newImages = request.ProductDto.Images.Where(x => !newProduct.Images.Any(y => y.FilePath == x.FilePath)).ToList();
        foreach (var image in newImages)
        {
            newProduct.AddImage(image.FilePath, image.AltText, image.DisplayOrder ?? 1);
        }

        // add domain events
        newProduct.AddDomainEvent(new ProductCreatedEvent(newProduct));
        product.AddDomainEvent(new ProductDeletedEvent(product.Id));

        try
        {
            // save changes
            await context.Products.AddAsync(newProduct);
            context.Products.Update(product); // mark old product as deleted (isActive flag has been changed when creating a new version)

            await context.SaveChangesAsync(cancellationToken);
        }
        catch(Exception e)
        {
            return new UpdateProductResult(false, "Product updated successfully");
        }


        return new UpdateProductResult(true, "Product updated successfully");
    }

    bool IsChanged(ProductDto productDto, Product product)
    {
        return 
            productDto.Sku != product.Sku.Value ||
            productDto.Name != product.Name ||
            productDto.Description != product.Description ||
            productDto.Price != product.Price.Value ||
            productDto.Category != product.Category.Name ||
            productDto.Color != product.Color.ToString() ||
            productDto.Attributes.Count != product.Attributes.Count ||
            productDto.Attributes.Any(x => !product.Attributes.Any(y => y.Key == x.Name && y.Value == x.Value)) ||
            productDto.Images.Count != product.Images.Count ||
            productDto.Images.Any(x => !product.Images.Any(y => y.FilePath == x.FilePath && y.AltText == x.AltText && y.DisplayOrder == x.DisplayOrder));
    }
        
}
