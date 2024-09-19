using Host.Infrastructure.Integration;
using Host.Infrastructure.Modules;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// modules
builder.Services.AddModules();

// ui
builder.Services
    .AddRazorPages()
    .AddRazorRuntimeCompilation()
    .AddWebModules();

// web
builder.Services
    .AddWebModulesFiles();

// queue
builder.Services
    .AddHostedService<QueueJob>()
    .AddSingleton<QueueProcessor>()
    .AddSingleton<EventPublisher>();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStaticFiles();
app.UseRouting();
// app.UseAuthentication();
// app.UseAuthorization();
app.UseApiModules();
app.MapGet("/meta/name", () => Assembly.GetExecutingAssembly().GetName());
app.MapGet("/health/alive", () => "alive");
app.MapGet("/health/ready", () => "ready");
app.MapRazorPages();
app.StartModules();
app.Run();