using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modern.Forms.Drawing
{
    [Flags]
    public enum TextDecoration
    {
        None,
        Bold = 1,
        Italic = 2,
        Underlined = 4,
    }
}
