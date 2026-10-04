using m_verify_BE.Models;
using Microsoft.EntityFrameworkCore;

namespace m_verify_BE.Data
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            if (await context.VerificationRecords.AnyAsync())
                return;

            var records = new List<VerificationRecord>
            {
                new() { IdNumber = "TEST001", FullName = "John Mwangi", Phone = "254700000001", Email = "john.mwangi@test.ke", Status = "Verified", Remarks = "Test record 1", Batch = "TEST", VerifiedAt = DateTime.UtcNow.AddDays(-10) },
                new() { IdNumber = "TEST002", FullName = "Mary Wanjiru", Phone = "254700000002", Email = "mary.wanjiru@test.ke", Status = "Pending", Remarks = "Test record 2", Batch = "TEST", VerifiedAt = null },
                new() { IdNumber = "TEST003", FullName = "Peter Otieno", Phone = "254700000003", Email = "peter.otieno@test.ke", Status = "Verified", Remarks = "Test record 3", Batch = "TEST", VerifiedAt = DateTime.UtcNow.AddDays(-5) },
                new() { IdNumber = "TEST004", FullName = "Grace Achieng", Phone = "254700000004", Email = "grace.achieng@test.ke", Status = "Invalid", Remarks = "Test record 4", Batch = "TEST", VerifiedAt = null },
                new() { IdNumber = "TEST005", FullName = "James Kiptoo", Phone = "254700000005", Email = "james.kiptoo@test.ke", Status = "Verified", Remarks = "Test record 5", Batch = "TEST", VerifiedAt = DateTime.UtcNow.AddDays(-2) }
            };

            await context.VerificationRecords.AddRangeAsync(records);
            await context.SaveChangesAsync();
        }
    }
}
