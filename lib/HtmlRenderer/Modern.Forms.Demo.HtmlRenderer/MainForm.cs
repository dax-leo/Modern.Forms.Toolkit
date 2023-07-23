using SkiaSharp;
using System.Diagnostics;
using TheArtOfDev.HtmlRenderer.Avalonia;

namespace Modern.Forms.Demo.HtmlRenderer
{
    public partial class MainForm : Form
    {
        private HtmlPanel _htmlPanel;
        private TreeView _tree;
        private ToolBar _toolbar;
        private HtmlRenderingHelper _helper;

        public MainForm()
        {
            TitleBar.Text = "Html Renderer Demo";

            string img_dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sample", "Images");
            _helper = new HtmlRenderingHelper(img_dir);

            img_dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sample");
            SamplesLoader.LoadSamples(img_dir);

            //var html = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sample", "03.Tables.htm");// "03.Tables.htm");
            //// html = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sample", "13.Tables.htm");// "03.Tables.htm");            
            ////html = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sample", "05.Images.htm");

            ////var text = File.ReadAllText(html);
            //_htmlCtrl.Html = File.ReadAllText(html);
            //_htmlCtrl.PerformLayout();

            AddHtmlPanel();            
            AddTreeview();
            AddToolbar();
        }

        public void SetHtml(string html)
        {
            _htmlPanel.Text = html;
            if (string.IsNullOrWhiteSpace(html))
            {
                _htmlPanel.PerformLayout();
            }
        }

        private void AddTreeview()
        {
            _tree = new TreeView
            {
                Dock = DockStyle.Left,
                ShowDropdownGlyph = true                
            };
            _tree.Style.BackgroundColor = SKColors.AliceBlue;
            _tree.Style.Border.Width = 0;
            _tree.Style.Border.Right.Width = 1;
            _tree.ItemSelected += _tree_ItemSelected;

            #region Data

            var showcaseRoot = _tree.Items.Add("HTML Renderer", ImageLoader.Get("menu_item.png"));
            foreach (var sample in SamplesLoader.ShowcaseSamples)
            {
                var item = showcaseRoot.Items.Add(sample.Name, ImageLoader.Get("menu_item.png"));
                item.Tag = sample;
            }

            var testSamplesRoot = _tree.Items.Add("Test Samples", ImageLoader.Get("menu_item.png"));
            foreach (var sample in SamplesLoader.TestSamples)
            {
                var item = testSamplesRoot.Items.Add(sample.Name, ImageLoader.Get("menu_item.png"));
                item.Tag = sample;
            }

            if (SamplesLoader.PerformanceSamples.Count > 0)
            {
                var perfTestSamplesRoot = _tree.Items.Add("Perf. Samples", ImageLoader.Get("menu_item.png"));
                foreach (var sample in SamplesLoader.PerformanceSamples)
                {
                    var item = perfTestSamplesRoot.Items.Add(sample.Name, ImageLoader.Get("menu_item.png"));
                    item.Tag = sample;
                }
            }

            showcaseRoot.Expand();

            if (showcaseRoot.Items.Count > 0)
            {
                _tree.SelectedItem = showcaseRoot.Items[0];
            }

            #endregion

            Controls.Add(_tree);
        }

        private void _tree_ItemSelected(object? sender, EventArgs<TreeViewItem> e)
        {
            var sample = e.Value.Tag as HtmlSample;
            if (sample != null)
            {                
                try
                {                    
                    _htmlPanel.Html = sample.Html;
                    _htmlPanel.PerformLayout();
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.ToString(), "Failed to render HTML");
                }

                //UpdateWebBrowserHtml();
            }
        }

        private void AddHtmlPanel()
        {
            _htmlPanel = new HtmlPanel();
            _htmlPanel.Dock = DockStyle.Fill;
            _htmlPanel.ImageLoad += _helper.OnImageLoad;
            _htmlPanel.StylesheetLoad += _helper.OnStylesheetLoad;
            _htmlPanel.LinkClicked += (s, e) => {
                if (e.Attributes["href"] == "SayHello")
                {
                    MessageBoxForm.Show(this, "Link Message", e.Attributes["href"]);
                }
            };

            Controls.Add(_htmlPanel);
        }

