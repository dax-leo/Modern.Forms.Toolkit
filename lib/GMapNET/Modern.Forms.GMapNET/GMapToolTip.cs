using Modern.Forms;
using SkiaSharp;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;

namespace GMap.NET.WindowsForms
{
    /// <summary>
    ///     GMap.NET marker
    /// </summary>
    [Serializable]
    public class GMapToolTip : ISerializable, IDisposable
    {
        GMapMarker _marker;

        public GMapMarker Marker
        {
            get
            {
                return _marker;
            }
            internal set
            {
                _marker = value;
            }
        }

        public Point Offset;

        public static readonly StringFormat DefaultFormat = new StringFormat();

        /// <summary>
        ///     string format
        /// </summary>
        [NonSerialized] public readonly StringFormat Format = DefaultFormat;

        public static readonly Font DefaultFont =
            new Font(FontFamily.GenericSansSerif, 11, FontStyle.Regular, GraphicsUnit.Pixel);

        public static readonly Font TitleFont =
            new Font(FontFamily.GenericSansSerif, 11, FontStyle.Bold, GraphicsUnit.Pixel);

        /// <summary>
        ///     font
        /// </summary>
        [NonSerialized] public Font Font = DefaultFont;

        public static readonly SKPaint DefaultStroke = new SKPaint() { Color = new SKColor(0, 0, 0, 140) };

        /// <summary>
        ///     specifies how the outline is painted
        /// </summary>
        [NonSerialized] public SKPaint Stroke = DefaultStroke;

        public static readonly Brush DefaultFill = new SolidBrush(Color.FromArgb(222, Color.White));

        /// <summary>
        ///     background color
        /// </summary>
        [NonSerialized] public Brush Fill = DefaultFill;

        public static readonly Brush DefaultForeground = new SolidBrush(Color.DimGray);

        /// <summary>
        ///     text foreground
        /// </summary>
        [NonSerialized] public Brush Foreground = DefaultForeground;

        /// <summary>
        ///     text padding
        /// </summary>
        public Size TextPadding = new Size(20, 21);

        static GMapToolTip()
        {
            //DefaultStroke.Width = 1;

            //DefaultStroke.LineJoin = LineJoin.Round;
            //DefaultStroke.StartCap = LineCap.RoundAnchor;

            DefaultFormat.LineAlignment = StringAlignment.Near;
            DefaultFormat.Alignment = StringAlignment.Near;
        }

        public GMapToolTip(GMapMarker marker)
        {
            Marker = marker;
            Offset = new Point(14, -44);
        }

        public virtual void OnRender(SKCanvas g)
        {
            var st = TextMeasurer.MeasureText(Marker.ToolTipText, SKTypeface.Default, (int)Font.Size);

            Rectangle r = new Rectangle(Marker.ToolTipPosition.X,
                Marker.ToolTipPosition.Y - (int)st.Height,
                (int)st.Width + TextPadding.Width,
                (int)st.Height + TextPadding.Height);
            RectangleF rect = r;
            Rectangle rectTextN = new Rectangle(Marker.ToolTipPosition.X,
                Marker.ToolTipPosition.Y - (int)st.Height,
                (int)st.Width + TextPadding.Width,
                (int)st.Height + TextPadding.Height);
            RectangleF rectText = rectTextN;

            rect.Offset(Offset.X, Offset.Y);
            rectText.Offset(Offset.X + 7, Offset.Y + 7);
            g.DrawLine(Marker.ToolTipPosition.X, Marker.ToolTipPosition.Y, rect.X, rect.Y + rect.Height / 2, SKColors.Red);
            g.FillRectangle(r, SKColors.Red);

            g.DrawRectangle(r, SKColors.Gold);

            WriteString(g, Marker.ToolTipText, rectTextN);

            //var st = g.MeasureString(Marker.ToolTipText, Font).ToSize();
            //RectangleF rect = new Rectangle(Marker.ToolTipPosition.X,
            //    Marker.ToolTipPosition.Y - st.Height,
            //    st.Width + TextPadding.Width,
            //    st.Height + TextPadding.Height);
            //RectangleF rectText = new Rectangle(Marker.ToolTipPosition.X,
            //    Marker.ToolTipPosition.Y - st.Height,
            //    st.Width + TextPadding.Width,
            //    st.Height + TextPadding.Height);

            //rect.Offset(Offset.X, Offset.Y);
            //rectText.Offset(Offset.X + 7, Offset.Y + 7);
            //g.DrawLine(Stroke, Marker.ToolTipPosition.X, Marker.ToolTipPosition.Y, rect.X, rect.Y + rect.Height / 2);
            //g.FillRectangle(Fill, rect);
            ////g.DrawRectangle(Stroke, rect);

            //DrawRoundRectangle(g, Stroke, rect.X, rect.Y, rect.Width, rect.Height, 8);
            ////g.DrawString(Marker.ToolTipText, Font, Foreground, rectText, Format);
            //WriteString(g, Marker.ToolTipText, rectText);
            //g.Flush();
        }

