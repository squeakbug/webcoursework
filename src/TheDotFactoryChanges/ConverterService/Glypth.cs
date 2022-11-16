using System;
using System.Collections.Generic;
using System.Drawing;
using System.Collections;

namespace Service
{
    //  +----------+
    //  |**+----+**|
    //  |**|    |**|-- _outerRect
    //  |**|    |**|
    //  |**|    |**|
    //  |**|    |------- _innerRect
    //  |**+----+**|
    //  +----------+
    //  
    //  ** - область, залитая фоновым цветом
    //  внутри _innerRect располагается глиф

    public class Glypth : IComponent
    {
        private char _character;
        private Font _font;
        private Color _backgroundColor = Color.White;
        private Color _foregroundColor = Color.Black;
        private Rectangle _innerRect;
        private Rectangle _outerRect;
        private Size _bmpSize;
        private RotateFlipType _rotateFlipType = RotateFlipType.RotateNoneFlipNone;
        private Bitmap _bmp = null;

        // === Costructors ===

        public Glypth(char ch, Font font)
        {
            _character = ch;
            _font = font;
            Rectangle defaultRect = GetDefaultRectangle(ch, font);
            _innerRect = _outerRect = defaultRect;
            _bmpSize.Width = defaultRect.Width;
            _bmpSize.Height = defaultRect.Height;

            UpdateBitmap();
        }
        public Glypth(char ch, Font font, Size minSize)
        {
            _character = ch;
            _font = font;
            Rectangle defaultRect = GetDefaultRectangle(ch, font);
            int width = Math.Max(defaultRect.Width, minSize.Width);
            int height = Math.Max(defaultRect.Height, minSize.Height);
            _innerRect = _outerRect = new Rectangle(0, 0, width, height);
            _bmpSize.Width = width;
            _bmpSize.Height = height;

            UpdateBitmap();
        }

        // === IComponent ===

        public int Width()
        {
            return _outerRect.Width;
        }
        public int Height()
        {
            return _outerRect.Height;
        }
        public Rectangle GetOuterRect()
        {
            return _outerRect;
        }
        public void SetOuterRect(Rectangle rect)
        {
            //if (!rect.Contains(_innerRect))
            //    throw new ApplicationException("Outer rect must be greater than inner rect");
            if (rect.X < 0)
                throw new ApplicationException("X coord must be positive");
            if (rect.Y < 0)
                throw new ApplicationException("Y coord must be positive");

            _outerRect = rect;
            UpdateBitmap();
        }
        public Rectangle GetInnerRect()
        {
            return _innerRect;
        }
        public void SetInnerRect(Rectangle rect)
        {
            //if (!_outerRect.Contains(rect))
            //    throw new ApplicationException("Outer rect must be greater than inner rect");
            if (rect.X < 0)
                throw new ApplicationException("X coord must be positive");
            if (rect.Y < 0)
                throw new ApplicationException("Y coord must be positive");

            _innerRect = rect;
            UpdateBitmap();
        }
        public void SetBackgroundColor(Color clr)
        {
            _backgroundColor = clr;
            UpdateBitmap();
        }
        public Color GetBackgroundColor()
        {
            return _backgroundColor;
        }
        public void SetForegroundColor(Color clr)
        {
            _foregroundColor = clr;
            UpdateBitmap();
        }
        public Color GetForegroundColor()
        {
            return _foregroundColor;
        }
        public void SetFont(Font font)
        {
            _font = font;
            UpdateBitmap();
        }
        public void SetRotateFlipType(RotateFlipType rotateFlipType)
        {
            _rotateFlipType = rotateFlipType;
            UpdateBitmap();
        }
        public void Crop(Rectangle rect)
        {
            _innerRect.Intersect(rect);
            _outerRect.Intersect(rect);
        }

        public Bitmap ToBitmap()
        {
            var bmp = new Bitmap(_outerRect.Width + _outerRect.X, _outerRect.Height + _outerRect.Y);
            Graphics gfx = Graphics.FromImage(bmp);
            var backgroundBrush = new SolidBrush(_backgroundColor);
            gfx.FillRectangle(backgroundBrush, _outerRect);
            var innerBmp = _bmp.Clone(_innerRect, _bmp.PixelFormat);
            gfx.DrawImage(innerBmp, _innerRect.X, _innerRect.Y);
            gfx.Flush();
            _bmp = bmp;

            /* TODO correct rotation
             * 
            Bitmap bgBmp = _bmp.Clone(_outerRect, _bmp.PixelFormat);
            var backgroundBrush = new SolidBrush(_backgroundColor);
            Graphics gfx1 = Graphics.FromImage(bgBmp);
            gfx1.FillRectangle(backgroundBrush, _outerRect);
            Bitmap fgBmp = _bmp.Clone(_innerRect, _bmp.PixelFormat);
            gfx1.DrawImage(fgBmp, _innerRect.X - _outerRect.X, _innerRect.Y - _outerRect.Y);
            gfx1.Flush();

            Bitmap rotatedBitmap = new Bitmap(_bmpSize.Width, _bmpSize.Height);
            Graphics gfx2 = Graphics.FromImage(rotatedBitmap);
            gfx2.DrawImage(bgBmp, (_bmpSize.Width - _outerRect.Width) / 2,
                (_bmpSize.Height - _outerRect.Height) / 2);
            gfx2.Flush();
            rotatedBitmap.RotateFlip(_rotateFlipType);
            return rotatedBitmap;
            */

            return _bmp.Clone(_outerRect, _bmp.PixelFormat);
        }

        public void Add(IComponent component)
        {
            throw new ApplicationException("It isn't collection");
        }
        public void Remove(IComponent component)
        {
            throw new ApplicationException("It isn't collection");
        }
        public bool IsCollection()
        {
            return false;
        }
        public int Count()
        {
            throw new ApplicationException("It isn't collection");
        }

        public IEnumerator GetEnumerator()
        {
            throw new ApplicationException("It isn't collection");
        }
        IEnumerator<IComponent> IEnumerable<IComponent>.GetEnumerator()
        {
            throw new ApplicationException("It isn't collection");
        }

        // === public ===

        public Font GetFont()
        {
            return _font;
        }
        public void SetChar(char ch)
        {
            _character = ch;
            UpdateBitmap();
        }
        public char GetChar()
        {
            return _character;
        }
        
        // === private ===

        private Rectangle GetDefaultRectangle(char ch, Font font)
        {
            var bmp = new Bitmap(font.Height, font.Height);
            Graphics gfx = Graphics.FromImage(bmp);
            SizeF size = gfx.MeasureString(ch.ToString(), font);
            return new Rectangle(0, 0, (int)(Math.Ceiling(size.Width) + 0.5), (int)(Math.Ceiling(size.Height) + 0.5));
        }
        private void UpdateBitmap()
        {
            _bmp = new Bitmap(_bmpSize.Width, _bmpSize.Height);
            Graphics gfx = Graphics.FromImage(_bmp);

            StringFormat drawFormat = new StringFormat();
            drawFormat.Alignment = StringAlignment.Center;
            string letterString = _character.ToString();

            var foregroundBrush = new SolidBrush(_foregroundColor);
            var backgroundBrush = new SolidBrush(_backgroundColor);
            // disable anti alias
            gfx.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
            var rect = new Rectangle(0, 0, _bmpSize.Width, _bmpSize.Height);
            gfx.FillRectangle(backgroundBrush, rect);
            gfx.DrawString(letterString, _font, foregroundBrush, rect, drawFormat);
            gfx.Flush();
        }
    }
}
