using SkiaSharp;
using System;
using System.Drawing;
using System.Runtime.Serialization;

namespace GMap.NET.WindowsForms.Markers
{
    [Serializable]
    public class GMarkerCross : GMapMarker, ISerializable
    {
        public static readonly SKPaint DefaultPen = new SKPaint() { Color = SKColors.Red };

        [NonSerialized] public SKPaint Pen = DefaultPen;

        public GMarkerCross(PointLatLng p)
            : base(p)
        {
            IsHitTestVisible = false;
        }

        public override void OnRender(SKCanvas g)
        {
            var p1 = new SKPoint(LocalPosition.X, LocalPosition.Y);
            p1.Offset(0, -10);
            var p2 = new SKPoint(LocalPosition.X, LocalPosition.Y);
            p2.Offset(0, 10);

            var p3 = new SKPoint(LocalPosition.X, LocalPosition.Y);
            p3.Offset(-10, 0);
            var p4 = new SKPoint(LocalPosition.X, LocalPosition.Y);
            p4.Offset(10, 0);

            g.DrawLine(p1, p2, Pen);
            g.DrawLine(p3, p4, Pen);
        }

        public override void Dispose()
        {
            base.Dispose();
        }

        #region ISerializable Members

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

        protected GMarkerCross(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion
    }
}
