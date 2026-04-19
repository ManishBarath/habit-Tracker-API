using System.Threading.Tasks;
using HabitTracker.API.Features.Authentication.Commands;
using HabitTracker.API.Features.Authentication.Strategies;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HabitTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { Error = result.ErrorMessage });
            }

            return Ok(result);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { Error = result.ErrorMessage });
            }

            return Ok(result);
        }
    }
}
