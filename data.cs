using Microsoft.EntityFrameworkCore;
using models;

namespace Practice.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Attempt> Attempts { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<Answer> Answers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Attempt>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.StartedAt)
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(e => e.Status)
                    .HasConversion<string>()
                    .HasMaxLength(20);

                entity.HasOne(a => a.Student)
                    .WithMany(s => s.Attempts)
                    .HasForeignKey(a => a.StudentId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(a => a.Test)
                    .WithMany(t => t.Attempts)
                    .HasForeignKey(a => a.TestId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Test>()
                .HasMany(t => t.AllowedStudents)
                .WithMany(s => s.AvailableTests)
                .UsingEntity<Dictionary<string, object>>(
                    "TestStudent",
                    j => j.HasOne<Student>().WithMany().HasForeignKey("StudentId"),
                    j => j.HasOne<Test>().WithMany().HasForeignKey("TestId"),
                    j => j.HasKey("TestId", "StudentId"));

            modelBuilder.Entity<Attempt>()
                .HasIndex(a => new { a.StudentId, a.TestId });

            modelBuilder.Entity<Attempt>()
                .HasIndex(a => a.Status);

            modelBuilder.Entity<Attempt>()
                .HasIndex(a => a.StartedAt);
        }
    }
}

