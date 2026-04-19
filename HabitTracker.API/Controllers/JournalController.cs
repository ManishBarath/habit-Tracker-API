using HabitTracker.API.Features.Journal.Commands;
using HabitTracker.API.Features.Journal.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace HabitTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JournalController : ControllerBase
    {
        private readonly IMediator _mediator;

        public JournalController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{userId}/{date}")]
        public async Task<IActionResult> GetEntry(Guid userId, DateTime date)
        {
            var query = new GetJournalEntryQuery { UserId = userId, LogDate = date };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> SaveEntry([FromBody] SaveJournalEntryCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
