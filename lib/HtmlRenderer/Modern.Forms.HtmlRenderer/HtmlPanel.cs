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

using Modern.Forms;
using SkiaSharp;
using System.Drawing;
using TheArtOfDev.HtmlRenderer.Avalonia.Adapters;
using TheArtOfDev.HtmlRenderer.Core.Entities;
using TheArtOfDev.HtmlRenderer.Core.Utils;

namespace TheArtOfDev.HtmlRenderer.Avalonia
{
    /// <summary>
    /// Provides HTML rendering using the text property.<br/>
    /// Avalonia control that will render html content in it's client rectangle.<br/>
    /// If the layout of the html resulted in its content beyond the client bounds of the panel it will show scrollbars (horizontal/vertical) allowing to scroll the content.<br/>
    /// The control will handle mouse and keyboard events on it to support html text selection, copy-paste and mouse clicks.<br/>
    /// </summary>
    /// <remarks>
    /// See <see cref="HtmlControl"/> for more info.
    /// </remarks>
    public class HtmlPanel : HtmlControl
    {
        #region Fields and Consts

        /// <summary>
        /// the vertical scroll bar for the control to scroll to html content out of view
        /// </summary>
        protected ScrollBar _verticalScrollBar;

        /// <summary>
        /// the horizontal scroll bar for the control to scroll to html content out of view
        /// </summary>
        protected ScrollBar _horizontalScrollBar;

        #endregion
                
        /// <summary>
        /// Creates a new HtmlPanel and sets a basic css for it's styling.
        /// </summary>
        public HtmlPanel()
        {
            ModernFormsAdapter.Instance.ScalingFactor = Parent != null ? Parent.Scaling : 1;

            _verticalScrollBar = new VerticalScrollBar();            
            _verticalScrollBar.Width = 18;
            _verticalScrollBar.Scroll += OnScrollBarScroll;            
            Controls.Add(_verticalScrollBar);
            
            _horizontalScrollBar = new HorizontalScrollBar();            
            _horizontalScrollBar.Height = 18;
            _horizontalScrollBar.Scroll += OnScrollBarScroll;
            Controls.Add(_horizontalScrollBar);

            _htmlContainer.ScrollChange += OnScrollChange;

            Style.BackgroundColor = SKColors.White;            
        }

        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            ModernFormsAdapter.Instance.ScalingFactor = Parent != null ? Parent.Scaling : 1;            
            PerformLayout();            
            Invalidate();
        }
                

        /// <summary>
        /// Adjust the scrollbar of the panel on html element by the given id.<br/>
        /// The top of the html element rectangle will be at the top of the panel, if there
        /// is not enough height to scroll to the top the scroll will be at maximum.<br/>
        /// </summary>
        /// <param name="elementId">the id of the element to scroll to</param>
        public virtual void ScrollToElement(string elementId)
        {
            ArgChecker.AssertArgNotNullOrEmpty(elementId, "elementId");

            if (_htmlContainer != null)
            {
                var rect = _htmlContainer.GetElementRectangle(elementId);
                if (rect.HasValue)
                {
                    ScrollToPoint(rect.Value.X, rect.Value.Y);
                    if (LastPointerArgs != null)
                    {
                        _htmlContainer.HandleMouseMove(this, LastPointerArgs.Location);
                    }
                }
            }
        }


        #region Private methods

        protected override void OnLayout(LayoutEventArgs e)
        {
            var size = PerformHtmlLayout(ClientRectangle.Size);

            // to handle if scrollbar is appearing or disappearing
            bool relayout = false;
            var htmlWidth = HtmlWidth(ClientRectangle.Size);
            var htmlHeight = HtmlHeight(ClientRectangle.Size);

            if (htmlHeight == 1 || htmlWidth == 1) return;

            if ((_verticalScrollBar.Visible == false && size.Height > htmlHeight) ||
                (_verticalScrollBar.Visible == true && size.Height <= htmlHeight))
            {
                _verticalScrollBar.Visible = _verticalScrollBar.Visible == true ? false : true;
                relayout = true;
            }

            if ((_horizontalScrollBar.Visible == false && size.Width > htmlWidth) ||
                (_horizontalScrollBar.Visible == true && size.Width <= htmlWidth))
            {
                _horizontalScrollBar.Visible = _horizontalScrollBar.Visible == true ? false : true;
                relayout = true;
            }

            if (relayout)
                PerformHtmlLayout(ClientRectangle.Size);
            
            AdjustScrollbars();
        }