        #region ISerializable Members

        /// <summary>
        ///     Initializes a new instance of the <see cref="GMapToolTip" /> class.
        /// </summary>
        /// <param name="info">The info.</param>
        /// <param name="context">The context.</param>
        protected GMapToolTip(SerializationInfo info, StreamingContext context)
        {
            Offset = Extensions.GetStruct(info, "Offset", Point.Empty);
            TextPadding = Extensions.GetStruct(info, "TextPadding", new Size(10, 10));
        }

        /// <summary>
        ///     Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo" /> with the data needed to serialize the
        ///     target object.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> to populate with data.</param>
        /// <param name="context">
        ///     The destination (see <see cref="T:System.Runtime.Serialization.StreamingContext" />) for this
        ///     serialization.
        /// </param>
        /// <exception cref="T:System.Security.SecurityException">
        ///     The caller does not have the required permission.
        /// </exception>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Offset", Offset);
            info.AddValue("TextPadding", TextPadding);
        }

        #endregion

        #region IDisposable Members

        bool _disposed;

        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
            }
        }

        #endregion

        public void DrawRoundRectangle(SKCanvas g, Pen pen, float h, float v, float width, float height)
        {
            using (var gp = new SKPath())
            {
                gp.AddPoly(new SKPoint[] { new SKPoint(h, v), new SKPoint(h + width, v) });
                //gp.AddArc(h + width , v, 2,  2, 270, 90);
                gp.AddPoly(new SKPoint[] { new SKPoint(h + width, v), new SKPoint(h + width, v + height) });
                //gp.AddArc(h + width, v + height * 2,  2,  2, 0, 90); // Corner
                gp.AddPoly(new SKPoint[] { new SKPoint(h + width, v + height), new SKPoint(h, v + height) });
                //gp.AddArc(h, v + height,  2,  2, 90, 90);
                gp.AddPoly(new SKPoint[] { new SKPoint(h, v + height), new SKPoint(h, v) });
                //gp.AddArc(h, v, 0, 0, 180, 90);

                //g.FillPath(Fill, gp);
                g.DrawPath(gp, new SKPaint() { Color = SKColors.GreenYellow });
            }
        }

        private void WriteString(SKCanvas g, string text, Rectangle rectText)
        {
            string[] vec1 = text.Split('\n');
            for (int i = 0; i < vec1.Length; i++)
            {
                if (vec1[i] != "")
                {
                    string[] vec2 = vec1[i].Split('|');
                    if (vec2.Length > 0)
                    {
                        //var st = g.MeasureString(vec2[0], TitleFont).ToSize();
                        var st = TextMeasurer.MeasureText(vec2[0], SKTypeface.Default, (int)TitleFont.Size);
                        //g.DrawString(String.Format("{0}", vec2[0]), TitleFont, Foreground, rectText, Format);
                        g.DrawText(String.Format("{0}", vec2[0]), SKTypeface.Default, (int)TitleFont.Size, rectText,SKColors.Red, Modern.Forms.ContentAlignment.MiddleLeft);
                        rectText.X += (int)st.Width + 2;
                        if (vec2.Length > 1)
                        {
                            //g.DrawString(vec2[1], Font, Foreground, rectText, Format);
                            g.DrawText(vec2[0], SKTypeface.Default, (int)TitleFont.Size, rectText, SKColors.Red, Modern.Forms.ContentAlignment.MiddleLeft);
                        }

                        rectText.X -= (int)st.Width + 2;
                        rectText.Y += (int)st.Height;
                    }
                }
                else
                {                    
                    var st = TextMeasurer.MeasureText("\n", SKTypeface.Default, (int)TitleFont.Size).ToSize();
                    rectText.Y += st.Height;
                }
            }
        }
    }
}
