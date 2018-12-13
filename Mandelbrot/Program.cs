using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Numerics;

namespace Mandelbrot
{
    class Program
    {
        static void Main(string[] args)
        {
            string filename = @"C:\Users\Steven\Desktop\Mandelbrot.png";
            int scale = 1600;
            int maxIterations = 80;

            // Dimensions are dimensions 3 x 2
            int width = 3 * scale;
            int height = 2 * scale;

            // Plot boundaries, 
            int r_start = -2;
            int r_end = 1;
            int i_start = -1;
            int i_end = 1;

            double r_increment = ((double)(r_end - r_start)) / (double)width;
            double i_increment = ((double)(i_end - i_start)) / (double)height;

            using (var bitmap = new Bitmap(width, height))
            {
                for (double r = r_start; r < r_end; r += r_increment)
                    for (double i = i_start; i < i_end; i += i_increment)
                    {
                        Complex c = new Complex(r, i);
                        int iterations = Mandelbrot(c, maxIterations);
                        int colorVal = 255 * iterations / maxIterations;
                        bitmap.SetPixel((int)((r - r_start) / 3 * width), (int)((i - i_start) / 2 * height), Color.FromArgb(colorVal, colorVal, colorVal));
                        
                    }
                bitmap.Save(filename, ImageFormat.Png);
            }
        }

        static int Mandelbrot(Complex c, int maxIterations)
        {
            Complex z = new Complex(0, 0);
            int n = 0;

            while (z.Magnitude <= 2 && n < maxIterations)
            {
                z = z * z + c;
                n++;
            }

            return n;
        }
    }
}
