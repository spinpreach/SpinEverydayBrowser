using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Diagnostics;
using System.Drawing;

namespace Spinpreach.SpinEverydayBrowser.Common
{
    public class ShapeMemory
    {

        public Point location { get; set; }
        public Size size { get; set; }

        public static void Save(Form form)
        {
            if (!Directory.Exists(directory())) { Directory.CreateDirectory(directory()); }
            form.WindowState = FormWindowState.Normal;
            var xs = new XmlSerializer(typeof(ShapeMemory));
            using (var fs = new FileStream(filePath(form), FileMode.Create))
            {
                var sm = new ShapeMemory();
                sm.location = new Point(form.Left, form.Top);
                sm.size = new Size(form.Width, form.Height);
                xs.Serialize(fs, sm);
                fs.Close();
            }
        }

        public static void Load(Form form)
        {
            if (!File.Exists(filePath(form))) return;
            form.StartPosition = FormStartPosition.Manual;
            var xs = new XmlSerializer(typeof(ShapeMemory));
            using (var fs = new FileStream(filePath(form), FileMode.Open))
            {
                var value = (ShapeMemory)xs.Deserialize(fs);
                form.Location = value.location;
                //https://msdn.microsoft.com/ja-jp/library/hw8kes41(v=VS.110).aspx
                switch (form.FormBorderStyle)
                {
                    case FormBorderStyle.None: break;
                    case FormBorderStyle.FixedSingle: break;
                    case FormBorderStyle.Fixed3D: break;
                    case FormBorderStyle.FixedDialog: break;
                    case FormBorderStyle.Sizable: form.Size = value.size; break;
                    case FormBorderStyle.FixedToolWindow: break;
                    case FormBorderStyle.SizableToolWindow: form.Size = value.size; break;
                    default: break;
                }
                fs.Close();
            }
        }

        private static string directory()
        {
            return (Path.Combine(Directory.GetCurrentDirectory(), new StackFrame().GetMethod().DeclaringType.Name));
        }

        private static string filePath(Form form)
        {
            return (Path.Combine(directory(), string.Format("{0}.xml", form.GetType().FullName)));
        }
    }
}