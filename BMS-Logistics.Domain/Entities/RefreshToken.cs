namespace BMS_Logistics.Domain.Entities
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public string? Token { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string? DeviceInfo { get; set; }

        public bool Used { get; set; } = false;
    }
}
