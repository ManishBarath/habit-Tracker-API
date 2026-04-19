using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace HabitTracker.API.Features.Authentication.Strategies
{
    // The Factory is used so that the MediatR handler can ask for a specific strategy
    // based on the request (Email/Password, OAuth, MagicLink, etc)
    public class AuthStrategyFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public AuthStrategyFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IAuthenticationStrategy GetStrategy(string provider)
        {
            if (provider == "EmailPassword")
            {
                // The factory pulls dependencies (like DbContext) so the caller doesn't have to concern itself with it
                return _serviceProvider.GetRequiredService<EmailPasswordStrategy>(); 
            }
            
            // For example, in the future:
            if (provider == "Google") return _serviceProvider.GetRequiredService<GoogleOAuthStrategy>();

            throw new NotSupportedException($"The authentication provider '{provider}' is not supported.");
        }
    }
}
