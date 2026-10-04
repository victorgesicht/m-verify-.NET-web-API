using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using m_verify_BE.Models;

namespace m_verify_BE.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<VerificationRecord> VerificationRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<VerificationRecord>(entity =>
            {
                entity.HasIndex(r => r.IdNumber).IsUnique();
                entity.HasIndex(r => r.Email);
                entity.HasIndex(r => r.Phone);
            });
        }
    }
}
