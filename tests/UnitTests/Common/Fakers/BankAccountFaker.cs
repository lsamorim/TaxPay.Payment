using Bogus;
using Domain.ScheduleAggregate.ValueObjects;

namespace UnitTests.Common.Fakers
{
    public class BankAccountFaker
    {
        private static Faker _faker = new Faker();

        public List<BankAccount> Generate(int count)
        {
            return Enumerable.Range(0, count).Select(_ => Generate()).ToList();
        }

        public BankAccount Generate()
        {
            var institutionNumber = _faker.Random.Int(1, 100).ToString().PadLeft(3, '0');
            var branchNumber = _faker.Random.Int(1, 99999).ToString().PadLeft(5, '0');
            var accountNumber = _faker.Random.Int(1, 9999999).ToString().PadLeft(11, '0');
            return new BankAccount(institutionNumber, branchNumber, accountNumber);
        }
    }
}
