using System.Diagnostics;
using System.Drawing;
using System.Text;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using Modern.Forms;
using Modern.Forms.GMapNET;

namespace Modern.Forms.Demo.GMapNET
{
    public partial class MainForm : Form
    {
        private Control? current_panel;
        private TreeView tree;
        private GMapControl _gmap;
        private Control _root;

        public MainForm ()
        {
            Text = "GMapNET Gallery (Modern Forms UI)";
            Image = ImageLoader.Get("globe_africa.png");

            tree = new TreeView {
                Dock = DockStyle.Left,
                ShowDropdownGlyph = true                
            };            
            tree.Style.Border.Width = 0;
            tree.Style.Border.Right.Width = 1;

            tree.Items.Add ("Basic Map", ImageLoader.Get ("globe_africa.png"));
            
            var tlv=tree.Items.Add ("Map Providers", ImageLoader.Get ("layers_map.png"));
            tlv.Items.Add ("OpenStreet", ImageLoader.Get ("openstreet.png"));
            tlv.Items.Add ("Google", ImageLoader.Get ("google.png"));
            tlv.Items.Add ("Bing", ImageLoader.Get ("bing.png"));
            tlv.Expand ();

            tlv = tree.Items.Add("WMS", ImageLoader.Get("layer_wms.png"));
            tlv.Items.Add("Marine Regions - Adriatic Sea");
            tlv.Items.Add("Weather - Wind data Germany");            
            tlv.Expand();

            tree.Items.Add("Polygons", ImageLoader.Get("draw_polygon.png"));
            tree.Items.Add("Markers", ImageLoader.Get("location_pin.png"));

            tree.ItemSelected += Tree_ItemSelected;
            Controls.Add (tree);

            // Add root panel for showcase panels
            _root = new Panel();
            _root.Dock = DockStyle.Fill;
            Controls.Insert(0, _root);

            // Create Gmap control
            CreateGmap();
        }

        private void Tree_ItemSelected (object? sender, EventArgs<TreeViewItem> e)
        {
            if (current_panel != null) {
                current_panel.Controls.Clear ();
                _root.Controls.Remove (current_panel);
                current_panel.Dispose ();
                current_panel = null;
            }

            var new_panel = CreatePanel (e.Value.Text);

            if (new_panel != null) {
                current_panel = new_panel;
                new_panel.Dock = DockStyle.Fill;
                _root.Controls.Insert (0, new_panel);                
            }
        }

        private Control? CreatePanel (string text)
        {
            switch (text)
            {
                case "Basic Map":
                    return BasicMap();
                case "Map Providers":
                    return MapProviders_Openstreet();
                case "OpenStreet":
                    return MapProviders_Openstreet();
                case "Google":
                    return MapProviders_Google();
                case "Bing":
                    return MapProviders_Bing();
                case "Marine Regions - Adriatic Sea":
                    return Wms();
                case "Weather - Wind data Germany":
                    return Wms2();
                case "Polygons":
                    return Polygon();
                case "Markers":
                    return Markers();
            }
            
            return null;
        }

        #region Panels

        private Control? BasicMap()
        {
            current_panel = new Control (); 
            current_panel.Dock = DockStyle.Fill;

            _gmap.MapProvider = OpenStreetMapProvider.Instance;
            _gmap.Position = new GMap.NET.PointLatLng(43.5147, 16.4435);
            _gmap.Zoom = 13;
            _gmap.Overlays.Clear();

            current_panel.Controls.Add(_gmap);

            return current_panel;
        }

        private Control? MapProviders_Openstreet()
        {
            current_panel = new Control();
            current_panel.Dock = DockStyle.Fill;

            _gmap.Position = new GMap.NET.PointLatLng(43.5147, 16.4435);
            _gmap.Zoom = 13;
            _gmap.Overlays.Clear();

            _gmap.MapProvider = OpenStreetMapProvider.Instance;

            current_panel.Controls.Add(_gmap);

            return current_panel;
        }

        private Control? MapProviders_Google()
        {
            current_panel = new Control();
            current_panel.Dock = DockStyle.Fill;

            _gmap.Position = new GMap.NET.PointLatLng(43.5147, 16.4435);
            _gmap.Zoom = 13;
            _gmap.Overlays.Clear();

            _gmap.MapProvider = GoogleMapProvider.Instance;

            current_panel.Controls.Add(_gmap);

            return current_panel;
        }

        private Control? MapProviders_Bing()
        {
            current_panel = new Control();
            current_panel.Dock = DockStyle.Fill;

            _gmap.Position = new GMap.NET.PointLatLng(43.5147, 16.4435);
            _gmap.Zoom = 13;
            _gmap.Overlays.Clear();

            _gmap.MapProvider = BingMapProvider.Instance;

            current_panel.Controls.Add(_gmap);

            return current_panel;
        }

