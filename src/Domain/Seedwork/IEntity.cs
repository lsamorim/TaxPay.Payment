namespace Domain.Seedwork
{
    public interface IEntity
    {
        public int Id { get; }

        public DateTimeOffset CreatedAt { get; }

        public DateTimeOffset UpdatedAt { get; }
    }
}
