
using Catalog.Write.Application.Repositories;
using Catalog.Write.Domain.Exceptions;

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
        RuleFor(x => x.ProductReviewDto.UserId).NotEmpty();
        RuleFor(x => x.ProductReviewDto.Email).NotEmpty().EmailAddress();
    }
}
public class ReviewProductHandler(IApplicationDbContext context, IProductRepository productRepository)
    : ICommandHandler<ReviewProductCommand, ReviewProductResult>
{
    public async Task<ReviewProductResult> Handle(ReviewProductCommand request, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetAsync(request.ProductReviewDto.ProductId);

        var customer = await context.Customers.FirstOrDefaultAsync(x => x.Id == request.ProductReviewDto.UserId);
        if (customer == null)
        {
            throw new DomainException($"Customer with ID {request.ProductReviewDto.UserId} not found");
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
