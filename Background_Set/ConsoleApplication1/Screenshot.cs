//ref:https://github.com/reterVision/win32-screencapture
//ref:https://social.msdn.microsoft.com/Forums/vstudio/en-US/79efecc4-fa6d-4078-afe4-bb1379bb968b/print-screen-in-c?forum=csharpgeneral
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Background
{
    class Screenshot
    {
        void PrintScreen() {
            Bitmap printscreen = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            Graphics g = Graphics.FromImage((Image)printscreen);//(printscreen as Image);        
            g.CopyFromScreen(0, 0, 0, 0, printscreen.Size);
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            printscreen.Save(@"C:\Temp\printscreen.jpg", ImageFormat.Jpeg);
        }
    }
}
