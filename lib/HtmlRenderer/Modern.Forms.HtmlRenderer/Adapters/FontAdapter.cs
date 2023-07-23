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

using System.Diagnostics;
using System.Globalization;
using SkiaSharp;
using TheArtOfDev.HtmlRenderer.Adapters;
using TheArtOfDev.HtmlRenderer.Adapters.Entities;

namespace TheArtOfDev.HtmlRenderer.Avalonia.Adapters
{
    /// <summary>
    /// Adapter for Avalonia Font.
    /// </summary>
    internal sealed class FontAdapter : RFont
    {
        #region Fields and Consts

        /// <summary>
        /// the underline win-forms font.
        /// </summary>
        private readonly SKTypeface _font;

        /// <summary>
        /// the size of the font
        /// </summary>
        private  double _size;

        /// <summary>
        /// the vertical offset of the font underline location from the top of the font.
        /// </summary>
        private readonly double _underlineOffset = -1;

        /// <summary>
        /// Cached font height.
        /// </summary>
        private  double _height = -1;

        /// <summary>
        /// Cached font whitespace width.
        /// </summary>
        private double _whitespaceWidth = -1;

        #endregion


        /// <summary>
        /// Init.
        /// </summary>
        public FontAdapter(SKTypeface font, double size)
        {
            _font = font;
            _size = (size + 2) * ModernFormsAdapter.Instance.ScalingFactor;         //scaling
            _height = _size;// + 2 * ModernFormsAdapter.Instance.ScalingFactor;
            _underlineOffset = _height + 0.5f;

            //FontManager.Current.TryGetGlyphTypeface(font, out _glyphTypeface);
            //var emHeight = font.UnitsPerEm;
            //_height = 96d / 72d * (_size / emHeight) * 2472;//_glyphTypeface.Metrics.LineSpacing;
            //_underlineOffset = 96d / 72d * (_size / emHeight) * (2000 + 178) * ModernFormsAdapter.Instance.ScalingFactor;
        }

        /// <summary>
        /// the underline win-forms font.
        /// </summary>
        public SKTypeface Font
        {
            get { return _font; }
        }

        public override double Size
        {
            get { return _size; }
        }
        
        public void changesize(double value)
        {
            _size = value;
        }

        public override double UnderlineOffset
        {
            get { return _underlineOffset; }
        }

        public override double Height
        {
            get { 
                return _height; }
            set { _height = value; }
        }

        public override double LeftPadding
        {
            get { return _height / 6f; }
        }

        public override double GetWhitespaceWidth(RGraphics graphics)
        {
            if (_whitespaceWidth < 0)
            {
                _whitespaceWidth = graphics.MeasureString(" ", this).Width;
            }
            return _whitespaceWidth;
        }
    }
}