using System;
using HabitTracker.API.Core.Entities;

namespace HabitTracker.API.Core.Interfaces
{
    public interface IStreakCalculationStrategy
    {
        void CalculateNextStreak(Habit habit, ScorecardLog newLog, DateTime? lastLogDate);
    }
}
