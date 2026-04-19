using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HabitTracker.API.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HabitTracker.API.Features.Goals.Queries
{
    public class GetUserGoalsQuery : IRequest<List<GoalDto>>
    {
        public Guid UserId { get; set; }
    }

    public class GoalDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? MotivationWhy { get; set; }
        public string TermType { get; set; } = string.Empty;
        public DateTime? TargetDate { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class GetUserGoalsQueryHandler : IRequestHandler<GetUserGoalsQuery, List<GoalDto>>
    {
        private readonly AppDbContext _context;

        public GetUserGoalsQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<GoalDto>> Handle(GetUserGoalsQuery request, CancellationToken cancellationToken)
        {
            return await _context.Goals
                .AsNoTracking()
                .Where(g => g.UserId == request.UserId)
                .Select(g => new GoalDto
                {
                    Id = g.Id,
                    Title = g.Title,
                    MotivationWhy = g.MotivationWhy,
                    TermType = g.TermType,
                    TargetDate = g.TargetDate,
                    Status = g.Status
                })
                .ToListAsync(cancellationToken);
        }
    }
}
