namespace SmartMatrix.Application.Interfaces.Services.Essential
{
    public interface IDateTimeService : IService
    {
        DateTime UtcNow { get; }
    }
}