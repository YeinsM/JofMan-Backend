using BMS_Logistics.Application.DTOs;
using BMS_Logistics.Application.Responses;
using BMS_Logistics.Domain.Entities;

namespace BMS_Logistics.Application.Interfaces
{
    public interface IUser
    {
        Task<LoginResponse> Login(string userName, string password);
        Task<User?> GetByUserName(string userName);
        Task<LoginResponse> ForgetPassword(string userName);
        Task<LoginResponse> ChangePassword(ChangePasswordDto userData);

        // REFRESH TOKEN METHODS    
        Task<RefreshToken> SaveRefreshToken(int userId, string refreshToken, DateTime expiryDate);
        Task<User?> ValidateRefreshToken(string refreshToken);
        Task<LoginResponse> GenerateTokenForUser(User user);
    }
}
