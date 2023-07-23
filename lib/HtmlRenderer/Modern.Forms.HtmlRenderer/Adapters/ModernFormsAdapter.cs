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
using Modern.Forms.Drawing;
using Modern.WindowKit.Platform.Storage;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using TheArtOfDev.HtmlRenderer.Adapters;
using TheArtOfDev.HtmlRenderer.Adapters.Entities;
using TheArtOfDev.HtmlRenderer.Avalonia.Utilities;

namespace TheArtOfDev.HtmlRenderer.Avalonia.Adapters
{
    /// <summary>
    /// Adapter for Avalonia platform.
    /// </summary>
    internal sealed class ModernFormsAdapter : RAdapter
    {
        #region Fields and Consts

        /// <summary>
        /// Singleton instance of global adapter.
        /// </summary>
        private static readonly ModernFormsAdapter _instance = new ModernFormsAdapter();

        #endregion

        /// <summary>
        /// Init installed font families and set default font families mapping.
        /// </summary>
        private ModernFormsAdapter()
        {
            AddFontFamilyMapping("monospace", "Courier New");
            AddFontFamilyMapping("Helvetica", "Arial");

            foreach (var family in FontFamily.Families)
            {
	            try
	            {
	                AddFontFamily(new FontFamilyAdapter(family));
	            }
	            catch
	            {
	            }
            }
        }

        /// <summary>
        /// Singleton instance of global adapter.
        /// </summary>
        public static ModernFormsAdapter Instance
        {
            get { return _instance; }
        }

        public double ScalingFactor { get; set; }

        protected override RColor GetColorInt(string colorName)
        {
            var color = Color.FromName(colorName);
            var c = Utils.Convert(color);            
            return c;
        }

        protected override RPen CreatePen(RColor color)
        {
            return new PenAdapter(Utils.Convert(color));
        }

        protected override RBrush CreateSolidBrush(RColor color)
        {
            return new BrushAdapter(GetSolidColorBrush(color));
        }

        protected override RBrush CreateLinearGradientBrush(RRect rect, RColor color1, RColor color2, double angle)
        {
            var startColor = angle <= 180 ? Utils.Convert(color1) : Utils.Convert(color2);
            var endColor = angle <= 180 ? Utils.Convert(color2) : Utils.Convert(color1);

            angle = angle <= 180 ? angle : angle - 180;
            double x = angle < 135 ? Math.Max((angle - 45) / 90, 0) : 1;
            double y = angle <= 45 ? Math.Max(0.5 - angle / 90, 0) : angle > 135 ? Math.Abs(1.5 - angle / 90) : 0;

            var grad_brush = new GradientBrush(new SKPoint((float)x, (float)y), new SKPoint((float)(1 - x), (float)(1 - y)), new[]
            {
                new Tuple<float, SKColor>(0, startColor),
                new Tuple<float, SKColor>(1, endColor),
            });
            
            return new BrushAdapter(grad_brush);
        }

        protected override RImage ConvertImageInt(object image)
        {
            return image != null ? new ImageAdapter((SKBitmap)image) : null;
        }

        protected override RImage ImageFromStreamInt(Stream memoryStream)
        {
            var bitmap = SKBitmap.Decode(memoryStream);
            return new ImageAdapter(bitmap);
        }

        protected override RFont CreateFontInt(string family, double size, RFontStyle style)
        {
            var typeface = SKTypeface.FromFamilyName(family, GetFontStyle(style));            
            return new FontAdapter(typeface, size);
        }

        protected override RFont CreateFontInt(RFontFamily family, double size, RFontStyle style)
        {
            var fam = (FontFamilyAdapter)(family);
            var typeface = SKTypeface.FromFamilyName(fam.Name, GetFontStyle(style));            
            var fa = new FontAdapter(typeface, size);            
            return fa;            
        }

        protected override object GetClipboardDataObjectInt(string html, string plainText)
        {
            var dataObject = new Modern.WindowKit.Input.DataObject();
            dataObject.Set(Modern.WindowKit.Input.DataFormats.Text, plainText);
            return dataObject;
        }