        private void AddToolbar()
        {
            _toolbar = new ToolBar ();
           
            // Open Sample Window
            var sampleWinItem = _toolbar.Items.Add(new MenuItem("Open Sample Window", ImageLoader.Get("application.png")));
            sampleWinItem.Click += (s, e) => {
                using (var f = new SampleForm())
                {
                    f.ShowDialog(this);
                }
            };
            _toolbar.Items.Add(new MenuSeparatorItem());

            // Open in external browser
            var openExtItem = _toolbar.Items.Add(new MenuItem("Open External", ImageLoader.Get("firefox.png")));
            openExtItem.Click += (s, e) => {
                var tmpFile = Path.ChangeExtension(Path.GetTempFileName(), ".htm");
                File.WriteAllText(tmpFile, _htmlPanel.GetHtml());

                new Process
                {
                    StartInfo = new ProcessStartInfo(tmpFile)
                    {
                        UseShellExecute = true
                    }
                }.Start();
            };
            _toolbar.Items.Add(new MenuSeparatorItem());

            // Generate image
            var genImgItem = _toolbar.Items.Add(new MenuItem("Generate Image", ImageLoader.Get("picture.png")));
            genImgItem.Click += async (s, e) =>
            {
                using (var f = new GenImageForm(_htmlPanel.GetHtml(), _helper))
                {
                    var t = await f.ShowDialog(this);
                }
            };
            _toolbar.Items.Add(new MenuSeparatorItem());

            // Run performance check
            _toolbar.Items.Add(new MenuItem("Run Performance", ImageLoader.Get("timeline.png")));

            Controls.Add(_toolbar);
        }

        ///// <summary>
        ///// Loads the tree of document samples
        ///// </summary>
        //private void LoadSamples()
        //{
        //    var showcaseRoot = new TreeViewItem();
        //    showcaseRoot.Header = "HTML Renderer";
        //    ((ItemCollection)_samplesTreeView.Items!).Add(showcaseRoot);

        //    foreach (var sample in SamplesLoader.ShowcaseSamples)
        //    {
        //        AddTreeItem(showcaseRoot, sample);
        //    }

        //    var testSamplesRoot = new TreeViewItem();
        //    testSamplesRoot.Header = "Test Samples";
        //    ((ItemCollection)_samplesTreeView.Items!).Add(testSamplesRoot);

        //    foreach (var sample in SamplesLoader.TestSamples)
        //    {
        //        AddTreeItem(testSamplesRoot, sample);
        //    }

        //    if (SamplesLoader.PerformanceSamples.Count > 0)
        //    {
        //        var perfTestSamplesRoot = new TreeViewItem();
        //        perfTestSamplesRoot.Header = PerformanceSamplesTreeNodeName;
        //        ((ItemCollection)_samplesTreeView.Items!).Add(perfTestSamplesRoot);

        //        foreach (var sample in SamplesLoader.PerformanceSamples)
        //        {
        //            AddTreeItem(perfTestSamplesRoot, sample);
        //        }
        //    }

        //    showcaseRoot.IsExpanded = true;

        //    if (((ItemCollection)showcaseRoot.Items!).Count > 0)
        //        ((TreeViewItem)((ItemCollection)showcaseRoot.Items!)[0]).IsSelected = true;
        //}

        ///// <summary>
        ///// Add an html sample to the tree and to all samples collection
        ///// </summary>
        //private void AddTreeItem(TreeViewItem root, HtmlSample sample)
        //{
        //    var html = sample.Html.Replace("$$Release$$", _htmlPanel.GetType().Assembly.GetName().Version.ToString());

        //    var node = new TreeViewItem();
        //    node.Header = sample.Name;
        //    node.Tag = new HtmlSample(sample.Name, sample.FullName, html);
        //    ((ItemCollection)root.Items!).Add(node);
        //}

    }
}