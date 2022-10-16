using System;
using System.Drawing;

using DataAccessInterface;

namespace Service
{
    public abstract class BaseVisualizer
    {
        protected Bitmap ManipulateBitmap(Bitmap bmp, Rectangle tightestCommonBorder,
            Configuration cfg, Color fgClr, Color bgClr, int minWidth, int minHeight)
        {
            Rectangle bitmapBorder = Utils.GetBitmapBorder(bmp, fgClr, bgClr);

            if (bitmapBorder.Width < minWidth)
            {
                bitmapBorder.Width = minWidth;
            }
            if (bitmapBorder.Height < minHeight)
            {
                bitmapBorder.Height = minHeight;
            }

            if (cfg.paddingRemovalHorizontal == PaddingRemoval.Fixed)
            {
                
            }
            else if (cfg.paddingRemovalHorizontal == PaddingRemoval.None)
            {
                bitmapBorder.X = 0;
                bitmapBorder.Width = bmp.Width - 1;
            }
            else if (cfg.paddingRemovalHorizontal == PaddingRemoval.Tighest)
            {
                bitmapBorder.X = tightestCommonBorder.X;
                bitmapBorder.Width = tightestCommonBorder.Width;
            }

            if (cfg.paddingRemovalVertical == PaddingRemoval.Fixed)
            {
                
            }
            else if (cfg.paddingRemovalVertical == PaddingRemoval.None)
            {
                bitmapBorder.Y = 0;
                bitmapBorder.Height = bmp.Height - 1;
            }
            else if (cfg.paddingRemovalVertical == PaddingRemoval.Tighest)
            {
                bitmapBorder.Y = tightestCommonBorder.Y;
                bitmapBorder.Height = tightestCommonBorder.Height;
            }
            return bmp.Clone(bitmapBorder, bmp.PixelFormat);
        }
        protected byte GetMask(ByteOrder byteOrder, int bitNum)
        {
            if (byteOrder == ByteOrder.MsbFirst)
                return (byte)(1 << (7 - bitNum));

            return (byte)(1 << bitNum);
        }
        protected PageArray ConvertBitmapToPageArray(Bitmap bmp, Configuration cfg)
        {
            byte[,] pageArray = new byte[bmp.Height, (bmp.Width + 7) / 8];  // byte / bit == 8
            for (int row = 0; row < bmp.Height; row++)
            {
                byte currentValue = 0;
                int bitsRead = 0, pageArrCol = 0;
                for (int bmpCol = 0; bmpCol < bmp.Width; ++bmpCol)
                {
                    if (bmp.GetPixel(bmpCol, row).ToArgb() == Color.Black.ToArgb())
                    {
                        currentValue |= GetMask(cfg.byteOrder, bitsRead);
                    }

                    ++bitsRead;
                    if (bitsRead == 8)
                    {
                        pageArray[row, pageArrCol] = currentValue;
                        currentValue = 0;
                        bitsRead = 0;
                        pageArrCol++;
                    }
                }

                if (bitsRead != 0)
                {
                    pageArray[row, pageArrCol] = currentValue;
                }
            }

            var result = new PageArray(pageArray, cfg.bitLayout);
            if (cfg.bitLayout == BitLayout.ColumnMajor)
            {
                result.Transpose(cfg);
            }
            return result;
        }

        protected string GetCharacterDisplayString(char character)
        {
            if (character < 255)
            {
                return String.Format("'{0}'", character);
            }
            else
            {
                int numericValue = (int)character;
                return numericValue.ToString();
            }
        }
        protected int ConvertValueByDescriptorFormat(DescriptorFormat descFormat, int valueInBits)
        {
            if (descFormat == DescriptorFormat.DisplayInBytes)
            {
                int valueInBytes = valueInBits / 8;
                if (valueInBits % 8 != 0) valueInBytes++;
                return valueInBytes;
            }
            else
            {
                return valueInBits;
            }
        }
    }
}
