using System;
using System.Collections.Generic;
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
    public class PageArrayTest
    {
        [Test]
        public void ToStringTest()
        {
            var byteArr = new byte[1, 1];
            byteArr[0, 0] = 0x7e;
            var sut = new PageArray(byteArr, BitLayout.RowMajor);
            var cfg = new Configuration();

            var res = sut.ToString(cfg);

            var target = "\t//  ###### 0x7E, \n";
            Assert.AreEqual(target, res);
        }

        [Test]
        public void TransposeTest()
        {
            var byteArr = new byte[2, 2];
            byteArr[0, 0] = 0xff;
            byteArr[0, 1] = 0xff;
            byteArr[1, 0] = 0x00;
            byteArr[1, 1] = 0x0;
            var sut = new PageArray(byteArr, BitLayout.RowMajor);
            var cfg = new Configuration();

            sut.Transpose(cfg);

            var res = sut.ToStringLines(cfg);
            var target = new string[2];
            target[0] = "0xFF, 0xFF, ";
            target[1] = "0x00, 0x00, ";
            Assert.AreEqual(target, res);
        }

        [Test]
        public void ToStringLinesTest()
        {
            var byteArr = new byte[1, 1];
            byteArr[0, 0] = 0x7e;
            var sut = new PageArray(byteArr, BitLayout.RowMajor);
            var cfg = new Configuration();
            cfg.bmpVisualizerChar = "+";
            cfg.commentStyle = CommentStyle.Cpp;

            var res = sut.ToStringLines(cfg);

            var target = new string[1];
            target[0] = "0x7E, ";
            Assert.AreEqual(target, res);
        }

        [Test]
        public void ToOutputHeaderLinesTest()
        {
            var byteArr = new byte[1, 1];
            byteArr[0, 0] = 0x7e;
            var sut = new PageArray(byteArr, BitLayout.RowMajor);
            var cfg = new Configuration();
            cfg.bmpVisualizerChar = "+";
            cfg.commentStyle = CommentStyle.Cpp;

            var res = sut.ToOutputHeaderLines(cfg);

            var target = new string[1];
            target[0] = "//  ++++++ ";
            Assert.AreEqual(target, res);
        }

        [Test]
        public void HeightTest()
        {
            var sut = new PageArray(10, 10);

            var res = sut.Height();

            Assert.AreEqual(10, res);
        }

        [Test]
        public void WidthTest()
        {
            var sut = new PageArray(10, 10);

            var res = sut.Width();

            Assert.AreEqual(10, res);
        }
    }
}
