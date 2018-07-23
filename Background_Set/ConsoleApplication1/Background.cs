using System;
//using Newtonsoft.Json;
using System.Drawing;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace Background
{
    //extended class to remember the filename/path as well as the image datatype in memory.
    class ImageExt
    {
        public Image Image { get; set; }
        public String Filename { get; set; }

        public ImageExt(string Filename)
        {
            this.Image = Image.FromFile(Filename);
            this.Filename = Filename;
        }
    }

    class Background
    {
        public void SetBackground(ImageExt background, PicturePosition style)
        {
            Console.WriteLine("Setting Wallpaper");

            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true);
            //PrintUserKeys(key);
            switch (style)
            {
                case PicturePosition.Tile:
                    key.SetValue(@"PicturePosition", "0");
                    key.SetValue(@"TileWallpaper", "1");
                    break;
                case PicturePosition.Center:
                    key.SetValue(@"PicturePosition", "0");
                    key.SetValue(@"TileWallpaper", "0");
                    break;
                case PicturePosition.Stretch:
                    key.SetValue(@"PicturePosition", "2");
                    key.SetValue(@"TileWallpaper", "0");
                    break;
                case PicturePosition.Fit:
                    key.SetValue(@"PicturePosition", "6");
                    key.SetValue(@"TileWallpaper", "0");
                    break;
                case PicturePosition.Fill:
                    key.SetValue(@"PicturePosition", "10");
                    key.SetValue(@"TileWallpaper", "0");
                    break;
            }
            key.Close();

            const int SET_DESKTOP_BACKGROUND = 20;
            const int UPDATE_INI_FILE = 1;
            const int SEND_WINDOWS_INI_CHANGE = 2;
            NativeMethods.SystemParametersInfo(SET_DESKTOP_BACKGROUND, 0, getBackgroundPath(background), UPDATE_INI_FILE | SEND_WINDOWS_INI_CHANGE);

            Console.WriteLine("Wallpaper set to Image at path ");
        }

        private static string getBackgroundPath(ImageExt background)
        {
            return background.Filename;
        }

        public enum PicturePosition
        {
            Tile,
            Center,
            Fill,
            Stretch,
            Fit
        }

        internal sealed class NativeMethods
        {
            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            internal static extern int SystemParametersInfo(
                int uAction,
                int uParam,
                String lpvParam,
                int fuWinIni
                );
        }
    }
}
