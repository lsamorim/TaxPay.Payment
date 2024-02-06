using Domain.ScheduleAggregate.ValueObjects;

namespace Domain.ScheduleAggregate
{
    public class Schedule
    {
        public DateOnly ScheduleDate { get; private set; }

        public BankAccount BankAccount { get; private set; }

        public decimal Amount { get; private set; }

        public Schedule(DateOnly scheduleDate, BankAccount bankAccount)
        {
            ScheduleDate = scheduleDate;
            BankAccount = bankAccount;
        }
    }
}
