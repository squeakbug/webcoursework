using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

using DataAccessInterface;

namespace Service
{
    public class FontVisualizer : BaseFontVisualizer, IFontVisualiser
    {
        private Configuration _cfg;

        // === Constructors ===

        public FontVisualizer(Configuration cfg)
        {
            _cfg = cfg;
        }

        // === public ===

        public void GetDump(System.Drawing.Font font, string template, out string source, out string header, ITextRenderer textRenderer)
        {
            GlypthCollection collection = PopulateFontInfo(font, template, _cfg, false, textRenderer);

            source = GenerateSourceText(collection, font, _cfg);
            header = GenerateHeaderText(collection, font, _cfg);
        }

        // === private ===

        private string GenerateHeaderText(GlypthCollection collection, System.Drawing.Font font, Configuration cfg)
        {
            string resultTextHeader = "";
            if (_cfg.commentVariableName)
            {
                resultTextHeader += String.Format("{0}Font data for {1} {2}pt{3}'n", cfg.commentStartString,
                    font.Name, Math.Round(font.Size), cfg.commentEndString);
            }
            resultTextHeader += GenerateHeaderStringsFromFontInfo(collection, font, cfg);
            return resultTextHeader;
        }
        private string GenerateSourceText(GlypthCollection collection, System.Drawing.Font font, Configuration cfg)
        {
            string resultTextSource = "";
            if (_cfg.commentVariableName)
            {
                resultTextSource += String.Format("{0}\n{1} Font data for {2} {3}pt\n{4}\n\n", cfg.commentStartString,
                    cfg.commentBlockMiddleString, font.Name, Math.Round(font.Size), cfg.commentBlockEndString);
            }
            resultTextSource += GenerateSourceStringsFromFontInfo(collection, font, cfg);
            return resultTextSource;
        }

        private static string GetCharBitmapVarName(System.Drawing.Font font, Configuration cfg)
        {
            return String.Format(cfg.varNfBitmaps, Utils.GetFontName(font));
        }
        private static string GetFontInfoVarName(System.Drawing.Font font, Configuration cfg)
        {
            return String.Format(cfg.varNfFontInfo, Utils.GetFontName(font));
        }
        private static string GetCharacterDescriptorArrayLookupDisplayString(System.Drawing.Font font)
        {
            return String.Format("{0}BlockLookup", Utils.GetFontName(font));
        }
        private static string GetVarNfCharInfo(System.Drawing.Font font, Configuration cfg)
        {
            return String.Format(cfg.varNfCharInfo, Utils.GetFontName(font));
        }