        protected override void SetToClipboardInt(string text)
        {            
            Clipboard.SetTextAsync(text);
        }

        protected override void SetToClipboardInt(string html, string plainText)
        {            
            Clipboard.SetTextAsync(plainText);
        }

        protected override void SetToClipboardInt(RImage image)
        {
            //Do not crash, just ignore
            //TODO: implement image clipboard support            
        }

        protected override RContextMenu CreateContextMenuInt()
        {
            return new ContextMenuAdapter();
        }

        protected override async void SaveToFileInt(RImage image, string name, string extension, RControl control = null)
        {
            //using (var saveDialog = new SaveFileDialog())
            //{
            //    saveDialog.Filter = "Images|*.png;*.bmp;*.jpg";
            //    saveDialog.FileName = name;
            //    saveDialog.DefaultExt = extension;

            //    var dialogResult = control == null ? saveDialog.ShowDialog() : saveDialog.ShowDialog(((ControlAdapter)control).Control);
            //    if (dialogResult == DialogResult.OK)
            //    {
            //        ((ImageAdapter)image).Image.Save(saveDialog.FileName);
            //    }
            //}
        }

//        protected override async void SaveToFileInt(RImage image, string name, string extension, RControl control = null)
//        {
//            var topLevel = TryGetTopLevel(control);

//            if (topLevel is null)
//            {
//                throw new InvalidOperationException("No TopLevel available");
//            }

//            var file = await topLevel.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
//            {
//                FileTypeChoices = new []
//                {
//                    FilePickerFileTypes.ImagePng
//                },
//                SuggestedFileName = name,
//                DefaultExtension = "png"
//            });

//            if (file is null)
//            {
//                return;
//            }

//#if NET6_0_OR_GREATER
//            await using var stream = await file.OpenWriteAsync();
//#else
//            var stream = await file.OpenWriteAsync();
//#endif
            
//            ((ImageAdapter)image).Image.Save(stream);

//            await stream.FlushAsync();
//        }


        #region Private/Protected methods

        /// <summary>
        /// Get solid color brush for the given color.
        /// </summary>
        private static IBrush GetSolidColorBrush(RColor color)
        {            
            var solidBrush = new ColorBrush(Utils.Convert(color));
            return solidBrush;

            //if (color == RColor.White)
            //    solidBrush = Brushes.White;
            //else if (color == RColor.Black)
            //    solidBrush = Brushes.Black;
            //else if (color.A < 1)
            //    solidBrush = Brushes.Transparent;
            //else
            //    solidBrush = new ImmutableSolidColorBrush(Utils.Convert(color));
            //return solidBrush;
        }

        /// <summary>
        /// Get Avalonia font style for the given style.
        /// </summary>
        private static SKFontStyle GetFontStyle(RFontStyle style)
        {
            if ((style & RFontStyle.Italic) == RFontStyle.Italic && (style & RFontStyle.Bold) == RFontStyle.Bold)
                return SKFontStyle.BoldItalic;

            if ((style & RFontStyle.Italic) == RFontStyle.Italic)
                return SKFontStyle.Italic;
            else if ((style & RFontStyle.Bold) == RFontStyle.Bold)
                return SKFontStyle.Bold;

            return SKFontStyle.Normal;
        }

        /// <summary>
        /// Get Avalonia font style for the given style.
        /// </summary>
        private static SKFontStyleWeight GetFontWidth(RFontStyle style)
        {
            if ((style & RFontStyle.Bold) == RFontStyle.Bold)
                return SKFontStyleWeight.Bold;
            
            return SKFontStyleWeight.Normal;
        }

        // TODO pass actual top level reference to the adapter or clipboard APIs, might require changing in the HtmlRenderer code.
        //private static TopLevel TryGetTopLevel(RControl control = null)
        //{
        //    return TopLevel.GetTopLevel(((ControlAdapter)control)?.Control)
        //           ?? (Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow
        //           ?? TopLevel.GetTopLevel((Application.Current?.ApplicationLifetime as ISingleViewApplicationLifetime)?.MainView);
        //}
        
        #endregion
    }
}