using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Numerics;

namespace Mandelbrot
{
    class Program
    {
        static void Main(string[] args)
        {

            string filename = @"C:\Users\Steven\Desktop\Lorenz.png";
            int maxIterations = 300000;
            bool inverted = false;

            int width = 1920;
            int height = 1080;

            // Plot boundaries, 
            int x_start = -30;
            int x_end = 30;
            int y_start = -30;
            int y_end = 30;
            int z_start = -1;
            int z_end = 100;

            int x_span = x_end - x_start;
            int y_span = y_end - y_start;
            int z_span = z_end - z_start;

            using (var bitmap = new Bitmap(width, height))
            {
                InitBitmap(inverted, width, height, bitmap);
                List<Vector3> pointsInSet = GetLorenzSet(maxIterations);
                foreach (var point in pointsInSet)
                {
                    if (point.X < x_start || point.X > y_end)
                        continue;
                    if (point.Y < y_start || point.Y > y_end)
                        continue;

                    var xpos = (point.X - x_start) / x_span * width;
                    var ypos = (point.Y - y_start) / y_span * height;
                    int colorVal = DetermineColor(inverted, z_start, z_span, point);
                    bitmap.SetPixel((int)xpos, (int)ypos, Color.FromArgb(colorVal, colorVal, colorVal));
                }

                bitmap.Save(filename, ImageFormat.Png);
            }
        }

        private static int DetermineColor(bool inverted, int z_start, int z_span, Vector3 point)
        {
            var colorVal = (point.Z - z_start) / z_span * 255;
            colorVal = inverted ? colorVal : 255 - colorVal;
            colorVal = colorVal < 128 ? Math.Max(colorVal, 0) : Math.Min(colorVal, 255);
            return (int)colorVal;
        }

        private static void InitBitmap(bool inverted, int width, int height, Bitmap bitmap)
        {
            var bgColor = inverted ? Color.White : Color.Black;
            for (int xpos = 0; xpos < width; xpos++)
                for (int ypos = 0; ypos < height; ypos++)
                    bitmap.SetPixel(xpos, ypos, bgColor);
        }

        private static List<Vector3> GetLorenzSet(int maxIterations)
        {
            var x = 0.1f;
            var y = 0f;
            var z = 0f;
            var a = 10.0f;
            var b = 28.0f;
            var c = 8.0f / 3.0f;
            var t = 0.01f;

            List<Vector3> pointsInSet = new List<Vector3>(maxIterations + 1);
            //Iterate and update x,y and z locations
            //based upon the Lorenz equations
            for (int i = 0; i < maxIterations; i++)
            {
                x += t * a * (y - x);
                y += t * (x * (b - z) - y);
                z += t * (x * y - c * z);
                Vector3 lorenzPoint = new Vector3(x, y, z);
                pointsInSet.Add(lorenzPoint);
            }

            return pointsInSet;
        }
    }
}
