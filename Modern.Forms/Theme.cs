using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using SkiaSharp;

namespace Modern.Forms
{
    /// <summary>
    /// Provides a repository of color resources.
    /// </summary>
    public static class Theme
    {
        private static int suspend_count = 0;
        private static bool suspended_raise_waiting = false;

        private static Dictionary<string, object> values;// = new Dictionary<string, object> ();        
        private static ThemeStyle _style;

        static Theme ()
        {
            setTheme(ThemeStyle.Light);
        }

        public static void setTheme (ThemeStyle style)
        {
            Style = style;
            switch (style){                
                case ThemeStyle.Dark: setDarkTheme (); break;
                default: setLightTheme(); break;
            }
            RaiseThemeChanged ();
        }

        private static void setLightTheme ()
        {
            values?.Clear ();
            Dictionary<string, object> values_style = new Dictionary<string, object> ();

            values_style[nameof (PrimaryColor)] = new SKColor (0, 120, 212);
            values_style[nameof (PrimaryTextColor)] = SKColors.Black;
            values_style[nameof (DisabledTextColor)] = new SKColor (190, 190, 190);
            values_style[nameof (LightTextColor)] = SKColors.White;
            values_style[nameof (FormBackgroundColor)] = new SKColor (240, 240, 240);
            values_style[nameof (FormCloseHighlightColor)] = new SKColor (232, 17, 35);
            values_style[nameof (HighlightColor)] = new SKColor (42, 138, 208);
            values_style[nameof (ItemHighlightColor)] = new SKColor (198, 198, 198);
            values_style[nameof (ItemSelectedColor)] = new SKColor (176, 176, 176);
            values_style[nameof (DarkNeutralGray)] = new SKColor (225, 225, 225);
            values_style[nameof (NeutralGray)] = new SKColor (243, 242, 241);
            values_style[nameof (LightNeutralGray)] = new SKColor (251, 251, 251);
            values_style[nameof (BorderGray)] = new SKColor (171, 171, 171);
            values_style[nameof (ScrollBack)] = new SKColor (243, 242, 241);
            values_style[nameof (ScrollHandle)] = SKColors.White;
            values_style[nameof (ScrollArrowBorder)] = new SKColor (225, 225, 225);
            values_style[nameof (ScrollBorder)] = new SKColor (204, 204, 204);
            values_style[nameof (MetroText)] = new SKColor (50, 50, 50);

            //values_style[nameof (UIFont)] = SKTypeface.FromFamilyName ("Segoe UI Emoji", SKFontStyleWeight.Normal, SKFontStyleWidth.Normal, SKFontStyleSlant.Upright);
            //values_style[nameof (UIFontBold)] = SKTypeface.FromFamilyName ("Segoe UI Emoji", SKFontStyleWeight.Bold, SKFontStyleWidth.Normal, SKFontStyleSlant.Upright);

            values_style[nameof (UIFont)] = SKTypeface.FromFamilyName ("Tahoma", SKFontStyleWeight.Normal, SKFontStyleWidth.Normal, SKFontStyleSlant.Upright);
            values_style[nameof (UIFontBold)] = SKTypeface.FromFamilyName ("Tahoma", SKFontStyleWeight.Bold, SKFontStyleWidth.Normal, SKFontStyleSlant.Upright);


            values_style[nameof (FontSize)] = 14;
            values_style[nameof (ItemFontSize)] = 12;

            values = values_style;
            //themes["light"] = values_style;
        }

        private static void setDarkTheme ()
        {
            Dictionary<string, object> values_style = new Dictionary<string, object> ();

            values_style[nameof (PrimaryColor)] = new SKColor (0, 120, 212);
            values_style[nameof (PrimaryTextColor)] = SKColors.White;
            values_style[nameof (DisabledTextColor)] = new SKColor (71, 71, 71);
            values_style[nameof (LightTextColor)] = SKColors.White;
            values_style[nameof (FormBackgroundColor)] = new SKColor (31, 31, 31);
            values_style[nameof (FormCloseHighlightColor)] = new SKColor (232, 17, 35);
            values_style[nameof (HighlightColor)] = new SKColor (42, 138, 208);
            values_style[nameof (ItemHighlightColor)] = new SKColor (45, 71, 101);
            values_style[nameof (ItemSelectedColor)] = new SKColor (176, 176, 176);
            values_style[nameof (DarkNeutralGray)] = new SKColor (225, 225, 225);
            values_style[nameof (NeutralGray)] = new SKColor (37, 37, 38);
            values_style[nameof (LightNeutralGray)] = new SKColor (31, 31, 31);
            values_style[nameof (BorderGray)] = new SKColor (61, 61, 61);
            values_style[nameof (ScrollBack)] = new SKColor (71, 71, 71);
            values_style[nameof (ScrollHandle)] = new SKColor (153, 153, 153);
            values_style[nameof (ScrollArrowBorder)] = new SKColor (71, 71, 71);
            values_style[nameof (ScrollBorder)] = new SKColor (153, 153, 153);
            values_style[nameof (MetroText)] = SKColors.DarkGray;

            //values_style[nameof (UIFont)] = SKTypeface.FromFamilyName ("Segoe UI Emoji", SKFontStyleWeight.Thin, SKFontStyleWidth.Normal, SKFontStyleSlant.Upright);
            //values_style[nameof (UIFontBold)] = SKTypeface.FromFamilyName ("Segoe UI Emoji", SKFontStyleWeight.Bold, SKFontStyleWidth.Normal, SKFontStyleSlant.Upright);

            values_style[nameof (UIFont)] = SKTypeface.FromFamilyName ("Tahoma", SKFontStyleWeight.Thin, SKFontStyleWidth.Normal, SKFontStyleSlant.Upright);
            values_style[nameof (UIFontBold)] = SKTypeface.FromFamilyName ("Tahoma", SKFontStyleWeight.Bold, SKFontStyleWidth.Normal, SKFontStyleSlant.Upright);

            values_style[nameof (FontSize)] = 14;
            values_style[nameof (ItemFontSize)] = 12;

            values = values_style;
            //themes["light"] = values_style;

        }

