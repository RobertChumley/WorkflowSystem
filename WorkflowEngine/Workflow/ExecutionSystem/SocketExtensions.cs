using System;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace WorkflowEngine.Workflow.ExecutionSystem
{
    public class ReceiveSocketAwaitable : SocketAwaitable
    {
        public ReceiveSocketAwaitable(SocketAsyncEventArgs eventArgs) : base(eventArgs)
        {

        }
        public new int GetResult()
        {
            if (MEventArgs.SocketError != SocketError.Success)
            {
                throw new SocketException((int)MEventArgs.SocketError);
            }
            return MEventArgs.BytesTransferred;
        }
        public new ReceiveSocketAwaitable GetAwaiter() { return this; }

    }
    public class SocketAwaitable : INotifyCompletion
    {
        private static readonly Action Sentinel = () => { };
        internal bool MWasCompleted;
        internal Action MContinuation;
        internal SocketAsyncEventArgs MEventArgs;

        public SocketAwaitable(SocketAsyncEventArgs eventArgs)
        {
            if (eventArgs == null) throw new ArgumentNullException(nameof(eventArgs));
            MEventArgs = eventArgs;
            eventArgs.Completed += delegate
            {
                (MContinuation ??
                    Interlocked.CompareExchange(ref MContinuation, Sentinel, null))?.Invoke();

            };
        }
        internal void Reset()
        {
            MWasCompleted = false;
            MContinuation = null;
        }
        public SocketAwaitable GetAwaiter() { return this; }

        public bool IsCompleted => MWasCompleted;
        public int ReadSize { get; set; }

        public void OnCompleted(Action continuation)
        {
            if (MContinuation == Sentinel ||
                Interlocked.CompareExchange(ref MContinuation, continuation, null) == Sentinel)
            {
                Task.Run(continuation);
            }
        }
        public void GetResult()
        {
            if (MEventArgs.SocketError != SocketError.Success)
                throw new SocketException((int)MEventArgs.SocketError);
        }
    }
    public static class SocketExtensions
    {
        public static ReceiveSocketAwaitable ReceiveAsync(this Socket socket,
            ReceiveSocketAwaitable awaitable)
        {
            awaitable.Reset();
            if (!socket.ReceiveAsync(awaitable.MEventArgs))
                awaitable.MWasCompleted = true;
            return awaitable;
        }
        public static SocketAwaitable SendAsync(this Socket socket,
            SocketAwaitable awaitable)
        {
            awaitable.Reset();
            if (!socket.SendAsync(awaitable.MEventArgs))
                awaitable.MWasCompleted = true;
            return awaitable;
        }
        public static SocketAwaitable ConnectAsync(this Socket socket,
            SocketAwaitable awaitable)
        {
            awaitable.Reset();
            if (!socket.ConnectAsync(awaitable.MEventArgs))
                awaitable.MWasCompleted = true;
            return awaitable;
        }
        
        // … 
    }
}
