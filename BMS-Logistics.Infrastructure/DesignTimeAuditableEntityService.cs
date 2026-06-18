using BMS_Logistics.API.Interfaces;
using BMS_Logistics.Domain.Entities;

namespace BMS_Logistics.Infrastructure
{
    internal class DesignTimeAuditableEntityService : IAuditableEntityService
    {
        public void ApplyAuditInfo(IEnumerable<SharedProperty> entities)
        {
            throw new NotImplementedException();
        }

        public string GetCurrentUserId() => "design-time-user";
    }
}
