namespace Domain.ScheduleAggregate.ValueObjects
{
    public class BankAccount
    {
        public string InstitutionNumber { get; private set; }
        public string BranchNumber { get; private set; }
        public string AccountNumber { get; private set; }

        public BankAccount(string institutionNumber, string branchNumber, string accountNumber)
        {
            InstitutionNumber = institutionNumber;
            BranchNumber = branchNumber;
            AccountNumber = accountNumber;
        }
    }
}
