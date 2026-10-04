namespace m_verify_BE.DTOs
{
    public class VerifyRequestDto
    {
        public string Query { get; set; } = string.Empty;
        public string Type { get; set; } = "auto";
    }

    public class VerifyResponseDto
    {
        public bool Found { get; set; }
        public string Query { get; set; } = string.Empty;
        public string? IdNumber { get; set; }
        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Status { get; set; }
        public string? Remarks { get; set; }
        public DateTime? VerifiedAt { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}