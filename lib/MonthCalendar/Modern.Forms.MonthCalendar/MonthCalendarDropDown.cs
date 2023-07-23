using Modern.Forms.MonthCalendar.Renderer;
using System;
using System.Drawing;

namespace Modern.Forms.MonthCalendar
{    
    public class MonthCalendarDropDown : Control
    {
        private PopupWindow? popup;
        private readonly MonthCalendar popup_cal;
        private bool suppress_popup_close;
        private MonthCalendarDropDownRenderer _renderer;

        /// <summary>
        /// Initializes a new instance of the MonthCalendar class.
        /// </summary>
        public MonthCalendarDropDown()
        {
            suppress_popup_close = true;
            _renderer = new MonthCalendarDropDownRenderer();
            popup_cal = new MonthCalendar { Dock = DockStyle.Fill, UseShortestDayNames = true, CalendarDimensions = new System.Drawing.Size(1, 1) };
            popup_cal.Style.Border.Width = 0;
            popup_cal.DateSelected += Popup_cal_DateSelected;
        }

        /// <inheritdoc/>
        protected override Cursor DefaultCursor => Cursors.Hand;

        /// <inheritdoc/>
        protected override Padding DefaultPadding => new Padding(4, 0, 3, 0);

        /// <inheritdoc/>
        protected override Size DefaultSize => new Size(120, 28);

        /// <summary>
        /// The default ControlStyle for all instances of MonthCalendar.
        /// </summary>
        public new static ControlStyle DefaultStyle = new ControlStyle(Control.DefaultStyle,
            (style) => {
                style.Border.Width = 1;
                style.BackgroundColor = Theme.NeutralGray;
            });

        /// <inheritdoc/>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            popup?.Close();
            popup = null;

            popup_cal.Dispose();
        }

        /// <summary>
        /// Raised when the drop down portion of the MonthCalendar is closed.
        /// </summary>
        public event EventHandler? DropDownClosed;

        /// <summary>
        /// Raised when the drop down portion of the MonthCalendar is opened.
        /// </summary>
        public event EventHandler? DropDownOpened;

        /// <summary>
        /// Gets or sets whether the drop down portion of the MonthCalendar is currently shown.
        /// </summary>
        public bool DroppedDown
        {
            get => popup?.Visible == true;
            set
            {
                if (DroppedDown && !value)
                {
                    popup?.Hide();
                    OnDropDownClosed(EventArgs.Empty);
                }
                else if (!DroppedDown && value)
                {
                    if (FindForm() is not Form form)
                        throw new InvalidOperationException("Cannot drop down a MonthCalendar that is not parented to a Form");

                    popup ??= new PopupWindow(form)
                    {
                        Size = popup_cal.Size,                        
                    };
                    popup.Style.Border.Width = 0;

                    popup.Controls.Add(popup_cal);
                    popup.Show(this, 1, Height);

                    OnDropDownOpened(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Gets the collection of items contained by this MonthCalendar.
        /// </summary>
        public SelectionRange SelectionRange => popup_cal.SelectionRange;

        // When the selected date range of the popup MonthCalendar changes, update the MonthCalendar
        private void Popup_cal_DateSelected(object sender, DateRangeEventArgs e)
        {
            if (!suppress_popup_close)
                DroppedDown = false;

            Invalidate();

            OnSelectedIndexChanged(e);
        }

        // When the selected item of the popup ListBox changes, update the MonthCalendar
        //private void ListBox_SelectedIndexChanged(object? sender, EventArgs e)
        //{
        //    if (popup_listbox.SelectedIndex > -1)
        //    {
        //        if (!suppress_popup_close)
        //            DroppedDown = false;

        //        Invalidate();

        //        OnSelectedIndexChanged(e);
        //    }
        //}

        /// <inheritdoc/>
        protected override void OnClick(MouseEventArgs e)
        {
            base.OnClick(e);

            DroppedDown = !DroppedDown;
        }

        /// <inheritdoc/>
        protected override void OnDeselected(EventArgs e)
        {
            base.OnDeselected(e);

            DroppedDown = false;
        }
        
        /// <summary>
        /// Raises the DropDownOpened event.
        /// </summary>
        protected virtual void OnDropDownClosed(EventArgs e) => DropDownClosed?.Invoke(this, e);

        /// <summary>
        /// Raises the DropDownOpened event.
        /// </summary>
        protected virtual void OnDropDownOpened(EventArgs e) => DropDownOpened?.Invoke(this, e);

        ///// <inheritdoc/>
        //protected override void OnKeyUp(KeyEventArgs e)
        //{
        //    // Alt+Up/Down toggles the dropdown
        //    if (e.Alt && e.KeyCode.In(Keys.Up, Keys.Down))
        //    {
        //        DroppedDown = !DroppedDown;
        //        e.Handled = true;
        //        return;
        //    }

        //    // If dropdown is shown, Esc/Enter will close it
        //    if (e.KeyCode.In(Keys.Escape, Keys.Enter) && DroppedDown)
        //    {
        //        DroppedDown = false;
        //        e.Handled = true;
        //        return;
        //    }

        //    // If you mouse click an item we automatically close the dropdown,
        //    // we don't want that behavior when using the keyboard.
        //    suppress_popup_close = true;
        //    //popup_listbox.RaiseKeyUp(e);            
        //    suppress_popup_close = false;

        //    if (e.Handled)
        //        return;

        //    base.OnKeyUp(e);
        //}

        /// <inheritdoc/>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
                        
            _renderer.Render(this, e);
        }

        /// <summary>
        /// Raises the SelectedIndexChanged event.
        /// </summary>
        protected virtual void OnSelectedIndexChanged(EventArgs e) => SelectedRangeChanged?.Invoke(this, e);

        /// <summary>
        /// Raised when the value of the SelectedRange property changes.
        /// </summary>
        public event EventHandler? SelectedRangeChanged;

        /// <inheritdoc/>
        public override ControlStyle Style { get; } = new ControlStyle(DefaultStyle);
    }
}
