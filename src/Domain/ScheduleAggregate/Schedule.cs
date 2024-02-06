using Domain.ScheduleAggregate.ValueObjects;
using Domain.Seedwork;

namespace Domain.ScheduleAggregate
{
    public class Schedule : Entity
    {
        public DateOnly ScheduleDate { get; private set; }

        public BankAccount BankAccount { get; private set; }

        public decimal Amount { get; private set; }

        public ScheduleStatus Status { get; private set; }

        public Schedule(DateOnly scheduleDate, BankAccount bankAccount)
        {
            ScheduleDate = scheduleDate;
            BankAccount = bankAccount;
            Status = ScheduleStatus.Scheduled;
        }
    }
}
