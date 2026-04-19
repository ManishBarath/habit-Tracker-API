using System;
using System.Threading;
using System.Threading.Tasks;
using HabitTracker.API.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HabitTracker.API.Features.Habits.Commands
{
    public class UpdateHabitCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class UpdateHabitCommandHandler : IRequestHandler<UpdateHabitCommand, bool>
    {
        private readonly AppDbContext _context;

        public UpdateHabitCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(UpdateHabitCommand request, CancellationToken cancellationToken)
        {
            var habit = await _context.Habits.FirstOrDefaultAsync(h => h.Id == request.Id, cancellationToken);
            if (habit == null)
            {
                return false;
            }

            habit.Name = request.Name;
            habit.Description = request.Description;
            habit.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
