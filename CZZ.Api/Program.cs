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

    IWebHostEnvironment env = builder.Environment;

    builder.Services.AddCors(options =>
    {
        options.AddPolicy(name: "api",
                          builder =>
                          {
                              if (env.IsDevelopment())
                              {
                                  builder.WithOrigins(new string[]{
                                  "https://localhost:7225/"});
                              }

                              // allow all option
                              builder.AllowAnyOrigin();
                              builder.AllowAnyMethod();
                              builder.AllowAnyHeader();

                          });
    });

    var app = builder.Build();

    app.UseCors("api");

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