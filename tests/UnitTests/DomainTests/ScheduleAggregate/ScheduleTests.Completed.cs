using Bogus;
using Domain.ScheduleAggregate;
using Domain.Seedwork;
using FluentAssertions;
using UnitTests.Common.Fakers;

namespace UnitTests.DomainTests.ScheduleAggregate
{
    public partial class ScheduleTests
    {
        [Fact]
        public void Canceled_schedule_cannot_be_completed()
        {
            // Arrange
            var faker = new Faker();
            var scheduleDate = faker.Date.FutureOffset();
            var bankAccount = new BankAccountFaker().Generate();

            var sut = new Schedule(scheduleDate, bankAccount);
            sut.Canceled();

            // Act
            var act = () => sut.Completed();

            // Assert
            act.Should().Throw<DomainException>();
        }

        [Fact]
        public void Complete_a_schedule()
        {
            // Arrange
            var faker = new Faker();
            var scheduleDate = faker.Date.FutureOffset();
            var bankAccount = new BankAccountFaker().Generate();

            var sut = new Schedule(scheduleDate, bankAccount);

            // Act
            sut.Completed();

            // Assert
            sut.Status.Should().Be(ScheduleStatus.Completed);
        }
    }
}
