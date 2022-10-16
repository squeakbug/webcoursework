using System;
using System.Drawing;
using System.Windows.Forms;

using Service;

namespace View
{
    public class WinFormsTextRendererAdapter : ITextRenderer
    {
        public Size MeasureText(string text, Font font)
        {
            return TextRenderer.MeasureText(text, font);
        }
    }
}
