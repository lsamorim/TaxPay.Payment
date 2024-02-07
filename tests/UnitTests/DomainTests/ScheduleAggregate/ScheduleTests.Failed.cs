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
        public void Canceled_schedule_cannot_fail_anymore()
        {
            // Arrange
            var faker = new Faker();
            var scheduleDate = DateOnly.FromDateTime(faker.Date.Future());
            var bankAccount = new BankAccountBuilder().Build();

            var sut = new Schedule(scheduleDate, bankAccount);
            sut.Canceled();

            // Act
            var act = () => sut.Failed("Failed");

            // Assert
            act.Should().Throw<DomainException>();
        }

        [Fact]
        public void Completed_schedule_cannot_fail_anymore()
        {
            // Arrange
            var faker = new Faker();
            var scheduleDate = DateOnly.FromDateTime(faker.Date.Future());
            var bankAccount = new BankAccountBuilder().Build();

            var sut = new Schedule(scheduleDate, bankAccount);
            sut.Completed();

            // Act
            var act = () => sut.Failed("Failed");

            // Assert
            act.Should().Throw<DomainException>();
        }

        [Fact]
        public void Schedule_failed()
        {
            // Arrange
            var faker = new Faker();
            var scheduleDate = DateOnly.FromDateTime(faker.Date.Future());
            var bankAccount = new BankAccountBuilder().Build();

            var comment = "Failed";
            var sut = new Schedule(scheduleDate, bankAccount);

            // Act
            sut.Failed(comment);

            // Assert
            sut.Status.Should().Be(ScheduleStatus.Failed);
            sut.Comment.Should().Be(comment);
        }
    }
}
