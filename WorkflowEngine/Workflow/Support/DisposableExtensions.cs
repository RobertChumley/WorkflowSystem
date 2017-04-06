using System;

namespace WorkflowEngine.Workflow.Support
{
    public static class DisposableExtensions
    {
        public static T DisposeStatement<T>(this IDisposable disposable,Func<IDisposable, T> executeAction)
        {
            using (disposable)
            {
                return executeAction(disposable);
            }
        }
    }
}