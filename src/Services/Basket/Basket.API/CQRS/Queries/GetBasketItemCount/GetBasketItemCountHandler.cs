namespace Basket.API.CQRS.Queries.GetBasketItemCount;

public class GetBasketItemCountHandler(IBasketRepository repository)
    : IQueryHandler<GetBasketItemCountQuery, GetBasketItemCountResult>
{
    public async Task<GetBasketItemCountResult> Handle(GetBasketItemCountQuery request, CancellationToken cancellationToken)
    {
        var itemCount = await repository.GetBasketItemCount(request.UserName, cancellationToken);

        return new GetBasketItemCountResult(itemCount);
    }
}

