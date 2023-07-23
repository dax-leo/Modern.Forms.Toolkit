using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modern.Forms.MonthCalendar.Renderer
{
    internal class MonthCalendarDropDownRenderer
    {
        /// <summary>
        /// Size of the drop down glyph.
        /// </summary>
        protected const int GLYPH_SIZE = 15;

        /// <inheritdoc/>
        public void Render(MonthCalendarDropDown control, PaintEventArgs e)
        {
            var text_area = GetTextArea(control, e);
            var unit_3 = e.LogicalToDeviceUnits(3);

            if (control.Selected && control.ShowFocusCues)
            {
                var focus_bounds = new Rectangle(text_area.Left - unit_3, text_area.Top, text_area.Width + unit_3, text_area.Height);
                e.Canvas.DrawFocusRectangle(focus_bounds, unit_3);
            }

            // Draw the text of the selected item
            if (control.SelectionRange.Start != control.SelectionRange.End)
            {                
                var range = string.Format("{0} > {1}", control.SelectionRange.Start.ToString("yyyy-MM-dd"), control.SelectionRange.End.ToString("yyyy-MM-dd"));
                e.Canvas.DrawText(range!, text_area, control, ContentAlignment.MiddleLeft, maxLines: 1);
            }
            else
            {
                e.Canvas.DrawText(control.SelectionRange.Start.ToString("yyyy-MM-dd")!, text_area, control, ContentAlignment.MiddleLeft, maxLines: 1);
            }

            // Draw the drop down glyph
            var button_bounds = GetDropDownButtonArea(control, e);

            ControlPaint.DrawArrowGlyph(e, button_bounds, control.Enabled ? Theme.PrimaryTextColor : Theme.DisabledTextColor, ArrowDirection.Down);
        }

        /// <summary>
        /// Gets the bounding box of the area to draw the drop down glyph.
        /// </summary>
        protected Rectangle GetDropDownButtonArea(MonthCalendarDropDown control, PaintEventArgs e)
        {
            var glyph_size = e.LogicalToDeviceUnits(GLYPH_SIZE);

            return new Rectangle(control.ScaledWidth - glyph_size, 0, glyph_size - e.LogicalToDeviceUnits(control.Padding.Right), control.ScaledHeight);
        }

        /// <summary>
        /// Gets the bounding box of the area to draw the ComboBox text.
        /// </summary>
        protected Rectangle GetTextArea(MonthCalendarDropDown control, PaintEventArgs e)
        {
            var area = control.PaddedClientRectangle;

            area.Width -= e.LogicalToDeviceUnits(GLYPH_SIZE);

            return area;
        }
    }
}
