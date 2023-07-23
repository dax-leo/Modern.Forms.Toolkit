using System.Drawing;
using System.Runtime.InteropServices;
using ScottPlot;
using ScottPlot.Control;
using ScottPlot.Extensions;
using SkiaSharp;

namespace Modern.Forms.Scott
{
    public partial class ModernPlot : Control, IPlotControl
    {
        private Plot _plot;

        public ModernPlot ()
        {            
            Interaction = new (this) {               
            };

            this.MouseDown += OnMouseDown;
            this.MouseUp += OnMouseUp;
            this.MouseMove += OnMouseMove;
            this.MouseWheel += OnMouseWheel;
            //this.KeyDown += OnKeyDown;
            //this.KeyUp += OnKeyUp;
            this.DoubleClick += OnDoubleClick;
            //this.SizeChanged += OnSizeChanged;
        }

        protected override void OnPaint (PaintEventArgs args)
        {
            base.OnPaint (args);

            SKImageInfo imageInfo = args.Info;
            imageInfo.Width = ScaledWidth;
            imageInfo.Height = ScaledHeight;

            using var surface = SKSurface.Create (imageInfo);
            if (surface is null)
                return;

            Plot.Render (surface);
            
            args.Canvas.DrawSurface (surface, 0, 0);
        }


        public Plot Plot { 
            get { if (_plot == null) _plot = new (); return _plot; }
            private set { _plot = value; }
        } 

        public Interaction Interaction { get; private set; }

        public GRContext? GRContext => null;

        public void Refresh ()
        {
            Invalidate ();
        }

        public Plot Reset()
        {
            Plot newPlot = new()
            {
                FigureBackground = ScottPlot.Colors.White
            };

            return Reset(newPlot);
        }

        public Plot Reset(Plot newPlot)
        {
            Plot oldPlot = Plot;
            this.Plot = newPlot;
            oldPlot?.Dispose();
            return newPlot;
        }

        public void Replace (Interaction interaction)
        {
            Interaction = interaction;
        }

        public void ShowContextMenu (Pixel position)
        {
            var menu = GetContextMenu ();
            menu.Show (this, new System.Drawing.Point((int)position.X, (int)position.Y));
        }

        private ContextMenu GetContextMenu ()
        {
            ContextMenu menu = new ();
            foreach (var curr in Interaction.ContextMenuItems) {
                var menuItem = new MenuItem () { Text = curr.Label };
                menuItem.Click += (s, e) => curr.OnInvoke ();

                menu.Items.Add (menuItem);
            }

            return menu;
        }

        private void OnMouseDown (object? sender, MouseEventArgs e)
        {
            Interaction.MouseDown (new Pixel (e.X, e.Y), MouseButton.Left);
        }

        private void OnMouseUp (object? sender, MouseEventArgs e)
        {
            Interaction.MouseUp (new Pixel (e.X, e.Y), MouseButton.Left);
        }

        private void OnMouseWheel (object? sender, MouseEventArgs e)
        {
            Interaction.MouseWheelVertical (new Pixel (e.X, e.Y), e.Delta.Y);
        }

        private void OnDoubleClick (object? sender, MouseEventArgs e)
        {
            Interaction.DoubleClick ();
        }

        private void OnMouseMove (object? sender, MouseEventArgs e)
        {
            Interaction.OnMouseMove (new Pixel (e.X, e.Y));
        }
    }
}
