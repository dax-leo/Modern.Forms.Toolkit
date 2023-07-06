using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;

namespace Modern.Forms.Drawing
{
    public interface IBrush
    {
        void Text (SKCanvas canvas, string text, SKRect frame, SKTypeface typeface, float size, TextDecoration decorations);

        void Fill (SKCanvas canvas, SKPath path);

        void FillRect (SKCanvas canvas, float x, float y, float width, float height);

        void Stroke (SKCanvas canvas, SKPath path, float size, StrokeStyle style, SKStrokeCap cap, SKStrokeJoin join);
    }
}
