using System;
using Microsoft.AspNetCore.Authentication;

namespace CtsContestWeb.Middleware.Auth
{
    public static class AzureLoginExtensions
    {
        public static AuthenticationBuilder AddAzureLoginAuth(this AuthenticationBuilder builder, Action<AzureAuthOptions> configureOptions)
        {
            return builder.AddScheme<AzureAuthOptions, AzureAuthHandler>("Azure Login Scheme", "Azure Auth", configureOptions);
        }
    }
}