using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using WorkflowEngine.Workflow.ExecutionSystem;
using WorkflowEngine.Workflow.Model.WorkflowListeners;

namespace WorkflowEngine.Workflow.Listeners
{
    //ToDo: SOcket server?  SHould be a way for devices to regester with the central socket server.
    public class WorkflowSocketListener : WorkflowListenerBase
    {
        public WorkflowSocketListener(Func<WorkflowListenerConfigBase> listenerBaseConfigFunc) : base(listenerBaseConfigFunc)
        {
            WorkflowSocketListenerConfig = (WorkflowSocketListenerConfig) listenerBaseConfigFunc();
        }

        public WorkflowSocketListenerConfig WorkflowSocketListenerConfig { get; set; }

        public async Task RegisterListener()
        {
            await Run((data,count) =>
            {
                var retBytes = new byte[count];
                Console.WriteLine($"Reading bytes {count}");
                Array.Copy(data,retBytes,count);
                return retBytes;
            } );
        }
        public async Task Run(Func<byte[], int,byte[]> dataManager)
        {
            await IPAddress
                .Parse(WorkflowSocketListenerConfig.IPAddress)
                .ListenToAddress(WorkflowSocketListenerConfig.PortNumber,async(listener) =>
                {
                    using (var socket = await listener.AcceptSocketAsync()) {
                        var args = new SocketAsyncEventArgs();
                        args.SetBuffer(new byte[0x1000], 0, 0x1000);
                        var awaitable = new ReceiveSocketAwaitable(args);
                        int bytesRead;
                        while ((bytesRead = await socket.ReceiveAsync(awaitable)) > 0)
                        {
                            var args2 = new SocketAsyncEventArgs();
                            args2.SetBuffer(args.Buffer, 0, bytesRead);
                            var awaiter = new SocketAwaitable(args2);
                            await socket.SendAsync(awaiter);
                        }
                        socket.Close();
                    }
                }).ConfigureAwait(false);

        }
    }
    public static class NetworkStreamExtensions
    {
        public static async Task ListenToAddress(this IPAddress address,int port, Func<TcpListener, Task> processor)
        {
            await new TcpListener(address , port).LoopUntil(processor);
        }
        public static async Task LoopUntil(this TcpListener listener,Func<TcpListener,Task> datLoop)
        {
            listener.Start();
            while (true)
            {
                await datLoop(listener);
            }
        }
        public static async Task<NetworkStream> Process(this Socket tcpClient, Func<byte[], int, byte[]> dataManager)
        {
            return await tcpClient.CarefullyExecute(
                async (client) =>
                {
                    if (tcpClient.Connected)
                    {
                        using (var nstream = new NetworkStream(tcpClient, false))
                        {
                            await nstream.HandleRequests(dataManager);
                            //nstream.Close();
                        }
                    }
                    return null;

                }
                , (error) =>
                {
                    Console.WriteLine(error.StackTrace);
                    Console.WriteLine(error.Message);

                });
        }

        public static async Task<NetworkStream> HandleRequests(this NetworkStream stream, Func<byte[], int, byte[]> dataManager)
        {
            await stream.LoopWithCondition((nstream) => nstream.DataAvailable, 
                async (bytes,length) =>
                {
                    await stream.WriteBufferAsync(dataManager(bytes, length));
                });
            return stream;
        }

        public static async Task LoopWithCondition(this NetworkStream stream, Func<NetworkStream,bool> loopCond, Func<byte[],int,Task> asyncTask)
        {
            var buffer = new byte[2048];
            int length;
            await Task.Delay(100);
            while (loopCond(stream) &&  (length = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
            {
                Console.WriteLine($"Reading Data {length}");
                await asyncTask(buffer,length);
                buffer = new byte[2048];
            }
        }
        public static async Task<NetworkStream> WriteBufferAsync(this NetworkStream stream, byte[] data)
        {
            await stream.WriteAsync(data, 0, data.Length);
            return stream;
        }
        public static async Task<NetworkStream> CarefullyExecute(this Socket obj, Func<Socket,Task<NetworkStream>> func,Action<Exception> handler)
        {
            try
            {
                return await func(obj);
            }
            catch (Exception e)
            {
                handler(e);
            }
            return null;
        }

        

        private static void ReadComplete(IAsyncResult ar)
        {
            throw new NotImplementedException();
        }



    }

 
}
