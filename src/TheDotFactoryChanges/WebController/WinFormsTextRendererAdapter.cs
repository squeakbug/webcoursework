using System;
using System.Drawing;

using Service;

namespace WebController
{
    public class WinFormsTextRendererAdapter : ITextRenderer
    {
        public Size MeasureText(string text, Font font)
        {
            SizeF result;
            using (var image = new Bitmap(1, 1))
            {
                using (var g = Graphics.FromImage(image))
                {
                    result = g.MeasureString(text, font);
                }
            }

            return result.ToSize();
        }
    }
}
