using HabitTracker.API.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace HabitTracker.API.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Goal> Goals { get; set; } = null!;
        public DbSet<Habit> Habits { get; set; } = null!;
        public DbSet<ScorecardLog> ScorecardLogs { get; set; } = null!;
        public DbSet<JournalEntry> JournalEntries { get; set; } = null!;
        public DbSet<WhiteboardNote> WhiteboardNotes { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Goals
            modelBuilder.Entity<Goal>()
                .HasOne(g => g.User)
                .WithMany(u => u.Goals)
                .HasForeignKey(g => g.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Habits
            modelBuilder.Entity<Habit>()
                .HasOne(h => h.User)
                .WithMany(u => u.Habits)
                .HasForeignKey(h => h.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Habit>()
                .HasOne(h => h.Goal)
                .WithMany(g => g.Habits)
                .HasForeignKey(h => h.GoalId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Habit>()
                .HasOne(h => h.ReplacementHabit)
                .WithMany()
                .HasForeignKey(h => h.ReplacementHabitId)
                .OnDelete(DeleteBehavior.SetNull);

            // Logs
            modelBuilder.Entity<ScorecardLog>()
                .HasOne(s => s.Habit)
                .WithMany(h => h.Logs)
                .HasForeignKey(s => s.HabitId)
                .OnDelete(DeleteBehavior.Cascade);
                
            modelBuilder.Entity<ScorecardLog>()
                .HasOne(s => s.User)
                .WithMany()
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
