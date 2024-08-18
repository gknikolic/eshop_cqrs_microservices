
namespace Ordering.Application.CQRS.Commands.CreateCustomer;
public class CreateCustomerCommandHandler(IApplicationDbContext context)
    : ICommandHandler<CreateCustomerCommand, bool>
{
    public async Task<bool> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = Customer.Create(CustomerId.Of(request.CustomerDto.Id), request.CustomerDto.Name, request.CustomerDto.Email);
        //TODO: Delete this when read user from jwt
        customer.CreatedBy = request.CustomerDto.CreatedBy;

        await context.Customers.AddAsync(customer, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
