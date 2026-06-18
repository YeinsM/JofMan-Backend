using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BMS_Logistics.Domain.Entities
{
    public class SharedProperty
    {
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        [MaxLength(50)]
        public string? CreatedBy { get; set; }

        [MaxLength(50)]
        public string? UpdatedBy { get; set; }

        public void SetCreated(string? username)
        {
            CreatedAt = DateTime.UtcNow;
            CreatedBy = username;
        }

        public void SetUpdated(string? username)
        {
            UpdatedAt = DateTime.UtcNow;
            UpdatedBy = username;
        }
    }
}
