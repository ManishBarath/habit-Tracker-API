using System;
using HabitTracker.API.Core.Interfaces;

namespace HabitTracker.API.Features.Scorecard.Commands
{
    public class StreakStrategyFactory
    {
        public static IStreakCalculationStrategy GetStrategy(string frequency)
        {
            return frequency.ToLower() switch
            {
                "daily" => new DailyStreakStrategy(),
                // "weekly" => new WeeklyStreakStrategy(),
                // "monthly" => new MonthlyStreakStrategy(),
                _ => new DailyStreakStrategy() // Default
            };
        }
    }
}
