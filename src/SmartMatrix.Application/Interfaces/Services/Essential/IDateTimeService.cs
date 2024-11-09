namespace SmartMatrix.Domain.Interfaces.Services.Essential
{
    public interface IDateTimeService : IService
    {
        DateTime UtcNow { get; }
    }
}