using Learning.MT;
using Serilog;
using Serilog.Events;

var format = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}";
// var format = "[{Timestamp:HH:mm:ss} {SourceContext} {Level:u3}] {Message:lj}{NewLine}{Exception}";

// This creates a global logger that can be used during
// bootstrap, but can later be configured using the host
// builder
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console(outputTemplate: format)
    .CreateBootstrapLogger();

// Attach a global error handler
AppDomain.CurrentDomain.UnhandledException += (_, eventArgs) =>
{
    Log.Fatal((Exception)eventArgs.ExceptionObject, "Global Unhandled Exception");
};

var host = Host.CreateDefaultBuilder()
    .UseSerilog((cxt, cfg) =>
    {
        // hide these logs for the demo
        cfg.MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Error);
        cfg.WriteTo.Console(outputTemplate: format);
    })
    .ConfigureAppConfiguration((cxt, builder) =>
    {
        builder.Sources.Clear();
        builder.AddEnvironmentVariables();
    }).ConfigureWebHostDefaults(web =>
    {
        web.UseKestrel(options =>
        {
            options.ListenAnyIP(3000);
        });

        web.UseStartup<Startup>();
    }).Build();

await host.RunAsync();
