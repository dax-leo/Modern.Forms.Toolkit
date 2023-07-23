using System;
using System.Drawing;
using HarfBuzzSharp;
using Modern.Forms.Layout;

namespace Modern.Forms.Renderers
{
    /// <summary>
    /// Represents a class that can render a Button.
    /// </summary>
    public class ButtonRenderer : Renderer<Button>
    {
        /// <inheritdoc/>
        protected override void Render (Button control, PaintEventArgs e)
        {
            //if (SkiaExtensions.IgnorePixelScaling) {

            //    PaintNoScaling (control, e);
            //    return;
            //}

            if (control.Selected && control.ShowFocusCues)
                e.Canvas.DrawFocusRectangle (control.ClientRectangle, e.LogicalToDeviceUnits (3));

            if (control.Image is not null) {
                var face = LayoutUtils.DeflateRect (control.ClientRectangle, control.Padding);

                var img_size = new Size (e.LogicalToDeviceUnits(control.Image.Width), e.LogicalToDeviceUnits(control.Image.Height));

                var layout = new TextImageRelationLayoutUtils {
                    Bounds = face,
                    Font = control.CurrentStyle.GetFont (),
                    FontSize = control.LogicalToDeviceUnits (control.CurrentStyle.GetFontSize ()),
                    ImageAlign = control.ImageAlign,
                    ImageSize = img_size,// control.Image?.GetSize () ?? System.Drawing.Size.Empty,
                    Text = control.Text,
                    TextAlign = control.TextAlign,
                    TextImageRelation = control.TextImageRelation,
                };
                
                (var image_bounds, var text_bounds) = layout.Layout ();
                
                e.Canvas.DrawBitmap (control.Image, image_bounds, !control.Enabled);

                Rectangle r = control.PaddedClientRectangle;
                r.Offset (image_bounds.Width + e.LogicalToDeviceUnits(10), 0);

                e.Canvas.DrawText (control.Text, r, control, control.TextAlign);
            }else
                e.Canvas.DrawText (control.Text, control.PaddedClientRectangle, control, control.TextAlign);
        }

        //private void PaintNoScaling (Button control, PaintEventArgs e)
        //{
        //    if (control.Selected && control.ShowFocusCues)
        //        e.Canvas.DrawFocusRectangle (control.ClientRectangle, 3);

        //    if (control.Image is not null) {
        //        var face = LayoutUtils.DeflateRect (control.ClientRectangle, control.Padding);

        //        var img_size = new Size (e.LogicalToDeviceUnits (control.Image.Width), control.Image.Height);

        //        var layout = new TextImageRelationLayoutUtils {
        //            Bounds = face,
        //            Font = control.CurrentStyle.GetFont (),
        //            FontSize = control.CurrentStyle.GetFontSize (),
        //            ImageAlign = control.ImageAlign,
        //            ImageSize = img_size,// control.Image?.GetSize () ?? System.Drawing.Size.Empty,
        //            Text = control.Text,
        //            TextAlign = control.TextAlign,
        //            TextImageRelation = control.TextImageRelation,
        //        };

        //        (var image_bounds, var text_bounds) = layout.Layout ();

        //        e.Canvas.DrawBitmap (control.Image, image_bounds, !control.Enabled);

        //        Rectangle r = control.PaddedClientRectangle;
        //        r.Offset (image_bounds.Width + 10, 0);

        //        e.Canvas.DrawText (control.Text, r, control, control.TextAlign);
        //    } else
        //        e.Canvas.DrawText (control.Text, control.PaddedClientRectangle, control, control.TextAlign);
        //}
    }
}
