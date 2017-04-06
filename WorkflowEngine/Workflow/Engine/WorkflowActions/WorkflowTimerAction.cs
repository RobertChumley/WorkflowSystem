using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using WorkflowEngine.Workflow.Model.Types;
using WorkflowEngine.Workflow.Model.WorkflowActions;

namespace WorkflowEngine.Workflow.Engine.WorkflowActions
{
    public class WorkflowTimerAction : WorkflowAction
    {
        private CancellationTokenSource _cancellationTokenSource;
        public WorkflowTimerAction(
            Func<WorkflowActionBaseConfig> workflowActionConfiguration) : base(workflowActionConfiguration)
        {   
        }
        public Func<WorkflowAction> Execute(Func<WorkflowAction, Task> action,
            CancellationTokenSource cancellationTokenSource)
        {
            StartRunner(action, cancellationTokenSource);
            return () => WorkflowActionRegistry()[WorkflowActionConfiguration().NextAction];
        }

        public void Cancel()
        {
            _cancellationTokenSource.Cancel();
        }
        public void StartRunner(Func<WorkflowAction, Task> action, CancellationTokenSource cancellationTokenSource)
        {
            _cancellationTokenSource = cancellationTokenSource;
            var workflowTimerActionConfig = (WorkflowTimerActionConfig)WorkflowActionConfiguration();
            var token = cancellationTokenSource.Token;
            var currentIter = 0;
            do
            {
                try
                {
                    Task.Run(async () =>
                    {
                        await ProcessActionAsync(action, cancellationTokenSource);
                    }, token).Wait(token);
                    currentIter++;

                    if (workflowTimerActionConfig.Iterations > 0 && currentIter >= workflowTimerActionConfig.Iterations)
                        _cancellationTokenSource.Cancel();
                }
                catch (Exception e)
                {
                    Debug.WriteLine("<p>Error in Client</p> <p>Exception</p> <p>" + e.Message + "</p><p>" + e.StackTrace + "</p> ");
                }
            } while (!token.IsCancellationRequested);
        }
        private async Task ProcessActionAsync(Func<WorkflowAction, Task> action, CancellationTokenSource cancellationTokenSource)
        {   
            await action(WorkflowActionRegistry()[((WorkflowTimerActionConfig)WorkflowActionConfiguration()).TimerAction]);
            await Task.Delay(GetTickDelay().Milliseconds, cancellationTokenSource.Token);
        }
        private TimeSpan GetTickDelay()
        {
            var workflowTimerActionConfig = (WorkflowTimerActionConfig) WorkflowActionConfiguration();
            switch (workflowTimerActionConfig.TickType)
            {
                case TickType.Millisecond:
                    return DateTime.Now.AddMilliseconds(workflowTimerActionConfig.Tick).TimeOfDay;
                case TickType.Second:
                    return DateTime.Now.AddSeconds(workflowTimerActionConfig.Tick).TimeOfDay;
                case TickType.Minute:
                    return DateTime.Now.AddMinutes(workflowTimerActionConfig.Tick).TimeOfDay;
                case TickType.Hour:
                    return DateTime.Now.AddHours(workflowTimerActionConfig.Tick ).TimeOfDay;
                case TickType.Day:
                    return DateTime.Now.AddDays(workflowTimerActionConfig.Tick).TimeOfDay;
                case TickType.Week:
                    return DateTime.Now.AddDays(workflowTimerActionConfig.Tick  * 7).TimeOfDay;
                case TickType.Month:
                    return DateTime.Now.AddMonths(workflowTimerActionConfig.Tick).TimeOfDay;
                case TickType.Year:
                    return DateTime.Now.AddYears(workflowTimerActionConfig.Tick).TimeOfDay;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}