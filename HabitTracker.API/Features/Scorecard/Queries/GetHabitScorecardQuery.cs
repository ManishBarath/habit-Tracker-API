using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HabitTracker.API.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HabitTracker.API.Features.Scorecard.Queries
{
    public class GetHabitScorecardQuery : IRequest<List<ScorecardLogDto>>
    {
        public Guid HabitId { get; set; }
    }

    public class ScorecardLogDto
    {
        public Guid Id { get; set; }
        public DateTime LogDate { get; set; }
        public string CompletionStatus { get; set; } = string.Empty;
        public decimal? MetricValue { get; set; }
        public string? ContextNotes { get; set; }
    }

    public class GetHabitScorecardQueryHandler : IRequestHandler<GetHabitScorecardQuery, List<ScorecardLogDto>>
    {
        private readonly AppDbContext _context;

        public GetHabitScorecardQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ScorecardLogDto>> Handle(GetHabitScorecardQuery request, CancellationToken cancellationToken)
        {
            return await _context.ScorecardLogs
                .AsNoTracking()
                .Where(log => log.HabitId == request.HabitId)
                .OrderByDescending(log => log.LogDate)
                .Select(log => new ScorecardLogDto
                {
                    Id = log.Id,
                    LogDate = log.LogDate,
                    CompletionStatus = log.CompletionStatus,
                    MetricValue = log.MetricValue,
                    ContextNotes = log.ContextNotes
                })
                .ToListAsync(cancellationToken);
        }
    }
}
