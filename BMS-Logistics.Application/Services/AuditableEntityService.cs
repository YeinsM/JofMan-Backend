using BMS_Logistics.API.Interfaces;
using BMS_Logistics.Application.Interfaces;
using BMS_Logistics.Domain.Entities;

namespace BMS_Logistics.Application.Services
{
    public class AuditableEntityService : IAuditableEntityService
    {
        private readonly ICurrentUserContext _currentUserService;

        public AuditableEntityService(ICurrentUserContext currentUserService)
        {
            _currentUserService = currentUserService;
        }

        public void ApplyAuditInfo(IEnumerable<SharedProperty> entities)
        {
            var username = _currentUserService.UserName;

            foreach (var entity in entities)
            {
                if (entity.CreatedAt == default)
                    entity.SetCreated(username);
                else
                    entity.SetUpdated(username);
            }
        }
    }
}
