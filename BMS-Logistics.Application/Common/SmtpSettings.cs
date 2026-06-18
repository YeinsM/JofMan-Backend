namespace BMS_Logistics.Application.Common
{
    public class SmtpSettings
    {
        public string? FromMail { get; set; }
        public string? Password { get; set; }
        public string? FromAlias { get; set; }
        public string? ToCopy { get; set; }
        public int SmtpPort { get; set; }
        public string? SmtpHost { get; set; }
    }
}
