namespace Basket.API.Endpoints.GetBasketItemCount;

public record GetBasketItemCountQuery(string UserName) : IQuery<GetBasketItemCountResult>;
public record GetBasketItemCountResult(int Count);

public class GetBasketItemCountHandler(IBasketRepository repository)
    : IQueryHandler<GetBasketItemCountQuery, GetBasketItemCountResult>
{
    public async Task<GetBasketItemCountResult> Handle(GetBasketItemCountQuery request, CancellationToken cancellationToken)
    {
        var itemCount = await repository.GetBasketItemCount(request.UserName, cancellationToken);

        return new GetBasketItemCountResult(itemCount);
    }
}

