using System;
using System.Threading.Tasks;
using HabitTracker.API.Features.Scorecard.Commands;
using HabitTracker.API.Features.Scorecard.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HabitTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScorecardController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ScorecardController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("log")]
        public async Task<IActionResult> LogScorecard([FromBody] LogScorecardCommand command)
        {
            var logId = await _mediator.Send(command);
            return Ok(new { Id = logId });
        }

        [HttpGet("habit/{habitId}")]
        public async Task<IActionResult> GetHabitScorecard(Guid habitId)
        {
            var query = new GetHabitScorecardQuery { HabitId = habitId };
            var logs = await _mediator.Send(query);
            return Ok(logs);
        }
    }
}