        private Control? Wms()
        {
            current_panel = new Control();
            current_panel.Dock = DockStyle.Fill;

            _gmap.Position = new GMap.NET.PointLatLng(43.5147, 16.4435);
            _gmap.Zoom = 8;
            _gmap.Overlays.Clear();

            WMSProvider.szWmsLayer = "eez_12nm";
            WMSProvider.CustomWMSURL = @"http://geo.vliz.be/geoserver/MarineRegions/wms";
            _gmap.MapProvider = WMSProvider.Instance;
            ((WMSProvider)(_gmap.MapProvider)).BackgroundOverlayProvider = GoogleMapProvider.Instance;

            current_panel.Controls.Add(_gmap);

            return current_panel;
        }

        private Control? Wms2()
        {
            current_panel = new Control();
            current_panel.Dock = DockStyle.Fill;

            _gmap.Position = new GMap.NET.PointLatLng(49.5147, 14);
            _gmap.Zoom = 8;
            _gmap.Overlays.Clear();

            WMSProvider.szWmsLayer = "00_wetter_in_0std";
            WMSProvider.CustomWMSURL = @"https://kosm.geoinformation.htw-dresden.de/geoserver/wetterdienst/ows";           
            _gmap.MapProvider = WMSProvider.Instance;
            ((WMSProvider)(_gmap.MapProvider)).BackgroundOverlayProvider = GoogleMapProvider.Instance;

            current_panel.Controls.Add(_gmap);

            return current_panel;
        }

        private Control? Polygon()
        {
            current_panel = new Control();
            current_panel.Dock = DockStyle.Fill;

            _gmap.Position = new GMap.NET.PointLatLng(48.866383, 2.323575);
            _gmap.Zoom = 16;
            _gmap.Overlays.Clear();

            GMapOverlay polygons = new GMapOverlay("polygons");
            List<PointLatLng> points = new List<PointLatLng>();
            points.Add(new PointLatLng(48.866383, 2.323575));
            points.Add(new PointLatLng(48.863868, 2.321554));
            points.Add(new PointLatLng(48.861017, 2.330030));
            points.Add(new PointLatLng(48.863727, 2.331918));
            GMapPolygon polygon = new GMapPolygon(points, "[Jardin des Tuileries]");
            polygon.IsHitTestVisible = true;            
            polygon.Tag = "<Nice place to visit>";
            polygons.Polygons.Add(polygon);
            _gmap.Overlays.Add(polygons);

            //polygon.Fill = new SolidBrush(Color.FromArgb(50, Color.Red));
            //polygon.Stroke = new Pen(Color.Red, 1);

            current_panel.Controls.Add(_gmap);

            return current_panel;
        }

        private Control? Markers()
        {
            current_panel = new Control();
            current_panel.Dock = DockStyle.Fill;

            _gmap.Position = new GMap.NET.PointLatLng(43.5147, 16.4435);
            _gmap.Zoom = 13;
            _gmap.Overlays.Clear();

            GMapOverlay markers = new GMapOverlay("markers");

            GMapMarker marker =
                new GMap.NET.WindowsForms.Markers.GMarkerCross(
                    new GMap.NET.PointLatLng(43.5147, 16.4435)
                     )
                { 
                    Pen = new SkiaSharp.SKPaint() { 
                        StrokeWidth = 5, 
                        Color = SkiaSharp.SKColors.DeepPink }, 
                    ToolTipText = "Hello!" , 
                    ToolTipMode = MarkerTooltipMode.OnMouseOver 
                };

            markers.Markers.Add(marker);
            _gmap.Overlays.Add(markers);

            current_panel.Controls.Add(_gmap);

            return current_panel;
        }

        #endregion

        private void CreateGmap()
        {
            _gmap = new GMapControl();
            _gmap.Dock = DockStyle.Fill;
            _gmap.Position = new GMap.NET.PointLatLng(43.5147, 16.4435);            
            _gmap.ScaleMode = ScaleModes.Integer;
            _gmap.MaxZoom = 20;
            _gmap.MinZoom = 2;
            _gmap.Zoom = 13;
            _gmap.CanDragMap = true;            
            _gmap.ScaleMode = ScaleModes.Integer;
            _gmap.LevelsKeepInMemory = 1;
            _gmap.MapProvider = OpenStreetMapProvider.Instance;
            _gmap.PolygonsEnabled = true;
            _gmap.OnPolygonClick += OnPolygonClick;

            GMaps.Instance.Mode = AccessMode.ServerOnly;
        }

        private void OnPolygonClick(GMapPolygon item, MouseEventArgs e)
        {
            MessageBoxForm.Show(this, "Click Info", String.Format("Polygon {0} with tag {1} was clicked", item.Name, item.Tag));
        }
    }
}
