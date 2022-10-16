using System;
using System.Drawing;

using DataAccessInterface;

namespace Service
{
    public class ImageVisualizer : BaseVisualizer, IImageVisualizer
    {
        private Configuration _cfg;
        
        // === Constructors ===

        public ImageVisualizer(Configuration cfg)
        {
            _cfg = cfg;
        }

        // === public ===

        public void GetDump(Bitmap bmp, string imageName, int minWidth, int minHeight, out string outputSourceText, out string outputHeaderText)
        {
            Color fgClr = GetForegroundColor();
            Color bgClr = GetBackgroundColor();

            imageName = Utils.ScrubVariableName(imageName);
            Rectangle bmpBorder = Utils.GetBitmapBorder(bmp, fgClr, bgClr);

            Bitmap bitmapManipulated = ManipulateBitmap(bmp, bmpBorder, _cfg, fgClr, bgClr, minWidth, minHeight);
            if (bitmapManipulated == null)
                throw new Exception("No black pixels found in bitmap (currently only monochrome bitmaps supported)");

            outputHeaderText = GenerateHeaderText(bmp, imageName, _cfg);
            outputSourceText = GenerateSourceText(bmp, imageName, _cfg);
        }

        // === private ===

        private Color GetForegroundColor()
        {
            return Color.Black;
        }
        private Color GetBackgroundColor()
        {
            return Color.White;
        }
        private string GenerateSourceText(Bitmap bmp, string imageName, Configuration cfg)
        {
            string outputSourceText = "";
            if (cfg.commentVariableName)
            {
                outputSourceText += String.Format("{0}\n" + "{1} Image data for {2}\n{3}\n",
                    cfg.commentStartString, cfg.commentBlockMiddleString, imageName, cfg.commentBlockEndString);
            }

            string dataVarName = String.Format(cfg.varNfBitmaps, imageName);
            outputSourceText += String.Format("{0} =\n" + "{{\n", dataVarName);

            PageArray pages = ConvertBitmapToPageArray(bmp, cfg);
            int pagesPerRow = ConvertValueByDescriptorFormat(DescriptorFormat.DisplayInBytes, bmp.Width);
            outputSourceText += pages.ToString(cfg);
            outputSourceText += String.Format("}};\n\n");
            if (cfg.commentVariableName)
            {
                outputSourceText += String.Format("{0}Bitmap sizes for {1}{2}\n",
                                                    cfg.commentStartString, imageName, cfg.commentEndString);
            }
            string heightVarName = String.Format(cfg.varNfHeight, imageName);
            string widthVarName = String.Format(cfg.varNfWidth, imageName);
            if (cfg.descImgWidth == DescriptorFormat.DisplayInBytes)
            {
                outputSourceText += String.Format("{0}Pages = {1};\n", widthVarName, pagesPerRow);
            }
            else
            {
                outputSourceText += String.Format("{0}Pixels = {1};\n", widthVarName, bmp.Width);
            }
            if (cfg.descImgHeight == DescriptorFormat.DisplayInBytes)
            {
                outputSourceText += String.Format("{0}Pages = {1};\n", heightVarName,
                    ConvertValueByDescriptorFormat(DescriptorFormat.DisplayInBytes, bmp.Height));
            }
            else
            {
                outputSourceText += String.Format("{0}Pixels = {1};\n", heightVarName, bmp.Height);
            }

            return outputSourceText;
        }
        private string GenerateHeaderText(Bitmap bmp, string imageName, Configuration cfg)
        {
            string outputHeaderText = "";
            if (cfg.commentVariableName)
            {
                outputHeaderText += String.Format("{0}Bitmap info for {1}{2}\n",
                    cfg.commentStartString, imageName, cfg.commentEndString);
            }

            string dataVarName = String.Format(cfg.varNfBitmaps, imageName);
            outputHeaderText += String.Format("extern {0};\n", dataVarName);

            string heightVarName = String.Format(cfg.varNfHeight, imageName);
            string widthVarName = String.Format(cfg.varNfWidth, imageName);
            string displayWidthSuffix = (cfg.descImgWidth == DescriptorFormat.DisplayInBytes) ? "Pages" : "Pixels";
            outputHeaderText += String.Format("extern {0}{1};\n", widthVarName, displayWidthSuffix);
            string displayHeightSuffix = (cfg.descImgHeight == DescriptorFormat.DisplayInBytes) ? "Pages" : "Pixels";
            outputHeaderText += String.Format("extern {0}{1};\n", heightVarName, displayHeightSuffix);

            return outputHeaderText;
        }
    }
}
