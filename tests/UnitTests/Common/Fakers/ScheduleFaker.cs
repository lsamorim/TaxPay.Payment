using Bogus;
using Domain.ScheduleAggregate;
using UnitTests.Common.Builders;

namespace UnitTests.Common.Fakers
{
    public class ScheduleFaker : Faker<Schedule>
    {
        public ScheduleFaker()
        {
            base.CustomInstantiator(faker => 
                new Schedule(
                    DateOnly
                        .FromDateTime(faker.Date.Future()),
                    new BankAccountBuilder()
                        .WithAccountNumber((faker.IndexFaker + 1))
                        .Build()
                ));
            RuleFor(schedule => schedule.Id, faker => faker.IndexFaker + 1);
        }
    }
}
