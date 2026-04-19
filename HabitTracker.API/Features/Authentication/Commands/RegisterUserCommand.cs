using System.Threading;
using System.Threading.Tasks;
using HabitTracker.API.Features.Authentication.Strategies;
using MediatR;

namespace HabitTracker.API.Features.Authentication.Commands
{
    public class RegisterUserCommand : IRequest<AuthResult>
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Provider { get; set; } = "EmailPassword"; // By default Email/Password
    }

    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, AuthResult>
    {
        private readonly AuthStrategyFactory _strategyFactory;

        public RegisterUserCommandHandler(AuthStrategyFactory strategyFactory)
        {
            _strategyFactory = strategyFactory;
        }

        public async Task<AuthResult> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var authStrategy = _strategyFactory.GetStrategy(request.Provider);

            var authRequest = new AuthRequest
            {
                Email = request.Email,
                Password = request.Password,
                Provider = request.Provider
            };

            return await authStrategy.RegisterAsync(authRequest, cancellationToken);
        }
    }
}
