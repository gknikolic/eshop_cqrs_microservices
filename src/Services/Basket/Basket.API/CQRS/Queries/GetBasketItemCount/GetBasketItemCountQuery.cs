namespace Basket.API.CQRS.Queries.GetBasketItemCount;

public record GetBasketItemCountQuery(string UserName) : IQuery<GetBasketItemCountResult>;

public record GetBasketItemCountResult(int Count);
