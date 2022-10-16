using System;
using System.Text;
using System.Drawing;
using System.Text.RegularExpressions;

using DataAccessInterface;

namespace Service
{
    public static class Utils
    {
        // === public === 

        public static Rectangle GetBitmapBorder(Bitmap bitmap, Color fgClr, Color bgClr)
        {
            int leftX, rightX, bottomY, upperY;
            for (leftX = 0; leftX < bitmap.Width; leftX++)
            {
                if (!IsColumnEmpty(bitmap, leftX, fgClr, bgClr)) break;
            }
            for (rightX = bitmap.Width - 1; rightX > leftX; rightX--)
            {
                if (!IsColumnEmpty(bitmap, rightX, fgClr, bgClr)) break;
            }
            for (upperY = 0; upperY < bitmap.Height; upperY++)
            {
                if (!IsRowEmpty(bitmap, upperY, fgClr, bgClr)) break;
            }
            for (bottomY = bitmap.Height - 1; bottomY > upperY; bottomY--)
            {
                if (!IsRowEmpty(bitmap, bottomY, fgClr, bgClr)) break;
            }
            return new Rectangle(leftX, upperY, rightX - leftX + 1, bottomY - upperY + 1);
        }

        public static RotateFlipType getOutputRotateFlipType(Configuration cfg)
        {
            bool fx = cfg.flipHorizontal;
            bool fy = cfg.flipVertical;
            Rotation rot = cfg.rotation;

            // zero degree rotation
            if (rot == Rotation.RotateZero)
            {
                // return according to flip
                if (!fx && !fy) return RotateFlipType.RotateNoneFlipNone;
                if (fx && !fy) return RotateFlipType.RotateNoneFlipX;
                if (!fx && fy) return RotateFlipType.RotateNoneFlipY;
                if (fx && fy) return RotateFlipType.RotateNoneFlipXY;
            }

            // 90 degree rotation
            if (rot == Rotation.RotateNinety)
            {
                // return according to flip
                if (!fx && !fy) return RotateFlipType.Rotate90FlipNone;
                if (fx && !fy) return RotateFlipType.Rotate90FlipX;
                if (!fx && fy) return RotateFlipType.Rotate90FlipY;
                if (fx && fy) return RotateFlipType.Rotate90FlipXY;
            }

            // 180 degree rotation
            if (rot == Rotation.RotateOneEighty)
            {
                // return according to flip
                if (!fx && !fy) return RotateFlipType.Rotate180FlipNone;
                if (fx && !fy) return RotateFlipType.Rotate180FlipX;
                if (!fx && fy) return RotateFlipType.Rotate180FlipY;
                if (fx && fy) return RotateFlipType.Rotate180FlipXY;
            }

            // 270 degree rotation
            if (rot == Rotation.RotateTwoSeventy)
            {
                // return according to flip
                if (!fx && !fy) return RotateFlipType.Rotate270FlipNone;
                if (fx && !fy) return RotateFlipType.Rotate270FlipX;
                if (!fx && fy) return RotateFlipType.Rotate270FlipY;
                if (fx && fy) return RotateFlipType.Rotate270FlipXY;
            }

            // unknown case, but just return no flip
            return RotateFlipType.RotateNoneFlipNone;
        }

        // get only the variable name from an expression in a specific format
        // e.g. input: const far unsigned int my_font[] = ; 
        //      output: my_font[]
        public static string GetVariableNameFromExpression(string expression)
        {
            int charIndex;
            const string invalidFormatString = "##Invalid format##";

            // Strip array parenthesis
            const string arrayRegexString = @"\[[0-9]*\]";
            expression = Regex.Replace(expression, arrayRegexString, "");

            // Find the string between '=' and a space, trimming spaces from end
            for (charIndex = expression.Length - 1; charIndex != 1; --charIndex)
            {
                if (expression[charIndex] != '=' && expression[charIndex] != ' ') break;
            }
            if (charIndex == 0) return invalidFormatString;
            int lastVariableNameCharIndex = charIndex;
            for (charIndex = lastVariableNameCharIndex; charIndex != 0; --charIndex)
            {
                if (expression[charIndex] == ' ') break;
            }

            if (charIndex == 0) return invalidFormatString;
            int firstVariableNameCharIndex = charIndex + 1;
            return expression.Substring(firstVariableNameCharIndex, lastVariableNameCharIndex - firstVariableNameCharIndex + 1);
        }
        // make 'name' suitable as a variable name, starting with '_'
        // or a letter and containing only letters, digits, and '_'
        public static string ScrubVariableName(string name)
        {
            StringBuilder outName = new StringBuilder();
            foreach (char ch in name)
            {
                if (Char.IsLetterOrDigit(ch) || ch == '_')
                    outName.Append(ch);
            }

            if (Char.IsDigit(outName[0]))
                outName.Insert(0, '_');

            outName[0] = Char.ToLower(outName[0]);

            return outName.ToString();
        }
        public static string GetFontName(System.Drawing.Font font)
        {
            return ScrubVariableName(font.Name + "_" + Math.Round(font.Size) + "pt");
        }

        public static bool CharacterRangePointParse(string rangePointString, ref int rangePoint)
        {
            rangePointString = rangePointString.Trim();

            try
            {
                // check if 0x is start of range
                if (rangePointString.Substring(0, 2) == "0x")
                {
                    // remove 0x
                    rangePointString = rangePointString.Substring(2, rangePointString.Length - 2);
                    rangePoint = Int32.Parse(rangePointString, System.Globalization.NumberStyles.HexNumber);
                }
                else
                {
                    rangePoint = Int32.Parse(rangePointString);
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
        public static string ExpandAndRemoveCharacterRanges(string chars)
        {
            var searchPattern = @"<<(?<rangeStart>.*?)-(?<rangeEnd>.*?)>>";
            Regex regex = new Regex(searchPattern, RegexOptions.Multiline);
            string result = chars;

            int charactersRemoved = 0;
            foreach (Match regexMatch in regex.Matches(result))
            {
                int rangeStart = 0, rangeEnd = 0;

                if (CharacterRangePointParse(regexMatch.Groups["rangeStart"].Value, ref rangeStart) &&
                    CharacterRangePointParse(regexMatch.Groups["rangeEnd"].Value, ref rangeEnd))
                {
                    result = result.Remove(regexMatch.Index - charactersRemoved, regexMatch.Length);

                    // save the number of chars removed so that we can fixup index (the index
                    // of the match changes as we remove characters)
                    charactersRemoved += regexMatch.Length;

                    // create a string from these values
                    for (int charIndex = rangeStart; charIndex <= rangeEnd; ++charIndex)
                    {
                        char unicodeChar = (char)charIndex;
                        result += unicodeChar;
                    }
                }
            }
            return result;
        }

        // === private ===

        private static bool IsColumnEmpty(Bitmap bitmap, int column, Color fgClr, Color bgClr)
        {
            for (int row = 0; row < bitmap.Height; ++row)
            {
                if (bitmap.GetPixel(column, row).ToArgb() != bgClr.ToArgb())
                    return false;
            }
            return true;
        }
        private static bool IsRowEmpty(Bitmap bitmap, int row, Color fgClr, Color bgClr)
        {
            for (int column = 0; column < bitmap.Width; ++column)
            {
                if (bitmap.GetPixel(column, row).ToArgb() != bgClr.ToArgb())
                    return false;
            }
            return true;
        }
    }
}
