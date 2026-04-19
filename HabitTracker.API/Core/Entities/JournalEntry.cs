using System;

namespace HabitTracker.API.Core.Entities
{
    public class JournalEntry
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime LogDate { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public User? User { get; set; }
    }
}
