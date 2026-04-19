using System;
using System.Threading;
using System.Threading.Tasks;

namespace HabitTracker.API.Features.Authentication.Strategies
{
    public class AuthRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public string Provider { get; set; } = "EmailPassword"; // e.g. Google, Apple, MagicLink
    }

    public class AuthResult
    {
        public bool IsSuccess { get; set; }
        public Guid? UserId { get; set; }
        public string? Token { get; set; }
        public string? ErrorMessage { get; set; }
    }

    public interface IAuthenticationStrategy
    {
        Task<AuthResult> LoginAsync(AuthRequest request, CancellationToken cancellationToken);
        Task<AuthResult> RegisterAsync(AuthRequest request, CancellationToken cancellationToken);
    }
}
