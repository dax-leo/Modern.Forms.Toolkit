using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SkiaSharp;

namespace Modern.Forms.Demo.ScottPlot
{
    public static class ImageLoader
    {
        private static Dictionary<string, SKBitmap> _cache = new Dictionary<string, SKBitmap> ();
        private static string _defaultLocation = Path.Combine (AppDomain.CurrentDomain.BaseDirectory, "Images");

        public static SKBitmap Get (string filename)
        {
            if (!_cache.ContainsKey (filename))
                _cache.Add (filename, SKBitmap.Decode (Path.Combine (_defaultLocation, filename)));

            return _cache[filename];
        }

        public static SKImage GetImageFromPath (string filename)
        {
            if (!_cache.ContainsKey (filename))
                _cache.Add (filename, SKBitmap.Decode (filename));

            return SKImage.FromBitmap (_cache[filename]);
        }

        //public static SKImage Img_Chart1 {
        //    get { 
        //        return GetImageFromPath (@"C:\Users\edarleo\source\repos\private\dax\out_peek\SimpleTreeViewForm\Resources\lte_1.png"); 
        //    }
        //}

        //public static SKImage Img_Chart2 {
        //    get { return GetImageFromPath (@"C:\Users\edarleo\source\repos\private\dax\out_peek\SimpleTreeViewForm\Resources\lte_2.png"); }
        //}
    }
}