        public static string GetCharacterDescName(string name, DescriptorFormat descFormat)
        {
            if (descFormat == DescriptorFormat.DontDisplay) return "";

            string descFormatName = "";
            if (descFormat == DescriptorFormat.DisplayInBits) descFormatName = "bits";
            if (descFormat == DescriptorFormat.DisplayInBytes) descFormatName = "bytes";

            return String.Format("[Char {0} in {1}], ", name, descFormatName);
        }
        private string GetCharDescArrayGetBlockName(GlypthCollection collection, System.Drawing.Font font, Configuration cfg, int currentBlockIndex,
                                                 bool includeTypeDefinition, bool includeBlockIndex)
        {
            string blockIdString = String.Format("Block{0}", currentBlockIndex);
            string variableName = String.Format(cfg.varNfCharInfo, Utils.GetFontName(font));
            if (!includeTypeDefinition)
                variableName = Utils.GetVariableNameFromExpression(variableName);
            return String.Format("{0}{1}{2}",
                                    variableName,
                                    includeBlockIndex ? blockIdString : "",
                                    includeTypeDefinition ? "[]" : "");
        }
        private string GenerateSourceTextFromCharacterDescriptorBlockList(GlypthCollection collection, System.Drawing.Font font,
            List<GlypthCollection> characterBlockList, ref bool blockLookupGenerated)
        {
            string resultTextSource = "";
            bool multipleDescBlocksExist = characterBlockList.Count > 1;
            blockLookupGenerated = multipleDescBlocksExist;

            foreach (GlypthCollection block in characterBlockList)
            {
                if (_cfg.commentVariableName)
                {
                    string blockNumberString = String.Format("(block #{0})", characterBlockList.IndexOf(block));

                    resultTextSource += String.Format("{0}Character descriptors for {1} {2}pt{3}{4}\n",
                                                        _cfg.commentStartString, font.Name,
                                                        Math.Round(font.Size), multipleDescBlocksExist ? blockNumberString : "",
                                                        _cfg.commentEndString);
                    resultTextSource += String.Format("{0}{{ {1}{2}[Offset into {3}CharBitmaps in bytes] }}{4}\n",
                                                        _cfg.commentStartString,
                                                        GetCharacterDescName("width", _cfg.descCharWidth),
                                                        GetCharacterDescName("height", _cfg.descCharHeight),
                                                        Utils.GetFontName(font),
                                                        _cfg.commentEndString);
                }
                resultTextSource += String.Format("{0} = \n{{\n",
                    GetCharDescArrayGetBlockName(block, font, _cfg, characterBlockList.IndexOf(block), true, multipleDescBlocksExist));

                int offset = 0;
                foreach (Glypth glypth in block)
                {
                    resultTextSource += String.Format("\t{{{0}{1}{2}}}, \t\t{3}{4}{5}\n",
                                                    GetCharacterDescString(_cfg.descCharWidth, glypth.Width()),
                                                    GetCharacterDescString(_cfg.descCharHeight, glypth.Height()),
                                                    offset,
                                                    _cfg.commentStartString,
                                                    glypth.GetChar(),
                                                    _cfg.commentEndString + " ");
                    offset += glypth.Width();
                }
                resultTextSource += "};\n\n";
            }

            if (multipleDescBlocksExist)
            {
                if (_cfg.commentVariableName)
                {
                    resultTextSource += String.Format("{0}Block lookup array for {1} {2}pt {3}\n",
                                                        _cfg.commentStartString, font.Name,
                                                        Math.Round(font.Size), _cfg.commentEndString);
                    resultTextSource += String.Format("{0}{{ start character, end character, ptr to descriptor block array }}{1}\n",
                                                        _cfg.commentStartString,
                                                        _cfg.commentEndString);
                }
                resultTextSource += String.Format("const FONT_CHAR_INFO_LOOKUP {0}[] = \n{{\n",
                                                    GetCharacterDescriptorArrayLookupDisplayString(font));

                int curIndx = 0;
                foreach (GlypthCollection block in characterBlockList)
                {
                    Glypth firstChar = (Glypth)block.GetGlypths()[0], lastChar = 
                        (Glypth)block.GetGlypths()[block.Count() - 1]; // TODO: remove GetGlytph method using
                    resultTextSource += String.Format("\t{{{0}, {1}, &{2}}},\n",
                        GetCharacterDisplayString(firstChar.GetChar()),
                        GetCharacterDisplayString(lastChar.GetChar()),
                        GetCharDescArrayGetBlockName(block, font, _cfg, curIndx, false, true));
                    curIndx++;
                }
                resultTextSource += "};\n\n";
            }
            return resultTextSource;
        }
        private string GenerateHeaderTextFromCharacterDescriptorBlockList(GlypthCollection collection,
            List<GlypthCollection> characterBlockList, ref bool blockLookupGenerated)
        {
            return "";
        }
        private string GetFontInfoDescriptorsString(System.Drawing.Font font, Configuration cfg, bool blockLookupGenerated)
        {
            string descriptorString = "";

            if (cfg.generateLookupBlocks)
            {
                descriptorString += String.Format("\t{0}, {1} Character block lookup{2}\n",
                    blockLookupGenerated ? GetCharacterDescriptorArrayLookupDisplayString(font) : "NULL",
                    cfg.commentStartString, cfg.commentEndString);

                descriptorString += String.Format("\t{0}, {1} Character descriptor array{2}\n",
                    blockLookupGenerated ? "NULL" : Utils.GetVariableNameFromExpression(GetVarNfCharInfo(font, cfg)),
                    cfg.commentStartString, cfg.commentEndString);
            }
            else
            {
                string varNfCharInfo = GetVarNfCharInfo(font, cfg);
                descriptorString += String.Format("\t{0}, {1} Character descriptor array{2}\n",
                    Utils.GetVariableNameFromExpression(Utils.GetVariableNameFromExpression(GetVarNfCharInfo(font, cfg))),
                    cfg.commentStartString, cfg.commentEndString);
            }

            return descriptorString;
        }
        private string GenerateHeaderCharacterDescriptorArray(GlypthCollection collection, Configuration cfg,
            ref bool blockLookupGenerated)
        {
            string result = "";
            if (cfg.generateLookupArray)
            {
                List<GlypthCollection> collectionList = collection.DivideByDistance(cfg);
                result = GenerateHeaderTextFromCharacterDescriptorBlockList(collection, collectionList,
                    ref blockLookupGenerated);
            }
            return result;
        }
        private string GenerateSourceCharacterDescriptorArray(GlypthCollection collection, System.Drawing.Font font, Configuration cfg,
            ref bool blockLookupGenerated)
        {
            string result = "";
            if (cfg.generateLookupArray)
            {
                List<GlypthCollection> collectionList = collection.DivideByDistance(cfg);
                result = GenerateSourceTextFromCharacterDescriptorBlockList(collection, font, collectionList,
                    ref blockLookupGenerated);
            }
            return result;
        }

