using SmartMatrix.Domain.Interfaces.Services.Essential;

namespace SmartMatrix.Infrastructure.Services.Essential
{
    public class SystemDateTimeService : IDateTimeService
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}