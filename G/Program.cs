using System;

namespace CubeZoom
{
    class Program
    {
        [STAThread]
        public static void Main()
        {
            using (T03_Immediate_Mode_Cube cube = new T03_Immediate_Mode_Cube())
            {
                cube.Run();
            }
        }
    }
}