        private string GetCharacterDescString(DescriptorFormat descFormat, int valueInBits)
        {
            if (descFormat == DescriptorFormat.DontDisplay) return "";
            return String.Format("{0}, ", ConvertValueByDescriptorFormat(descFormat, valueInBits));
        }
        private string GenerateHeaderStringsFromFontInfo(GlypthCollection collection, System.Drawing.Font font, Configuration cfg)
        {
            string resultHeaderText = "";
            string charBitmapVarName = GetCharBitmapVarName(font, cfg) + "[]";
            string fontInfoVarName = GetFontInfoVarName(font, cfg);
            string nfCharInfo = GetVarNfCharInfo(font, cfg);

            resultHeaderText += String.Format("extern {0};\n", charBitmapVarName);
            resultHeaderText += String.Format("extern {0};\n", fontInfoVarName);

            bool blockLookupGenerated = false;
            resultHeaderText += GenerateHeaderCharacterDescriptorArray(collection, cfg, ref blockLookupGenerated);
            if (blockLookupGenerated)
            {
                resultHeaderText += String.Format("extern const FONT_CHAR_INFO_LOOKUP {0}[];\n",
                    GetCharacterDescriptorArrayLookupDisplayString(font));
            }
            else
            {
                resultHeaderText += String.Format("extern {0}[];\n", nfCharInfo);
            }
            return resultHeaderText;
        }
        private string GenerateSourceStringsFromFontInfo(GlypthCollection collection, System.Drawing.Font font, Configuration cfg)
        {
            string resultTextSource = "";
            if (cfg.commentVariableName)
            {
                resultTextSource += String.Format("{0}Character bitmaps for {1} {2}pt{3}\n",
                                                    cfg.commentStartString, font.Name,
                                                    Math.Round(font.Size), cfg.commentEndString);
            }

            resultTextSource += String.Format("{0} = \n{{\n", GetCharBitmapVarName(font, cfg));

            foreach (Glypth glypth in collection)
            {
                if (cfg.commentCharDescriptor)
                {
                    resultTextSource += String.Format("\t{0}@{1} '{2}' ({3} pixels wide){4}\n",
                                                        cfg.commentStartString,
                                                        glypth.Width(),         // TODO ???
                                                        glypth.GetChar(),
                                                        glypth.Width(),
                                                        cfg.commentEndString);
                }

                Bitmap bitmap = glypth.ToBitmap();
                PageArray pageArray = ConvertBitmapToPageArray(bitmap, cfg);
                string[] lines = pageArray.ToStringLines(cfg);
                foreach (string line in lines)
                    resultTextSource += "\t" + line + "\n";
                resultTextSource += "\n";
            }
            resultTextSource += "};\n\n";

            bool blockLookupGenerated = false;
            resultTextSource += GenerateSourceCharacterDescriptorArray(collection, font, cfg, ref blockLookupGenerated);

            string fontCharHeightString = "", spaceCharacterPixelWidthString = "";
            if (cfg.commentVariableName)
            {
                resultTextSource += String.Format("{0}Font information for {1} {2}pt{3}\n",
                                                    cfg.commentStartString,
                                                    font.Name, Math.Round(font.Size),
                                                    cfg.commentEndString);
            }

            if (cfg.descFontHeight != DescriptorFormat.DontDisplay)
            {
                fontCharHeightString = String.Format("\t{0}, {1} Character height{2}\n",
                                              ConvertValueByDescriptorFormat(cfg.descFontHeight, collection.Height()),
                                              cfg.commentStartString,
                                              cfg.commentEndString);
            }

            if (!cfg.generateSpaceCharacterBitmap)
            {
                spaceCharacterPixelWidthString = String.Format("\t{0}, {1} Width, in pixels, of space character{2}\n",
                                                                cfg.spaceGenerationPixels,
                                                                cfg.commentStartString,
                                                                cfg.commentEndString);
            }

            string fontInfoVarName = GetFontInfoVarName(font, cfg);
            char firstChar = ((Glypth)collection.GetGlypths()[0]).GetChar();
            char lastChar = ((Glypth)collection.GetGlypths().Last()).GetChar();
            resultTextSource += String.Format("{2} =\n{{\n" +
                                              "{3}" +
                                              "\t{4}, {0} Start character{1}\n" +
                                              "\t{5}, {0} End character{1}\n" +
                                              "{6}" +
                                              "{7}" +
                                              "\t{8}, {0} Character bitmap array{1}\n" +
                                              "}};\n",
                                              cfg.commentStartString,
                                              cfg.commentEndString,
                                              fontInfoVarName,
                                              fontCharHeightString,
                                              GetCharacterDisplayString(firstChar),
                                              GetCharacterDisplayString(lastChar),
                                              spaceCharacterPixelWidthString,
                                              GetFontInfoDescriptorsString(font, cfg, blockLookupGenerated),
                                              Utils.GetVariableNameFromExpression(GetCharBitmapVarName(font, cfg)));
            return resultTextSource;
        }
    }
}
