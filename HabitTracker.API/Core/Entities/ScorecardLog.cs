using System;

namespace HabitTracker.API.Core.Entities
{
    public class ScorecardLog
    {
        public Guid Id { get; set; }
        public Guid HabitId { get; set; }
        public Guid UserId { get; set; }
        
        public DateTime LogDate { get; set; } // The day/week/month representing the log
        
        public string CompletionStatus { get; set; } = "Completed"; // Completed, Skipped, Failed
        public decimal? MetricValue { get; set; } // useful if habit requires a quantity (e.g. 5 miles)
        public string? ContextNotes { get; set; } // Reflection, why it was failed
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public Habit? Habit { get; set; }
        public User? User { get; set; }
    }
}
