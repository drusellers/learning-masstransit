namespace Learning.MT.Activities;

using MassTransit;


public record ActivityThreeArgs;

public record ActivityThreeLog(string Message);


public class ActivityThree : IActivity<ActivityThreeArgs, ActivityThreeLog>
{
    readonly ILogger<ActivityThree> _logger;

    public ActivityThree(ILogger<ActivityThree> logger)
    {
        _logger = logger;
    }

    public async Task<ExecutionResult> Execute(ExecuteContext<ActivityThreeArgs> context)
    {
        await Task.Delay(1);

        return context.Completed(new ActivityThreeLog("Yup"));
    }

    public async Task<CompensationResult> Compensate(CompensateContext<ActivityThreeLog> context)
    {
        await Task.Delay(1);

        var log = context.Log;
        _logger.LogInformation("Compensation: {X}", log.Message);

        return context.Compensated();
    }
}

public class ActivityThreeError : IActivity<ActivityThreeArgs, ActivityThreeLog>
{
    readonly ILogger<ActivityThreeError> _logger;

    public ActivityThreeError(ILogger<ActivityThreeError> logger)
    {
        _logger = logger;
    }

    public async Task<ExecutionResult> Execute(ExecuteContext<ActivityThreeArgs> context)
    {
        await Task.Delay(1);

        _logger.LogError("ERROR");
        // let's pretend to error
        return context.Faulted();
    }

    public async Task<CompensationResult> Compensate(CompensateContext<ActivityThreeLog> context)
    {
        await Task.Delay(1);

        var log = context.Log;
        _logger.LogInformation("Compensation 2: {X}", log.Message);

        return context.Compensated();
    }
}
