using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;
using NUnit.Allure.Core;

using DataAccessInterface;
using Service;

namespace ConverterServiceTest
{
    [AllureNUnit]
    public class FontVisualizerTest
    {
        public class WinFormsTextRendererAdapter : ITextRenderer
        {
            public Size MeasureText(string text, System.Drawing.Font font)
            {
                SizeF result;
                using (var image = new Bitmap(1, 1))
                {
                    using (var g = Graphics.FromImage(image))
                    {
                        result = g.MeasureString(text, font);
                    }
                }

                return result.ToSize();
            }
        }

        public class PEGFontVisualizerTest
        {
            [Test]
            public void GetDumpTest()
            {
                var font = new System.Drawing.Font("Verdana", 10);
                var cfg = new Configuration();
                cfg.paddingRemovalHorizontal = PaddingRemoval.Tighest;
                cfg.paddingRemovalVertical = PaddingRemoval.Tighest;
                var sut = new FontVisualizer(cfg);

                string src, head;
                sut.GetDump(font, "1", out src, out head, new WinFormsTextRendererAdapter());

                //Assert.AreEqual("//\n// Font data for Verdana 10pt\n//\n\n//Character bitmaps for Verdana 10pt//\nconst uint_8 verdana_10ptBitmaps = \n{\n\t//@14 '1' (14 pixels wide)//\n\t0x00, 0x00, \n\t0x00, 0x00, \n\t0x00, 0x00, \n\t0x00, 0x00, \n\t0x01, 0x00, \n\t0x07, 0x00, \n\t0x01, 0x00, \n\t0x01, 0x00, \n\t0x01, 0x00, \n\t0x01, 0x00, \n\t0x01, 0x00, \n\t0x01, 0x00, \n\t0x07, 0xC0, \n\t0x00, 0x00, \n\t0x00, 0x00, \n\t0x00, 0x00, \n\t0x00, 0x00, \n\t0x00, 0x00, \n\n};\n\n//Character descriptors for Verdana 10pt//\n//{ [Char width in bits], [Offset into verdana_10ptCharBitmaps in bytes] }//\nconst FONT_CHAR_INFO verdana_10ptDescriptors[] = \n{\n};\n\n//Font information for Verdana 10pt//\nconst FONT_INFO verdana_10ptFontInfo =\n{\n\t3, // Character height//\n\t'1', // Start character//\n\t'1', // End character//\n\t2, // Width, in pixels, of space character//\n\t##Invalid format##, // Character descriptor array//\n\tverdana_10ptBitmaps, // Character bitmap array//\n};\n", src);
                //Assert.AreEqual("//Font data for Verdana 10pt//'nextern const uint_8 verdana_10ptBitmaps[];\nextern const FONT_INFO verdana_10ptFontInfo;\nextern const FONT_CHAR_INFO verdana_10ptDescriptors[];\n", head);
            }
        }
    }
}
