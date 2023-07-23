using System;
using System.Drawing;
using Modern.Forms.Layout;
using Modern.Forms.Renderers;
using SkiaSharp;

namespace Modern.Forms
{
    /// <summary>
    /// Represents a Button control.
    /// </summary>
    public class Button : Control
    {
        private ContentAlignment text_align = ContentAlignment.MiddleCenter;
        private static readonly int s_propImage = PropertyStore.CreateKey ();
        private static readonly int s_propImageAlign = PropertyStore.CreateKey ();
        private static readonly int s_propTextAlign = PropertyStore.CreateKey ();
        private static readonly int s_propTextImageRelation = PropertyStore.CreateKey ();

        /// <summary>
        /// Initializes a new instance of the Button class.
        /// </summary>
        public Button ()
        {
            SetControlBehavior (ControlBehaviors.Hoverable);
            SetControlBehavior (ControlBehaviors.InvalidateOnTextChanged);
            TextAlign = ContentAlignment.MiddleCenter;
        }

        /// <inheritdoc/>
        protected override Cursor DefaultCursor => Cursors.Hand;

        /// <inheritdoc/>
        protected override Size DefaultSize => new Size (100, 30);

        /// <summary>
        /// The default ControlStyle for all instances of Button.
        /// </summary>
        public new static ControlStyle DefaultStyle = new ControlStyle (Control.DefaultStyle,
            (style) => style.Border.Width = 1);

        /// <summary>
        /// The default hover ControlStyle for all instances of Button.
        /// </summary>
        public new static ControlStyle DefaultStyleHover = new ControlStyle (DefaultStyle,
            (style) => {
                style.BackgroundColor = Theme.HighlightColor;
                style.Border.Color = Theme.PrimaryColor;
                style.ForegroundColor = Theme.LightTextColor;
            });

        /// <summary>
        /// Gets or sets a value that is returned to the parent form when the button is clicked.
        /// </summary>
        public DialogResult DialogResult { get; set; }

        /// <inheritdoc/>
        protected override void OnClick (MouseEventArgs e)
        {
            if (FindForm () is Form form)
                form.DialogResult = DialogResult;

            base.OnClick (e);
        }

        /// <inheritdoc/>
        protected override void OnPaint (PaintEventArgs e)
        {
            base.OnPaint (e);

            RenderManager.Render (this, e);
        }

        /// <inheritdoc/>
        protected override void OnKeyUp (KeyEventArgs e)
        {
            if (e.KeyCode.In (Keys.Space, Keys.Enter)) {
                PerformClick ();
                e.Handled = true;
                return;
            }

            base.OnKeyUp (e);
        }

        /// <summary>
        /// Generates a Click event for the Button.
        /// </summary>
        public void PerformClick ()
        {
            OnClick (new MouseEventArgs (MouseButtons.Left, 1, 0, 0, Point.Empty));
        }

        /// <inheritdoc/>
        public override ControlStyle Style { get; } = new ControlStyle (DefaultStyle);

        /// <inheritdoc/>
        public override ControlStyle StyleHover { get; } = new ControlStyle (DefaultStyleHover);

        ///// <summary>
        ///// Gets or sets the text alignment of the Button.
        ///// </summary>
        //public ContentAlignment TextAlign {
        //    get => text_align;
        //    set {
        //        if (text_align != value) {
        //            text_align = value;
        //            Invalidate ();
        //        }
        //    }
        //}

        /// <summary>
        /// Gets or sets a value indicating how text will be aligned within the Label.
        /// </summary>
        public ContentAlignment TextAlign {
            get => Properties.GetEnum (s_propTextAlign, ContentAlignment.TopLeft);
            set {
                SourceGenerated.EnumValidator.Validate (value);

                if (TextAlign != value) {
                    Properties.SetEnum (s_propTextAlign, value);
                    Invalidate ();

                    //OnTextAlignChanged (EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Gets or sets the image that is displayed on a <see cref='Label'/>.
        /// </summary>
        public SKBitmap? Image {
            get => Properties.GetObject<SKBitmap> (s_propImage);
            set {
                if (Image != value) {
                    Properties.SetObject (s_propImage, value);
                    Invalidate ();
                }
            }
        }

        /// <summary>
        /// Gets or sets the alignment of the image on the <see cref='Label'/>.
        /// </summary>
        public ContentAlignment ImageAlign {
            get => Properties.GetEnum (s_propImageAlign, ContentAlignment.MiddleCenter);
            set {
                SourceGenerated.EnumValidator.Validate (value);

                if (value != ImageAlign) {
                    Properties.SetEnum (s_propImageAlign, value);
                    LayoutTransaction.DoLayoutIf (AutoSize, Parent, this, PropertyNames.ImageAlign);
                    Invalidate ();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating how the Label's Image and Text are layed out relative to each other.
        /// </summary>
        public TextImageRelation TextImageRelation {
            get => Properties.GetEnum (s_propTextImageRelation, TextImageRelation.Overlay);
            set {
                SourceGenerated.EnumValidator.Validate (value);

                if (TextImageRelation != value) {
                    Properties.SetEnum (s_propTextImageRelation, value);
                    LayoutTransaction.DoLayoutIf (AutoSize, Parent, this, PropertyNames.TextImageRelation);
                    Invalidate ();
                }
            }
        }

        /// <inheritdoc/>
        public override string ToString () => $"{base.ToString ()}, Text: {Text}";
    }
}
