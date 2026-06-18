namespace BMS_Logistics.Application.DTOs
{
    public class ForgetPasswordEmailDto
    {
        public UserDto User { get; set; } = null!;
        public string Code { get; set; } = string.Empty;
    }
}