        /// <summary>
        /// Pause raising ThemeChanged for better performance if changing many Theme elements.
        /// Resume with EndUpdate ().
        /// </summary>
        public static void BeginUpdate ()
        {
            suspend_count++;
        }

        /// <summary>
        /// Metro style text color.
        /// </summary>
        public static SKColor MetroText {
            get => GetValue<SKColor> (nameof (MetroText));
            set => SetValue (nameof (MetroText), value);
        }

        /// <summary>
        /// A darker gray used as the default control border color.
        /// </summary>
        public static SKColor BorderGray {
            get => GetValue<SKColor> (nameof (BorderGray));
            set => SetValue (nameof (BorderGray), value);
        }

        /// <summary>
        /// Scroll area back color.
        /// </summary>
        public static SKColor ScrollBack {
            get => GetValue<SKColor> (nameof (ScrollBack));
            set => SetValue (nameof (ScrollBack), value);
        }

        /// <summary>
        /// Scroll handle color.
        /// </summary>
        public static SKColor ScrollHandle {
            get => GetValue<SKColor> (nameof (ScrollHandle));
            set => SetValue (nameof (ScrollHandle), value);
        }

        /// <summary>
        /// Scroll arrow border color.
        /// </summary>
        public static SKColor ScrollArrowBorder {
            get => GetValue<SKColor> (nameof (ScrollArrowBorder));
            set => SetValue (nameof (ScrollArrowBorder), value);
        }

        /// <summary>
        /// Scroll border color.
        /// </summary>
        public static SKColor ScrollBorder {
            get => GetValue<SKColor> (nameof (ScrollBorder));
            set => SetValue (nameof (ScrollBorder), value);
        }

        /// <summary>
        /// A darker gray generally used for showing an item is currently pressed.
        /// </summary>
        public static SKColor DarkNeutralGray {
            get => GetValue<SKColor> (nameof (DarkNeutralGray));
            set => SetValue (nameof (DarkNeutralGray), value);
        }

        /// <summary>
        /// The color used for disabled text.
        /// </summary>
        public static SKColor DisabledTextColor {
            get => GetValue<SKColor> (nameof (DisabledTextColor));
            set => SetValue (nameof (DisabledTextColor), value);
        }

        /// <summary>
        /// Resume raising the ThemeChanged event after pausing with BeginUpdate ().
        /// </summary>
        public static void EndUpdate ()
        {
            if (suspend_count == 0)
                throw new InvalidOperationException ("EndUpdate called without matching BeginUpdate");

            suspend_count--;

            if (suspended_raise_waiting)
                RaiseThemeChanged ();
        }

        /// <summary>
        /// The default font size used by control.
        /// </summary>
        public static int FontSize {
            get => GetValue<int> (nameof (FontSize));
            set => SetValue (nameof (FontSize), value);
        }

        /// <summary>
        /// The background color of a form.
        /// </summary>
        public static SKColor FormBackgroundColor {
            get => GetValue<SKColor> (nameof (FormBackgroundColor));
            set => SetValue (nameof (FormBackgroundColor), value);
        }

        /// <summary>
        /// The color used to draw a form's close button when highlighted.
        /// </summary>
        public static SKColor FormCloseHighlightColor {
            get => GetValue<SKColor> (nameof (FormCloseHighlightColor));
            set => SetValue (nameof (FormCloseHighlightColor), value);
        }

        private static T GetValue<T> (string name) => (T)values[name];

        /// <summary>
        /// The color used for highlighted elements, like hovered tabs or buttons.
        /// </summary>
        public static SKColor HighlightColor {
            get => GetValue<SKColor> (nameof (HighlightColor));
            set => SetValue (nameof (HighlightColor), value);
        }

        /// <summary>
        /// A smaller font size generally used for lists of items.
        /// </summary>
        public static int ItemFontSize {
            get => GetValue<int> (nameof (ItemFontSize));
            set => SetValue (nameof (ItemFontSize), value);
        }

