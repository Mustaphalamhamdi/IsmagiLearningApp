using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using IsmagiLearningApp.Models;

namespace IsmagiLearningApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ProgrammingLanguage> ProgrammingLanguages { get; set; }
        public DbSet<Difficulty> DifficultyLevels { get; set; }
        public DbSet<Level> Levels { get; set; }
        public DbSet<UserProgress> UserProgress { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("DefaultConnection", options =>
                {
                    options.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null);
                });
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Difficulty>()
                .HasOne(d => d.ProgrammingLanguage)
                .WithMany(p => p.DifficultyLevels)
                .HasForeignKey(d => d.ProgrammingLanguageId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Level>()
                .HasOne(l => l.ProgrammingLanguage)
                .WithMany(pl => pl.Levels)
                .HasForeignKey(l => l.ProgrammingLanguageId)
                .OnDelete(DeleteBehavior.Restrict); // Use Restrict to avoid cascade delete

            // Configure the relationship between Level and Difficulty
            modelBuilder.Entity<Level>()
                .HasOne(l => l.Difficulty)
                .WithMany(d => d.Levels)
                .HasForeignKey(l => l.DifficultyId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuring the one-to-many relationship between Difficulty and Level
            modelBuilder.Entity<Difficulty>()
                .HasMany(d => d.Levels)
                .WithOne(l => l.Difficulty)
                .HasForeignKey(l => l.DifficultyId)
                .IsRequired(false);  // Allow null values for DifficultyId

            modelBuilder.Entity<UserProgress>()
                .HasOne(up => up.Level)
                .WithMany()
                .HasForeignKey(up => up.LevelId);
        }
    }
}
