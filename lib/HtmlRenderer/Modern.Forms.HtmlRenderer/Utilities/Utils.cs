// "Therefore those skilled at the unorthodox
// are infinite as heaven and earth,
// inexhaustible as the great rivers.
// When they come to an end,
// they begin again,
// like the days and months;
// they die and are reborn,
// like the four seasons."
// 
// - Sun Tsu,
// "The Art of War"

using SkiaSharp;
using System.Drawing;
using System.Globalization;
using TheArtOfDev.HtmlRenderer.Adapters.Entities;

namespace TheArtOfDev.HtmlRenderer.Avalonia.Utilities
{
    /// <summary>
    /// Utilities for converting Avalonia entities to HtmlRenderer core entities.
    /// </summary>
    public static class Utils
    {

        /// <summary>
        /// Convert from WinForms color to core color.
        /// </summary>
        public static RColor Convert(Color c)
        {
            return RColor.FromArgb(c.A, c.R, c.G, c.B);
        }

        public static RPoint ConvertInt(Point p)
        {
            return new RPoint(p.X, p.Y);
        }

        /// <summary>
        /// Convert from Avalonia point to core point.
        /// </summary>
        public static RPoint Convert(SKPoint p)
        {
            return new RPoint(p.X, p.Y);
        }

        /// <summary>
        /// Convert from Avalonia point to core point.
        /// </summary>
        public static SKPoint[] Convert(RPoint[] points)
        {
            SKPoint[] myPoints = new SKPoint[points.Length];
            for (int i = 0; i < points.Length; i++)
                myPoints[i] = Convert(points[i]);
            return myPoints;
        }

        /// <summary>
        /// Convert from core point to Avalonia point.
        /// </summary>
        public static SKPoint Convert(RPoint p)
        {
            return new SKPoint((float)p.X, (float)p.Y);
        }

        /// <summary>
        /// Convert from core point to Avalonia point.
        /// </summary>
        public static Point ConvertInt(RPoint p)
        {
            return new Point((int)p.X, (int)p.Y);
        }

        /// <summary>
        /// Convert from core point to Avalonia point.
        /// </summary>
        public static Point ConvertRound(RPoint p)
        {
            return new Point((int)p.X, (int)p.Y);
        }

        /// <summary>
        /// Convert from Avalonia size to core size.
        /// </summary>
        public static RSize Convert(SKSize s)
        {
            return new RSize(s.Width, s.Height);
        }

        /// <summary>
        /// Convert from core size to Avalonia size.
        /// </summary>
        public static SKSize Convert(RSize s)
        {
            return new SKSize((float)s.Width, (float)s.Height);
        }

        /// <summary>
        /// Convert from core point to Avalonia point.
        /// </summary>
        public static SKSize ConvertRound(RSize s)
        {
            return new SKSize((float)s.Width, (float)s.Height);
        }

        /// <summary>
        /// Convert from Avalonia rectangle to core rectangle.
        /// </summary>
        public static RRect Convert(RectangleF r)
        {
            return new RRect(r.X, r.Y, r.Width, r.Height);
        }

        /// <summary>
        /// Convert from core rectangle to Avalonia rectangle.
        /// </summary>
        public static RectangleF Convert(RRect r)
        {
            //return new System.Drawing.Rectangle((int)r.X, (int)r.Y, (int)r.Width, (int)r.Height);
            return new RectangleF((float)r.X, (float)r.Y, (float)r.Width, (float)r.Height);
            //return new Rect(r.X, r.Y, r.Width, r.Height);
        }

        /// <summary>
        /// Convert from Avalonia rectangle to core rectangle.
        /// </summary>
        public static RRect ConvertSK(SKRect r)
        {
            return new RRect(r.Left, r.Top, r.Width, r.Height);
        }

        /// <summary>
        /// Convert from core rectangle to Avalonia rectangle.
        /// </summary>
        public static SKRect ConvertSK(RRect r)
        {
            //return new System.Drawing.Rectangle((int)r.X, (int)r.Y, (int)r.Width, (int)r.Height);
            return new SKRect((float)r.Left, (float)r.Top, (float)r.Right, (float)r.Bottom);
            //return new Rect(r.X, r.Y, r.Width, r.Height);
        }

        /// <summary>
        /// Convert from core rectangle to Avalonia rectangle.
        /// </summary>
        //public static SKRect ConvertRound(RRect r)
        //{
        //    return new Modern.WindowKit.Rect((int)r.X, (int)r.Y, (int)r.Width, (int)r.Height);
        //}

        /// <summary>
        /// Convert from Avalonia color to core color.
        /// </summary>
        public static RColor Convert(SKColor c)
        {
            return RColor.FromArgb(c.Alpha, c.Red, c.Green, c.Blue);
        }

        /// <summary>
        /// Convert from core color to Avalonia color.
        /// </summary>
        public static SKColor Convert(RColor c)
        {
            return new SKColor(c.R, c.G, c.B, c.A);
        }
    }
}