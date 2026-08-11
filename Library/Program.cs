var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Host.AddSerilog();

// Add services to the container.
builder.Services.AddApi(configuration);
builder.Services.AddApplication();
builder.Services.AddInfrastructure(configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
	await app.Services.SeedDatabaseAsync();
}

app.MapOpenApi().AllowAnonymous();
app.MapScalarApiReference().AllowAnonymous();

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseExceptionHandler();

app.UseAuthentication();

app.UseMiddleware<RequestLoggingMiddleware>();

app.UseSerilogRequestLogging(options =>
{
	options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
});

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }