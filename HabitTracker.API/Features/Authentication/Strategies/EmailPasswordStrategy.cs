using System;
using System.Threading;
using System.Threading.Tasks;
using HabitTracker.API.Core.Entities;
using HabitTracker.API.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity; // for PasswordHasher (makes it easy to hash without installing separate libraries)

namespace HabitTracker.API.Features.Authentication.Strategies
{
    public class EmailPasswordStrategy : IAuthenticationStrategy
    {
        private readonly AppDbContext _context;
        private readonly PasswordHasher<User> _passwordHasher;

        public EmailPasswordStrategy(AppDbContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<User>();
        }

        public async Task<AuthResult> LoginAsync(AuthRequest request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

            if (user == null || string.IsNullOrEmpty(user.PasswordHash))
            {
                return new AuthResult { IsSuccess = false, ErrorMessage = "Invalid credentials." };
            }

            var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);

            if (verificationResult == PasswordVerificationResult.Failed)
            {
                return new AuthResult { IsSuccess = false, ErrorMessage = "Invalid credentials." };
            }

            // In a real application, you would generate a JWT token here.
            // For now, returning the user Id as the token so the frontend can store it.
            return new AuthResult
            {
                IsSuccess = true,
                UserId = user.Id,
                Token = user.Id.ToString() // Replace with JWT logic in future phase
            };
        }

        public async Task<AuthResult> RegisterAsync(AuthRequest request, CancellationToken cancellationToken)
        {
            if (await _context.Users.AnyAsync(u => u.Email == request.Email, cancellationToken))
            {
                return new AuthResult { IsSuccess = false, ErrorMessage = "A user with this email already exists." };
            }

            var newUser = new User
            {
                Id = Guid.NewGuid(),
                Email = request.Email,
                ThemePreference = "light",
                Timezone = "UTC",
                CreatedAt = DateTime.UtcNow
            };

            newUser.PasswordHash = _passwordHasher.HashPassword(newUser, request.Password);

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync(cancellationToken);

            return new AuthResult
            {
                IsSuccess = true,
                UserId = newUser.Id,
                Token = newUser.Id.ToString() // Replace with JWT logic in future phase
            };
        }
    }
}