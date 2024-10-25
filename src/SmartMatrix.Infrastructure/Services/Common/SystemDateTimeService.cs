using SmartMatrix.Application.Interfaces.Services.Common;

namespace SmartMatrix.Infrastructure.Services.Common
{
    public class SystemDateTimeService : IDateTimeService
    {
        public DateTime NowUtc => DateTime.UtcNow;
    }
}