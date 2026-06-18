namespace BMS_Logistics.Application.Interfaces
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(int userId, string username, IEnumerable<string> roles);
    }
}
