using BMS_Logistics.Domain.Entities;

namespace BMS_Logistics.Application.Interfaces
{
    public interface IUserRepo : IGeneric<User>
    {
        Task<User?> GetByUserName(string userName);

        Task<string?> GetByUserRole(int userId);
    }
}
