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

    class Program
    {
        static void Main(string[] args)
        {
            ImageExt Background = new ImageExt(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) + "\\Wallpapers\\HigennoAce-1.jpg");//Image.FromFile(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) + "\\Wallpapers\\HigennoAce-1.jpg");
            SetBackground(Background, PicturePosition.Fill);
        }

        private static void SetBackground(ImageExt background, PicturePosition style)
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

            Console.ReadKey();
        }

        private static string getBackgroundPath(ImageExt background)
        {
            return background.Filename;
        }

        static void PrintUserKeys(RegistryKey key)
        {
            String[] names = key.GetSubKeyNames();

            int icount = 0;

            const string userRoot = "HKEY_CURRENT_USER";

            Console.WriteLine("subkeys for current user are ");
            Console.WriteLine("------------------------------");

            foreach (String s in names)
            {
                Console.WriteLine(s + Registry.CurrentUser.GetValue(s,"\tNone"));
                //TODO: figure out how to print all user subkey values
                icount++;
                if(icount > 10)
                {
                    break;
                }
            }
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
