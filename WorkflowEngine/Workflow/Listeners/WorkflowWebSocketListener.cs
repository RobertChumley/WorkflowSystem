using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using KafkaNet.Common;
using WorkflowEngine.Workflow.Model.WorkflowListeners;

namespace WorkflowEngine.Workflow.Listeners
{
    public class WorkflowWebSocketListener : WorkflowListenerBase
    {
        public WorkflowWebSocketListener(Func<WorkflowListenerConfigBase> listenerBaseConfigFunc)
            : base(listenerBaseConfigFunc)
        {
            WorkflowWebSocketListenerConfig = (WorkflowWebSocketListenerConfig)listenerBaseConfigFunc();
        }
        public WorkflowWebSocketListenerConfig WorkflowWebSocketListenerConfig { get; set; }
        public Func<NetworkStream>  RegisterListener()
        {
            var ipAddress = IPAddress.Parse(WorkflowWebSocketListenerConfig.IPAddress);
            var server = new TcpListener(ipAddress, WorkflowWebSocketListenerConfig.PortNumber);
            server.Start();
            Console.WriteLine("Server has started on 127.0.0.1:80.{0} Waiting for a connection...", Environment.NewLine);
            return () =>
            {
                var client = server.AcceptTcpClient();
                var stream = client.GetStream();
                //enter to an infinite cycle to be able to handle every change in stream
                while (true)
                {
                    if(!stream.DataAvailable) continue;

                    var bytes = new byte[client.Available];
                    stream.Read(bytes, 0, bytes.Length);
                    var data = Encoding.UTF8.GetString(bytes);
                    
                    if (new Regex("^GET").IsMatch(data))
                    {
                        var response = Encoding.UTF8.GetBytes("HTTP/1.1 101 Switching Protocols" + Environment.NewLine
                                + "Connection: Upgrade" + Environment.NewLine
                                + "Upgrade: websocket" + Environment.NewLine
                                + "Sec-WebSocket-Accept: " + Convert.ToBase64String(
                                    SHA1.Create().ComputeHash(
                                        Encoding.UTF8.GetBytes(
                                            new Regex("Sec-WebSocket-Key: (.*)").Match(data).Groups[1].Value.Trim() +
                                            "258EAFA5-E914-47DA-95CA-C5AB0DC85B11"
                                        )
                                    )
                                ) + Environment.NewLine
                                + Environment.NewLine);   
                        stream.Write(response, 0, response.Length);
                    }
                    else
                    {
                        //ToDo: Listen for objects, serialize, then send to the endpoint

                        var length = bytes[1]-128;
                        var MASK = new []{ bytes[2],bytes[3],bytes[4],bytes[5]};
                        var decoded = new byte[length];
                        for (var i = 0; i < length; i++)
                        {
                            decoded[i] = (byte) (bytes[i + 6] ^ MASK[i % 4]);
                        }
                        SendTextMessage("ImageStart", stream);
                        var rawImage = Image.FromFile("C:/test/test.jpg");
                        Console.WriteLine($"Image size w:{rawImage.Size.Width} h: {rawImage.Size.Height}");
                        SendFracturedString(rawImage, 64000,stream);
                        SendTextMessage("ImageEnd", stream);
                    }
                }
            };
            
        }

        private void SendFracturedString(Image image, int i, NetworkStream stream)
        {
            var sent = 0;
            var data = ImageToBase64(image, ImageFormat.Jpeg);
            var length = data.Length;
            
            while (sent < length)
            {
                var piece = data.Substring(sent, sent + i > length ? length - sent : i);
                SendTextMessage(piece, stream);
                sent += piece.Length;
            }
        }

        private void SendFractured(byte[] image, int i, NetworkStream stream)
        {
            var sent = 0;
            var length = image.Length;
            while (sent < length)
            {   var piece = new byte[sent + i > length ? length - sent : i];
                Array.Copy(piece, 0, image, sent, piece.Length);
                SendByteArray(piece, stream,2);
                sent += piece.Length;
            }
        }
        public string ImageToBase64(Image image, System.Drawing.Imaging.ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // Convert Image to byte[]
                image.Save(ms, format);
                byte[] imageBytes = ms.ToArray();

                // Convert byte[] to Base64 String
                string base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }
        }
        private void SendTextMessage(string text, NetworkStream stream)
        {
            SendByteArray((text).ToBytes(),stream);
            //var rawImage = ("Hey Mom its me this is great hte the constitution of the united states to herby form a more perfect union do unduely plan to have really long and boring conversations about abosolutly positiviely nothing.").ToBytes();
            
            
        }

        private void SendByteArray(byte[] data, NetworkStream stream,int opcode = 1)
        {
            var rawImage = data;
            byte[] frame = new byte[10];
            frame[0] = (byte)(128+ opcode);
            int indexStartRawData;
            if (rawImage.Length <= 125)
            {
                indexStartRawData = 2;
                frame[1] = (byte)(rawImage.Length);

            }
            else if (rawImage.Length >= 126 && rawImage.Length <= 65535)
            {
                indexStartRawData = 4;
                frame[1] = (byte)126;
                frame[2] = (byte)((rawImage.Length >> 8) & 255);
                frame[3] = (byte)((rawImage.Length) & 255);
            }
            else
            {
                indexStartRawData = 10;
                frame[1] = 127;
                frame[2] = (byte)((rawImage.Length >> 56) & 255);
                frame[3] = (byte)((rawImage.Length >> 48) & 255);
                frame[4] = (byte)((rawImage.Length >> 40) & 255);
                frame[5] = (byte)((rawImage.Length >> 32) & 255);
                frame[6] = (byte)((rawImage.Length >> 24) & 255);
                frame[7] = (byte)((rawImage.Length >> 16) & 255);
                frame[8] = (byte)((rawImage.Length >> 8) * 255);
                frame[9] = (byte)((rawImage.Length) & 255);

            }
            try
            {
                var response = new byte[indexStartRawData + rawImage.Length];
                Array.Copy(frame, 0, response, 0, indexStartRawData);
                Array.Copy(rawImage, 0, response, indexStartRawData, rawImage.Length);
                //var response = Encoding.UTF8.GetBytes(System.Text.Encoding.Default.GetString(decoded));
                Console.WriteLine($"Sending {response.Length} bytes");
                stream.Write(response, 0, response.Length);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }
    }
}
