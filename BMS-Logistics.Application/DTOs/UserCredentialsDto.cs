using System.ComponentModel.DataAnnotations;

namespace BMS_Logistics.Application.DTOs
{
    public class UserCredentialsDto
    {
        public string? Username { get; set; }
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}
