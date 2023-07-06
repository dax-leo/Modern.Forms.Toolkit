﻿using System;
using System.Drawing;
using System.Runtime.Versioning;
using Modern.Forms.Extensions;
using SkiaSharp;

namespace Modern.Forms
{
    /// <summary>
    /// A collection of extension methods to facilitate working with Skia.
    /// </summary>
    public static class SkiaExtensions
    {
        #region Ignore Pixel Scaling 

        private static bool _ignorePixelScaling = false;
        //private static PixelScalingMode _scalingmode = PixelScalingMode.Medium;
        //public static float ScalingReduction = 0.5f;

        //public static PixelScalingMode ScalingMode {
        //    get {
        //        return _scalingmode;
        //    }
        //    set {
        //        _scalingmode = value;

        //        switch (value) {
        //            case PixelScalingMode.Low:
        //                ScalingReduction = 0.25f; break;
        //            case PixelScalingMode.Medium:
        //                ScalingReduction = 0.5f; break;
        //            case PixelScalingMode.High:
        //                ScalingReduction = 0.75f; break;
        //            case PixelScalingMode.Full:
        //                ScalingReduction = 1; break;
        //        }
        //    }
        //}

        public static bool IgnorePixelScaling { get => _ignorePixelScaling; set => _ignorePixelScaling = value; }

        #endregion

        private static readonly SKColorFilter disabled_matrix = SKColorFilter.CreateColorMatrix (new float[]
                {
                    0.21f, 0.72f, 0.07f, 0, 0,
                    0.21f, 0.72f, 0.07f, 0, 0,
                    0.21f, 0.72f, 0.07f, 0, 0,
                    0,     0,     0,     1, 0
                });

        /// <summary>
        /// Clips a canvas to the specified rectangle.
        /// </summary>
        public static void Clip (this SKCanvas canvas, Rectangle rectangle) => canvas.ClipRect (rectangle.ToSKRect ());

        /// <summary>
        /// Draws a control's background.
        /// </summary>
        public static void DrawBackground (this SKCanvas canvas, Rectangle bounds, ControlStyle style)
        {
            var radius = style.Border.GetRadius ();

            if (radius > 0) {
                canvas.Clear (SKColors.Transparent);
                canvas.FillRoundedRectangle (0, 0, bounds.Width - style.Border.GetWidth (), bounds.Height - style.Border.GetWidth (), style.GetBackgroundColor (), radius, radius, style.Border.GetWidth ());
                return;
            }

            canvas.Clear (style.GetBackgroundColor ());
        }

        /// <summary>
        /// Draws a bitmap.
        /// </summary>
        public static void DrawBitmap (this SKCanvas canvas, SKBitmap bitmap, Rectangle rect, bool disabled = false)
        {
            using var paint = new SKPaint { FilterQuality = SKFilterQuality.High };

            if (disabled)
                paint.ColorFilter = disabled_matrix;

            canvas.DrawBitmap (bitmap, rect.ToSKRect (), paint);
        }

        /// <summary>
        /// Draws a bitmap.
        /// </summary>
        public static void DrawBitmap (this SKCanvas canvas, SKBitmap bitmap, float x, float y, bool disabled = false)
        {
            using var paint = new SKPaint { FilterQuality = SKFilterQuality.High };

            if (disabled)
                paint.ColorFilter = disabled_matrix;

            canvas.DrawBitmap (bitmap, x, y, paint);
        }

        /// <summary>
        /// Draws a control's border.
        /// </summary>
        public static void DrawBorder (this SKCanvas canvas, Rectangle bounds, ControlStyle style)
        {
            // If using border radius, currently all border sides are drawn, and all are the same color
            var radius = style.Border.GetRadius ();

            if (radius > 0) {
                canvas.DrawRoundedRectangle (0, 0, bounds.Width - style.Border.GetWidth (), bounds.Height - style.Border.GetWidth (), style.Border.GetColor (), radius, radius, style.Border.GetWidth ());
                return;
            }

            // Left Border
            if (style.Border.Left.GetWidth () > 0) {
                var left_offset = style.Border.Left.GetWidth () / 2f;
                canvas.DrawLine (left_offset, 0, left_offset, bounds.Height, style.Border.Left.GetColor (), style.Border.Left.GetWidth ());
            }

            // Right Border
            if (style.Border.Right.GetWidth () > 0) {
                var right_offset = style.Border.Right.GetWidth () / 2f;
                canvas.DrawLine (bounds.Width - right_offset, 0, bounds.Width - right_offset, bounds.Height, style.Border.Right.GetColor (), style.Border.Right.GetWidth ());
            }

            // Top Border
            if (style.Border.Top.GetWidth () > 0) {
                var top_offset = style.Border.Top.GetWidth () / 2f;
                canvas.DrawLine (0, top_offset, bounds.Width, top_offset, style.Border.Top.GetColor (), style.Border.Top.GetWidth ());
            }

            // Bottom Border
            if (style.Border.Bottom.GetWidth () > 0) {
                var bottom_offset = style.Border.Bottom.GetWidth () / 2f;
                canvas.DrawLine (0, bounds.Height - bottom_offset, bounds.Width, bounds.Height - bottom_offset, style.Border.Bottom.GetColor (), style.Border.Bottom.GetWidth ());
            }
        }

