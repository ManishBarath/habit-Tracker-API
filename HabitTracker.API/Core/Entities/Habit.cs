using System;
using System.Collections.Generic;

namespace HabitTracker.API.Core.Entities
{
    public class Habit
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid? GoalId { get; set; }
        
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        
        public string Type { get; set; } = "Positive"; // Positive, Negative
        public string Frequency { get; set; } = "Daily"; // Daily, Weekly, Monthly
        
        public Guid? ReplacementHabitId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Streaks Tracking
        public int CurrentStreak { get; set; }
        public int LongestStreak { get; set; }

        // Navigation
        public User? User { get; set; }
        public Goal? Goal { get; set; }
        public Habit? ReplacementHabit { get; set; }
        public ICollection<ScorecardLog> Logs { get; set; } = new List<ScorecardLog>();
    }
}
