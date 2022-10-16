using System;
using System.Text;

using DataAccessInterface;

namespace Service
{
    public class PageArray
    {
        private byte[,] _pages;
        private BitLayout _bitLayout;
        private readonly string _placeHolder = " ";

        // === Constructors ===

        public PageArray(int height, int width)
        {
            _pages = new byte[height, width];
            _bitLayout = BitLayout.RowMajor;
        }
        public PageArray(int height, int width, BitLayout bitLayout)
        {
            _pages = new byte[height, width];
            _bitLayout = bitLayout;
        }
        public PageArray(byte[,] pages, BitLayout bitLayout)
        {
            _pages = pages;
            _bitLayout = bitLayout;
        }

        // === public ===

        public int Height()
        {
            return _pages.GetLength(0);
        }
        public int Width()
        {
            return _pages.GetLength(1);
        }

        public string[] ToStringLines(Configuration cfg)
        {
            return ToStringLinesImpl(_pages, cfg);
        }
        public string[] ToOutputHeaderLines(Configuration cfg)
        {
            return ToOutputHeaderLinesImpl(_pages, cfg);
        }
        public string ToString(Configuration cfg)
        {
            string[] lines = ToStringLinesImpl(_pages, cfg);
            string[] asciiVisualization = ToOutputHeaderLinesImpl(_pages, cfg);
            System.Diagnostics.Debug.Assert(asciiVisualization.Length == lines.Length);

            string result = "";
            if (cfg.bitLayout == BitLayout.RowMajor)
            {
                result = GetRowMajorString(asciiVisualization, lines, cfg);
            }
            else if (cfg.bitLayout == BitLayout.ColumnMajor)
            {
                result = GetColumnMajorString(asciiVisualization, lines, cfg);
            }
            return result;
        }
        public void Transpose(Configuration cfg)
        {
            int width = _pages.GetLength(0);
            int height = _pages.GetLength(1);

            int rowMajorPagesPerRow = (width + 7) / 8;
            int colMajorPagesPerRow = width;
            int colMajorRowCount = (height + 7) / 8;

            var colMajorPages = new byte[colMajorPagesPerRow, colMajorRowCount];
            for (int row = 0; row < height; ++row)
            {
                for (int col = 0; col < width; ++col)
                {
                    int srcIdx = row * rowMajorPagesPerRow + (col / 8);
                    int page = _pages[row, col / 8];

                    int bitMask = GetBitMask(7 - (col % 8), cfg);

                    if ((page & bitMask) != 0)
                    {
                        byte p = _pages[col, (row / 8)];
                        colMajorPages[col, (row / 8)] = (byte)(p | GetBitMask(row % 8, cfg));
                    }
                }
            }
        }

        // === private ===

        // return a bitMask to pick out the 'bitIndex'th bit allowing for byteOrder
        // MsbFirst: bitIndex = 0 = 0x01, bitIndex = 7 = 0x80
        // LsbFirst: bitIndex = 0 = 0x80, bitIndex = 7 = 0x01
        public static byte GetBitMask(int bitIndex, Configuration cfg)
        {
            return cfg.byteOrder == ByteOrder.MsbFirst
                ? (byte)(0x01 << bitIndex)
                : (byte)(0x80 >> bitIndex);
        }
        private string[] ToOutputHeaderLinesImpl(byte[,] pages, Configuration cfg)
        {
            int height = (cfg.bitLayout == BitLayout.RowMajor) ? pages.GetLength(0) : pages.GetLength(0) * 8;
            int width = (cfg.bitLayout == BitLayout.RowMajor) ? pages.GetLength(1) * 8 : pages.GetLength(1);

            string[] lines = new string[height];
            for (int i = 0; i < height; ++i)
                lines[i] = "// ";

            if (cfg.bitLayout == BitLayout.RowMajor)
            {
                for (int row = 0; row != height; ++row)
                {
                    for (int col = 0; col != width; ++col)
                    {
                        byte page = pages[row, col / 8];
                        byte bitMask = GetBitMask(7 - (col % 8), cfg);

                        lines[row] += (bitMask & page) == 0 ? _placeHolder : cfg.bmpVisualizerChar;
                    }
                }
            }
            else if (cfg.bitLayout == BitLayout.ColumnMajor)
            {
                for (int col = 0; col != width; ++col)
                {
                    for (int row = 0; row != height; ++row)
                    {
                        byte page = pages[row / 8, col];
                        byte bitMask = GetBitMask(7 - (row % 8), cfg);

                        lines[row] += (bitMask & page) == 1 ? cfg.bmpVisualizerChar : _placeHolder;
                    }
                }
            }
            else
            {
                throw new NotImplementedException("Not defined bit layout");
            }
            return lines;
        }
        private string[] ToStringLinesImpl(byte[,] pages, Configuration cfg)
        {
            int width = pages.GetLength(1);
            int height = pages.GetLength(0);

            string[] data = new string[height];
            for (int row = 0; row != height; ++row)
            {
                data[row] = "";
                for (int col = 0; col != width; ++col)
                {
                    byte page = pages[row, col];
                    data[row] += cfg.byteLeadingString;

                    if (cfg.byteFormat == ByteFormat.Hex)
                    {
                        data[row] += page.ToString("X").PadLeft(2, '0');
                    }
                    else
                    {
                        data[row] += Convert.ToString(page, 2).PadLeft(8, '0');
                    }
                    data[row] += ", ";
                }
            }
            return data;
        }
        private string GetRowMajorString(string[] data, string[] visualizer, Configuration cfg)
        {
            var strBuilder = new StringBuilder();
            if (cfg.lineWrap == LineWrap.AtColumn)
            {
                for (int row = 0; row != data.Length; ++row)
                {
                    strBuilder.Append("\t").Append(data[row]).Append(visualizer[row]).Append("\n");
                }
            }
            else if (cfg.lineWrap == LineWrap.AtBitmap)
            {
                strBuilder.Append("\t");
                for (int row = 0; row != data.Length; ++row)
                {
                    strBuilder.Append(data[row]);
                }
                strBuilder.Append("\n");
            }
            return strBuilder.ToString();
        }
        private string GetColumnMajorString(string[] data, string[] visualizer, Configuration cfg)
        {
            var strBuilder = new StringBuilder();
            for (int row = 0; row != visualizer.Length; ++row)
            {
                strBuilder.Append("\t").Append(visualizer[row]).Append("\n");
            }

            if (cfg.lineWrap == LineWrap.AtColumn)
            {
                for (int row = 0; row != data.Length; ++row)
                {
                    strBuilder.Append("\t").Append(data[row]).Append("\n");
                }
            }
            else if (cfg.lineWrap == LineWrap.AtBitmap)
            {
                strBuilder.Append("\t");
                for (int row = 0; row != data.Length; ++row)
                {
                    strBuilder.Append(data[row]);
                }
                strBuilder.Append("\n");
            }
            return strBuilder.ToString();
        }
    }
}
