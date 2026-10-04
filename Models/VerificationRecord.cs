using System.ComponentModel.DataAnnotations;

namespace m_verify_BE.Models
{
    public class VerificationRecord
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required][MaxLength(50)]
        public string IdNumber { get; set; } = string.Empty;
        [Required][MaxLength(100)]
        public string FullName { get; set; } = string.Empty;
        [MaxLength(20)]
        public string Phone { get; set; } = string.Empty;
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;
        [MaxLength(50)]
        public string Status { get; set; } = string.Empty;
        [MaxLength(200)]
        public string Remarks { get; set; } = string.Empty;
        [MaxLength(50)]
        public string Batch { get; set; } = string.Empty;
        public DateTime? VerifiedAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
