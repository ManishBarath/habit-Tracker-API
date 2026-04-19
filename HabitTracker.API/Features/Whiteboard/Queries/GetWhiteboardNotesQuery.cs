using HabitTracker.API.Core.Entities;
using HabitTracker.API.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HabitTracker.API.Features.Whiteboard.Queries
{
    public class GetWhiteboardNotesQuery : IRequest<List<WhiteboardNote>>
    {
        public Guid UserId { get; set; }
    }

    public class GetWhiteboardNotesQueryHandler : IRequestHandler<GetWhiteboardNotesQuery, List<WhiteboardNote>>
    {
        private readonly AppDbContext _context;

        public GetWhiteboardNotesQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<WhiteboardNote>> Handle(GetWhiteboardNotesQuery request, CancellationToken cancellationToken)
        {
            return await _context.WhiteboardNotes
                .AsNoTracking()
                .Where(w => w.UserId == request.UserId)
                .ToListAsync(cancellationToken);
        }
    }
}
