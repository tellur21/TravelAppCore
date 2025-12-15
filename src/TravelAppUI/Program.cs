using Application;
using TravelAppUI.Services;
using Serilog;

// Configure a bootstrap logger for startup diagnostics
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File(
        path: Path.Combine(AppContext.BaseDirectory, "Logs", "log-.txt"),
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 7,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
    )
    .CreateBootstrapLogger();

Log.Information("Starting up TravelAppUI");

try
{
    var builder = WebApplication.CreateBuilder(args);

    // --- Serilog Configuration ---
    // Remove default logging providers and use Serilog
    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext());

    // Add services to the container.    
    builder.Services.AddRazorPages();

    // Configure API settings
    var apiSettings = builder.Configuration.GetSection("ApiSettings");
    var baseUrl = apiSettings["BaseUrl"] ?? "https://api..travel";

    // Register HTTP clients
    builder.Services.AddHttpClient<ITravelPackageService, TravelPackageService>(client =>
    {
        client.BaseAddress = new Uri(baseUrl);
        client.DefaultRequestHeaders.Add("Accept", "application/json");
    });

    builder.Services.AddHttpClient<IBookingService, BookingService>(client =>
    {
        client.BaseAddress = new Uri(baseUrl);
        client.DefaultRequestHeaders.Add("Accept", "application/json");
    });

    var app = builder.Build();

    // --- Serilog Request Logging ---
    // This should be one of the first middleware components
    app.UseSerilogRequestLogging();

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();

    app.UseRouting();

    app.UseAuthorization();

    // Redirect to www..travel
    app.Use(async (context, next) =>
    {
        if (context.Request.Host.Host == ".travel")
        {
            var newUrl = $"https://www..travel{context.Request.Path}{context.Request.QueryString}";
            Console.WriteLine($"Redirecting to: {newUrl}");
            context.Response.Redirect(newUrl, permanent: true);
            return;
        }
        await next();
    });

    app.MapStaticAssets();
    app.MapRazorPages()
       .WithStaticAssets();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}

