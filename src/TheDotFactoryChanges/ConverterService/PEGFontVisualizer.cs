using System;
using System.Drawing;

using DataAccessInterface;

namespace Service
{
    public class PEGFontVisualizer : BaseFontVisualizer, IFontVisualiser
    {
        private readonly int fontType = 1;
        private readonly int _padding = 2;
        private string _PEGfontName { get; set; }
        private Configuration _cfg;

        // === Constructors ===

        public PEGFontVisualizer(Configuration cfg)
        {
            _cfg = cfg;
        }
        public PEGFontVisualizer(Configuration cfg, string PEGfontName)
        {
            _PEGfontName = PEGfontName;
            _cfg = cfg;
        }

        // === public ===

        public void GetDump(System.Drawing.Font font, string template, out string source, out string header, ITextRenderer textRenderer)
        {
            GlypthCollection collection = PopulateFontInfo(font, template, _cfg, true, textRenderer);

            Rectangle commonOuterRect = GetOuterRectangle(collection);
            if (_cfg.paddingRemovalVertical == PaddingRemoval.Fixed)
            {
                CropBitmaps(collection, commonOuterRect, false, true);
            }
            else if (_cfg.paddingRemovalVertical == PaddingRemoval.Tighest)
            {
                CropBitmaps(collection, commonOuterRect, false, true);      // TODO 
            }

            if (_cfg.paddingRemovalHorizontal == PaddingRemoval.Fixed)
            {
                CropBitmaps(collection, commonOuterRect, true, false);
            }
            else if (_cfg.paddingRemovalHorizontal == PaddingRemoval.Tighest)
            {
                CropBitmaps(collection, true, false);
                foreach (Glypth item in collection)
                {
                    Rectangle rect = item.GetOuterRect();
                    rect.Width += _padding; 
                    item.SetOuterRect(rect);
                }
            }

            source = GenerateSourceText(collection, _cfg);
            header = GenerateHeaderText(collection, _cfg);
        }

        // === private ===
        private Rectangle GetOuterRectangle(GlypthCollection collection)
        {
            if (collection.Count() == 0)
                return new Rectangle(0, 0, 0, 0);

            Glypth tmp = (Glypth)collection.GetGlypths()[0];
            Rectangle rect = Utils.GetBitmapBorder(tmp.ToBitmap(), tmp.GetForegroundColor(), tmp.GetBackgroundColor());
            foreach (Glypth item in collection)
            {
                Rectangle border = Utils.GetBitmapBorder(item.ToBitmap(),
                    item.GetForegroundColor(), item.GetBackgroundColor());
                rect = Rectangle.Union(border, rect);
            }
            return rect;
        }
        private void CropBitmaps(GlypthCollection collection, Rectangle outerRect, 
            bool xPaddingRemove, bool yPaddingRemove)
        {
            foreach (Glypth item in collection)
            {
                Rectangle cpyRect = outerRect;
                Bitmap bmp = item.ToBitmap();
                if (!xPaddingRemove)
                {
                    cpyRect.X = 0;
                    cpyRect.Width = bmp.Width;
                }
                if (!yPaddingRemove)
                {
                    cpyRect.Y = 0;
                    cpyRect.Height = bmp.Height;
                }
                item.Crop(cpyRect);
            }
        }
        // TODO: доработать логику для случая, если ширина/высота отсекателя
        // меньше минимальной ширины/высоты, указанной в настройках
        private void CropBitmaps(GlypthCollection collection, bool xPaddingRemove, bool yPaddingRemove)
        {
            foreach (IComponent item in collection)
            {
                Bitmap bmp = item.ToBitmap();
                Rectangle outerRect = Utils.GetBitmapBorder(bmp, item.GetForegroundColor(), item.GetBackgroundColor());
                if (!xPaddingRemove)
                {
                    outerRect.X = item.GetOuterRect().X;
                    outerRect.Width = item.GetOuterRect().Width;
                }
                if (!yPaddingRemove)
                {
                    outerRect.Y = item.GetOuterRect().Y;
                    outerRect.Height = item.GetOuterRect().Height;
                }
                if (outerRect.Width < _cfg.spaceGenerationPixels)
                {
                    outerRect.X = 0;
                    outerRect.Width = _cfg.spaceGenerationPixels;
                }
                if (outerRect.Height < _cfg.minHeight)
                {
                    outerRect.Y = 0;
                    outerRect.Height = _cfg.minHeight;
                }
                item.Crop(outerRect);
            }
        }

        private string GenerateHeaderText(GlypthCollection collection, Configuration cfg)
        {
            string resultTextHeader = "";
            foreach (Glypth glypth in collection)
            {
                PageArray pageArray = ConvertBitmapToPageArray(glypth.ToBitmap(), cfg);

                resultTextHeader += String.Format("{0} '{1}' ({2} pixels wide){3}\n",
                                                    _cfg.commentStartString,
                                                    glypth.GetChar(),
                                                    glypth.Width(),
                                                    _cfg.commentEndString);

                string[] visualizer = pageArray.ToOutputHeaderLines(_cfg);
                for (int row = 0; row < visualizer.Length; row++)
                    resultTextHeader += visualizer[row] + "\n";
            }
            return resultTextHeader;
        }
        private string GenerateSourceText(GlypthCollection collection, Configuration cfg)
        {
            PageArray pageArray = ConvertBitmapToPageArray(collection.ToBitmap(), cfg);

            string resultTextSource = "";
            resultTextSource += String.Format("#include \"peg.hpp\"\n");
            resultTextSource += String.Format("ROMDATA WORD AutoGenerated_{0}" +
                                                "_offset_table[{1}] = {{\n",
                                                _PEGfontName, collection.Count() + 1);
            int tableOffset = 0;
            foreach (Glypth item in collection)
            {
                resultTextSource += String.Format("0x{0}, ", tableOffset.ToString("X"));
                tableOffset += item.Width();
            }
            resultTextSource += String.Format("0x{0}, ", tableOffset.ToString("X"));
            resultTextSource += String.Format("}};\n");
            resultTextSource += String.Format("ROMDATA UCHAR AutoGenerated_{0}" +
                                                "_data_table[] = {{\n", _PEGfontName);

            string[] pageArrayLines = pageArray.ToStringLines(_cfg);
            foreach (var str in pageArrayLines)
                resultTextSource += str + "\n";

            resultTextSource += "\n};\n\n";
            resultTextSource += String.Format("PegFont AutoGenerated_{0} = {{" +
                                                "{1}, {2}, {3}, {4}, {5}, {6}, {7}, \n",
                                                _PEGfontName, fontType, collection.Height(), 0, collection.Height(),
                                                pageArray.Width(), 0, collection.Count() - 1);
            resultTextSource += String.Format("(WORD*)AutoGenerated_{0}_offset_table, NULL,\n", _PEGfontName);
            resultTextSource += String.Format("(UCHAR *) AutoGenerated_{0}_data_table}};\n", _PEGfontName);
            return resultTextSource;
        }
    }
}
