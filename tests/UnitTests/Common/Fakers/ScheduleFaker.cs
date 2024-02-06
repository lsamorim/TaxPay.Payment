using Bogus;
using Domain.ScheduleAggregate;

namespace UnitTests.Common.Fakers
{
    public class ScheduleFaker : Faker<Schedule>
    {
        public ScheduleFaker()
        {
            RuleFor(schedule => schedule.Id, faker => faker.IndexFaker + 1);
            RuleFor(schedule => schedule.Status, faker => ScheduleStatus.Scheduled);
            RuleFor(schedule => schedule.BankAccount, new BankAccountFaker().Generate());
            RuleFor(schedule => schedule.ScheduleDate, faker => DateOnly.FromDateTime(faker.Date.Future()));
        }
    }
}
