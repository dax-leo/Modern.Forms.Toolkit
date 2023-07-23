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

//using Avalonia.Media;
//using Avalonia.Media.Immutable;
using Modern.Forms.Drawing;
using SkiaSharp;
using TheArtOfDev.HtmlRenderer.Adapters;
using TheArtOfDev.HtmlRenderer.Adapters.Entities;

namespace TheArtOfDev.HtmlRenderer.Avalonia.Adapters
{
    /// <summary>
    /// Adapter for Avalonia pens objects for core.
    /// </summary>
    internal sealed class PenAdapter : RPen
    {
        public SKColor Color => _brush;
        private float[] _interval_dash = new float[] { 10, 5 };
        private float[] _interval_dot = new float[] { 1, 10 };

        private SKStrokeCap _strikeCap = SKStrokeCap.Square;
        private SKPathEffect _pathEffect;

        /// <summary>
        /// The actual Avalonia brush instance.
        /// </summary>
        private readonly SKColor _brush;

        /// <summary>
        /// the width of the pen
        /// </summary>
        private float _width;

        /// <summary>
        /// the dash style of the pen
        /// </summary>
        private StrokeStyle _dashStyle;

        /// <summary>
        /// Init.
        /// </summary>
        public PenAdapter(SKColor brush)
        {
            _brush = brush;
            _pathEffect = SKPathEffect.CreateDash(_interval_dash, 0);
        }

        public override float Width
        {
            get { return _width; }
            set { _width = value; }
        }

        public override RDashStyle DashStyle
        {
            set
            {
                switch (value)
                {
                    case RDashStyle.Dash:
                        _dashStyle = StrokeStyle.Dashed;
                        _strikeCap = SKStrokeCap.Butt;
                        _pathEffect = SKPathEffect.CreateDash(new float[] { 4 * Width, 4 * Width }, 0);
                        break;
                    case RDashStyle.Dot:
                        _strikeCap = SKStrokeCap.Butt;
                        _pathEffect = SKPathEffect.CreateDash(new float[] { 1f * Width, 4 * Width }, 0);
                        _dashStyle = StrokeStyle.Dotted;
                        break;
                    case RDashStyle.DashDot:
                        _dashStyle = StrokeStyle.Dotted;
                        break;
                    case RDashStyle.DashDotDot:
                        _dashStyle = StrokeStyle.Dotted;
                        break;
                    default:
                        _pathEffect = null;
                        _strikeCap = SKStrokeCap.Butt;
                        _dashStyle = StrokeStyle.Line;
                        break;
                }
            }
        }

        /// <summary>
        /// Create the actual Avalonia pen instance.
        /// </summary>
        public SKPaint CreatePen()
        {
            return new SKPaint() {
                Color = Color,
                StrokeWidth = _width,
                Style = SKPaintStyle.Stroke,
                StrokeCap = _strikeCap,
                PathEffect = _pathEffect
            }; 
        }
    }
}