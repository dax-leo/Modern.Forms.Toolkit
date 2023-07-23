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

using System;
using System.Collections.Generic;
using System.Globalization;
using TheArtOfDev.HtmlRenderer.Adapters;
using TheArtOfDev.HtmlRenderer.Adapters.Entities;
using TheArtOfDev.HtmlRenderer.Core.Utils;
using TheArtOfDev.HtmlRenderer.Avalonia.Utilities;
using SkiaSharp;
using Modern.Forms;
using System.Drawing;
using Modern.Forms.Drawing;
using Topten.RichTextKit;

namespace TheArtOfDev.HtmlRenderer.Avalonia.Adapters
{
    /// <summary>
    /// Adapter for Avalonia Graphics.
    /// </summary>
    internal sealed class GraphicsAdapter : RGraphics
    {
        #region Fields and Consts

        /// <summary>
        /// The wrapped Avalonia graphics object
        /// </summary>
        private readonly SKCanvas _g;

        /// <summary>
        /// if to release the graphics object on dispose
        /// </summary>
        private readonly bool _releaseGraphics;
        private readonly RRect _whiteSpaceSize;


        #endregion

        //private readonly Stack<DrawingContext.PushedState> _clipStackInt = new Stack<DrawingContext.PushedState>();

        /// <summary>
        /// Init.
        /// </summary>
        /// <param name="g">the Avalonia graphics object to use</param>
        /// <param name="initialClip">the initial clip of the graphics</param>
        /// <param name="releaseGraphics">optional: if to release the graphics object on dispose (default - false)</param>
        public GraphicsAdapter(SKCanvas g, RRect initialClip, bool releaseGraphics = false)
            : base(ModernFormsAdapter.Instance, initialClip)
        {
            ArgChecker.AssertArgNotNull(g, "g");

            _g = g;
            _releaseGraphics = releaseGraphics;
        }

        /// <summary>
        /// Init.
        /// </summary>
        public GraphicsAdapter()
            : base(ModernFormsAdapter.Instance, RRect.Empty)
        {
            _g = null;
            _releaseGraphics = false;
        }

        public override void PopClip()
        {
            //_clipStackInt.Pop().Dispose();
            _clipStack.Pop();
            //_g.clip(Utils.Convert(_clipStack.Peek()),  CombineMode.Replace);
        }

        public override void PushClip(RRect rect)
        {
            //_clipStackInt.Push(_g.PushClip(Utils.Convert(rect)));
            _clipStack.Push(rect);

            //_g.ClipRect(Utils.ConvertSK(rect), SKClipOperation.Intersect);
        }

        public override void PushClipExclude(RRect rect)
        {
            //var geometry = new CombinedGeometry();
            //geometry.Geometry1 = new RectangleGeometry(Utils.Convert(_clipStack.Peek()));
            //geometry.Geometry2 = new RectangleGeometry(Utils.Convert(rect));
            //geometry.GeometryCombineMode = GeometryCombineMode.Exclude;

            _clipStack.Push(_clipStack.Peek());
            //_g.ClipRect(Utils.ConvertSK(rect), SKClipOperation.Difference);
            //_clipStackInt.Push(_g.PushGeometryClip(geometry));
        }

        public override Object SetAntiAliasSmoothingMode()
        {
            return null;
        }

        public override void ReturnPreviousSmoothingMode(Object prevMode)
        { }

        public override RSize MeasureString(string str, RFont font)
        {
            var fontAdapter = (FontAdapter)font;
            var txt_size = TextMeasurer.MeasureText(str, fontAdapter.Font, (int)(fontAdapter.Size));            
            return Utils.Convert(txt_size);            
        }

        public override void MeasureString(string str, RFont font, double maxWidth, out int charFit, out double charFitWidth)
        {
            var fontAdapter = (FontAdapter)font;
            var size = TextMeasurer.MeasureText(str, fontAdapter.Font, (int)fontAdapter.Size);
            
            charFit = 0;
            charFitWidth = 0;

            for (int i = 1; i <= str.Length; i++)
            {
                charFit = i - 1;
                RSize pSize = MeasureString(str.Substring(0, i), font);
                
                if (pSize.Height <= size.Height && pSize.Width < maxWidth)
                    charFitWidth = pSize.Width;
                else
                    break;
            }
        }

