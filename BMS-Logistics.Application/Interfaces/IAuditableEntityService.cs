using BMS_Logistics.Application.Interfaces;
using BMS_Logistics.Domain.Entities;

namespace BMS_Logistics.API.Interfaces
{
    public interface IAuditableEntityService
    {
        void ApplyAuditInfo(IEnumerable<SharedProperty> entities);
    }
}
