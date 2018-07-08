//From URL: https://codereview.stackexchange.com/questions/60755/set-desktop-background
using System;
using Newtonsoft.Json;
using System.Drawing;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using Microsoft.Win32;



namespace Background
{
    class Program_REF_CODE_SET_BCKGROUND
    {
        static void Main_setting(string[] args)
        {
            using (WebClient webClient = new WebClient())
            {
                Rectangle resolution = new Rectangle(0, 0, 1920, 1080);//Screen.PrimaryScreen.Bounds;
                string json = webClient.DownloadString("super secret website url");
                dynamic results = JsonConvert.DeserializeObject<dynamic>(json);
                string url = "domain.com" + results.images[0].urlbase;
                if (resolution.Width <= 1920 && resolution.Height <= 1200)
                {
                    url += String.Format("_{0}x{1}.jpg", resolution.Width, resolution.Height);
                }
                else
                {
                    url += "_1920x1200.jpg";
                }
                DesktopBackground desktopBackground = new DesktopBackground();
                desktopBackground.Set(url, PicturePosition.Fill);
            }
        }
    }
    public enum PicturePosition
    {
        Tile, Center, Stretch, Fit, Fill
    }
    public class DesktopBackground
    {
        public DesktopBackground() { }
        const int SET_DESKTOP_BACKGROUND = 20;
        const int UPDATE_INI_FILE = 1;
        const int SEND_WINDOWS_INI_CHANGE = 2;
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);
        public void Set(string URL, PicturePosition style)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(URL);
            HttpWebResponse httpWebReponse = (HttpWebResponse)httpWebRequest.GetResponse();
            Stream stream = httpWebReponse.GetResponseStream();
            Image backgroundImage = Image.FromStream(stream);
            int backgroundNumber = 0;
            string directory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) + "/Backgrounds/";
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            string fullBackgroundFilePath = Path.Combine(directory, String.Format("background{0}.bmp", backgroundNumber));
            while (System.IO.File.Exists(fullBackgroundFilePath))
            {
                backgroundNumber++;
                fullBackgroundFilePath = Path.Combine(directory, String.Format("background{0}.bmp", backgroundNumber));
            }
            backgroundImage.Save(fullBackgroundFilePath, System.Drawing.Imaging.ImageFormat.Bmp);
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true);
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
            SystemParametersInfo(SET_DESKTOP_BACKGROUND, 0, fullBackgroundFilePath, UPDATE_INI_FILE | SEND_WINDOWS_INI_CHANGE);
        }
    }
}
