using TaskTracker.Api.Middlewares;
using TaskTracker.Api.Startup;

var builder = WebApplication.CreateBuilder(args);

builder.Services
	.ConfigureControllers()
	.ConfigureSwagger()
	.ConfigureDbContext(builder.Configuration)
	.ConfigureMediatR()
	.ConfigureRepositories()
	.ConfigureSettings(builder.Configuration)
	.RegisterServices();

var app = builder.Build();

await app.ApplyMigrationsAsync(builder.Configuration);

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.MapControllers();
app.UseMiddleware<JwtContextMiddleware>();

app.Run();