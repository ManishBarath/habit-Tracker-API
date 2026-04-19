using HabitTracker.API.Features.Whiteboard.Commands;
using HabitTracker.API.Features.Whiteboard.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace HabitTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WhiteboardController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WhiteboardController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetNotes(Guid userId)
        {
            var query = new GetWhiteboardNotesQuery { UserId = userId };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("sync")]
        public async Task<IActionResult> SyncNotes([FromBody] SyncWhiteboardNotesCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
