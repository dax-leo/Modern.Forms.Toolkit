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
using System;
using System.ComponentModel;
using System.Drawing;
using TheArtOfDev.HtmlRenderer.Core;
using TheArtOfDev.HtmlRenderer.Core.Entities;

namespace TheArtOfDev.HtmlRenderer.Avalonia
{
    /// <summary>
    /// Provides HTML rendering using the text property.<br/>
    /// Avalonia control that will render html content in it's client rectangle.<br/>
    /// The control will handle mouse and keyboard events on it to support html text selection, copy-paste and mouse clicks.<br/>
    /// <para>
    /// The major differential to use HtmlPanel or HtmlLabel is size and scrollbars.<br/>
    /// If the size of the control depends on the html content the HtmlLabel should be used.<br/>
    /// If the size is set by some kind of layout then HtmlPanel is more suitable, also shows scrollbars if the html contents is larger than the control client rectangle.<br/>
    /// </para>
    /// <para>
    /// <h4>LinkClicked event:</h4>
    /// Raised when the user clicks on a link in the html.<br/>
    /// Allows canceling the execution of the link.
    /// </para>
    /// <para>
    /// <h4>StylesheetLoad event:</h4>
    /// Raised when a stylesheet is about to be loaded by file path or URI by link element.<br/>
    /// This event allows to provide the stylesheet manually or provide new source (file or uri) to load from.<br/>
    /// If no alternative data is provided the original source will be used.<br/>
    /// </para>
    /// <para>
    /// <h4>ImageLoad event:</h4>
    /// Raised when an image is about to be loaded by file path or URI.<br/>
    /// This event allows to provide the image manually, if not handled the image will be loaded from file or download from URI.
    /// </para>
    /// <para>
    /// <h4>RenderError event:</h4>
    /// Raised when an error occurred during html rendering.<br/>
    /// </para>
    /// </summary>
    public class HtmlControl : Control
    {
        #region Fields and Consts

        /// <summary>
        /// Underline html container instance.
        /// </summary>
        protected readonly HtmlContainer _htmlContainer;

        /// <summary>
        /// the base stylesheet data used in the control
        /// </summary>
        protected CssData _baseCssData;

        /// <summary>
        /// The last position of the scrollbars to know if it has changed to update mouse
        /// </summary>
        protected Point _lastScrollOffset;

        #endregion


        #region Events

        public event EventHandler<HtmlImageLoadEventArgs> ImageLoad;
        public event EventHandler<HtmlStylesheetLoadEventArgs> StylesheetLoad;
        public event EventHandler<EventArgs> LoadComplete;
        public event EventHandler<HtmlLinkClickedEventArgs> LinkClicked;
        public event EventHandler<HtmlRenderErrorEventArgs> RenderError;

        #endregion

        internal MouseEventArgs LastPointerArgs { get; private set; }

        /// <summary>
        /// Creates a new HtmlPanel and sets a basic css for it's styling.
        /// </summary>
        public HtmlControl()
        {
            _htmlContainer = new HtmlContainer();

            _htmlContainer.AvoidAsyncImagesLoading = false;
            _htmlContainer.AvoidImagesLateLoading = true;

            _htmlContainer.LoadComplete += OnLoadComplete;
            _htmlContainer.LinkClicked += OnLinkClicked;
            _htmlContainer.RenderError += OnRenderError;
            _htmlContainer.Refresh += OnRefresh;
            _htmlContainer.StylesheetLoad += OnStylesheetLoad;
            _htmlContainer.ImageLoad += OnImageLoad;
            

            MouseMove += (sender, e) =>
            {
                LastPointerArgs = e;
                _htmlContainer.HandleMouseMove(this, e.Location);
            };
            MouseLeave += (sender, e) =>
            {
                LastPointerArgs = null;
                _htmlContainer.HandleMouseLeave(this);
            };
            MouseDown += (sender, e) =>
            {                
                LastPointerArgs = e;
                _htmlContainer?.HandleLeftMouseDown(this, e);
            };
            MouseUp += (sender, e) =>
            {                
                LastPointerArgs = e;
                _htmlContainer.HandleLeftMouseUp(this, e);
            };
            DoubleClick += (sender, e) =>
            {
                _htmlContainer.HandleMouseDoubleClick(this, e);
            };
            KeyDown += (sender, e) =>
            {
                _htmlContainer.HandleKeyDown(this, e);
            };
        }

        private void OnRenderError(object? sender, HtmlRenderErrorEventArgs e)
        {
            var handler = RenderError;
            if (handler != null)
                handler(this, e);
        }

        private void OnLinkClicked(object? sender, HtmlLinkClickedEventArgs e)
        {
            var handler = LinkClicked;
            if (handler != null)
                handler(this, e);
        }

