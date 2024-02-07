using Domain.ScheduleAggregate;
using Domain.ScheduleAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.SqlServer.Configurations.ScheduleAggregate
{
    public class ScheduleConfiguration : IEntityTypeConfiguration<Schedule>
    {
        public void Configure(EntityTypeBuilder<Schedule> builder)
        {
            builder.OwnsOne(e => e.BankAccount, e =>
            {
                e.Property(b => b.InstitutionNumber).HasColumnName(nameof(BankAccount.InstitutionNumber));
                e.Property(b => b.BranchNumber).HasColumnName(nameof(BankAccount.BranchNumber));
                e.Property(b => b.AccountNumber).HasColumnName(nameof(BankAccount.AccountNumber));
            });

            //builder.Property(e => e.ScheduleDate).HasConversion(from => from.ToDateTime())
        }
    }
}
