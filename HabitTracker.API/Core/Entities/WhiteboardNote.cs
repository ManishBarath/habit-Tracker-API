using System;

namespace HabitTracker.API.Core.Entities
{
    public class WhiteboardNote
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Text { get; set; } = string.Empty;
        public float X { get; set; }
        public float Y { get; set; }
        public string Color { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public User? User { get; set; }
    }
}
