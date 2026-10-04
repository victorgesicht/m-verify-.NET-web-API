using Microsoft.AspNetCore.Mvc;
using m_verify_BE.Data;
using Microsoft.EntityFrameworkCore;
using m_verify_BE.Models;

namespace m_verify_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("records")]
        public async Task<IActionResult> GetRecords()
        {
            var records = await _context.VerificationRecords.OrderByDescending(r => r.CreatedAt).ToListAsync();
            return Ok(records);
        }

        [HttpPost("records")]
        public async Task<IActionResult> CreateRecord([FromBody] VerificationRecord record)
        {
            record.Id = Guid.NewGuid();
            record.CreatedAt = DateTime.UtcNow;
            _context.VerificationRecords.Add(record);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetRecords), new { id = record.Id }, record);
        }

        [HttpPut("records/{id}")]
        public async Task<IActionResult> UpdateRecord(Guid id, [FromBody] VerificationRecord updated)
        {
            var existing = await _context.VerificationRecords.FindAsync(id);
            if (existing == null) return NotFound();
            existing.IdNumber = updated.IdNumber;
            existing.FullName = updated.FullName;
            existing.Phone = updated.Phone;
            existing.Email = updated.Email;
            existing.Status = updated.Status;
            existing.Remarks = updated.Remarks;
            existing.Batch = updated.Batch;
            existing.VerifiedAt = updated.VerifiedAt;
            await _context.SaveChangesAsync();
            return Ok(existing);
        }

        [HttpDelete("records/{id}")]
        public async Task<IActionResult> DeleteRecord(Guid id)
        {
            var existing = await _context.VerificationRecords.FindAsync(id);
            if (existing == null) return NotFound();
            _context.VerificationRecords.Remove(existing);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
