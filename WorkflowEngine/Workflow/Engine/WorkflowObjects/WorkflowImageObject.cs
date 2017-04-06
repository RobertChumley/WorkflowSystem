using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace WorkflowEngine.Workflow.Engine.WorkflowObjects
{
    public class WorkflowImageObject
    {
        public WorkflowImageObject(byte[] dataBytes)
        {
            DataBytes = dataBytes;
        }
        public byte[] DataBytes  { get; }

        public Image LoadFromFile(string url)
        {
            return Image.FromFile(url);
        }
        public string GetBase64String(Image image)
        {
            return image.ToBase64String();
        }
        public Image GetImage()
        {
            return DataBytes.ToImage();
        }

    }
}
