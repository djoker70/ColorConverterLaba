using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace VorobyevColorConverter
{
    static class Program
    {
        public struct HSVcolor
        {
            public ushort h;
            public byte s;
            public byte v;

            public HSVcolor(ushort _h, byte _s, byte _v)
            {
                h = _h;
                s = _s;
                v = _v;
            }
        }

        public class ColorConvert
        {
            public Bitmap rgbImage;
            public HSVcolor[,] hsvImage;

            byte clamp(byte value, byte min, byte max)
            {
                return Math.Min(Math.Max(value,min),max);
            }

            ushort clamp(ushort value, ushort min, ushort max)
            {
                return Math.Min(Math.Max(value, min), max);
            }

            public HSVcolor RGBtoHCV (Color source)
            {
                float eps = 0.00001f;
                ushort hue = 0;
                byte saturation = 0, value = 0;
                float R_ = source.R / 255f;
                float G_ = source.G / 255f;
                float B_ = source.B / 255f;
                float Cmax = Math.Max(R_, Math.Max(G_, B_));
                float Cmin = Math.Min(R_, Math.Min(G_, B_));
                float delta = Cmax - Cmin;

                value = (byte)(100 * Cmax);

                if (Cmax < eps)
                    saturation = 0;
                else
                    saturation = (byte)(100 * delta / Cmax);

                if (delta < eps)
                    hue = 0;
                else if (Cmax == R_)
                    hue = (ushort)(60f * (((G_ - B_) / delta) % 6f));
                else if (Cmax == G_)
                    hue = (ushort)(60f * (((B_ - R_) / delta) + 2f));
                else if (Cmax == B_)
                    hue = (ushort)(60f * (((R_ - G_) / delta) + 4f));
                
                return new HSVcolor(clamp(hue, 0, 359), saturation, value);
            }


            public Color HSVtoRGB(HSVcolor source)
            {
                float R_ = 0f, G_ = 0f, B_ = 0f;
                float C = source.s * source.v / 100f / 100f;
                float X = C * (1 - Math.Abs((source.h / 60f)%2f - 1));
                float m = source.v / 100f - C;

                if(source.h < 60)
                { R_ = C; G_ = X; B_ = 0f; }
                else if (source.h < 120)
                { R_ = X; G_ = C; B_ = 0f; }
                else if (source.h < 180)
                { R_ = 0f; G_ = C; B_ = X; }
                else if (source.h < 240)
                { R_ = 0f; G_ = X; B_ = C; }
                else if (source.h < 300)
                { R_ = X; G_ = 0f; B_ = C; }
                else if (source.h < 360)
                { R_ = C; G_ = 0f; B_ = X; }

                byte R = (byte)(255f * (R_ + m));
                byte G = (byte)(255f * (G_ + m));
                byte B = (byte)(255f * (B_ + m));

                return Color.FromArgb(clamp(R,0,255), clamp(G, 0, 255), clamp(B, 0, 255));
            }
            public void ConvertRGBimagetoHSV()
            {
                int width = rgbImage.Width;
                int height = rgbImage.Height;
                hsvImage = new HSVcolor[width, height];
                for(int i = 0; i < width; i++)
                    for(int j = 0; j < height; j++)
                    {
                        hsvImage[i, j] = RGBtoHCV(rgbImage.GetPixel(i,j));
                    }
            }

            public void ConvertHSVimagetoRGB()
            {
                int width = hsvImage.GetLength(0);
                int height = hsvImage.GetLength(1);
                rgbImage = new Bitmap(width, height);
                for (int i = 0; i < width; i++)
                    for (int j = 0; j < height; j++)
                    {
                        rgbImage.SetPixel(i,j, HSVtoRGB(hsvImage[i,j]));
                    }
            }
        }
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
