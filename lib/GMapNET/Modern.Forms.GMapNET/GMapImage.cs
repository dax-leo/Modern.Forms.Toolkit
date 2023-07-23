﻿using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using GMap.NET.Internals;
using GMap.NET.MapProviders;
using SkiaSharp;

namespace GMap.NET.WindowsForms
{
    /// <summary>
    ///     image abstraction
    /// </summary>
    public class GMapImage : PureImage
    {
        public SKImage Img;

        public override void Dispose()
        {
            if (Img != null)
            {
                Img.Dispose();
                Img = null;
            }

            if (Data != null)
            {
                Data.Dispose();
                Data = null;
            }
        }
    }

    /// <summary>
    ///     image abstraction proxy
    /// </summary>
    public class GMapImageProxy : PureImageProxy
    {
        GMapImageProxy()
        {
        }

        public static void Enable()
        {
            GMapProvider.TileImageProxy = Instance;
        }

        public static readonly GMapImageProxy Instance = new GMapImageProxy();

        internal ColorMatrix ColorMatrix;

        static readonly bool Win7OrLater = Stuff.IsRunningOnWin7OrLater();

        public override PureImage FromStream(Stream stream)
        {
            try
            {
                var m = SKImage.FromEncodedData(stream);
                if (m != null)
                {
                    return new GMapImage {Img = ColorMatrix != null ? ApplyColorMatrix(m, ColorMatrix) : m};
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("FromStream: " + ex);
            }

            return null;
        }

        public override bool Save(Stream stream, PureImage image)
        {
            var ret = image as GMapImage;
            bool ok = true;

            if (ret.Img != null)
            {
                // try png
                try
                {
                    //ret.Img.Save(stream, ImageFormat.Png);
                }
                catch
                {
                    // try jpeg
                    try
                    {
                        stream.Seek(0, SeekOrigin.Begin);
                        //ret.Img.Save(stream, ImageFormat.Jpeg);
                    }
                    catch
                    {
                        ok = false;
                    }
                }
            }
            else
            {
                ok = false;
            }

            return ok;
        }

        SKImage ApplyColorMatrix(SKImage original, ColorMatrix matrix)
        {
            // create a blank bitmap the same size as original
            //var newBitmap = new Bitmap(original.Width, original.Height);

            //using (original) // destroy original
            //{
            //    // get a graphics object from the new image
            //    using (var g = Graphics.FromImage(newBitmap))
            //    {
            //        // set the color matrix attribute
            //        using (var attributes = new ImageAttributes())
            //        {
            //            attributes.SetColorMatrix(matrix);
            //            g.DrawImage(original,
            //                new Rectangle(0, 0, original.Width, original.Height),
            //                0,
            //                0,
            //                original.Width,
            //                original.Height,
            //                GraphicsUnit.Pixel,
            //                attributes);
            //        }
            //    }
            //}

            //return newBitmap;

            return original;
        }
    }
}
