using Modern.WindowKit.X11;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheArtOfDev.HtmlRenderer.Core.Entities;

namespace Modern.Forms.Demo.HtmlRenderer
{
    public class HtmlRenderingHelper
    {
        #region Fields/Consts

        /// <summary>
        /// Cache for resource images
        /// </summary>
        private readonly Dictionary<string, SKBitmap> _imageCache = new Dictionary<string, SKBitmap>(StringComparer.OrdinalIgnoreCase);

        private string _defaultLocation;

        public string DefaultLocation { get => _defaultLocation; internal set => _defaultLocation = value; }

        #endregion

        public HtmlRenderingHelper(string images_dir) {
            _defaultLocation = images_dir;
        }

        /// <summary>
        /// Create image to be used to fill background so it will be clear that what's on top is transparent.
        /// </summary>
        public static SKBitmap CreateImageForTransparentBackground()
        {
            var image = new SKBitmap(10, 10);
            using (var g = new SKCanvas(image))
            {
                g.Clear(SKColors.White);
                g.FillRectangle(new Rectangle(0, 0, 5, 5), Theme.FormBackgroundColor);
                g.FillRectangle(new Rectangle(5, 5, 5, 5), Theme.FormBackgroundColor);
            }
            return image;
        }

        /// <summary>
        /// Get image by resource key.
        /// </summary>
        public SKBitmap TryLoadResourceImage(HtmlImageLoadEventArgs e)
        {
            if (!_imageCache.ContainsKey(e.Src))
            {
                var path = Path.Combine(_defaultLocation, e.Src + ".png");
                var bmp = SKBitmap.Decode(path);
                _imageCache.Add(e.Src, bmp);
            }

            return _imageCache[e.Src];
        }

        /// <summary>
        /// on image load in renderer set the image by event async.
        /// </summary>
        public void OnImageLoad(object sender, HtmlImageLoadEventArgs e)
        {
            ImageLoad(e);
        }

        /// <summary>
        /// On image load in renderer set the image by event async.
        /// </summary>
        public void ImageLoad(HtmlImageLoadEventArgs e)
        {
            var img = TryLoadResourceImage(e);
            if (img != null)
                e.Callback(img);            
        }

        /// <summary>
        /// Handle stylesheet resolve.
        /// </summary>
        public void OnStylesheetLoad(object sender, HtmlStylesheetLoadEventArgs e)
        {
            var stylesheet = GetStylesheet(e.Src);
            if (stylesheet != null)
                e.SetStyleSheet = stylesheet;
        }

        /// <summary>
        /// Get stylesheet by given key.
        /// </summary>
        public string GetStylesheet(string src)
        {
            if (src == "StyleSheet")
            {
                return @"h1, h2, h3 { color: navy; font-weight:normal; }
                    h1 { margin-bottom: .47em }
                    h2 { margin-bottom: .3em }
                    h3 { margin-bottom: .4em }
                    ul { margin-top: .5em }
                    ul li {margin: .25em}
                    body { font:10pt Tahoma }
		            pre  { border:solid 1px gray; background-color:#eee; padding:1em }
                    a:link { text-decoration: none; }
                    a:hover { text-decoration: underline; }
                    .gray    { color:gray; }
                    .example { background-color:#efefef; corner-radius:5px; padding:0.5em; }
                    .whitehole { background-color:white; corner-radius:10px; padding:15px; }
                    .caption { font-size: 1.1em }
                    .comment { color: green; margin-bottom: 5px; margin-left: 3px; }
                    .comment2 { color: green; }";
            }        
            return null;
        }

    }
}
