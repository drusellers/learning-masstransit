namespace Learning.MT;

using MassTransit;

public class Startup
{

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    /// <summary>
    /// This method gets called by the runtime. Use this method to add services to the container.
    /// </summary>
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers(o =>
        {

        });

        services.AddMassTransit(cfg =>
        {
            // needed so we can pull it from the container
            cfg.SetEndpointNameFormatter(new DefaultEndpointNameFormatter(false));

            cfg.AddConsumers(typeof(Startup).Assembly);
            cfg.AddActivities(typeof(Startup).Assembly);


            cfg.UsingRabbitMq((context, rmq) =>
            {
                rmq.Host("amqp://localhost:5672", o =>
                {
                    o.Username("guest");
                    o.Password("guest");
                });

                rmq.ConfigureEndpoints(context);
            });
        });
    }

    /// <summary>
    /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    /// </summary>
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // !! Order Matters here

        app.UseRouting();

        // Demo system (disable)
        // app.UseAuthentication();
        // app.UseAuthorization();

        app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

        app.UseEndpoints(ep =>
        {
            ep.MapControllers();
        });
    }
}
