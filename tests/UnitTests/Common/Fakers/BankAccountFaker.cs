using Bogus;
using Domain.ScheduleAggregate.ValueObjects;

namespace UnitTests.Common.Fakers
{
    public class BankAccountFaker : Faker<BankAccount>
    {
        public BankAccountFaker()
        {
            var institutionNumber = FakerHub.Random.Int(1, 100).ToString().PadLeft(3, '0');
            var branchNumber = FakerHub.Random.Int(1, 99999).ToString().PadLeft(5, '0');
            var accountNumber = FakerHub.Random.Int(1, 9999999).ToString().PadLeft(11, '0');

            CustomInstantiator(faker => new BankAccount(institutionNumber, branchNumber, accountNumber));
        }

        public BankAccountFaker WithAccountNumber(int accountNumber)
        {
            RuleFor(bankAccount => bankAccount.AccountNumber, accountNumber.ToString().PadLeft(11, '0'));
            return this;
        }
    }
}
