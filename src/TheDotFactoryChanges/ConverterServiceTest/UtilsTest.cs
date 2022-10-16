using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using Service;

namespace ConverterServiceTest
{
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

        }

        [Test]
        public void ExpandAndRemoveCharacterRangesTest()
        {

        }
    }
}
