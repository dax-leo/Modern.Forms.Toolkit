using System.Diagnostics;
using System.Drawing;
using System.Text;
using Modern.Forms;
using Modern.Forms.Scott;
using ScottPlotCookbook;

namespace Modern.Forms.Demo.ScottPlot
{
    public partial class MainForm : Form
    {        
        private TreeView tree;
        private ModernPlot _control;
        private Control _root;
        private Label _label;
        private TextBox _textBox;

        public MainForm ()
        {
            Text = "ScottPlot Gallery (Modern Forms UI)";
            Image = ImageLoader.Get("chart_pie_plane.png");

            tree = new TreeView {
                Dock = DockStyle.Left,
                ShowDropdownGlyph = true                
            };            
            tree.Style.Border.Width = 0;
            tree.Style.Border.Right.Width = 1;

            tree.ItemSelected += Tree_ItemSelected;
            
            // Add root panel for showcase panels
            _root = new Panel();
            _root.Dock = DockStyle.Fill;

            Controls.Add(tree);
            Controls.Insert(0, _root);

            InitPlot();

            _label = _root.Controls.Add(new Label() { Dock = DockStyle.Top, Multiline = true, Height = 70 });
            _label.Style.FontSize = 16;

            _textBox = _root.Controls.Add(new TextBox() { Text = "", Height = 200, Dock = DockStyle.Bottom, ScrollBars = ScrollBars.Vertical, MultiLine = true });

            // Load cookbook viewer            
            CookbookViewer_Load();
        }

        private void Tree_ItemSelected (object? sender, EventArgs<TreeViewItem> e)
        {            
            UpdateChart(e.Value.Text);
        }

        private void UpdateChart (string text)
        {
            if (tree.SelectedItem.Tag == null)
                return;

            IRecipe recipe = (IRecipe)tree.SelectedItem.Tag;
            _control.Reset();
            recipe.Recipe(_control.Plot);            
            _control.Refresh();

            _label.Text = recipe.Description;

            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "RecipeCode", recipe.Name + ".txt");
            if (Path.Exists(path))
                _textBox.Text = File.ReadAllText(path);
            else _textBox.Text = "";
        }

        private void CookbookViewer_Load()
        {
            tree.Items.Clear ();

            foreach (Chapter chapter in Cookbook.GetChapters())
            {
                foreach (RecipePageBase recipePage in Cookbook.GetPagesInChapter(chapter))
                {                    
                    var tlv = tree.Items.Add(recipePage.PageDetails.PageName);

                    foreach (IRecipe recipe in recipePage.GetRecipes())
                    {                        
                        var recipe_item = tlv.Items.Add(recipe.Name);
                        recipe_item.Tag = recipe;                             
                    }

                    tlv.Expand();
                }
            }

            tree.SelectedItem = tree.Items[0];            
        }

        private void InitPlot()
        {
            _control = new ModernPlot();
            _control.Dock = DockStyle.Fill;
            _root.Controls.Add(_control);
        }
    }
}
