using Microsoft.AspNetCore.Mvc;
using m_verify_BE.Data;
using m_verify_BE.DTOs;
using Microsoft.EntityFrameworkCore;

namespace m_verify_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VerifyController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public VerifyController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("search")]
        public async Task<IActionResult> Search([FromBody] VerifyRequestDto request)
        {
            var q = request.Query?.Trim() ?? string.Empty;
            if (string.IsNullOrEmpty(q))
                return BadRequest(new { error = "Query is required" });

            var query = _context.VerificationRecords.AsQueryable();
            var record = await query.FirstOrDefaultAsync(r =>
                r.IdNumber.ToLower() == q.ToLower() ||
                r.Phone.Contains(q) ||
                r.Email.ToLower() == q.ToLower() ||
                r.FullName.ToLower().Contains(q.ToLower()));

            if (record == null)
                return Ok(new { found = false, query = q });

            return Ok(new VerifyResultDto
            {
                Found = true,
                IdNumber = record.IdNumber,
                FullName = record.FullName,
                Phone = record.Phone,
                Email = record.Email,
                Status = record.Status,
                Remarks = record.Remarks,
                VerifiedAt = record.VerifiedAt
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var records = await _context.VerificationRecords.OrderBy(r => r.CreatedAt).ToListAsync();
            return Ok(records);
        }

        [HttpPost("seed")]
        public async Task<IActionResult> Seed()
        {
            if (await _context.VerificationRecords.AnyAsync())
                return Ok(new { message = "Already seeded", count = await _context.VerificationRecords.CountAsync() });

            var seed = new List<m_verify_BE.Models.VerificationRecord>
            {
                new() { IdNumber="TEST001", FullName="John Mwangi", Phone="254700000001", Email="john.mwangi@test.ke", Status="Verified", Remarks="Test 1", Batch="TEST" },
                new() { IdNumber="TEST002", FullName="Mary Wanjiru", Phone="254700000002", Email="mary.wanjiru@test.ke", Status="Pending", Remarks="Test 2", Batch="TEST" },
                new() { IdNumber="TEST003", FullName="Peter Otieno", Phone="254700000003", Email="peter.otieno@test.ke", Status="Verified", Remarks="Test 3", Batch="TEST" },
                new() { IdNumber="TEST004", FullName="Grace Achieng", Phone="254700000004", Email="grace.achieng@test.ke", Status="Invalid", Remarks="Test 4", Batch="TEST" },
                new() { IdNumber="TEST005", FullName="James Kiptoo", Phone="254700000005", Email="james.kiptoo@test.ke", Status="Verified", Remarks="Test 5", Batch="TEST" }
            };
            await _context.VerificationRecords.AddRangeAsync(seed);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Seeded 5 records", count = seed.Count });
        }
    }
}
