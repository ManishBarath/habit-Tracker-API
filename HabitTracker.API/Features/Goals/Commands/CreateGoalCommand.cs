using System;
using System.Threading;
using System.Threading.Tasks;
using HabitTracker.API.Core.Entities;
using HabitTracker.API.Infrastructure.Data;
using MediatR;

namespace HabitTracker.API.Features.Goals.Commands
{
    public class CreateGoalCommand : IRequest<Guid>
    {
        public Guid UserId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? MotivationWhy { get; set; }
        public string TermType { get; set; } = "ShortTerm";
        public DateTime? TargetDate { get; set; }
    }

    public class CreateGoalCommandHandler : IRequestHandler<CreateGoalCommand, Guid>
    {
        private readonly AppDbContext _context;

        public CreateGoalCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Handle(CreateGoalCommand request, CancellationToken cancellationToken)
        {
            var goal = new Goal
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                Title = request.Title,
                MotivationWhy = request.MotivationWhy,
                TermType = request.TermType,
                TargetDate = request.TargetDate?.ToUniversalTime(),
                Status = "Active",
                CreatedAt = DateTime.UtcNow
            };

            _context.Goals.Add(goal);
            await _context.SaveChangesAsync(cancellationToken);

            return goal.Id;
        }
    }
}
