using System;
using Modern.WindowKit;
using SkiaSharp;

namespace Modern.Forms.Renderers
{
    /// <summary>
    /// Represents a class that can render a SplitContainer.
    /// </summary>
    public class SplitContainerRenderer : Renderer<SplitContainer>
    {
        /// <inheritdoc/>
        protected override void Render (SplitContainer control, PaintEventArgs e)
        {
            // here we draw splitter 
            if (control.Orientation == Orientation.Horizontal) {
                int x1 = control.Panel1.ScaledWidth + control.SplitterWidth;
                int y1 = 0;
                int x2 = x1;
                int y2 = control.ScaledHeight;

                // splitter line
                e.Canvas.DrawLine (x1, y1, x2, y2, new SKPaint () { Color = Theme.BorderGray });

                // add 6 rectangles to serve as "handle"
                for (int index = 0; index < 6; ++index) {
                    e.Canvas.DrawRectangle (x1 - control.SplitterWidth, (y2 / 2 - 15) + index * 5, 1, 1, Theme.HighlightColor);
                }
            } else {
                // define vertical orientation
            }

            //var c = control.ScaledBounds;
            //c.Inflate (-2, -2);
            //c.Offset (-1, -1);
            //e.Canvas.DrawRectangle (c, SKColors.Red, 4);
        }
    }
}
