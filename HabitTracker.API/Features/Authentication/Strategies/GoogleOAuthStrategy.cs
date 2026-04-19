using System;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth;
using HabitTracker.API.Core.Entities;
using HabitTracker.API.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HabitTracker.API.Features.Authentication.Strategies
{
    public class GoogleOAuthStrategy : IAuthenticationStrategy
    {
        private readonly AppDbContext _context;

        public GoogleOAuthStrategy(AppDbContext context)
        {
            _context = context;
        }

        public async Task<AuthResult> LoginAsync(AuthRequest request, CancellationToken cancellationToken)
        {
            var payload = await VerifyGoogleTokenAsync(request.Token);
            if (payload == null)
            {
                return new AuthResult { IsSuccess = false, ErrorMessage = "Invalid Google token." };
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == payload.Email, cancellationToken);
            if (user == null)
            {
                // Automatically create a user if they don't exist yet via Google Login
                user = new User
                {
                    Id = Guid.NewGuid(),
                    Email = payload.Email,
                    ThemePreference = "light",
                    Timezone = "UTC",
                    CreatedAt = DateTime.UtcNow
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return new AuthResult
            {
                IsSuccess = true,
                UserId = user.Id,
                Token = user.Id.ToString() // Replace with JWT logic in future phase
            };
        }

        public async Task<AuthResult> RegisterAsync(AuthRequest request, CancellationToken cancellationToken)
        {
            // For OAuth, Registration and Login follow the same basic flow (create if not exists)
            return await LoginAsync(request, cancellationToken);
        }

        private async Task<GoogleJsonWebSignature.Payload?> VerifyGoogleTokenAsync(string idToken)
        {
            try
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings
                {
                    // If you want to validate Audience, add your Client ID here
                    // Audience = new[] { "YOUR_GOOGLE_CLIENT_ID.apps.googleusercontent.com" }
                };

                return await GoogleJsonWebSignature.ValidateAsync(idToken, settings);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Google token verification failed: {ex.Message}");
                return null;
            }
        }
    }
}
