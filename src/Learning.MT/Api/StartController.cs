namespace Learning.MT.Api;

using Learning.MT.Activities;
using MassTransit;
using MassTransit.Courier.Contracts;
using Microsoft.AspNetCore.Mvc;

[Route("start")]
[ApiController]
public class StartController : ControllerBase
{
    readonly IBus _bus;
    readonly IEndpointNameFormatter _formatter;

    public StartController(IBus bus, IEndpointNameFormatter formatter)
    {
        _bus = bus;
        _formatter = formatter;
    }

    [Route("1")]
    [HttpPost]
    public async Task StartSlip1()
    {
        // Typically I wrap this in a custom itinerary builder class
        var builder = new RoutingSlipBuilder(NewId.NextGuid());
        var address1 = new Uri($"exchange:{_formatter.ExecuteActivity<ActivityOne, ExecuteActivityOne>()}");
        builder.AddVariable("AccountId", "account-0");
        builder.AddActivity("Activity1", address1, new
        {
            // AccountId = "account-1"
        });
        builder.AddActivity("Activity2", address1, new
        {
            AccountId = "account-2"
        });
        builder.AddActivity("Activity3", address1, new
        {
            AccountId = "account-3"
        });

        var slip = builder.Build();
        await _bus.Execute(slip);
    }

    [Route("2")]
    [HttpPost]
    public async Task StartSlip2()
    {
        // Typically I wrap this in a custom itinerary builder class
        var builder = new RoutingSlipBuilder(NewId.NextGuid());
        var address2 = new Uri($"exchange:{_formatter.ExecuteActivity<ActivityTwo, ActivityTwoArgs>()}");
        builder.AddActivity("Activity1", address2, new
        {
            Name = "Dotnet"
        });
        builder.AddActivity("Activity1", address2, new
        {
            Name = "Dotnet2"
        });
        // builder.AddSubscription(new Uri("exchange:watch-the-throne"), RoutingSlipEvents.All, "");

        var slip = builder.Build();
        await _bus.Execute(slip);
    }

    [Route("3")]
    [HttpPost]
    public async Task StartSlip3()
    {
        // Typically I wrap this in a custom itinerary builder class
        var builder = new RoutingSlipBuilder(NewId.NextGuid());
        var address3 = new Uri($"exchange:{_formatter.ExecuteActivity<ActivityThree, ActivityThreeArgs>()}");
        var address3e = new Uri($"exchange:{_formatter.ExecuteActivity<ActivityThreeError, ActivityThreeArgs>()}");

        builder.AddActivity("Activity3e", address3);
        builder.AddActivity("Activity3e", address3e);

        var slip = builder.Build();
        await _bus.Execute(slip);
    }
}
