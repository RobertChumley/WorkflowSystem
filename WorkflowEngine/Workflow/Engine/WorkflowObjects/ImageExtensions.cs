using System;
using System.Drawing;
using System.IO;
using WorkflowEngine.Workflow.Support;

namespace WorkflowEngine.Workflow.Engine.WorkflowObjects
{
    public static class ImageExtensions
    {
        public static Image ToImage(this byte[] bytes)
        {
            return new MemoryStream(bytes,false).DisposeStatement((stream)=> ((MemoryStream)stream).ToImage());
        }
        public static Image ToImage<T>(this T stream) where T : Stream
        {
            return Image.FromStream(stream);
        }
        public static string ToBase64String(this Image image)
        {
            return new MemoryStream().DisposeStatement((ms) =>
                    Convert.ToBase64String(image.SaveImageToStream((MemoryStream)ms).ToArray()));
        }
        public static T SaveImageToStream<T>(this Image image, T ms) where T:Stream
        {
            image.Save(ms, image.RawFormat);
            return ms;
        }
    }
}