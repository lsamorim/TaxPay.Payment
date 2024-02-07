namespace Domain.Seedwork
{
    public interface IUnitOfWork
    {
        Task<int> SaveChanges(CancellationToken cancellationToken);
    }
}
