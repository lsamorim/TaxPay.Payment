using Bogus;
using Domain.ScheduleAggregate;
using Domain.Seedwork;
using FluentAssertions;
using UnitTests.Common.Builders;

namespace UnitTests.DomainTests.ScheduleAggregate
{
    public partial class ScheduleTests
    {
        [Fact]
        public void Completed_schedule_cannot_be_canceled()
        {
            // Arrange
            var faker = new Faker();
            var scheduleDate = DateOnly.FromDateTime(faker.Date.Future());
            var bankAccount = new BankAccountBuilder().Build();

            var sut = new Schedule(scheduleDate, bankAccount);
            sut.Completed();

            // Act
            var act = () => sut.Canceled();

            // Assert
            act.Should().Throw<DomainException>();
        }

        [Fact]
        public void Cancel_a_schedule()
        {
            // Arrange
            var faker = new Faker();
            var scheduleDate = DateOnly.FromDateTime(faker.Date.Future());
            var bankAccount = new BankAccountBuilder().Build();

            var sut = new Schedule(scheduleDate, bankAccount);

            // Act
            sut.Canceled();

            // Assert
            sut.Status.Should().Be(ScheduleStatus.Canceled);
        }
    }
}
