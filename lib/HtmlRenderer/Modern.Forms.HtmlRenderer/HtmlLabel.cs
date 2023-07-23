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
using System.ComponentModel;
using TheArtOfDev.HtmlRenderer.Adapters.Entities;
using TheArtOfDev.HtmlRenderer.Core;
using TheArtOfDev.HtmlRenderer.Avalonia.Adapters;
using Modern.Forms;
using SkiaSharp;
using TheArtOfDev.HtmlRenderer.Avalonia.Utilities;

namespace TheArtOfDev.HtmlRenderer.Avalonia
{
    /// <summary>
    /// Provides HTML rendering using the text property.<br/>
    /// Avalonia control that will render html content in it's client rectangle.<br/>
    /// Using <see cref="AutoSize"/> and <see cref="AutoSizeHeightOnly"/> client can control how the html content effects the
    /// size of the label. Either case scrollbars are never shown and html content outside of client bounds will be clipped.
    /// MaxWidth/MaxHeight and MinWidth/MinHeight with AutoSize can limit the max/min size of the control<br/>
    /// The control will handle mouse and keyboard events on it to support html text selection, copy-paste and mouse clicks.<br/>
    /// </summary>
    /// <remarks>
    /// See <see cref="HtmlControl"/> for more info.
    /// </remarks>
    public class HtmlLabel : HtmlControl
    {
        /// <summary>
        /// is to handle auto size of the control height only
        /// </summary>
        protected bool _autoSizeHight;

        /// <summary>
        /// Init.
        /// </summary>
        static HtmlLabel()
        {
            //BackgroundProperty.OverrideDefaultValue<HtmlLabel>(Brushes.Transparent);
            //AffectsMeasure<HtmlLabel>(AutoSizeProperty, AutoSizeHeightOnlyProperty);            
        }

        /// <summary>
        /// Automatically sets the size of the label by content size
        /// </summary>
        [Category("Layout")]
        [Description("Automatically sets the size of the label by content size.")]
        public bool AutoSize
        {
            get { return base.AutoSize; }
            set
            {
                base.AutoSize = value;
                if (value)
                {
                    _autoSizeHight = false;
                    PerformLayout();
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Automatically sets the height of the label by content height (width is not effected).
        /// </summary>
        [Category("Layout")]
        [Description("Automatically sets the height of the label by content height (width is not effected)")]
        public virtual bool AutoSizeHeightOnly
        {
            get { return _autoSizeHight; }
            set
            {
                _autoSizeHight = value;
                if (value)
                {
                    AutoSize = false;
                    PerformLayout();
                    Invalidate();
                }
            }
        }

        #region Private methods

        protected override void OnLayout(LayoutEventArgs e)
        {            
            if (_htmlContainer != null)
            {
                using (var ig = new GraphicsAdapter())
                {
                    var newSize = HtmlRendererUtils.Layout(ig,
                        _htmlContainer.HtmlContainerInt,
                        new RSize(ClientSize.Width - Padding.Horizontal, ClientSize.Height - Padding.Vertical),
                        new RSize(MinimumSize.Width - Padding.Horizontal, MinimumSize.Height - Padding.Vertical),
                        new RSize(MaximumSize.Width - Padding.Horizontal, MaximumSize.Height - Padding.Vertical),
                        AutoSize,
                        AutoSizeHeightOnly);

                    Size = Utils.ConvertRound(new RSize(newSize.Width + Padding.Horizontal, newSize.Height + Padding.Vertical)).ToSize();
                }
            }

            base.OnLayout(e);
        }

        /// <summary>
        /// Handle when dependency property value changes to update the underline HtmlContainer with the new value.
        /// </summary>
        //protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs e)
        //{
        //    base.OnPropertyChanged(e);

        //    if (e.Property == AutoSizeProperty)
        //    {
        //        if (e.GetNewValue<bool>())
        //        {
        //            SetValue(AutoSizeHeightOnlyProperty, false);
        //        }
        //    }
        //    else if (e.Property == AutoSizeHeightOnlyProperty)
        //    {
        //        if (e.GetNewValue<bool>())
        //        {
        //            SetValue(AutoSizeProperty, false);
        //        }
        //    }
        //}

        #endregion
    }
}