using System.Threading;
using System.Threading.Tasks;
using HabitTracker.API.Features.Authentication.Strategies;
using MediatR;

namespace HabitTracker.API.Features.Authentication.Commands
{
    public class LoginUserCommand : IRequest<AuthResult>
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Provider { get; set; } = "EmailPassword"; // By default Email/Password
        public string Token { get; set; } = string.Empty; // Added to support OAuth providers like Google
    }

    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, AuthResult>
    {
        private readonly AuthStrategyFactory _strategyFactory;

        public LoginUserCommandHandler(AuthStrategyFactory strategyFactory)
        {
            _strategyFactory = strategyFactory;
        }

        public async Task<AuthResult> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var authStrategy = _strategyFactory.GetStrategy(request.Provider);

            var authRequest = new AuthRequest
            {
                Email = request.Email,
                Password = request.Password,
                Provider = request.Provider,
                Token = request.Token // Passing the Google Token to the AuthRequest
            };

            return await authStrategy.LoginAsync(authRequest, cancellationToken);
        }
    }
}
