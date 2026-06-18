namespace BMS_Logistics.Application.DTOs
{
    public class VerificationEmailDto
    {
        public UserDto? User { get; set; }
        public string? Code { get; set; }
    }
}
