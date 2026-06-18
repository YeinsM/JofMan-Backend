namespace BMS_Logistics.Application.Responses
{
    public class RefreshTokenResponse
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public string? Token { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string? DeviceInfo { get; set; }

        public bool Used { get; set; } = false;
    }
}
