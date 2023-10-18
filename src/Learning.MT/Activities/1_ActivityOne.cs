namespace Learning.MT.Activities;

using MassTransit;

public record ExecuteActivityOne(string AccountId);

public class ActivityOne : IExecuteActivity<ExecuteActivityOne>
{
    readonly ILogger<ActivityOne> _logger;

    public ActivityOne(ILogger<ActivityOne> logger)
    {
        _logger = logger;
    }

    public async Task<ExecutionResult> Execute(ExecuteContext<ExecuteActivityOne> context)
    {
        var args = context.Arguments;

        _logger.LogInformation("Account: {A}", args.AccountId);

        await Task.Delay(1);

        return context.CompletedWithVariables(new
        {
            Avp = "123"
        });
    }
}
