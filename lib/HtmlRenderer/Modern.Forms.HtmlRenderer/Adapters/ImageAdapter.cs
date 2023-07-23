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
using TheArtOfDev.HtmlRenderer.Adapters;

namespace TheArtOfDev.HtmlRenderer.Avalonia.Adapters
{
    /// <summary>
    /// Adapter for Avalonia Image object for core.
    /// </summary>
    internal sealed class ImageAdapter : RImage
    {
        /// <summary>
        /// the underline Avalonia image.
        /// </summary>
        private readonly SKBitmap _image;

        /// <summary>
        /// Init.
        /// </summary>
        public ImageAdapter(SKBitmap image)
        {
            _image = image;
        }

        /// <summary>
        /// the underline Avalonia image.
        /// </summary>
        public SKBitmap Image
        {
            get { return _image; }
        }

        public override double Width
        {
            get { return _image.Width; }
        }

        public override double Height
        {
            get { return _image.Height; }
        }

        public override void Dispose()
        {
            _image.Dispose();
        }
    }
}