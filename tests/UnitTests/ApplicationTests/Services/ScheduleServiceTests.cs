using Application.Gateways;
using Application.Gateways.Dtos;
using Application.Services;
using Domain.ScheduleAggregate;
using Domain.ScheduleAggregate.ValueObjects;
using FluentAssertions;
using NSubstitute;
using UnitTests.Common.Fakers;

namespace UnitTests.ApplicationTests.Services
{
    public class ScheduleServiceTests
    {
        private readonly IBank _bank;
        private readonly IGovernment _government;
        private readonly IScheduleRepository _scheduleRepository;

        private readonly ScheduleService _sut;

        public ScheduleServiceTests()
        {
            _bank = Substitute.For<IBank>();
            _government = Substitute.For<IGovernment>();
            _scheduleRepository = Substitute.For<IScheduleRepository>();

            _sut = new ScheduleService(_bank, _government, _scheduleRepository);
        }

        [InlineData(1)]
        [InlineData(10)]
        [Theory]
        public async Task Settle_all_scheduled_payments(int schedulesCount)
        {
            // Arrrange
            var schedules = new ScheduleFaker()
                .Generate(schedulesCount);

            _scheduleRepository
                .GetSchedulesFor(default, default)
                .ReturnsForAnyArgs(schedules);

            _bank
                .Withdraw(default, default, default)
                .ReturnsForAnyArgs(GatewayOutput.Succeeded());

            _government
                .Deposit(default, default, default)
                .ReturnsForAnyArgs(GatewayOutput.Succeeded());

            // Act
            var output = await _sut.Execute(DateOnly.FromDateTime(DateTime.Now.Date), CancellationToken.None);

            // Assert
            output.Schedules.Should().HaveCount(schedulesCount);
            output.Schedules
                .Where(schedule => schedule.Status == ScheduleStatus.Completed)
                .Should().HaveCount(schedulesCount);
        }

        [Fact]
        public async Task No_settlement_when_there_are_no_scheduled_payment_for_the_date()
        {
            // Act
            var output = await _sut.Execute(DateOnly.FromDateTime(DateTime.Now.Date), CancellationToken.None);

            // Assert
            output.Schedules.Should().BeEmpty();
        }

        [InlineData(1, 1)]
        [InlineData(10, 5)]
        [InlineData(10, 10)]
        [Theory]
        public async Task Do_not_settle_scheduled_payments_with_insufficient_funds(int schedulesCount, int schedulesWithInsuficientFunds)
        {
            // Arrrange
            var schedules = new ScheduleFaker()
                .Generate(schedulesCount);

            _scheduleRepository
                .GetSchedulesFor(default, CancellationToken.None)
                .ReturnsForAnyArgs(schedules);

            _bank
                .Withdraw(Arg.Is<BankAccount>(bankAccount => int.Parse(bankAccount.AccountNumber) <= schedulesWithInsuficientFunds), default, default)
                .Returns(GatewayOutput.Failed("Insufficient funds"));

            _bank
                .Withdraw(Arg.Is<BankAccount>(bankAccount => int.Parse(bankAccount.AccountNumber) > schedulesWithInsuficientFunds), default, default)
                .Returns(GatewayOutput.Succeeded());

            _government
                .Deposit(default, default, default)
                .ReturnsForAnyArgs(GatewayOutput.Succeeded());

            // Act
            var output = await _sut.Execute(DateOnly.FromDateTime(DateTime.Now.Date), CancellationToken.None);

            // Assert
            output.Schedules.Should().HaveCount(schedulesCount);
            output.Schedules
                .Where(schedule => schedule.Status == ScheduleStatus.Failed)
                .Should().HaveCount(schedulesWithInsuficientFunds);
        }

        [InlineData(1, 1)]
        [InlineData(10, 5)]
        [InlineData(10, 10)]
        [Theory]
        public async Task Do_not_settle_scheduled_payments_when_refused_by_government(int schedulesCount, int schedulesRefusedByGovernment)
        {
            // Arrrange
            var schedules = new ScheduleFaker()
                .Generate(schedulesCount);

            _scheduleRepository
                .GetSchedulesFor(default, CancellationToken.None)
                .ReturnsForAnyArgs(schedules);

            _bank
                .Withdraw(default, default, default)
                .ReturnsForAnyArgs(GatewayOutput.Succeeded());

            _government
                .Deposit(Arg.Is<BankAccount>(bankAccount => int.Parse(bankAccount.AccountNumber) <= schedulesRefusedByGovernment), default, default)
                .Returns(GatewayOutput.Failed("Payment refused"));

            _government
                .Deposit(Arg.Is<BankAccount>(bankAccount => int.Parse(bankAccount.AccountNumber) > schedulesRefusedByGovernment), default, default)
                .Returns(GatewayOutput.Succeeded());

            // Act
            var output = await _sut.Execute(DateOnly.FromDateTime(DateTime.Now.Date), CancellationToken.None);

            // Assert
            output.Schedules.Should().HaveCount(schedulesCount);
            output.Schedules
                .Where(schedule => schedule.Status == ScheduleStatus.Failed)
                .Should().HaveCount(schedulesRefusedByGovernment);
        }
    }
}
