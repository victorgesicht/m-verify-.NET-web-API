using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using m_verify_BE.Data;
using m_verify_BE.DTOs;

namespace m_verify_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VerifyController : ControllerBase
    {
        private const int MaxQueryLength = 320;

        private readonly ApplicationDbContext _context;
        public VerifyController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("health")]
        [AllowAnonymous]
        public IActionResult Health()
        {
            return Ok(new { status = "ok", timestamp = DateTime.UtcNow });
        }

        [HttpPost("search")]
        [AllowAnonymous]
        [EnableRateLimiting("search")]
        public async Task<IActionResult> Search([FromBody] VerifyRequestDto request)
        {
            var q = request.Query?.Trim() ?? string.Empty;
            if (string.IsNullOrEmpty(q))
            {
                return BadRequest(new { error = "Query is required" });
            }

            if (q.Length > MaxQueryLength)
            {
                return BadRequest(new { error = $"Query must be {MaxQueryLength} characters or fewer" });
            }

            var record = await _context.VerificationRecords
                .Where(r =>
                    r.IdNumber.ToLower() == q.ToLower() ||
                    r.Email.ToLower() == q.ToLower() ||
                    r.Phone.Contains(q) ||
                    r.FullName.ToLower().Contains(q.ToLower()))
                .Select(r => new { r.IdNumber, r.FullName, r.Status, r.Phone, r.Email, r.Remarks, r.VerifiedAt, r.CreatedAt })
                .FirstOrDefaultAsync();

            if (record == null)
            {
                return Ok(new VerifyResponseDto { Found = false, Query = q });
            }

            return Ok(new VerifyResponseDto
            {
                Found = true,
                Query = q,
                IdNumber = record.IdNumber,
                FullName = record.FullName,
                Phone = record.Phone,
                Email = record.Email,
                Status = record.Status,
                Remarks = record.Remarks,
                VerifiedAt = record.VerifiedAt,
                CreatedAt = record.CreatedAt
            });
        }

        [HttpGet("search")]
        [AllowAnonymous]
        [EnableRateLimiting("search")]
        public async Task<IActionResult> SearchGet([FromQuery] string query)
        {
            return await Search(new VerifyRequestDto { Query = query ?? string.Empty });
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var records = await _context.VerificationRecords
                .OrderBy(r => r.CreatedAt)
                .ToListAsync();
            return Ok(records);
        }

        [HttpPost("seed")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Seed()
        {
            var count = await _context.VerificationRecords.CountAsync();
            return Ok(new { message = count > 0 ? "Already seeded" : "Already seeded", count });
        }
    }
}
