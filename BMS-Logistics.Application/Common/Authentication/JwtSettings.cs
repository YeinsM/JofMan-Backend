namespace BMS_Logistics.Application.Common.Authentication
{
    public class JwtSettings
    {
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        // Clave simétrica usada para firmar tokens (cargar desde secretos en producción)
        public string SecretKey { get; set; } = string.Empty;

        // Tiempo de expiración en minutos
        public int ExpireMinutes { get; set; } = 60;
    }
}
