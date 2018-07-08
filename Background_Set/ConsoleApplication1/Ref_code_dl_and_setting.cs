//From URL: https://codereview.stackexchange.com/questions/71730/change-desktop-background


using System;
using Newtonsoft.Json;
using System.Drawing;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using Microsoft.Win32;


namespace Background
{
    class Program_REF_CODE_DL_FROM_WEB_AND_SET
    {
        static void Main_dl_and_setting(String[] args)
        {
            String URL = getBackgroundURL();
            Image background = downloadBackground(URL + getResolutionExtension());
            saveBackground(background);
            setBackground(background, PicturePosition.Fill);
        }

        public static String getBackgroundURL()
        {
            using (WebClient webClient = new WebClient())
            {
                Console.WriteLine("Downloading JSON...");
                String jsonString = webClient.DownloadString("super secret URL");
                dynamic jsonObject = JsonConvert.DeserializeObject<dynamic>(jsonString);
                String backgroundURL = "image_url.com" + jsonObject.images[0].urlbase;
                Console.WriteLine("Downloaded JSON!\n");
                return backgroundURL;
            }
        }

        public static Boolean websiteExists(String URL)
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(URL) as HttpWebRequest;
                request.Method = "HEAD";
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                return (response.StatusCode == HttpStatusCode.OK);
            }
            catch { return false; }
        }

        public static String getResolutionExtension()
        {
            Rectangle resolution = new Rectangle(0, 0, 1920, 1080);
            //Rectangle resolution = Screen.PrimaryScreen.Bounds;
            String potentialURL = "_" + resolution.Width + "x" + resolution.Height + ".jpg";
            if (websiteExists(potentialURL)) return potentialURL;
            else return "_1920x1080.jpg";
        }

        public static Image downloadBackground(String URL)
        {
            Console.WriteLine("Downloading background...");
            HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(URL);
            HttpWebResponse httpWebReponse = (HttpWebResponse)httpWebRequest.GetResponse();
            Stream stream = httpWebReponse.GetResponseStream();
            Image background = Image.FromStream(stream);
            Console.WriteLine("Downloaded background!\n");
            return background;
        }

        public static String getBackgroundPath()
        {
            String directory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) + "/Backgrounds/";
            Directory.CreateDirectory(directory);
            return Path.Combine(directory, DateTime.Now.ToString("M-d-yyyy") + ".bmp");
        }

        public static Boolean saveBackground(Image background)
        {
            try
            {
                Console.WriteLine("Saving background...");
                background.Save(getBackgroundPath(), System.Drawing.Imaging.ImageFormat.Bmp);
                Console.WriteLine("Saved background!\n");
                return true;
            }
            catch { return false; }
        }

        public enum PicturePosition
        {
            Tile, Center, Stretch, Fit, Fill
        }

        internal sealed class NativeMethods
        {
            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            internal static extern int SystemParametersInfo(
                int uAction,
                int uParam,
                String lpvParam,
                int fuWinIni);
        }

        public static void setBackground(Image background, PicturePosition style)
        {
            Console.WriteLine("Setting background...");
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop",true);
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
            NativeMethods.SystemParametersInfo(SET_DESKTOP_BACKGROUND, 0, getBackgroundPath(), UPDATE_INI_FILE | SEND_WINDOWS_INI_CHANGE);
            Console.WriteLine("Set background!\n");
        }

    }
}
