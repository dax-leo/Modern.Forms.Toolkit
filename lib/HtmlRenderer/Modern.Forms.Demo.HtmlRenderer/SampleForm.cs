using SkiaSharp;
using TheArtOfDev.HtmlRenderer.Avalonia;

namespace Modern.Forms.Demo.HtmlRenderer
{
    public partial class SampleForm : Form
    {
        private SplitContainer _splitContainer;
        private HtmlPanel _htmlPanel;    
        private HtmlLabel _label;
        
        public SampleForm()
        {
            this.Size = new System.Drawing.Size(500, 400);

            _splitContainer = new SplitContainer();
            _splitContainer.Dock = DockStyle.Fill;
            _splitContainer.Orientation = Orientation.Horizontal;
            _splitContainer.Panel1.Width = 300;
            Controls.Add(_splitContainer);
            AddHtmlPanel();
        }

        public void SetHtml(string html)
        {
            _htmlPanel.Text = html;
            if (string.IsNullOrWhiteSpace(html))
            {
                _htmlPanel.PerformLayout();
            }
        }

        private void AddHtmlPanel()
        {
            _htmlPanel = new HtmlPanel();
            _htmlPanel.Dock = DockStyle.Fill;

            _label = new HtmlLabel();
            _label.Dock = DockStyle.Fill;

            _label.Html = DemoUtils.SampleHtmlLabelText;
            _htmlPanel.Html = DemoUtils.SampleHtmlPanelText;

            _splitContainer.Panel1.Controls.Add(_htmlPanel);
            _splitContainer.Panel2.Controls.Add(_label);
        }

    }
}