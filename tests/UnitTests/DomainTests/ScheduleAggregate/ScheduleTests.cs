using Bogus;
using Domain.ScheduleAggregate;
using FluentAssertions;
using UnitTests.Common.Builders;

namespace UnitTests.DomainTests.ScheduleAggregate
{
    public partial class ScheduleTests
    {
        [Fact]
        public void Create_a_valid_schedule()
        {
            // Arrange
            var faker = new Faker();
            var scheduleDate = DateOnly.FromDateTime(faker.Date.Future());
            var bankAccount = new BankAccountBuilder().Build();

            // Act
            var sut = new Schedule(scheduleDate, bankAccount);

            // Assert
            sut.Status.Should().Be(ScheduleStatus.Scheduled);
        }
    }
}
