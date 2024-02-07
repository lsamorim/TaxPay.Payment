using Application.Gateways;
using Application.Gateways.Dtos;
using Application.Services;
using Domain.ScheduleAggregate;
using Domain.ScheduleAggregate.ValueObjects;
using Domain.Seedwork;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReceivedExtensions;
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
            _scheduleRepository.UnitOfWork.Returns(Substitute.For<IUnitOfWork>());

            _sut = new ScheduleService(_bank, _government, _scheduleRepository);
        }

        private void SetupScheduleRepository(int count)
        {
            var schedules = new ScheduleFaker()
                .Generate(count);

            _scheduleRepository
                .GetSchedulesFor(default, CancellationToken.None)
                .ReturnsForAnyArgs(schedules);
        }

        [InlineData(1)]
        [InlineData(10)]
        [Theory]
        public async Task Settle_all_scheduled_payments(int schedulesCount)
        {
            // Arrrange
            SetupScheduleRepository(schedulesCount);

            _bank
                .Withdraw(default, default, default)
                .ReturnsForAnyArgs(GatewayOutput.Succeeded());

            _government
                .Deposit(default, default, default)
                .ReturnsForAnyArgs(GatewayOutput.Succeeded());

            // Act
            var output = await _sut.Execute(DateTimeOffset.UtcNow, CancellationToken.None);

            // Assert
            output.Schedules.Should().HaveCount(schedulesCount);
            output.Schedules
                .Where(schedule => schedule.Status == ScheduleStatus.Completed)
                .Should().HaveCount(schedulesCount);

            await _bank
                .ReceivedWithAnyArgs(schedulesCount)
                .Withdraw(default, default, default);

            await _government
                .ReceivedWithAnyArgs(schedulesCount)
                .Deposit(default, default, default);

            await _bank
                .DidNotReceiveWithAnyArgs()
                .Deposit(default, default, default);

            await _scheduleRepository.UnitOfWork
                .ReceivedWithAnyArgs(1)
                .SaveChanges(default);
        }

        [Fact]
        public async Task No_settlement_when_there_are_no_scheduled_payment_for_the_date()
        {
            // Act
            var output = await _sut.Execute(DateTimeOffset.UtcNow, CancellationToken.None);

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
            SetupScheduleRepository(schedulesCount);

            _bank
                .Withdraw(Arg.Is<BankAccount>(bankAccount => int.Parse(bankAccount.AccountNumber) <= schedulesWithInsuficientFunds), default, default)
                .Returns(GatewayOutput.Failed("Insufficient funds"));

            _bank
                .Withdraw(Arg.Is<BankAccount>(bankAccount => int.Parse(bankAccount.AccountNumber) > schedulesWithInsuficientFunds), default, default)
                .Returns(GatewayOutput.Succeeded());

            _government
                .Deposit(default, default, default)
                .ReturnsForAnyArgs(GatewayOutput.Succeeded());

            var expectedGovernmentDepositsCount = schedulesCount - schedulesWithInsuficientFunds;

            // Act
            var output = await _sut.Execute(DateTimeOffset.UtcNow, CancellationToken.None);

            // Assert
            output.Schedules.Should().HaveCount(schedulesCount);
            output.Schedules
                .Where(schedule => schedule.Status == ScheduleStatus.Failed)
                .Should().HaveCount(schedulesWithInsuficientFunds);

            await _bank
                .ReceivedWithAnyArgs(schedulesCount)
                .Withdraw(default, default, default);

            await _government
                .ReceivedWithAnyArgs(expectedGovernmentDepositsCount)
                .Deposit(default, default, default);

            await _bank
                .DidNotReceiveWithAnyArgs()
                .Deposit(default, default, default);

            await _scheduleRepository.UnitOfWork
                .ReceivedWithAnyArgs(1)
                .SaveChanges(default);
        }

        [InlineData(1, 1)]
        [InlineData(10, 5)]
        [InlineData(10, 10)]
        [Theory]
        public async Task Refund_scheduled_payments_when_refused_by_government(int schedulesCount, int schedulesRefusedByGovernment)
        {
            // Arrrange
            SetupScheduleRepository(schedulesCount);

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
            var output = await _sut.Execute(DateTimeOffset.UtcNow, CancellationToken.None);

            // Assert
            output.Schedules.Should().HaveCount(schedulesCount);
            output.Schedules
                .Where(schedule => schedule.Status == ScheduleStatus.Failed)
                .Should().HaveCount(schedulesRefusedByGovernment);

            await _bank
                .ReceivedWithAnyArgs(schedulesCount)
                .Withdraw(default, default, default);

            await _government
                .ReceivedWithAnyArgs(schedulesCount)
                .Deposit(default, default, default);

            await _bank
                .ReceivedWithAnyArgs(schedulesRefusedByGovernment)
                .Deposit(default, default, default);

            await _scheduleRepository.UnitOfWork
                .ReceivedWithAnyArgs(1)
                .SaveChanges(default);
        }

        [Fact]
        public async Task Refund_failure()
        {
            // Arrrange
            SetupScheduleRepository(1);

            _bank
                .Withdraw(default, default, default)
                .ReturnsForAnyArgs(GatewayOutput.Succeeded());

            _government
                .Deposit(default, default, default)
                .ReturnsForAnyArgs(GatewayOutput.Failed("Payment refused"));

            _bank
                .Deposit(default, default, default)
                .ReturnsForAnyArgs(GatewayOutput.Failed("Deposit failed"));

            // Act
            var output = await _sut.Execute(DateTimeOffset.UtcNow, CancellationToken.None);

            // Assert
            output.Schedules.Should().HaveCount(1);
            output.Schedules
                .Where(schedule => schedule.Status == ScheduleStatus.Failed)
                .Should().HaveCount(1);

            await _bank
                .ReceivedWithAnyArgs(1)
                .Withdraw(default, default, default);

            await _government
                .ReceivedWithAnyArgs(1)
                .Deposit(default, default, default);

            await _bank
                .ReceivedWithAnyArgs(1)
                .Deposit(default, default, default);

            await _scheduleRepository.UnitOfWork
                .ReceivedWithAnyArgs(1)
                .SaveChanges(default);
        }

        [Fact]
        public async Task Exception_thrown_during_settlement()
        {
            // Arrrange
            SetupScheduleRepository(1);

            _bank
                .Withdraw(default, default, default)
                .ThrowsAsync(new Exception());

            // Act
            var output = await _sut.Execute(DateTimeOffset.UtcNow, CancellationToken.None);

            // Assert
            output.Schedules.Should().HaveCount(1);
            output.Schedules
                .Where(schedule => schedule.Status == ScheduleStatus.Failed)
                .Should().HaveCount(1);

            await _scheduleRepository.UnitOfWork
                .ReceivedWithAnyArgs(1)
                .SaveChanges(default);
        }
    }
}
