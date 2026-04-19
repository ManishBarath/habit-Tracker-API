using System;
using System.Collections.Generic;

namespace HabitTracker.API.Core.Entities
{
    public class Goal
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? MotivationWhy { get; set; }
        public string TermType { get; set; } = "ShortTerm"; // ShortTerm, LongTerm
        public DateTime? TargetDate { get; set; }
        public string Status { get; set; } = "Active"; // Active, Completed, Abandoned
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public User? User { get; set; }
        public ICollection<Habit> Habits { get; set; } = new List<Habit>();
    }
}
