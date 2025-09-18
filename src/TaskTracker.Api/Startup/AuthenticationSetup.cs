using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace TaskTracker.Api.Startup;

public static class AuthenticationSetup
{
	public static IServiceCollection ConfigureAuthentication(this IServiceCollection services,
		IConfiguration configuration)
	{
		services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey =
						new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["AuthSettings:JwtSigningKey"]!)),
					ValidateIssuer = false,
					ValidateAudience = false,
					ValidateLifetime = true
				};
				
				options.Events = new JwtBearerEvents
				{
					OnMessageReceived = context =>
					{
						if (context.Request.Cookies.TryGetValue("access_token", out var token))
						{
							context.Token = token;
						}

						return Task.CompletedTask;
					}
				};
			});

		return services;
	}
}