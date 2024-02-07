using Bogus;
using Domain.ScheduleAggregate;

namespace UnitTests.Common.Fakers
{
    public class ScheduleFaker : Faker<Schedule>
    {
        public ScheduleFaker()
        {
            base.CustomInstantiator(faker => 
                new Schedule(
                    faker.Date.FutureOffset(),
                    new BankAccountFaker()
                        .WithAccountNumber((faker.IndexFaker + 1))
                        .Generate()
                ));
        }
    }
}
