namespace Ordering.Application.CQRS.Queries.GetOrdersByCustomer;

public record GetOrdersByCustomerQuery(Guid CustomerId)
    : IQuery<GetOrdersByCustomerResult>;

public record GetOrdersByCustomerResult(IEnumerable<OrderDto> Orders);
