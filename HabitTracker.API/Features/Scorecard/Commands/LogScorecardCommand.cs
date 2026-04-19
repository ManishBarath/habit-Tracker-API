using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HabitTracker.API.Core.Entities;
using HabitTracker.API.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using MediatR;

namespace HabitTracker.API.Features.Scorecard.Commands
{
    public class LogScorecardCommand : IRequest<Guid>
    {
        public Guid HabitId { get; set; }
        public Guid UserId { get; set; }
        public DateTime LogDate { get; set; }
        public string CompletionStatus { get; set; } = "Completed";
        public decimal? MetricValue { get; set; }
        public string? ContextNotes { get; set; }
    }

    public class LogScorecardCommandHandler : IRequestHandler<LogScorecardCommand, Guid>
    {
        private readonly AppDbContext _context;

        public LogScorecardCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Handle(LogScorecardCommand request, CancellationToken cancellationToken)
        {
            var habit = await _context.Habits
                .Include(h => h.Logs)
                .FirstOrDefaultAsync(h => h.Id == request.HabitId, cancellationToken);
            
            if (habit == null)
            {
                throw new Exception("Habit not found");
            }

            var requestDate = request.LogDate.ToUniversalTime().Date;
            var log = habit.Logs.FirstOrDefault(l => l.LogDate.Date == requestDate);

            if (log != null)
            {
                log.CompletionStatus = request.CompletionStatus;
                log.MetricValue = request.MetricValue ?? log.MetricValue;
                log.ContextNotes = request.ContextNotes ?? log.ContextNotes;
                _context.ScorecardLogs.Update(log);
            }
            else 
            {
                log = new ScorecardLog
                {
                    Id = Guid.NewGuid(),
                    HabitId = request.HabitId,
                    UserId = request.UserId,
                    LogDate = request.LogDate.ToUniversalTime(),
                    CompletionStatus = request.CompletionStatus,
                    MetricValue = request.MetricValue,
                    ContextNotes = request.ContextNotes,
                    CreatedAt = DateTime.UtcNow
                };
                _context.ScorecardLogs.Add(log);
            }

            // Get last log to calculate streak, excluding the one we are creating.
            var lastLogDate = habit.Logs
                .Where(l => l.CompletionStatus == "Completed" && l.LogDate.Date < log.LogDate.Date && l.Id != log.Id)
                .OrderByDescending(l => l.LogDate)
                .Select(l => (DateTime?)l.LogDate)
                .FirstOrDefault();

            // Factory Pattern to supply the Strategy Pattern 
            var streakStrategy = StreakStrategyFactory.GetStrategy(habit.Frequency);
            streakStrategy.CalculateNextStreak(habit, log, lastLogDate);

            // Save state
            _context.Habits.Update(habit);
            await _context.SaveChangesAsync(cancellationToken);

            return log.Id;
        }
    }
}