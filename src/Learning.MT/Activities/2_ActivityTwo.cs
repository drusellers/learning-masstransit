namespace Learning.MT.Activities;

using MassTransit;

public record ActivityTwoArgs(string Name);

public class ActivityTwo : IExecuteActivity<ActivityTwoArgs>
{
    readonly ILogger<ActivityTwo> _logger;
    readonly IEndpointNameFormatter _endpointNameFormatter;

    public ActivityTwo(ILogger<ActivityTwo> logger, IEndpointNameFormatter endpointNameFormatter)
    {
        _logger = logger;
        _endpointNameFormatter = endpointNameFormatter;
    }

    public async Task<ExecutionResult> Execute(ExecuteContext<ActivityTwoArgs> context)
    {
        await Task.Delay(1);

        var args = context.Arguments;

        _logger.LogInformation("Name: {Name}", args.Name);

        var optionalAddress = _endpointNameFormatter.ExecuteActivity<ActivityTwoOptional, ActivityTwoOptionalArgs>();

        return context.ReviseItinerary(builder =>
        {
            builder.AddActivitiesFromSourceItinerary();
            builder.AddActivity("Deviation", new Uri($"exchange:{optionalAddress}"));
        });
    }
}


public record ActivityTwoOptionalArgs;

public class ActivityTwoOptional : IExecuteActivity<ActivityTwoOptionalArgs>
{
    readonly ILogger<ActivityTwoOptional> _logger;

    public ActivityTwoOptional(ILogger<ActivityTwoOptional> logger)
    {
        _logger = logger;
    }

    public async Task<ExecutionResult> Execute(ExecuteContext<ActivityTwoOptionalArgs> context)
    {
        _logger.LogInformation("Optional Step");

        await Task.Delay(1);

        return context.Completed();
    }
}
