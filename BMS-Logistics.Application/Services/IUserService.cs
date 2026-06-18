using BMS_Logistics.Application.DTOs;

namespace BMS_Logistics.Application.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllAsync();

        Task<UserDto?> GetByIdAsync(int id);

        Task<UserDto> CreateAsync(UserCreateDto dto);

        Task<bool> UpdateAsync(int id, UserCreateDto dto);

        Task<bool> DeleteAsync(int id);

        Task<bool> ActivateAsync(int id);
        Task<bool> UnlockAsync(int id);
    }
}
