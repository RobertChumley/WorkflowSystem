using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WorkflowEngine.Workflow.Model.WorkflowActions;
using WorkflowEngine.Workflow.ExecutionSystem;

namespace WorkflowEngine.Workflow.Engine.WorkflowActions
{
    public class WorkflowSendSocketAction : WorkflowAction
    {
        public WorkflowSendSocketAction(Func<WorkflowActionBaseConfig> workflowActionConfiguration)
            : base(workflowActionConfiguration)
        {
            WorkflowSendSocketActionConfig = (WorkflowSendSocketActionConfig) workflowActionConfiguration();
            _port = WorkflowSendSocketActionConfig.PortNumber;
        }

        public WorkflowSendSocketActionConfig WorkflowSendSocketActionConfig { get; set; }
        // The port number for the remote device.
        private readonly int _port;
        
        public Func<WorkflowAction> Execute()
        {
            
            // Establish the remote endpoint for the socket.
            // The name of the 
            ProtocolType ptype;
            if (!Enum.TryParse(WorkflowSendSocketActionConfig.Protocol, out ptype))
            {
                return () => WorkflowActionRegistry()[WorkflowActionConfiguration().NextAction];
            }

            try
            {
                SendMessage();
                //Task.Run(Sendata);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception " + e.Message);
            }


            //Task.Run(SendMessage).ConfigureAwait(false);
           
            return () => WorkflowActionRegistry()[WorkflowActionConfiguration().NextAction];
        }

        private async Task Sendata()
        {
            try
            {
                using (var socketClient = new Socket(SocketType.Stream, ProtocolType.Tcp))
                {
                    var ea = new SocketAsyncEventArgs
                    {
                        RemoteEndPoint = new DnsEndPoint(WorkflowSendSocketActionConfig.IPAddress, _port)
                    };
                    //socketClient.Connect(IPAddress.Parse(WorkflowSendSocketActionConfig.IPAddress), _port);
                    //IPAddress.Parse(WorkflowSendSocketActionConfig.IPAddress), _port
                    var connectAwaiter = new SocketAwaitable(ea);
                    await socketClient.ConnectAsync(connectAwaiter);
                    Debug.WriteLine("Connected");
                    
                    var send = new SocketAsyncEventArgs();
                    var dat = Encoding.ASCII.GetBytes("Hello World Again");
                    send.SetBuffer(dat,0, dat.Length);
                    var awaitable = new SocketAwaitable(send);
                    await socketClient.SendAsync(awaitable);
                    Debug.WriteLine("Sent Completed");

                    int recCount = 0;
                    var recBuffer = new byte[2048];
                    var receive = new SocketAsyncEventArgs();
                    receive.SetBuffer(recBuffer,0,recBuffer.Length);
                    var receiveAwaiter = new ReceiveSocketAwaitable(receive);

                    while ((recCount = await socketClient.ReceiveAsync(receiveAwaiter)) > 0)
                    {
                        Debug.WriteLine($"Received {recCount}");
                        Debug.WriteLine(
                            $"Read through the buffer: {Encoding.ASCII.GetString(recBuffer, 0, recCount)}");
                        recBuffer = new byte[2048];
                    }
                    //while (!socketClient.Connected){}
                    /*var cancel = new CancellationToken();
                    using (var networkStream = new NetworkStream(socketClient))
                    {
                        networkStream.ReadTimeout = 1000;
                        networkStream.WriteTimeout = 1000;
                        var dat = Encoding.ASCII.GetBytes("Hello World Again");
                        await networkStream.WriteAsync(dat, 0, dat.Length, cancel);
                        Debug.WriteLine("Writing");
                        int recCount = 0;
                        var recBuffer = new byte[2048];
                        while ((recCount = await networkStream.ReadAsync(recBuffer, 0, 2048, cancel)) > 0)
                        {
                            Debug.WriteLine($"Received {recCount}");
                            Debug.WriteLine(
                                $"Read through the buffer: {Encoding.ASCII.GetString(recBuffer, 0, recCount)}");
                            recBuffer = new byte[2048];
                        }
                        
                    }*/
                    //socketClient.Shutdown(SocketShutdown.Send);
                    //socketClient.Shutdown(SocketShutdown.Both);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                //await Task.FromException(e);
            }
            
        }
        private static ManualResetEvent connectDone =
       new ManualResetEvent(false);
        private static ManualResetEvent sendDone =
            new ManualResetEvent(false);
        private static ManualResetEvent receiveDone =
            new ManualResetEvent(false);
        private void SendMessage()
        {
            
            using (var client = new TcpClient(WorkflowSendSocketActionConfig.IPAddress, _port))
            {
                try
                {
                    var dat = Encoding.ASCII.GetBytes("Hello World");
                    using (var stream = client.GetStream())
                    {       
                        
                        stream.BeginWrite(dat, 0, dat.Length, (res) =>
                        {
                            Debug.WriteLine("Finished writing");
                            var outBuffer = new byte[2048];
                            NetworkStream rstream = (NetworkStream)res.AsyncState;
                            rstream.EndWrite(res);
                            rstream.BeginRead(outBuffer, 0, 2048,
                                (ires) =>
                                {
                                    var irstream = (NetworkStream) ires.AsyncState;
                                    var count = irstream.EndRead(ires);
                                    Debug.WriteLine($"REad {count} bytes");
                                    Debug.WriteLine($"Read through the buffer: {Encoding.ASCII.GetString(outBuffer, 0, count)}");
                                    receiveDone.Set();
                                },
                                rstream);
                            receiveDone.WaitOne();
                            sendDone.Set();
                        }, stream);
                        sendDone.WaitOne();
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
            }
        }

        public void WriteData(byte[] data, int length )
        {
            
        }
    }
    
   
}