        /// <summary>
        /// Draws an unfilled circle.
        /// </summary>
        public static void DrawCircle (this SKCanvas canvas, int x, int y, int radius, SKColor color, int strokeWidth = 1)
        {
            using var paint = new SKPaint { Color = color, IsStroke = true, StrokeWidth = strokeWidth, IsAntialias = true };

            canvas.DrawCircle (x, y, radius, paint);
        }

        /// <summary>
        /// Draws a focus rectangle.
        /// </summary>
        public static void DrawFocusRectangle (this SKCanvas canvas, int x, int y, int width, int height, int inset = 0)
        {
            // Draw a white rectangle
            canvas.DrawRectangle (x + inset, y + inset, width - (2 * inset) - 1, height - (2 * inset) - 1, SKColors.White);

            // Draw a black dashed rectangle on top of it
            var effect = SKPathEffect.CreateDash (new[] { 1f, 1f }, 0);
            using var paint = new SKPaint { Color = SKColors.Black, IsStroke = true, StrokeWidth = 1, PathEffect = effect };

            canvas.DrawRect (x + inset, y + inset, width - (2 * inset) - 1, height - (2 * inset) - 1, paint);
        }

        /// <summary>
        /// Draws an focus rectangle.
        /// </summary>
        public static void DrawFocusRectangle (this SKCanvas canvas, Rectangle rectangle, int inset = 0)
            => DrawFocusRectangle (canvas, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, inset);

        /// <summary>
        /// Draws a line.
        /// </summary>
        public static void DrawLine (this SKCanvas canvas, float x1, float y1, float x2, float y2, SKColor color, int thickness = 1)
        {
            using var paint = new SKPaint { Color = color, StrokeWidth = thickness };

            canvas.DrawLine (x1, y1, x2, y2, paint);
        }

        /// <summary>
        /// Draws an unfilled rectangle.
        /// </summary>
        public static void DrawRectangle (this SKCanvas canvas, int x, int y, int width, int height, SKColor color, int strokeWidth = 1)
        {
            using var paint = new SKPaint { Color = color, IsStroke = true, StrokeWidth = strokeWidth };

            canvas.DrawRect (x, y, width, height, paint);
        }

        /// <summary>
        /// Draws an unfilled rectangle.
        /// </summary>
        public static void DrawRectangle (this SKCanvas canvas, Rectangle rectangle, SKColor color, int strokeWidth = 1)
            => DrawRectangle (canvas, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, color, strokeWidth);

        /// <summary>
        /// Draws an unfilled rectangle with rounded corners.
        /// </summary>
        public static void DrawRoundedRectangle (this SKCanvas canvas, int x, int y, int width, int height, SKColor color, int rx = 3, int ry = 3, float strokeWidth = 1)
        {
            using var paint = new SKPaint {
                Color = color,
                IsStroke = true,
                IsAntialias = true,
                StrokeWidth = strokeWidth
            };

            canvas.DrawRoundRect (x + .5f, y + .5f, width, height, rx, ry, paint);
        }

        /// <summary>
        /// Draws a filled circle.
        /// </summary>
        public static void FillCircle (this SKCanvas canvas, int x, int y, int radius, SKColor color)
        {
            using var paint = new SKPaint { Color = color, IsAntialias = true };

            canvas.DrawCircle (x, y, radius, paint);
        }

        /// <summary>
        /// Draws a filled rectangle.
        /// </summary>
        public static void FillRectangle (this SKCanvas canvas, Rectangle rectangle, SKColor color)
            => FillRectangle (canvas, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, color);

        /// <summary>
        /// Draws a filled rectangle.
        /// </summary>
        public static void FillRectangle (this SKCanvas canvas, int x, int y, int width, int height, SKColor color)
        {
            using var paint = new SKPaint { Color = color };

            canvas.DrawRect (x, y, width, height, paint);
        }

