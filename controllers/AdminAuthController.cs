using Microsoft.AspNetCore.Mvc;

namespace m_verify_BE.Controllers
{
    [Route("api/admin/auth")]
    [ApiController]
    public class AdminAuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        public AdminAuthController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost("verify")]
        public IActionResult Verify([FromBody] AdminLoginDto login)
        {
            var key = _config.GetValue<string>("ApiKey");
            if (!string.IsNullOrWhiteSpace(key) && key.Equals(login.ApiKey))
                return Ok(new { valid = true });
            return Unauthorized(new { valid = false });
        }
    }

    public class AdminLoginDto
    {
        public string ApiKey { get; set; } = string.Empty;
    }
}