        private void AdjustScrollbars()
        {
            var scrollHeight = HtmlHeight(Bounds.Size) + Padding.Top + Padding.Bottom;
            scrollHeight = scrollHeight > 1 ? scrollHeight : 1;
            
            var scrollWidth = HtmlWidth(Bounds.Size) + Padding.Left + Padding.Right;
            scrollWidth = scrollWidth > 1 ? scrollWidth : 1;

            _verticalScrollBar.Bounds = new Rectangle(
                System.Math.Max(Bounds.Size.Width - _verticalScrollBar.Width, 0), 1, _verticalScrollBar.Width, (int)scrollHeight
                );

            _horizontalScrollBar.Bounds = new Rectangle(
                1, System.Math.Max(Bounds.Size.Height - _horizontalScrollBar.Height - 1, 0), (int)scrollWidth, _horizontalScrollBar.Height
                );

            if (_htmlContainer != null)
            {
                if (_verticalScrollBar.Visible)
                {                    
                    _verticalScrollBar.SmallChange = 25;
                    _verticalScrollBar.LargeChange = (int)(_verticalScrollBar.ScaledSize.Height * .9);
                    _verticalScrollBar.Maximum = (int)(_htmlContainer.ActualSize.Height - _verticalScrollBar.ScaledSize.Height);
                }

                if (_horizontalScrollBar.Visible)
                {                    
                    _horizontalScrollBar.SmallChange = 25;
                    _horizontalScrollBar.LargeChange = (int)(_horizontalScrollBar.ScaledSize.Width * .9);
                    _horizontalScrollBar.Maximum = (int)(_htmlContainer.ActualSize.Width - _horizontalScrollBar.ScaledSize.Width);
                }

                // update the scroll offset because the scroll values may have changed
                UpdateScrollOffsets();
            }
        }

        /// <summary>
        /// Perform the layout of the html in the control.
        /// </summary>
        //protected override Size MeasureOverride(Size constraint)
        //{
        //    Size size = PerformHtmlLayout(constraint);

        //    // to handle if scrollbar is appearing or disappearing
        //    bool relayout = false;
        //    var htmlWidth = HtmlWidth(constraint);
        //    var htmlHeight = HtmlHeight(constraint);

        //    if ((_verticalScrollBar.Visible == false && size.Height > htmlHeight) ||
        //        (_verticalScrollBar.Visible == true && size.Height <= htmlHeight))
        //    {
        //        _verticalScrollBar.Visible = _verticalScrollBar.Visible == true ? false : true;
        //        relayout = true;
        //    }

        //    if ((_horizontalScrollBar.Visible == false && size.Width > htmlWidth) ||
        //        (_horizontalScrollBar.Visible == true && size.Width <= htmlWidth))
        //    {
        //        _horizontalScrollBar.Visible = _horizontalScrollBar.Visible == true ? false : true;
        //        relayout = true;
        //    }

        //    if (relayout)
        //        PerformHtmlLayout(constraint);

        //    if (double.IsPositiveInfinity(constraint.Width) || double.IsPositiveInfinity(constraint.Height))
        //        constraint = size;

        //    return constraint;
        //}

        /// <summary>
        /// Perform html container layout by the current panel client size.
        /// </summary>
        protected SKSize PerformHtmlLayout(Size constraint)
        {
            if (_htmlContainer != null)
            {
                _htmlContainer.MaxSize = new SKSize((float)HtmlWidth(constraint), (float)HtmlHeight(constraint));
                _htmlContainer.PerformLayout();
                return _htmlContainer.ActualSize;
            }
            return default;
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            if (_verticalScrollBar.Visible)
            {
                var delta = _verticalScrollBar.Value - (e.Delta.Y * 20);
                if(delta < 0)
                {
                    delta = 0;
                }else if(delta > _verticalScrollBar.Maximum)
                {
                    delta = _verticalScrollBar.Maximum;
                }
                _verticalScrollBar.Value = delta;
                UpdateScrollOffsets();                
            }
        }

