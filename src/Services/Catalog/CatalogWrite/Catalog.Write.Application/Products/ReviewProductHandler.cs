
using Catalog.Write.Application.Repositories;
using Microsoft.AspNetCore.Http;
using BuildingBlocks.Authorization;

namespace Catalog.Write.Application.Products;
public record ReviewProductCommand(ProductReviewDto ProductReviewDto) : ICommand<ReviewProductResult>;
public record ReviewProductResult(bool Success, Guid? ProductReviewId);
public class ReviewProductCommandValidator : AbstractValidator<ReviewProductCommand>
{
    public ReviewProductCommandValidator()
    {
        RuleFor(x => x.ProductReviewDto).NotNull();
        RuleFor(x => x.ProductReviewDto.Comment).NotEmpty();
        RuleFor(x => x.ProductReviewDto.Rating).InclusiveBetween(1, 5);
    }
}
public class ReviewProductHandler(IApplicationDbContext context, IProductRepository productRepository, IHttpContextAccessor httpContextAccessor)
    : ICommandHandler<ReviewProductCommand, ReviewProductResult>
{
    public async Task<ReviewProductResult> Handle(ReviewProductCommand request, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetAsync(request.ProductReviewDto.ProductId);

        var userId = httpContextAccessor.HttpContext?.User?.GetUserId();

        var customer = await context.Customers.FirstOrDefaultAsync(x => x.Id == userId);
        if (customer == null)
        {
            throw new DomainException($"Customer with ID {userId} not found");
        }

        var review = new ProductReview(
            rating: request.ProductReviewDto.Rating,
            comment: request.ProductReviewDto.Comment,
            customer: customer
        );

        product.AddReview(review);

        product.AddDomainEvent(new ProductReviewedEvent(product.Id, review));

        await context.SaveChangesAsync(cancellationToken);

        return new ReviewProductResult(true, review.Id);
    }
}
