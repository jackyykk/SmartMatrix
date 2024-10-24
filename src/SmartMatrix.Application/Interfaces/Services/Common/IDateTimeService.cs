namespace SmartMatrix.Application.Interfaces.Services.Common
{
    public interface IDateTimeService : IService
    {
        DateTime NowUtc { get; }
    }
}