using System.Drawing;

namespace Service
{
    public interface ITextRenderer
    {
        Size MeasureText(string text, Font font);
    }
}
