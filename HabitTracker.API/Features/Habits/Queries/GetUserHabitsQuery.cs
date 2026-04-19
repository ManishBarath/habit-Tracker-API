using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HabitTracker.API.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HabitTracker.API.Features.Habits.Queries
{
    public class GetUserHabitsQuery : IRequest<List<HabitDto>>
    {
        public Guid UserId { get; set; }
    }

    public class HabitDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Frequency { get; set; } = string.Empty;
        public int CurrentStreak { get; set; }
        public int LongestStreak { get; set; }
    }

    public class GetUserHabitsQueryHandler : IRequestHandler<GetUserHabitsQuery, List<HabitDto>>
    {
        private readonly AppDbContext _context;

        public GetUserHabitsQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<HabitDto>> Handle(GetUserHabitsQuery request, CancellationToken cancellationToken)
        {
            return await _context.Habits
                .AsNoTracking()
                .Where(h => h.UserId == request.UserId)
                .Select(h => new HabitDto
                {
                    Id = h.Id,
                    Name = h.Name,
                    Description = h.Description,
                    Type = h.Type,
                    Frequency = h.Frequency,
                    CurrentStreak = h.CurrentStreak,
                    LongestStreak = h.LongestStreak
                })
                .ToListAsync(cancellationToken);
        }
    }
}
