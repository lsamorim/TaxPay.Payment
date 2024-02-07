using Bogus;
using Domain.ScheduleAggregate.ValueObjects;

namespace UnitTests.Common.Builders
{
    public class BankAccountBuilder
    {
        private readonly Faker _faker = new Faker();

        public string? _institutionNumber;
        public string? _branchNumber;
        public string? _accountNumber;

        public BankAccountBuilder WithAccountNumber(int accountNumber)
        {
            _accountNumber = accountNumber.ToString().PadLeft(11, '0');
            return this;
        }

        public BankAccount Build()
        {
            _institutionNumber ??= _faker.Random.Int(1, 100).ToString().PadLeft(3, '0');
            _branchNumber ??= _faker.Random.Int(1, 99999).ToString().PadLeft(5, '0');
            _accountNumber ??= _faker.Random.Int(1, 9999999).ToString().PadLeft(11, '0');

            return new BankAccount(_institutionNumber, _branchNumber, _accountNumber);
        }
    }
}
