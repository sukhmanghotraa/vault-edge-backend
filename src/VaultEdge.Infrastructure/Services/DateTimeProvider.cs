using VaultEdge.Application.Common.Interfaces.Services;

namespace VaultEdge.Infrastructure.Services
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}