using HabitTracker.API.Core.Entities;
using HabitTracker.API.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HabitTracker.API.Features.Whiteboard.Commands
{
    public class WhiteboardNoteDto
    {
        public Guid Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public float X { get; set; }
        public float Y { get; set; }
        public string Color { get; set; } = string.Empty;
    }

    public class SyncWhiteboardNotesCommand : IRequest<List<WhiteboardNote>>
    {
        public Guid UserId { get; set; }
        public List<WhiteboardNoteDto> Notes { get; set; } = new();
    }

    public class SyncWhiteboardNotesCommandHandler : IRequestHandler<SyncWhiteboardNotesCommand, List<WhiteboardNote>>
    {
        private readonly AppDbContext _context;

        public SyncWhiteboardNotesCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<WhiteboardNote>> Handle(SyncWhiteboardNotesCommand request, CancellationToken cancellationToken)
        {
            var existingNotes = await _context.WhiteboardNotes
                .Where(w => w.UserId == request.UserId)
                .ToListAsync(cancellationToken);

            _context.WhiteboardNotes.RemoveRange(existingNotes);

            var newNotes = request.Notes.Select(n => new WhiteboardNote
            {
                Id = n.Id,
                UserId = request.UserId,
                Text = n.Text,
                X = n.X,
                Y = n.Y,
                Color = n.Color,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }).ToList();

            await _context.WhiteboardNotes.AddRangeAsync(newNotes, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return newNotes;
        }
    }
}
