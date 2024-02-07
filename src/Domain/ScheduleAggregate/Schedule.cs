﻿using Domain.ScheduleAggregate.ValueObjects;
using Domain.Seedwork;

namespace Domain.ScheduleAggregate
{
    public class Schedule : IEntity
    {
        public int Id { get; private set; }

        public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.UtcNow;

        public DateTimeOffset UpdatedAt { get; private set; } = DateTimeOffset.UtcNow;

        public DateOnly ScheduleDate { get; private set; }

        public BankAccount BankAccount { get; private set; }

        public decimal Amount { get; }

        public ScheduleStatus Status { get; private set; }

        private Schedule() { }

        public Schedule(DateOnly scheduleDate, BankAccount bankAccount)
        {
            ScheduleDate = scheduleDate;
            BankAccount = bankAccount;
            Status = ScheduleStatus.Scheduled;
        }
        public bool IsIn(params ScheduleStatus[] scheduleStatus) => scheduleStatus.Contains(Status);

        public void Failed(string comment)
        {
            if (IsIn(ScheduleStatus.Completed, ScheduleStatus.Failed))
                throw new DomainException($"Cannot change schedule status to {ScheduleStatus.Failed}. Schedule is already {Status}");

            //Comment = comment;
            Status = ScheduleStatus.Failed;
        }
    }
}