        private void OnLoadComplete(object? sender, EventArgs e)
        {
            var handler = LoadComplete;
            if (handler != null)
                handler(this, e);
        }

        private void OnRefresh(object? sender, HtmlRefreshEventArgs e)
        {
            if (e.Layout)
                PerformLayout();
            Invalidate();
        }

        private void OnStylesheetLoad(object? sender, HtmlStylesheetLoadEventArgs e)
        {
            var handler = StylesheetLoad;
            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// Propagate the image load event from root container.
        /// </summary>
        protected virtual void OnImageLoad(object? sender, HtmlImageLoadEventArgs e)
        {
            var handler = ImageLoad;
            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// Gets or sets a value indicating if image loading only when visible should be avoided (default - false).<br/>
        /// True - images are loaded as soon as the html is parsed.<br/>
        /// False - images that are not visible because of scroll location are not loaded until they are scrolled to.
        /// </summary>
        /// <remarks>
        /// Images late loading improve performance if the page contains image outside the visible scroll area, especially if there is large 
        /// amount of images, as all image loading is delayed (downloading and loading into memory).<br/>
        /// Late image loading may effect the layout and actual size as image without set size will not have actual size until they are loaded
        /// resulting in layout change during user scroll.<br/>
        /// Early image loading may also effect the layout if image without known size above the current scroll location are loaded as they
        /// will push the html elements down.
        /// </remarks>
        [Category("Behavior")]
        [Description("If image loading only when visible should be avoided")]
        public bool AvoidImagesLateLoading
        {
            get { return _htmlContainer.AvoidImagesLateLoading; }
            set { _htmlContainer.AvoidImagesLateLoading = value; }
        }

        /// <summary>
        /// Is content selection is enabled for the rendered html (default - true).<br/>
        /// If set to 'false' the rendered html will be static only with ability to click on links.
        /// </summary>
        [Category("Behavior")]
        [Description("Is content selection is enabled for the rendered html.")]
        public bool IsSelectionEnabled
        {
            get { return _htmlContainer.IsSelectionEnabled; }
            set { _htmlContainer.IsSelectionEnabled = value; }
        }

        /// <summary>
        /// Is the build-in context menu enabled and will be shown on mouse right click (default - true)
        /// </summary>
        [Category("Behavior")]
        [Description("Is the build-in context menu enabled and will be shown on mouse right click.")]
        public bool IsContextMenuEnabled
        {
            get { return _htmlContainer.IsContextMenuEnabled; }
            set { _htmlContainer.IsContextMenuEnabled = value; }
        }

        ///// <summary>
        ///// Set base stylesheet to be used by html rendered in the panel.
        ///// </summary>
        //[Category("Appearance")]
        //[Description("Set base stylesheet to be used by html rendered in the control.")]
        //public string BaseStylesheet
        //{
        //    get { return _htmlContainer.ba; }
        //    set { SetValue(BaseStylesheetProperty, value); }
        //}

        /// <summary>
        /// Gets or sets the text of this panel
        /// </summary>
        [Description("Sets the html of this control.")]
        public string Html
        {            
            set {
                var baseCssData = HtmlRender.ParseStyleSheet(value);
                _baseCssData = baseCssData;
                _htmlContainer.ScrollOffset = new Point(0, 0);

                var hor_scroll = Controls.Where(x => x.GetType() == typeof(HorizontalScrollBar) || x.GetType() == typeof(VerticalScrollBar));
                foreach (ScrollBar sc in hor_scroll)
                    if (sc != null) sc.Value = 0;

                _htmlContainer.SetHtml(value, baseCssData);
                _htmlContainer.PerformLayout();

                Invalidate();
            }
        }

        //public Thickness BorderThickness
        //{
        //    get { return (Thickness) GetValue(BorderThicknessProperty); }
        //    set { SetValue(BorderThicknessProperty, value); }
        //}

        //public IBrush BorderBrush
        //{
        //    get { return (IBrush)GetValue(BorderBrushProperty); }
        //    set { SetValue(BorderThicknessProperty, value); }
        //}

        //public Thickness Padding
        //{
        //    get { return (Thickness)GetValue(PaddingProperty); }
        //    set { SetValue(PaddingProperty, value); }
        //}

        //public Modern.Forms.Drawing.IBrush Background
        //{
        //    get { return (Modern.Forms.Drawing.IBrush) GetValue(BackgroundProperty); }
        //    set { SetValue(BackgroundProperty, value);}
        //}

        /// <summary>
        /// Get the currently selected text segment in the html.
        /// </summary>
        [Browsable(false)]
        public virtual string SelectedText
        {
            get { return _htmlContainer.SelectedText; }
        }

        /// <summary>
        /// Copy the currently selected html segment with style.
        /// </summary>
        [Browsable(false)]
        public virtual string SelectedHtml
        {
            get { return _htmlContainer.SelectedHtml; }
        }

        /// <summary>
        /// Get html from the current DOM tree with inline style.
        /// </summary>
        /// <returns>generated html</returns>
        public virtual string GetHtml()
        {
            return _htmlContainer != null ? _htmlContainer.GetHtml() : null;
        }

        /// <summary>
        /// Get the rectangle of html element as calculated by html layout.<br/>
        /// Element if found by id (id attribute on the html element).<br/>
        /// Note: to get the screen rectangle you need to adjust by the hosting control.<br/>
        /// </summary>
        /// <param name="elementId">the id of the element to get its rectangle</param>
        /// <returns>the rectangle of the element or null if not found</returns>
        public virtual RectangleF? GetElementRectangle(string elementId)
        {
            return _htmlContainer != null ? _htmlContainer.GetElementRectangle(elementId) : null;
        }

        /// <summary>
        /// Clear the current selection.
        /// </summary>
        public void ClearSelection()
        {
            if (_htmlContainer != null)
                _htmlContainer.ClearSelection();
        }


        #region Private methods

        /// <summary>
        /// Perform paint of the html in the control.
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            var context = e.Canvas;

            base.OnPaint(e);

            if (_htmlContainer != null)
            {   
                context.Clip(ClientRectangle);

                _htmlContainer.Location = new Point(Padding.Left, Padding.Top);                
                _htmlContainer.PerformPaint(context, ClientRectangle);
                
                if (!_lastScrollOffset.Equals(_htmlContainer.ScrollOffset))
                {
                    _lastScrollOffset = _htmlContainer.ScrollOffset;
                    InvokeMouseMove();
                }
            }

            //context.FillRectangle(Background,  new Rect(RenderSize));

            //if (BorderThickness != new Thickness(0) && BorderBrush != null)
            //{
            //    var brush = new ImmutableSolidColorBrush(Colors.Black);
            //    if (BorderThickness.Top > 0)
            //        context.FillRectangle(brush, new Rect(0, 0, RenderSize.Width, BorderThickness.Top));
            //    if (BorderThickness.Bottom > 0)
            //        context.FillRectangle(brush, new Rect(0, RenderSize.Height - BorderThickness.Bottom, RenderSize.Width, BorderThickness.Bottom));
            //    if (BorderThickness.Left > 0)
            //        context.FillRectangle(brush, new Rect(0, 0, BorderThickness.Left, RenderSize.Height));
            //    if (BorderThickness.Right > 0)
            //        context.FillRectangle(brush, new Rect(RenderSize.Width - BorderThickness.Right, 0, BorderThickness.Right, RenderSize.Height));
            //}

            //var htmlWidth = HtmlWidth(RenderSize);
            //var htmlHeight = HtmlHeight(RenderSize);

            //if (_htmlContainer != null && htmlWidth > 0 && htmlHeight > 0)
            //{
            //    using (context.PushClip(new Rect(Padding.Left + BorderThickness.Left, Padding.Top + BorderThickness.Top,
            //        htmlWidth, (int) htmlHeight)))
            //    {
            //        _htmlContainer.Location = new Point(Padding.Left + BorderThickness.Left,
            //            Padding.Top + BorderThickness.Top);

            //        _htmlContainer.PerformPaint(context,
            //            new Rect(Padding.Left + BorderThickness.Left, Padding.Top + BorderThickness.Top, htmlWidth,
            //                htmlHeight));
            //    }

            //    if (!_lastScrollOffset.Equals(_htmlContainer.ScrollOffset))
            //    {
            //        _lastScrollOffset = _htmlContainer.ScrollOffset;
            //        InvokeMouseMove();
            //    }
            //}
        }

        
        /// <summary>
        /// Get the width the HTML has to render in (not including vertical scroll iff it is visible)
        /// </summary>
        protected virtual double HtmlWidth(Size size)
        {
            return size.Width - Padding.Left - Padding.Right - 1 - 1;
        }

        /// <summary>
        /// Get the width the HTML has to render in (not including vertical scroll iff it is visible)
        /// </summary>
        protected virtual double HtmlHeight(Size size)
        {
            return size.Height - Padding.Top - Padding.Bottom - 1 - 1;
        }

        /// <summary>
        /// call mouse move to handle paint after scroll or html change affecting mouse cursor.
        /// </summary>
        protected virtual void InvokeMouseMove()
        {
            if (LastPointerArgs != null)
            {
                _htmlContainer.HandleMouseMove(this, LastPointerArgs.Location);
            }
        }

        #endregion
    }
}