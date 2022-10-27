using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using Service;
using NUnit.Allure.Core;

namespace ConverterServiceTest
{
    [AllureNUnit]
    public class UtilsTest
    {
        [Test]
        public void GetBitmapBorderTest()
        {
            var bmp = new Bitmap(3, 3);
            using (Graphics gfx = Graphics.FromImage(bmp))
            {
                gfx.FillRectangle(new SolidBrush(Color.Black), 0, 0, 3, 3);
            }
            bmp.SetPixel(1, 1, Color.White);

            var rect = Utils.GetBitmapBorder(bmp, Color.White, Color.Black);

            Assert.IsNotNull(rect);
            Assert.AreEqual(1, rect.X);
            Assert.AreEqual(1, rect.Y);
            Assert.AreEqual(1, rect.Width);
            Assert.AreEqual(1, rect.Height);
        }

        [Test]
        public void GetVariableNameFromExpressionTest()
        {
            var expr = "const far unsigned int my_font[] = ";

            var varname = Utils.GetVariableNameFromExpression(expr);

            Assert.AreEqual("my_font", varname);
        }

        [Test]
        public void ScrubVariableNameTest()
        {
            var varname = "1[]_hello_world(){}0%";

            var scrubbedName = Utils.ScrubVariableName(varname);

            Assert.AreEqual("_1_hello_world0", scrubbedName);
        }

        [Test]
        public void GetFontNameTest()
        {
            var font = new Font("Arial", 10);

            var fontName = Utils.GetFontName(font);

            Assert.AreEqual("arial_10pt", fontName);
        }

        [Test]
        public void CharacterRangePointParseTest()
        {
            string вход = "0x100";

            int выход = 0;
            var b = Utils.CharacterRangePointParse(вход, ref выход);

            int нужный_выход = 256;
            Assert.AreEqual(нужный_выход, выход);
        }

        [Test]
        public void ExpandAndRemoveCharacterRangesTest()
        {
            var input = "abcd";

            var res = Utils.ExpandAndRemoveCharacterRanges(input);

            Assert.AreEqual("abcd", res);
        }
    }
}
