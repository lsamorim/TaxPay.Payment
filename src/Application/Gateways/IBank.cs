using Application.Gateways.Dtos;
using Domain.ScheduleAggregate.ValueObjects;

namespace Application.Gateways
{
    public interface IBank
    {
        Task<GatewayOutput> Withdraw(BankAccount bankAccount, decimal amount, CancellationToken cancellationToken);

        Task<GatewayOutput> Deposit(BankAccount bankAccount, decimal amount, CancellationToken cancellationToken);
    }
}
