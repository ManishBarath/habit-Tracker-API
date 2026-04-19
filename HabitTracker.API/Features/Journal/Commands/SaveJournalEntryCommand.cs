using HabitTracker.API.Core.Entities;
using HabitTracker.API.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HabitTracker.API.Features.Journal.Commands
{
    public class SaveJournalEntryCommand : IRequest<JournalEntry>
    {
        public Guid UserId { get; set; }
        public DateTime LogDate { get; set; }
        public string Content { get; set; } = string.Empty;
    }

    public class SaveJournalEntryCommandHandler : IRequestHandler<SaveJournalEntryCommand, JournalEntry>
    {
        private readonly AppDbContext _context;

        public SaveJournalEntryCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<JournalEntry> Handle(SaveJournalEntryCommand request, CancellationToken cancellationToken)
        {
            var date = DateTime.SpecifyKind(request.LogDate.Date, DateTimeKind.Utc);
            var entry = await _context.JournalEntries
                .FirstOrDefaultAsync(j => j.UserId == request.UserId && j.LogDate.Date == date, cancellationToken);
            
            if (entry == null)
            {
                entry = new JournalEntry
                {
                    Id = Guid.NewGuid(),
                    UserId = request.UserId,
                    LogDate = date,
                    Content = request.Content,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                _context.JournalEntries.Add(entry);
            }
            else
            {
                entry.Content = request.Content;
                entry.UpdatedAt = DateTime.UtcNow;
                _context.JournalEntries.Update(entry);
            }

            await _context.SaveChangesAsync(cancellationToken);
            return entry;
        }
    }
}
