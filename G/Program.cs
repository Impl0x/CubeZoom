using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace G
{
    class Program
    {
        /// <summary>
        /// Entry point of this example.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            using (T03_Immediate_Mode_Cube example = new T03_Immediate_Mode_Cube())
            {
                example.Run();
            }
        }
    }
}
