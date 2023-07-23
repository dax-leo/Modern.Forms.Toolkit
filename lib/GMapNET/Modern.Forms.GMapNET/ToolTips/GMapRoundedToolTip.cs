using Modern.Forms;
using SkiaSharp;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;

namespace GMap.NET.WindowsForms.ToolTips
{
    /// <summary>
    ///     GMap.NET marker
    /// </summary>
    [Serializable]
    public class GMapRoundedToolTip : GMapToolTip, ISerializable
    {
        public float Radius = 10f;

        public GMapRoundedToolTip(GMapMarker marker)
            : base(marker)
        {
            TextPadding = new Size((int)Radius, (int)Radius);
        }

        public override void OnRender(SKCanvas g)
        {
            var st = TextMeasurer.MeasureText(Marker.ToolTipText, SKTypeface.Default, (int)Font.Size);

            var rect = new Rectangle(Marker.ToolTipPosition.X,
                Marker.ToolTipPosition.Y - (int)st.Height,
                 (int)st.Width + TextPadding.Width * 2,
                 (int)st.Height + TextPadding.Height);
            rect.Offset(Offset.X, Offset.Y);

            SKRect rf = new SKRect(rect.Left, rect.Top, rect.Right, rect.Bottom);

            int lineOffset = 0;
            if (!g.LocalClipBounds.Contains(rf))
            {
                var clippingOffset = new Point();
                if (rect.Right > g.LocalClipBounds.Right)
                {
                    clippingOffset.X = -((rect.Left - Marker.LocalPosition.X) / 2 + rect.Width);
                    lineOffset = -(rect.Width - (int)Radius);
                }

                if (rect.Top < g.LocalClipBounds.Top)
                {
                    clippingOffset.Y = ((rect.Bottom - Marker.LocalPosition.Y) + (rect.Height * 2));
                }

                rect.Offset(clippingOffset);
            }

            g.DrawLine(
                Marker.ToolTipPosition.X,
                Marker.ToolTipPosition.Y,
                (rect.X - lineOffset) + Radius / 2,
                rect.Y + rect.Height - Radius / 2, Stroke);

            g.DrawRectangle(rect.X, rect.Y, rect.Width, rect.Height, SKColors.Gold);

            if (Format.Alignment == StringAlignment.Near)
            {
                rect.Offset(TextPadding.Width, 0);
            }

            g.DrawText(Marker.ToolTipText, SKTypeface.Default, (int)TitleFont.Size, rect, SKColors.Red, Modern.Forms.ContentAlignment.MiddleLeft);            
        }


        #region ISerializable Members

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Radius", Radius);

            base.GetObjectData(info, context);
        }

        protected GMapRoundedToolTip(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            Radius = Extensions.GetStruct(info, "Radius", 10f);
        }

        #endregion
    }
}
