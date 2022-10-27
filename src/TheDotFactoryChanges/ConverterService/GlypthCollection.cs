using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

using DataAccessInterface;

namespace Service
{
    // Коллекция и Размещение (layout) для элементов
    //
    // +--------------------------------------------+  
    // |***+---------+----------+---------------+***|
    // |***|         |          |               |***|
    // |***|         |          |               |***|
    // |***|         |          |               |***|
    // |***|   ch1   |   ch2    |      ch3      |***|
    // |***|         |          |               |***|
    // |***|         |          |               |----- inner rect
    // |***|         |          |               |***|
    // |***+---------+----------+---------------+***|---outer rect
    // +--------------------------------------------+
    //
    // ** -- фоновая заливка

    public class GlypthCollection : IComponent, IEnumerable<IComponent>
    {
        private List<IComponent> _glypths;
        private List<int> _glypthsOffsets;
        private Color _backgroundColor = Color.White;
        private Color _foregroundColor = Color.Black;
        private Rectangle _outerRectangle;
        private Rectangle _innerRectangle;
        private Size _bmpSize;
        private ITextRenderer _textRenderer;

        // === Constructors ===

        public GlypthCollection()
        {
            _glypths = new List<IComponent>();
            _glypthsOffsets = new List<int>();
            _glypthsOffsets.Add(0);
        }
        public GlypthCollection(string chars, System.Drawing.Font font, Size minSize, ITextRenderer textRenderer)
        {
            _textRenderer = textRenderer;

            _bmpSize = new Size(0, 0);
            _glypths = new List<IComponent>(chars.Length);
            foreach (char ch in chars)
            {
                var glypth = new Glypth(ch, font, minSize);
                AddComponentImpl(glypth);
            }

            _glypthsOffsets = new List<int>(chars.Length + 1);
            int curOffset = 0;
            foreach (Glypth glypth in _glypths)
            {
                _glypthsOffsets.Add(curOffset);
                curOffset += glypth.Width();
            }
            _glypthsOffsets.Add(curOffset);

            _outerRectangle = _innerRectangle = new Rectangle(0, 0, _bmpSize.Width, _bmpSize.Height);
        }

        // === IComponent ===

        public int Width()
        {
            int width = 0;
            foreach (var item in _glypths)
                width += item.Width();
            return width;
        }
        public int Height()
        {
            if (_glypths.Count == 0)
                return 0;
            return _glypths[0].Height();
        }
        public Rectangle GetOuterRect()
        {
            return _outerRectangle;
        }
        public void SetOuterRect(Rectangle rect)
        {
            _outerRectangle = rect;
        }
        public Rectangle GetInnerRect()
        {
            return _innerRectangle;
        }
        public void SetInnerRect(Rectangle rect)
        {
            _innerRectangle = rect;
        }
        public Color GetBackgroundColor()
        {
            return _backgroundColor;
        }
        public void SetBackgroundColor(Color clr)
        {
            foreach (var item in _glypths)
                item.SetBackgroundColor(clr);
            _backgroundColor = clr;
        }
        public Color GetForegroundColor()
        {
            return _foregroundColor;
        }
        public void SetForegroundColor(Color clr)
        {
            foreach (var item in _glypths)
                item.SetForegroundColor(clr);
            _foregroundColor = clr;
        }
        public void SetFont(System.Drawing.Font font)
        {
            foreach (var item in _glypths)
                item.SetFont(font);
        }
        public void SetRotateFlipType(RotateFlipType rotateFlipType)
        {
            throw new NotImplementedException();
        }
        public void Crop(Rectangle rect)
        {
            throw new NotImplementedException();
        }

        public Bitmap ToBitmap()
        {
            return ToBitmapImpl();
        }

        public void Add(IComponent component)
        {
            _glypths.Add(component);
        }
        public void Remove(IComponent component)
        {
            _glypths.Remove(component);
        }
        public bool IsCollection()
        {
            return true;
        }
        public int Count()
        {
            return _glypths.Count();
        }

        public IEnumerator GetEnumerator()
        {
            return _glypths.GetEnumerator();
        }
        IEnumerator<IComponent> IEnumerable<IComponent>.GetEnumerator()
        {
            return _glypths.GetEnumerator();
        }

        // === public ===

        public List<IComponent> GetGlypths()
        {
            return _glypths;
        }
        public List<GlypthCollection> DivideByDistance(Configuration cfg)
        {
            char previousCharacter = '\0';

            var collectionList = new List<GlypthCollection>();
            var curCollection = new GlypthCollection();
            int differenceBetweenCharsForNewGroup = cfg.generateLookupBlocks ?
                    cfg.lookupBlocksNewAfterCharCount : int.MaxValue;

            foreach (Glypth glypth in _glypths)
            {
                char ch = glypth.GetChar();
                if (ch - previousCharacter < differenceBetweenCharsForNewGroup && previousCharacter != '\0')
                {
                    for (char sequentialCharIndex = (char)(previousCharacter + 1);
                            sequentialCharIndex < ch;
                            ++sequentialCharIndex)
                    {
                        curCollection.Add(glypth);
                    }
                }
                else
                {
                    collectionList.Add(curCollection);
                    curCollection = new GlypthCollection();
                }

                previousCharacter = ch;
            }
            if (curCollection.Count() > 0)
                collectionList.Add(curCollection);
            return collectionList;
        }

        // === private ===

        private void AddComponentImpl(IComponent component)
        {
            _glypths.Add(component);
            _bmpSize.Height = Math.Max(_bmpSize.Height, component.Height());
            _bmpSize.Width += component.Width();
        }

        private Rectangle GetLargestBitmap(List<Glypth> glypths)
        {
            Rectangle largestRect = new Rectangle(0, 0, 0, 0);

            foreach (Glypth item in glypths)
            {
                string letterString = item.ToString();

                Size stringSize = _textRenderer.MeasureText(letterString, item.GetFont());

                largestRect.Height = Math.Max(largestRect.Height, stringSize.Height);
                largestRect.Width = Math.Max(largestRect.Width, stringSize.Width);
            }
            return largestRect;
        }
        private Bitmap ToBitmapImpl()
        {
            int height = Height();
            int width = Width();
            var bmp = new Bitmap(width, height);

            int col = 0;
            for (int i = 0; i < height; ++i)
            {
                foreach (Glypth item in _glypths)
                {
                    Bitmap itemBitmap = item.ToBitmap();
                    for (int j = 0; j < item.Width(); ++j)
                    {
                        bmp.SetPixel(col, i, itemBitmap.GetPixel(j, i));
                        col++;
                    }
                }
                col = 0;
            }
            return bmp;
        }
        private int GetMaxHeight()
        {
            int maxHeight = 0;
            foreach (var item in _glypths)
            {
                if (item.Height() > maxHeight)
                    maxHeight = item.Height();
            }    
            return maxHeight;
        }
        private byte[,] CollectPageArrays(List<byte[,]> pageArrays)
        {
            if (pageArrays.Count == 0)
                return null;

            int width = 0;
            foreach (var item in pageArrays)
                width += item.GetLength(1);
            int height = pageArrays[0].GetLength(0);

            var pageArray = new byte[height, width];

            int col = 0;
            for (int i = 0; i < height; ++i)
            {
                foreach (var item in pageArrays)
                {
                    for (int j = 0; j < item.GetLength(1); ++j)
                    {
                        pageArray[i, col] = item[i, j];
                        col++;
                    }
                }
                col = 0;
            }
            return pageArray;
        }
    }
}
