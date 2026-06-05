namespace BuildingBlocks.Messaging.Events.OrderFullfilment;
public record OrderCreatedIntegrationEvent : IntegrationEvent
{
    public OrderDto Order { get; set; }
}

public record OrderDto
{
    public Guid Id { get; init; }
    public Guid CustomerId { get; init; }
    public string OrderName { get; init; }
    public AddressDto ShippingAddress { get; init; }
    public AddressDto BillingAddress { get; init; }
    public PaymentDto Payment { get; init; }
    public List<OrderItemDto> OrderItems { get; set; }
}

public record AddressDto(string FirstName, string LastName, string EmailAddress, string AddressLine, string Country, string State, string ZipCode);

public record PaymentDto(string CardName, string CardNumber, string Expiration, string Cvv, int PaymentMethod);

public record OrderItemDto(Guid OrderId, Guid ProductId, int Quantity, decimal Price);
