using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using Modern.WindowKit.Platform;
using SkiaSharp;

namespace Modern.Forms
{
    internal class ControlAdapter : ScrollableControl
    {
        private Control? selected_control;

        public ControlAdapter(WindowBase parent)
        {
            ParentForm = parent;
            SetControlBehavior(ControlBehaviors.Selectable, false);
        }

        // We need to override this because the ControlAdapter doesn't need to be scaled
        public override Rectangle ClientRectangle
        {
            get
            {
                var x = CurrentStyle.Border.Left.GetWidth();
                var y = CurrentStyle.Border.Top.GetWidth();
                var w = Width - CurrentStyle.Border.Right.GetWidth() - x;
                var h = Height - CurrentStyle.Border.Bottom.GetWidth() - y;
                return new Rectangle(x, y, w, h);
            }
        }

        public WindowBase ParentForm { get; }

        protected override void OnPaint(PaintEventArgs e)
        {
            // We have this special version for the Adapter because it is
            // given the Form's native surface including any managed Form
            // borders, and it needs to not draw on top of those borders.
            // That is, this often needs to start drawing at (1, 1) instead of (0, 0)
            // This could probably eliminated in the future with Canvas.Translate.
            var form_border = ParentForm.CurrentStyle.Border;

            var form_x = form_border.Left.GetWidth();
            var form_y = form_border.Top.GetWidth();

            var buffer = GetBackBuffer();

            foreach (var control in Controls.GetAllControls(true).Where(c => c.Visible).ToArray())
            {
                if (control.Width <= 0 || control.Height <= 0)
                    continue;

                Paint(control, buffer);
            }

            e.Canvas.DrawBitmap(buffer, form_x + 0, form_y + 0);
        }

        private void Paint(Control control, SKBitmap? buffer)
        {
            if (control.ThisNeedsPaint && control.Visible)
            {
                var info = new SKImageInfo(control.Width, control.Height, SKImageInfo.PlatformColorType, SKAlphaType.Premul);

                using (var canvas = new SKCanvas(buffer))
                {
                    // start drawing
                    var args = new PaintEventArgs(info, canvas, Scaling);

                    var ctrl_position = control.GetPositionInForm();
                    var parent_pos = control.Parent == this ? Point.Empty : control.Parent.GetPositionInForm();

                    // points require scaling offset
                    var scaled_ctrl_pos = new Point(
                        (int)Math.Round(ctrl_position.X * Scaling, MidpointRounding.ToEven),
                        (int)Math.Round(ctrl_position.Y * Scaling, MidpointRounding.ToEven));
                    var scaled_parent_pos = new Point(
                        (int)Math.Round(parent_pos.X * Scaling, MidpointRounding.ToEven),
                        (int)Math.Round(parent_pos.Y * Scaling, MidpointRounding.ToEven));

                    // Clipping must be within Parent bounds.
                    // To make sure rendering is always within parent control we create [control] intersection against [control].Parent.
                    var rec = new Rectangle(scaled_ctrl_pos, control.ScaledBounds.Size);
                    var new_parent_bounds = new Rectangle(scaled_parent_pos, control.Parent.ScaledBounds.Size);
                    rec.Intersect(new_parent_bounds);

                    canvas.Clip(rec);
                    canvas.Translate((scaled_ctrl_pos.X), (scaled_ctrl_pos.Y));

                    control.RaisePaintBackground(args);
                    control.RaisePaint(args);

                    canvas.Flush();

                    Debug.WriteLine(control.GetType().Name + "   " + rec);
                }
            }

            // Recursively scan for other child controls.
            foreach (var c in control.Controls.GetAllControls(true))
            {
                Paint(c, buffer);
            }
        }


        //protected override void OnPaint (PaintEventArgs e)
        //{
        //    // We have this special version for the Adapter because it is
        //    // given the Form's native surface including any managed Form
        //    // borders, and it needs to not draw on top of those borders.
        //    // That is, this often needs to start drawing at (1, 1) instead of (0, 0)
        //    // This could probably eliminated in the future with Canvas.Translate.
        //    var form_border = ParentForm.CurrentStyle.Border;

