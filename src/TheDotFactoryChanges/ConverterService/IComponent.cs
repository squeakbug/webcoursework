using System.Collections.Generic;
using System.Drawing;

namespace Service
{
    public interface IComponent : IEnumerable<IComponent>
    {
        int Height();
        int Width();
        Rectangle GetOuterRect();
        void SetOuterRect(Rectangle rect);
        Rectangle GetInnerRect();
        void SetInnerRect(Rectangle rect);
        Color GetBackgroundColor();
        void SetBackgroundColor(Color bgClr);
        Color GetForegroundColor();
        void SetForegroundColor(Color fgClr);
        void SetFont(Font font);
        void SetRotateFlipType(RotateFlipType rotateFlipType);
        void Crop(Rectangle rect);

        Bitmap ToBitmap();

        void Add(IComponent component);
        void Remove(IComponent component);
        bool IsCollection();
        int Count();
    }
}
