using CZZ.Api.CZZRepostiory;
using CZZ.Api.CZZService;
using Serilog;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
   .MinimumLevel.Information()
   .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
   .Enrich.FromLogContext()
   .WriteTo.Console()
   .WriteTo.File("logs/host-.txt", rollingInterval: RollingInterval.Hour)
   .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddControllersWithViews();

    builder.Services.AddScoped<ICZZServiceWrapper, CZZServiceWrapper>();
    builder.Services.AddScoped<ICZZRepostioryWrapper, CZZRepostioryWrapper>();
    builder.Services.AddScoped<IHouseObjectService, HouseObjectService>();
    builder.Services.AddScoped<IHouseObjectRepostiory, HouseObjectRepostiory>();

    builder.Services.AddSwaggerDocument(config =>
    {
        config.PostProcess = document =>
        {
            document.Info.Version = "0.1";
            document.Info.Title = "CZZ API";
            document.Info.Description = "CZZ.API Testing";
        };
    });

    Log.Information("Starting web host");
    builder.Host.UseSerilog();

    var app = builder.Build();

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseSerilogRequestLogging();

    app.UseOpenApi();
    app.UseSwaggerUi3();

    app.UseRouting();

    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "主機意外終止");
}
finally
{
    Log.CloseAndFlush();
}