namespace Domain.Seedwork
{
    public interface IRepository
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
