using SkiaSharp;
using System.Drawing;
using System.Drawing.Text;
using System.Reflection;
using TheArtOfDev.HtmlRenderer.Avalonia;
using TheArtOfDev.HtmlRenderer.Core.Entities;
using TheArtOfDev.HtmlRenderer.Core;
using System.Diagnostics;

namespace Modern.Forms.Demo.HtmlRenderer
{
    public partial class GenImageForm : Form
    {
        private string _html;
        private ToolBar _toolbar;
        private PictureBox _pictureBox;
        private HtmlRenderingHelper _helper;
        private bool _loaded = false;

        public GenImageForm(string html, HtmlRenderingHelper helper)
        {    
            this.Size = new Size(500, 400);
            _html = html;
            _helper = helper;

            _pictureBox = new PictureBox();
            _pictureBox.Dock = DockStyle.Fill;
            _pictureBox.Resize += (s,e) => { GenerateImage(); };
            Controls.Add(_pictureBox);

            AddToolbar();
            _loaded = true;     
            
            GenerateImage();
        }

        private void GenerateImage()
        {
            if (!_loaded)
            {
                return;
            }

            var img = HtmlRender.RenderToImageGdiPlus(
                _html,                 
                new SKSize(_pictureBox.ClientSize.Width, _pictureBox.ClientSize.Height), 
                null, 
                DemoUtils.OnStylesheetLoad, 
                _helper.OnImageLoad);

            _pictureBox.Image = img;
        }

        private void AddToolbar()
        {
            _toolbar = new ToolBar();

            // Generate image
            var genImgItem = _toolbar.Items.Add(new MenuItem("Save Image", ImageLoader.Get("picture.png")));
            genImgItem.Click += (s, e) =>
            {
                var saveDialog = new SaveFileDialog();
                {
                    saveDialog.AddFilter("Images", "*.png");// = "Images|*.png;*.bmp;*.jpg";
                    saveDialog.FileName = "image";
                    saveDialog.DefaultExtension = ".png";

                    var dialogResult = saveDialog.ShowDialog(this).Result;
                    if (dialogResult == DialogResult.OK)
                    {                        
                        using (var image = SKImage.FromBitmap(_pictureBox.Image))
                        using (var data = image.Encode(SKEncodedImageFormat.Png, 90))
                        {
                            // save the data to a stream
                            using (var stream = File.OpenWrite(saveDialog.FileName))
                            {
                                data.SaveTo(stream);
                                MessageBoxForm.Show(this, "Save Image", "Image saved!");
                            }
                        }
                    }
                }
            };
            
            Controls.Add(_toolbar);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            Controls.Clear();
            _html = null;
            _pictureBox.Image.Dispose();
            _pictureBox.Dispose();
            _toolbar.Dispose();
            _helper = null;
            _pictureBox = null;
            _toolbar = null;
        }
    }
}