using System;
using HabitTracker.API.Core.Entities;
using HabitTracker.API.Core.Interfaces;

namespace HabitTracker.API.Features.Scorecard.Commands
{
    public class DailyStreakStrategy : IStreakCalculationStrategy
    {
        public void CalculateNextStreak(Habit habit, ScorecardLog newLog, DateTime? lastLogDate)
        {
            if (newLog.CompletionStatus != "Completed")
            {
                habit.CurrentStreak = 0;
                return;
            }

            if (!lastLogDate.HasValue)
            {
                habit.CurrentStreak = 1;
            }
            else
            {
                var daysBetween = (newLog.LogDate.Date - lastLogDate.Value.Date).Days;
                
                if (daysBetween == 1) // Consecutive day
                {
                    habit.CurrentStreak++;
                }
                else if (daysBetween > 1) // Streak broken
                {
                    habit.CurrentStreak = 1;
                }
                // If daysBetween == 0, duplicate log for today, do nothing.
            }

            if (habit.CurrentStreak > habit.LongestStreak)
            {
                habit.LongestStreak = habit.CurrentStreak;
            }
        }
    }
}