        //    var form_x = form_border.Left.GetWidth ();
        //    var form_y = form_border.Top.GetWidth ();

        //    foreach (var control in Controls.GetAllControls ().Where (c => c.Visible).ToArray ()) {
        //        if (control.Width <= 0 || control.Height <= 0)
        //            continue;

        //        var info = new SKImageInfo (control.Width, control.Height, SKImageInfo.PlatformColorType, SKAlphaType.Premul);
        //        var buffer = control.GetBackBuffer ();
        //        Debug.WriteLine (buffer.ByteCount);
        //        if (control.NeedsPaint) {
        //            using (var canvas = new SKCanvas (buffer)) {
        //                // start drawing
        //                var args = new PaintEventArgs (info, canvas, Scaling);

        //                control.RaisePaintBackground (args);
        //                control.RaisePaint (args);

        //                canvas.Flush ();
        //            }
        //        }

        //        e.Canvas.DrawBitmap (buffer, form_x + control.ScaledLeft, form_y + control.ScaledTop);
        //    }
        //}

        //protected override void OnPaint (PaintEventArgs e)
        //{
        //    // We have this special version for the Adapter because it is
        //    // given the Form's native surface including any managed Form
        //    // borders, and it needs to not draw on top of those borders.
        //    // That is, this often needs to start drawing at (1, 1) instead of (0, 0)
        //    // This could probably eliminated in the future with Canvas.Translate.
        //    var form_border = ParentForm.CurrentStyle.Border;

        //    var form_x = form_border.Left.GetWidth ();
        //    var form_y = form_border.Top.GetWidth ();

        //    var buffer = this.GetBackBuffer ();// control.GetBackBuffer ();

        //    foreach (var control in Controls.GetAllControls ().Where (c => c.Visible).ToArray ()) {
        //        if (control.Width <= 0 || control.Height <= 0)
        //            continue;

        //        var info = new SKImageInfo (control.Width, control.Height, SKImageInfo.PlatformColorType, SKAlphaType.Premul);

        //        if (control.NeedsPaint) {
        //            using (var canvas = new SKCanvas (buffer)) {
        //                // start drawing
        //                var args = new PaintEventArgs (info, canvas, Scaling);

        //                canvas.Clip (control.ScaledBounds);
        //                canvas.Translate (control.ScaledLeft, control.ScaledTop);


        //                control.RaisePaintBackground (args);
        //                control.RaisePaint (args);

        //                canvas.Flush ();
        //            }
        //        }


        //    }

        //    e.Canvas.DrawBitmap (buffer, form_x + 0, form_y + 0);
        //}

        public override bool Visible
        {
            get => ParentForm != null;
            set { }
        }

        internal Control? SelectedControl
        {
            get => selected_control;
            set
            {
                if (selected_control == value)
                    return;

                selected_control?.Deselect();

                if (value is ControlAdapter)
                    return;

                // Note they could be setting this to null
                selected_control = value;
                selected_control?.Select();
            }
        }

        internal void RaiseParentVisibleChanged(EventArgs e)
        {
            OnParentVisibleChanged(e);
        }


        private SKBitmap? back_buffer;

        internal SKBitmap GetBackBuffer()
        {
            if (back_buffer is null || back_buffer.Width != ScaledSize.Width || back_buffer.Height != ScaledSize.Height)
            {
                FreeBackBuffer();
                back_buffer = new SKBitmap(ScaledSize.Width, ScaledSize.Height, SKImageInfo.PlatformColorType, SKAlphaType.Premul);
                SetState(States.IsDirty, true);
                Invalidate();
            }

            return back_buffer;
        }

        private void FreeBackBuffer()
        {
            if (back_buffer != null)
            {
                back_buffer.Dispose();
                back_buffer = null;
            }
        }
    }
}
