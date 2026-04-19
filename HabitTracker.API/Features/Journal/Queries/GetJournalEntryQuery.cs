using HabitTracker.API.Core.Entities;
using HabitTracker.API.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HabitTracker.API.Features.Journal.Queries
{
    public class GetJournalEntryQuery : IRequest<JournalEntry?>
    {
        public Guid UserId { get; set; }
        public DateTime LogDate { get; set; }
    }

    public class GetJournalEntryQueryHandler : IRequestHandler<GetJournalEntryQuery, JournalEntry?>
    {
        private readonly AppDbContext _context;

        public GetJournalEntryQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<JournalEntry?> Handle(GetJournalEntryQuery request, CancellationToken cancellationToken)
        {
            var date = DateTime.SpecifyKind(request.LogDate.Date, DateTimeKind.Utc);
            return await _context.JournalEntries
                .AsNoTracking()
                .FirstOrDefaultAsync(j => j.UserId == request.UserId && j.LogDate.Date == date, cancellationToken);
        }
    }
}
