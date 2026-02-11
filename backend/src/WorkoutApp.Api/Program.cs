using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using WorkoutApp.Application;
using WorkoutApp.Infrastructure;
using WorkoutApp.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Initialize Firebase Admin SDK
var firebaseCredentialPath = Path.Combine(builder.Environment.ContentRootPath, "firebase-service-account.json");
if (File.Exists(firebaseCredentialPath))
{
    FirebaseApp.Create(new AppOptions
    {
        Credential = GoogleCredential.FromFile(firebaseCredentialPath),
        ProjectId = builder.Configuration["Firebase:ProjectId"]
    });
}
else
{
    // In development without the file, Firebase auth will be disabled
    Console.WriteLine("WARNING: firebase-service-account.json not found. Firebase auth disabled.");
}

// OpenTelemetry configuration - uses OTEL_* environment variables
// Run with: OTEL_EXPORTER_OTLP_ENDPOINT, OTEL_EXPORTER_OTLP_HEADERS, OTEL_EXPORTER_OTLP_PROTOCOL
builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource
        .AddService("workoutapp")
        .AddAttributes(new Dictionary<string, object>
        {
            ["service.namespace"] = "my-application-group",
            ["deployment.environment"] = "production"
        }))
    .WithMetrics(metrics => metrics
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddRuntimeInstrumentation()
        .AddOtlpExporter())
    .WithTracing(tracing => tracing
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddEntityFrameworkCoreInstrumentation()
        .AddOtlpExporter());

// Configure logging to use OpenTelemetry
builder.Logging.AddOpenTelemetry(logging =>
{
    logging.IncludeFormattedMessage = true;
    logging.IncludeScopes = true;
    logging.AddOtlpExporter();
});

// Add services to the container
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Firebase authentication
builder.Services.AddAuthentication("Firebase")
    .AddScheme<AuthenticationSchemeOptions, WorkoutApp.Infrastructure.Authentication.FirebaseAuthHandler>("Firebase", null);
builder.Services.AddAuthorization();

// Add application layers
builder.Services.AddApplication();
builder.Services.AddInfrastructure();

var app = builder.Build();

// Apply migrations and seed database
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await context.Database.EnsureCreatedAsync();
    await AppDbContextSeed.SeedAsync(context);
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
