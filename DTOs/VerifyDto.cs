namespace m_verify_BE.DTOs
{
    public class VerifyRequestDto
    {
        public string Query { get; set; } = string.Empty;
        public string Type { get; set; } = "auto";
    }

    public class VerifyResultDto
    {
        public bool Found { get; set; }
        public string IdNumber { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Remarks { get; set; } = string.Empty;
        public DateTime? VerifiedAt { get; set; }
    }
}
