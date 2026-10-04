using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using m_verify_BE.Data;
using m_verify_BE.Models;

namespace m_verify_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    [EnableRateLimiting("admin")]
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
            var records = await _context.VerificationRecords
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
            return Ok(records);
        }

        [HttpGet("records/{id}")]
        public async Task<IActionResult> GetRecord(Guid id)
        {
            var record = await _context.VerificationRecords.FindAsync(id);
            if (record == null) return NotFound();
            return Ok(record);
        }

        [HttpPost("records")]
        public async Task<IActionResult> Create([FromBody] VerificationRecord record)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            record.Id = Guid.NewGuid();
            record.CreatedAt = DateTime.UtcNow;
            record.VerifiedAt = null;

            _context.VerificationRecords.Add(record);

            var idNumberTaken = await _context.VerificationRecords
                .AnyAsync(r => r.IdNumber.ToLower() == record.IdNumber.ToLower());
            if (idNumberTaken)
            {
                return Conflict(new { error = $"IdNumber '{record.IdNumber}' already exists" });
            }

            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetRecord), new { id = record.Id }, record);
        }

        [HttpPut("records/{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] VerificationRecord updated)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existing = await _context.VerificationRecords.FindAsync(id);
            if (existing == null) return NotFound();

            var duplicate = await _context.VerificationRecords
                .AnyAsync(r => r.IdNumber.ToLower() == updated.IdNumber.ToLower() && r.Id != id);
            if (duplicate)
            {
                return Conflict(new { error = $"IdNumber '{updated.IdNumber}' already exists" });
            }

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
        public async Task<IActionResult> Delete(Guid id)
        {
            var existing = await _context.VerificationRecords.FindAsync(id);
            if (existing == null) return NotFound();
            _context.VerificationRecords.Remove(existing);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
