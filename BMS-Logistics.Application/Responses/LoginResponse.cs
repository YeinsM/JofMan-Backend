using BMS_Logistics.Application.DTOs;

namespace BMS_Logistics.Application.Responses
{
    public partial class LoginResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public UserDto? User { get; set; }

        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
    }
}
