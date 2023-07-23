using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modern.Forms.Demo.HtmlRenderer
{
    internal static class ImageLoader
    {
        private static Dictionary<string, SKBitmap> _cache = new Dictionary<string, SKBitmap>();
        private static string _defaultLocation = "Images";

        public static SKBitmap Get(string filename)
        {
            if (!_cache.ContainsKey(filename.ToLowerInvariant()))
                _cache.Add(filename.ToLowerInvariant(), SKBitmap.Decode(Path.Combine(_defaultLocation, filename)));

            return _cache[filename.ToLowerInvariant()];
        }
    }
}
