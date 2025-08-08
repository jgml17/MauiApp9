var builder = WebApplication.CreateBuilder(args);

// Configure Kestrel to listen on all interfaces
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5001); // HTTP
    options.ListenAnyIP(7281, listenOptions =>
    {
        listenOptions.UseHttps(); // HTTPS
    });
});

builder.Services.AddControllers();
builder.Services.AddSingleton<PluginAPI.Services.PluginConfigService>();

// Add CORS for mobile app
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// API is ready

app.UseCors();
// app.UseHttpsRedirection(); // Disabled for mobile development
app.UseAuthorization();
app.MapControllers();

app.Run();