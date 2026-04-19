using System;
using System.Collections.Generic;

namespace HabitTracker.API.Core.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string? PasswordHash { get; set; } // Will be null for OAuth users
        public string? ThemePreference { get; set; }
        public string Timezone { get; set; } = "UTC";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public ICollection<Goal> Goals { get; set; } = new List<Goal>();
        public ICollection<Habit> Habits { get; set; } = new List<Habit>();
    }
}
