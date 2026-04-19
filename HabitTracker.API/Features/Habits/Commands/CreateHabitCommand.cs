using System;
using System.Threading;
using System.Threading.Tasks;
using HabitTracker.API.Core.Entities;
using HabitTracker.API.Infrastructure.Data;
using MediatR;

namespace HabitTracker.API.Features.Habits.Commands
{
    public class CreateHabitCommand : IRequest<Guid>
    {
        public Guid UserId { get; set; }
        public Guid? GoalId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Type { get; set; } = "Positive";
        public string Frequency { get; set; } = "Daily";
    }

    public class CreateHabitCommandHandler : IRequestHandler<CreateHabitCommand, Guid>
    {
        private readonly AppDbContext _context;

        public CreateHabitCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Handle(CreateHabitCommand request, CancellationToken cancellationToken)
        {
            var habit = new Habit
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                GoalId = request.GoalId,
                Name = request.Name,
                Description = request.Description,
                Type = request.Type,
                Frequency = request.Frequency,
                CreatedAt = DateTime.UtcNow,
                CurrentStreak = 0,
                LongestStreak = 0
            };

            _context.Habits.Add(habit);
            await _context.SaveChangesAsync(cancellationToken);

            return habit.Id;
        }
    }
}
