using VaultEdge.Application.Abstractions;

namespace VaultEdge.Application.Customers.Commands.DeleteCustomer
{
    public record DeleteCustomerCommand(Guid CustomerId) : ICommand<Guid>;
}
