using System;
using System.Drawing;

namespace Modern.Forms.Renderers
{
    /// <summary>
    /// Represents a class that can render a Splitter.
    /// </summary>
    public class SplitterRenderer : Renderer<Splitter>
    {
        /// <inheritdoc/>
        protected override void Render (Splitter control, PaintEventArgs e)
        {
            // Draw splitter handle (as rectangle dots)
            if(control.Orientation == Orientation.Horizontal) {
                Rectangle rectangle = new Rectangle (
                    control.ClientRectangle.X + 1, 
                    control.ClientRectangle.Y + (control.ClientRectangle.Height - e.LogicalToDeviceUnits(35)) / 2,
                    control.ClientRectangle.Width,
                    e.LogicalToDeviceUnits (35));

                //int x = rectangle.X;
                //int y = rectangle.Y;

                //for (int index = 0; index < 6; ++index) {
                //    e.Canvas.FillRectangle (x, y + index * 5, 1, 1, Theme.HighlightColor);
                //}
                
                e.Canvas.FillRectangle(rectangle, Theme.HighlightColor);    
            } else {
                Rectangle rectangle = new Rectangle (
                    control.ClientRectangle.X + (control.ClientRectangle.Width - e.LogicalToDeviceUnits (35)) / 2, 
                    control.ClientRectangle.Y + 1,
                    e.LogicalToDeviceUnits (35), 
                    control.ClientRectangle.Height);

                //int x = rectangle.X;
                //int y = rectangle.Y;

                //for (int index = 0; index < 6; ++index) {
                //    e.Canvas.FillRectangle (x + index * 5, y + 1, 2, 2, Theme.HighlightColor);
                //}

                e.Canvas.FillRectangle (rectangle, Theme.HighlightColor);
            }
        }
    }
}