        /// <summary>
        /// The color used for a highlighted item's background.
        /// </summary>
        public static SKColor ItemHighlightColor {
            get => GetValue<SKColor> (nameof (ItemHighlightColor));
            set => SetValue (nameof (ItemHighlightColor), value);
        }

        /// <summary>
        /// The color used for a selected item's background.
        /// </summary>
        public static SKColor ItemSelectedColor {
            get => GetValue<SKColor> (nameof (ItemSelectedColor));
            set => SetValue (nameof (ItemSelectedColor), value);
        }

        /// <summary>
        /// A lighter gray used primarily used as the background of list items.
        /// </summary>
        public static SKColor LightNeutralGray {
            get => GetValue<SKColor> (nameof (LightNeutralGray));
            set => SetValue (nameof (LightNeutralGray), value);
        }

        /// <summary>
        /// A lighter text color, often used when an element is selected with a darker background.
        /// </summary>
        public static SKColor LightTextColor {
            get => GetValue<SKColor> (nameof (LightTextColor));
            set => SetValue (nameof (LightTextColor), value);
        }

        /// <summary>
        /// A neutral gray used as the default background of controls.
        /// </summary>
        public static SKColor NeutralGray {
            get => GetValue<SKColor> (nameof (NeutralGray));
            set => SetValue (nameof (NeutralGray), value);
        }

        /// <summary>
        /// The primary color for elements such as the title bar, tabs, checkboxes and radio button glyph.
        /// </summary>
        public static SKColor PrimaryColor {
            get => GetValue<SKColor> (nameof (PrimaryColor));
            set => SetValue (nameof (PrimaryColor), value);
        }

        /// <summary>
        /// The primary text color.
        /// </summary>
        public static SKColor PrimaryTextColor {
            get => GetValue<SKColor> (nameof (PrimaryTextColor));
            set => SetValue (nameof (PrimaryTextColor), value);
        }

        private static void RaiseThemeChanged ()
        {
            if (suspend_count > 0) {
                suspended_raise_waiting = true;
                return;
            }

            ThemeChanged?.Invoke (null, EventArgs.Empty);
            suspended_raise_waiting = false;
        }

        private static void SetValue (string key, object value)
        {
            values[key] = value;

            RaiseThemeChanged ();
        }

        /// <summary>
        /// Raised when a theme color is changed. Controls listen
        /// for this event to know when to repaint themselves.
        /// </summary>
        public static event EventHandler? ThemeChanged;

        /// <summary>
        /// The default font used by controls.
        /// </summary>
        public static SKTypeface UIFont {
            get => GetValue<SKTypeface> (nameof (UIFont));
            set => SetValue (nameof (UIFont), value);
        }

        /// <summary>
        /// The default bold font used by controls.
        /// </summary>
        public static SKTypeface UIFontBold {
            get => GetValue<SKTypeface> (nameof (UIFontBold));
            set => SetValue (nameof (UIFontBold), value);
        }
        public static ThemeStyle Style { get => _style; internal set => _style = value; }

        public static void GenerateColorMapImage (string img_save_dir)
        {
            var info = new SKImageInfo ();
            info.Width = 600;
            info.Height = 70 * values.Count;
            info.ColorType = SKColorType.Rgba8888;
            info.AlphaType = SKAlphaType.Premul;

            using var surface = SKSurface.Create (info);
            var canvas = surface.Canvas;
            canvas.Clear (SKColors.White);

            SKPoint location = new SKPoint (200, 10);
            foreach(var item in values) {
                string color_id = item.Key;
                                
                if (typeof(SKColor) != item.Value.GetType()) continue;
                SKColor color = (SKColor)item.Value;

                // Draw Text
                using (SKPaint skPaint1 = new SKPaint ()) {
                    skPaint1.Style = SKPaintStyle.Fill;
                    skPaint1.IsAntialias = true;
                    skPaint1.Color = SKColors.Black;
                    skPaint1.TextAlign = SKTextAlign.Left;
                    skPaint1.TextSize = 16;

                    canvas.DrawText (color_id, 20, location.Y + 40, skPaint1);
                }

                // Draw rectangle colored by "color"
                SKPaint skPaint = new SKPaint () {
                    Style = SKPaintStyle.Fill,
                    Color = color,                    
                    IsAntialias = true,
                };
                SKRect skRectangle = new SKRect ();
                skRectangle.Size = new SKSize (100, 50);
                skRectangle.Location = location;

                // shift point by 70 (Y axis)
                location.Offset (0, 70);

                canvas.DrawRect (skRectangle, skPaint);
            }

            // save as image
            using (var image = surface.Snapshot ())
            using (var data = image.Encode (SKEncodedImageFormat.Png, 90))
            using (var stream = File.OpenWrite (Path.Combine (img_save_dir, Style.ToString() + "_color_map.png"))) {
                // save the data to a stream
                data.SaveTo (stream);
            }
        }
    }

    public enum ThemeStyle
    {
        Light,
        Dark
    }
}