        public override void DrawString(string str, RFont font, RColor color, RPoint point, bool rtl)
        {            
            var fa = (FontAdapter)font;

            var rs = new RichString()
                .Alignment(TextAlignment.Left)
                .FontFamily(fa.Font.FamilyName)
                .TextDirection(rtl ? TextDirection.RTL : TextDirection.LTR)
                .Bold(fa.Font.IsBold)
                .FontItalic(fa.Font.IsItalic)                
                .Add(str, fontSize: (float?)(fa.Size), textColor: Utils.Convert(color));

            rs.Paint(_g, Utils.Convert(point));
        }

        public override RBrush GetTextureBrush(RImage image, RRect dstRect, RPoint translateTransformLocation)
        {
            var brush = new ImageBrush(((ImageAdapter)image).Image, 1);            
            //brush.TranslateTransform((float)translateTransformLocation.X, (float)translateTransformLocation.Y);
            return new BrushAdapter(brush);

            //var brush = new Modern.Forms.Drawing.ImageBrush(((ImageAdapter)image).Image);
            //brush.Stretch = Stretch.None;
            //brush.TileMode = TileMode.Tile;
            //brush.DestinationRect = new RelativeRect(Utils.Convert(dstRect).Translate(Utils.Convert(translateTransformLocation) - new Point()), RelativeUnit.Absolute);
            //brush.Transform = new TranslateTransform(translateTransformLocation.X, translateTransformLocation.Y);

            //return new BrushAdapter(brush.ToImmutable());
        }

        public override RGraphicsPath GetGraphicsPath()
        {
            return new GraphicsPathAdapter();
        }

        public override void Dispose()
        {
            if (_releaseGraphics)
                _g.Dispose();
        }


        #region Delegate graphics methods

        public override void DrawLine(RPen pen, double x1, double y1, double x2, double y2)
        {
            x1 = (int)x1;
            x2 = (int)x2;
            y1 = (int)y1;
            y2 = (int)y2;

            var adj = pen.Width;
            if (Math.Abs(x1 - x2) < .1f && Math.Abs(adj % 2 - 1) < .1f)
            {
                x1 += .5f;
                x2 += .5f;
            }

            if (Math.Abs(y1 - y2) < .1f && Math.Abs(adj % 2 - 1) < .1f)
            {
                y1 += .5f;
                y2 += .5f;
            }

            _g.DrawLine(new SKPoint((float)x1, (float)y1), new SKPoint((float)x2, (float)y2), ((PenAdapter)pen).CreatePen());
        }

        public override void DrawRectangle(RPen pen, double x, double y, double width, double height)
        {            
            var adj = pen.Width;

            if (Math.Abs(adj % 2 - 1) < .1f)
            {
                x += .5f;
                y += .5f;
            }

            _g.DrawRectangle(new Rectangle((int)x, (int)y, (int)width, (int)height), ((PenAdapter)pen).Color, (int)((PenAdapter)pen).Width);            
        }

        public override void DrawRectangle(RBrush brush, double x, double y, double width, double height)
        {            
            ((BrushAdapter)brush).Brush.FillRect(_g, (float)x, (float)y, (float)width, (float)height);            
        }

        public override void DrawImage(RImage image, RRect destRect, RRect srcRect)
        {
            _g.DrawBitmap(((ImageAdapter)image).Image, Utils.ConvertSK(srcRect), Utils.ConvertSK(destRect)
                , new SKPaint() { IsAntialias = true, FilterQuality = SKFilterQuality.High });            
        }

        public override void DrawImage(RImage image, RRect destRect)
        {
            var dest = Utils.ConvertSK(destRect);

            using var p = new SKPaint() { IsAntialias = true, FilterQuality = SKFilterQuality.High };
            _g.DrawBitmap(((ImageAdapter)image).Image, dest, p);
        }

        public override void DrawPath(RPen pen, RGraphicsPath path)
        {            
            _g.DrawPath(((GraphicsPathAdapter)path).GetClosedGeometry(), ((PenAdapter)pen).CreatePen());
        }

        public override void DrawPath(RBrush brush, RGraphicsPath path)
        {
            ((BrushAdapter)brush).Brush.Fill(_g, ((GraphicsPathAdapter)path).GetClosedGeometry());
        }

        public override void DrawPolygon(RBrush brush, RPoint[] points)
        {
            if (points != null && points.Length > 0)
            {
                using (SKPath p = new SKPath())
                {
                    p.MoveTo(Utils.Convert(points[0]));
                    for (int i = 1; i < points.Length; i++)
                        p.LineTo(Utils.Convert(points[i]));
                    p.Close();

                    ((BrushAdapter)brush).Brush.Fill(_g, p);
                }
            }
        }


        #endregion
    }
}