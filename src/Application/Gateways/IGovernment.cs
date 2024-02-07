using Application.Gateways.Dtos;
using Domain.ScheduleAggregate.ValueObjects;

namespace Application.Gateways
{
    public interface IGovernment
    {
        Task<GatewayOutput> Deposit(BankAccount fromBankAccount, decimal amount, CancellationToken cancellationToken);
    }
}