        /// <summary>
        /// Draws a filled rectangle with rounded corners.
        /// </summary>
        public static void FillRoundedRectangle (this SKCanvas canvas, int x, int y, int width, int height, SKColor color, int rx = 3, int ry = 3, float strokeWidth = 1)
        {
            using var paint = new SKPaint {
                Color = color,
                IsStroke = false,
                IsAntialias = true,
                StrokeWidth = strokeWidth
            };
            var r = new SKRoundRect ();

            canvas.DrawRoundRect (x + .5f, y + .5f, width, height, rx, ry, paint);
        }

        /// <summary>
        /// Gets the size of the specified bitmap.
        /// </summary>
        public static Size GetSize (this SKBitmap bitmap) => new Size (bitmap.Width, bitmap.Height);

        /// <summary>
        /// Convers an SKImage to a Bitmap.
        /// </summary>
        [SupportedOSPlatform ("windows")]
        public static Bitmap ToBitmap (this SKImage skiaImage)
        {
            // TODO: maybe keep the same color types where we can, instead of just going to the platform default
            var bitmap = new Bitmap (skiaImage.Width, skiaImage.Height, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
            var data = bitmap.LockBits (new Rectangle (0, 0, bitmap.Width, bitmap.Height), System.Drawing.Imaging.ImageLockMode.WriteOnly, bitmap.PixelFormat);

            // copy
            using (var pixmap = new SKPixmap (new SKImageInfo (data.Width, data.Height), data.Scan0, data.Stride))
                skiaImage.ReadPixels (pixmap, 0, 0);

            bitmap.UnlockBits (data);
            return bitmap;
        }

        /// <summary>
        /// Convers an SKBitmap to a Bitmap.
        /// </summary>
        [SupportedOSPlatform ("windows")]
        public static Bitmap ToBitmap (this SKBitmap skiaBitmap)
        {
            using (var image = SKImage.FromPixels (skiaBitmap.PeekPixels ()))
                return ToBitmap (image);
        }

        public static SKBitmap ToSKBitmap (this System.Drawing.Bitmap bitmap)
        {
            // TODO: maybe keep the same color types where we can, instead of just going to the platform default

            var info = new SKImageInfo (bitmap.Width, bitmap.Height);
            var skiaBitmap = new SKBitmap (info);
            using (var pixmap = skiaBitmap.PeekPixels ()) {
                bitmap.ToSKPixmap (pixmap);
            }
            return skiaBitmap;
        }

        public static void ToSKPixmap (this System.Drawing.Bitmap bitmap, SKPixmap pixmap)
        {
            // TODO: maybe keep the same color types where we can, instead of just going to the platform default

            if (pixmap.ColorType == SKImageInfo.PlatformColorType) {
                var info = pixmap.Info;
                using (var tempBitmap = new System.Drawing.Bitmap (info.Width, info.Height, info.RowBytes, System.Drawing.Imaging.PixelFormat.Format32bppPArgb, pixmap.GetPixels ()))
                using (var gr = System.Drawing.Graphics.FromImage (tempBitmap)) {
                    // Clear graphic to prevent display artifacts with transparent bitmaps					
                    gr.Clear (System.Drawing.Color.Transparent);

                    gr.DrawImageUnscaled (bitmap, 0, 0);
                }
            } else {
                // we have to copy the pixels into a format that we understand
                // and then into a desired format
                // TODO: we can still do a bit more for other cases where the color types are the same
                using (var tempImage = bitmap.ToSKImage ()) {
                    tempImage.ReadPixels (pixmap, 0, 0);
                }
            }
        }

        public static SKImage ToSKImage (this System.Drawing.Bitmap bitmap)
        {
            // TODO: maybe keep the same color types where we can, instead of just going to the platform default

            var info = new SKImageInfo (bitmap.Width, bitmap.Height);
            var image = SKImage.Create (info);
            using (var pixmap = image.PeekPixels ()) {
                bitmap.ToSKPixmap (pixmap);
            }
            return image;
        }

        /// <summary>
        /// Converts an SKRect to a Rectangle.
        /// </summary>
        public static Rectangle ToRectangle (this SKRect rect) => new Rectangle ((int)rect.Left, (int)rect.Top, (int)rect.Width, (int)rect.Height);

        /// <summary>
        /// Converts an SKSize to a Size.
        /// </summary>
        public static Size ToSize (this SKSize size) => new Size ((int)size.Width, (int)size.Height);

        /// <summary>
        /// Converts a Rectangle to an SKRect.
        /// </summary>
        public static SKRect ToSKRect (this Rectangle rect) => new SKRect (rect.X, rect.Y, rect.Right, rect.Bottom);

        /// <summary>
        /// Converts a Size to an SKSize.
        /// </summary>
        public static SKSize ToSKSize (this Size size) => new SKSize (size.Width, size.Height);
    }
}
