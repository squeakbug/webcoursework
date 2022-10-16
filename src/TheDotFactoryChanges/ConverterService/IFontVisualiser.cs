using System.Drawing;

namespace Service
{
    public interface IFontVisualiser
    {
        void GetDump(System.Drawing.Font font, string template, out string source, out string header, ITextRenderer textRenderer);
    }
}
