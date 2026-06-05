using Discount.Grpc;

namespace Basket.API.CQRS.Commands.StoreBasket;

public class StoreBasketCommandHandler
    (IBasketRepository repository, DiscountProtoService.DiscountProtoServiceClient discountProto)
    : ICommandHandler<StoreBasketCommand, StoreBasketResult>
{
    public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
    {
        // handle duplicates
        command.Cart.Items = command.Cart.Items.GroupBy(x => x.ProductId)
            .Select(y => 
            { 
                var item = y.First(); 
                item.Quantity = y.Sum(x => x.Quantity);
                return item;
            })
            .ToList();

        await DeductDiscount(command.Cart, cancellationToken);

        await repository.StoreBasket(command.Cart, cancellationToken);

        return new StoreBasketResult(command.Cart.UserName);
    }

    private async Task DeductDiscount(ShoppingCart cart, CancellationToken cancellationToken)
    {
        // Communicate with Discount.Grpc and calculate lastest prices of products into sc
        foreach (var item in cart.Items)
        {
            var coupon = await discountProto.GetDiscountAsync(new GetDiscountRequest { ProductName = item.ProductName }, cancellationToken: cancellationToken);
            item.Discount = coupon.Amount;
        }
    }
}
