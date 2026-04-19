using System;
using System.Threading;
using System.Threading.Tasks;
using HabitTracker.API.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HabitTracker.API.Features.Habits.Commands
{
    public class DeleteHabitCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }

    public class DeleteHabitCommandHandler : IRequestHandler<DeleteHabitCommand, bool>
    {
        private readonly AppDbContext _context;

        public DeleteHabitCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeleteHabitCommand request, CancellationToken cancellationToken)
        {
            var habit = await _context.Habits.FirstOrDefaultAsync(h => h.Id == request.Id, cancellationToken);
            if (habit == null)
                return false;

            _context.Habits.Remove(habit);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