        /// <summary>
        /// Handle key down event for selection, copy and scrollbars handling.
        /// </summary>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            
            if (_verticalScrollBar.Visible)
            {
                if(_verticalScrollBar.Value + _verticalScrollBar.SmallChange > _verticalScrollBar.Maximum)
                {
                    _verticalScrollBar.Value = _verticalScrollBar.Maximum;
                }

                if (e.KeyCode == Keys.Up)
                {
                    if (_verticalScrollBar.Value - _verticalScrollBar.SmallChange < 0)
                    {
                        _verticalScrollBar.Value = 0;
                    }
                    else
                        _verticalScrollBar.Value -= _verticalScrollBar.SmallChange;

                    UpdateScrollOffsets();
                    e.Handled = true;
                }
                else if (e.KeyCode == Keys.Down)
                {
                    if (_verticalScrollBar.Value + _verticalScrollBar.SmallChange > _verticalScrollBar.Maximum)
                    {
                        _verticalScrollBar.Value = _verticalScrollBar.Maximum;
                    }else 
                        _verticalScrollBar.Value += _verticalScrollBar.SmallChange;

                    UpdateScrollOffsets();
                    e.Handled = true;
                }
                else if (e.KeyCode == Keys.PageUp)
                {
                    if (_verticalScrollBar.Value - _verticalScrollBar.LargeChange < 0)
                    {
                        _verticalScrollBar.Value = 0;
                    }
                    else
                        _verticalScrollBar.Value -= _verticalScrollBar.LargeChange;

                    UpdateScrollOffsets();
                    e.Handled = true;
                }
                else if (e.KeyCode == Keys.PageDown)
                {
                    if (_verticalScrollBar.Value + _verticalScrollBar.LargeChange > _verticalScrollBar.Maximum)
                    {
                        _verticalScrollBar.Value = _verticalScrollBar.Maximum;
                    }else
                        _verticalScrollBar.Value += _verticalScrollBar.LargeChange;

                    UpdateScrollOffsets();
                    e.Handled = true;
                }
                else if (e.KeyCode == Keys.Home)
                {
                    _verticalScrollBar.Value = 0;
                    UpdateScrollOffsets();
                    e.Handled = true;
                }
                else if (e.KeyCode == Keys.End)
                {
                    _verticalScrollBar.Value = _verticalScrollBar.Maximum;
                    UpdateScrollOffsets();
                    e.Handled = true;
                }
            }

            if (_horizontalScrollBar.Visible)
            {
                if (e.KeyCode == Keys.Left)
                {
                    if (_horizontalScrollBar.Value - _horizontalScrollBar.SmallChange < 0)
                    {
                        _horizontalScrollBar.Value = 0;
                    }
                    else
                        _horizontalScrollBar.Value -= _horizontalScrollBar.SmallChange;

                    UpdateScrollOffsets();
                    e.Handled = true;
                }
                else if (e.KeyCode == Keys.Right)
                {
                    if (_horizontalScrollBar.Value + _horizontalScrollBar.SmallChange > _horizontalScrollBar.Maximum)
                    {
                        _horizontalScrollBar.Value = _horizontalScrollBar.Maximum;
                    }
                    else
                        _horizontalScrollBar.Value += _horizontalScrollBar.SmallChange;

                    UpdateScrollOffsets();
                    e.Handled = true;
                }
            }
        }

        /// <summary>
        /// Get the width the HTML has to render in (not including vertical scroll iff it is visible)
        /// </summary>
        protected override double HtmlWidth(Size size)
        {
            var width = base.HtmlWidth(size) - (_verticalScrollBar != null && _verticalScrollBar.Visible ? _verticalScrollBar.Width : 0);
            return width > 1 ? width : 1;
        }

        /// <summary>
        /// Get the width the HTML has to render in (not including vertical scroll iff it is visible)
        /// </summary>
        protected override double HtmlHeight(Size size)
        {
            var height = base.HtmlHeight(size) - (_horizontalScrollBar != null && _horizontalScrollBar.Visible ? _horizontalScrollBar.Height : 0);
            return height > 1 ? height : 1;
        }

        /// <summary>
        /// On HTML container scroll change request scroll to the requested location.
        /// </summary>
        private void OnScrollChange(object sender, HtmlScrollEventArgs e)
        {
            ScrollToPoint(e.X, e.Y);
        }

        /// <summary>
        /// Set the control scroll offset to the given values.
        /// </summary>
        private void ScrollToPoint(double x, double y)
        {
            _horizontalScrollBar.Value = (int)x;
            _verticalScrollBar.Value = (int)y;
            UpdateScrollOffsets();
        }

        /// <summary>
        /// On scrollbar scroll update the scroll offsets and invalidate.
        /// </summary>
        private void OnScrollBarScroll(object sender, ScrollEventArgs e)
        {
            UpdateScrollOffsets();
        }

        /// <summary>
        /// Update the scroll offset of the HTML container and invalidate visual to re-render.
        /// </summary>
        private void UpdateScrollOffsets()
        {
            var newScrollOffset = new Point(-_horizontalScrollBar.Value, -_verticalScrollBar.Value);
            if (!newScrollOffset.Equals(_htmlContainer.ScrollOffset))
            {
                _htmlContainer.ScrollOffset = newScrollOffset;
                Invalidate();
            }
        }

        #endregion
    }
}