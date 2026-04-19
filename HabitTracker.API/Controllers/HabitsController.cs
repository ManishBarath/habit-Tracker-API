using System;
using System.Threading.Tasks;
using HabitTracker.API.Features.Habits.Commands;
using HabitTracker.API.Features.Habits.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HabitTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HabitsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public HabitsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateHabit([FromBody] CreateHabitCommand command)
        {
            var habitId = await _mediator.Send(command);
            return Ok(new { Id = habitId });
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserHabits(Guid userId)
        {
            var query = new GetUserHabitsQuery { UserId = userId };
            var habits = await _mediator.Send(query);
            return Ok(habits);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateHabit(Guid id, [FromBody] UpdateHabitCommand command)
        {
            if (id != command.Id) return BadRequest();
            var result = await _mediator.Send(command);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHabit(Guid id)
        {
            var result = await _mediator.Send(new DeleteHabitCommand { Id = id });
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
