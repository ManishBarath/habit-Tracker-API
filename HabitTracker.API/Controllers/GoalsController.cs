using System;
using System.Threading.Tasks;
using HabitTracker.API.Features.Goals.Commands;
using HabitTracker.API.Features.Goals.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HabitTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GoalsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GoalsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateGoal([FromBody] CreateGoalCommand command)
        {
            var goalId = await _mediator.Send(command);
            return Ok(new { Id = goalId });
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserGoals(Guid userId)
        {
            var query = new GetUserGoalsQuery { UserId = userId };
            var goals = await _mediator.Send(query);
            return Ok(goals);
        }
    }
}
