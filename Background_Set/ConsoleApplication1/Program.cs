using System;

namespace Background
{
    class Program
    {
        static void Main(string[] args)
        {
            Screenshot scr = new Screenshot();
            Background bg = new Background();
            ImageExt img = new ImageExt(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) + "\\Wallpapers\\HigennoAce-1.jpg");//Image.FromFile(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) + "\\Wallpapers\\HigennoAce-1.jpg");

            bg.SetBackground(img, Background.PicturePosition.Fill);

            Console.ReadKey();
        }
       
    }
}
