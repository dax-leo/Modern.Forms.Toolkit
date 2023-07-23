using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Modern.Forms.Demo.HtmlRenderer
{
    internal class SamplesLoader
    {
        /// <summary>
        /// Samples to showcase the HTML Renderer capabilities
        /// </summary>
        private static readonly List<HtmlSample> _showcaseSamples = new List<HtmlSample>();

        /// <summary>
        /// Samples to test the different features of HTML Renderer that they work correctly
        /// </summary>
        private static readonly List<HtmlSample> _testSamples = new List<HtmlSample>();

        /// <summary>
        /// Samples used to test extreme performance
        /// </summary>
        private static readonly List<HtmlSample> _performanceSamples = new List<HtmlSample>();


        /// <summary>
        /// Samples to showcase the HTML Renderer capabilities
        /// </summary>
        public static List<HtmlSample> ShowcaseSamples
        {
            get { return _showcaseSamples; }
        }

        /// <summary>
        /// Samples to test the different features of HTML Renderer that they work correctly
        /// </summary>
        public static List<HtmlSample> TestSamples
        {
            get { return _testSamples; }
        }

        /// <summary>
        /// Samples used to test extreme performance
        /// </summary>
        public static List<HtmlSample> PerformanceSamples
        {
            get { return _performanceSamples; }
        }

        public static void LoadSamples(string path)
        {
            var files = Directory.GetFiles(path, "*.htm", SearchOption.AllDirectories);
            
            Array.Sort(files);

            foreach (string file in files)
            {
                int extPos = file.LastIndexOf('.');
                int namePos = extPos > 0 && file.Length > 1 ? file.LastIndexOf('.', extPos - 1) : 0;
                string ext = file.Substring(extPos >= 0 ? extPos : 0);
                string shortName = namePos > 0 && file.Length > 2 ? file.Substring(namePos + 1, file.Length - namePos - ext.Length - 1) : file;

                if (".htm".IndexOf(ext, StringComparison.Ordinal) >= 0)
                {
                    var html = File.ReadAllText(file);
                    
                    if (html != null)
                    {
                        if (file.Contains("TestSamples"))
                        {
                            _testSamples.Add(new HtmlSample(shortName, file, html));
                        }
                        else if (file.Contains("PerfSamples"))
                        {
                            _performanceSamples.Add(new HtmlSample(shortName, file, html));
                        }
                        else
                        {                            
                            _showcaseSamples.Add(new HtmlSample(shortName, file, html));
                        }

                    }
                }
            }
        }


    }
}
