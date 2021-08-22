using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace AirTransportationModern.Classes
{
    public static class CapchaClass
    {
        public static string TextCapcha = String.Empty;
        public static string GenerateText(string value,int count)
        {
            Random rnd = new Random();
            string text = String.Empty;
            for (int i = 0; i < count; i++)
                text += value[rnd.Next(value.Length)];
            return text;
        }
        public static Bitmap SetBitmap(int Width, int Height)
        {
            Random rnd = new Random();
            Bitmap result = new Bitmap(Width, Height);
            int Xpos = rnd.Next(0, Width - 50);
            int Ypos = rnd.Next(15, Height - 15);
            System.Drawing.Brush[] colors = { System.Drawing.Brushes.Black };
            Graphics g = Graphics.FromImage((System.Drawing.Image)result);
            g.Clear(System.Drawing.Color.Gray);
            TextCapcha = GenerateText("1234567890QWERTYUIOPASDFGHJKLZXCVBNM",5);
            g.DrawString(TextCapcha, new Font("Arial", 15), colors[rnd.Next(colors.Length)], new PointF(Xpos, Ypos));
            g.DrawLine(Pens.Black, new System.Drawing.Point(0, 0), new System.Drawing.Point(Width - 1, Height - 1));
            g.DrawLine(Pens.Black, new System.Drawing.Point(0, Height - 1), new System.Drawing.Point(Width - 1, 0));
            for (int i = 0; i < Width; ++i)
                for (int j = 0; j < Height; ++j)
                    if (rnd.Next() % 20 == 0)
                        result.SetPixel(i, j, System.Drawing.Color.White);
            return result;
        }
        public static BitmapImage ConvertS(System.Drawing.Bitmap src)
        {
            MemoryStream ms = new MemoryStream();
            ((System.Drawing.Bitmap)src).Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();
            return image;
        }
    }
}
