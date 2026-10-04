using m_verify_BE.Models;
using Microsoft.EntityFrameworkCore;

namespace m_verify_BE.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<VerificationRecord> VerificationRecords { get; set; }
    }
}
