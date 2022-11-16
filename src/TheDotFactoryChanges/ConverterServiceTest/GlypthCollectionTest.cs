using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Allure.Core;
using NUnit.Framework;

using Service;

namespace ConverterServiceTest
{
    public class GlypthBuilder
    {
        public Font Font { get; set; }
        public char Ch { get; set; }
        public Color BgClr { get; set; }
        public Color FgClr { get; set; }
        public Rectangle InnerRect { get; set; }
        public Rectangle OuterRect { get; set; }

        public GlypthBuilder(char ch, Font f)
        {
            this.Font = f;
            this.Ch = ch;
        }

        public Glypth Build()
        {
            var glypth = new Glypth(Ch, Font);
            glypth.SetBackgroundColor(BgClr);
            glypth.SetForegroundColor(FgClr);
            glypth.SetInnerRect(InnerRect);
            glypth.SetOuterRect(OuterRect);
            return glypth;
        }
    }

    [AllureNUnit]
    public class GlypthCollectionTest
    {
        [Test]
        public void ToBitmapTest()
        {
            var innerRect = new Rectangle(1, 1, 8, 8);
            var outerRect = new Rectangle(0, 0, 10, 10);

            var font = new Font("Arial", 10);
            var glypthBuilder = new GlypthBuilder('1', font);
            glypthBuilder.BgClr = Color.FromArgb(255, 0, 0, 0);
            glypthBuilder.FgClr = Color.FromArgb(255, 255, 255, 255);
            glypthBuilder.InnerRect = innerRect;
            glypthBuilder.OuterRect = outerRect;
            var glypth1 = glypthBuilder.Build();
            glypthBuilder.Ch = '2';
            var glypth2 = glypthBuilder.Build();
            var sut = new GlypthCollection() { glypth1, glypth2 };

            //var bmp = sut.ToBitmap();

            //Assert.IsNotNull(bmp);
            //Assert.That(bmp.Width, Is.EqualTo(20));
            //Assert.That(bmp.Height, Is.EqualTo(10));
            //Assert.That(bmp.GetPixel(0, 4), Is.EqualTo(Color.FromArgb(255, 0, 0, 0)));
            //Assert.That(bmp.GetPixel(2, 4), Is.EqualTo(Color.FromArgb(255, 0, 0, 0)));
            //Assert.That(bmp.GetPixel(4, 4), Is.EqualTo(Color.FromArgb(255, 0, 0, 0)));
            //Assert.That(bmp.GetPixel(6, 4), Is.EqualTo(Color.FromArgb(255, 255, 255, 255)));
            //Assert.That(bmp.GetPixel(8, 4), Is.EqualTo(Color.FromArgb(255, 0, 0, 0)));
            //Assert.That(bmp.GetPixel(10, 4), Is.EqualTo(Color.FromArgb(255, 0, 0, 0)));
            //Assert.That(bmp.GetPixel(12, 4), Is.EqualTo(Color.FromArgb(255, 0, 0, 0)));
            //Assert.That(bmp.GetPixel(14, 4), Is.EqualTo(Color.FromArgb(255, 0, 0, 0)));
            //Assert.That(bmp.GetPixel(16, 4), Is.EqualTo(Color.FromArgb(255, 0, 0, 0)));
            //Assert.That(bmp.GetPixel(18, 4), Is.EqualTo(Color.FromArgb(255, 255, 255, 255)));
        }

        [Test]
        public void SetBackgroundColorTest()
        {
            var innerRect = new Rectangle(1, 1, 8, 8);
            var outerRect = new Rectangle(0, 0, 10, 10);

            var font = new Font("Arial", 10);
            var glypthBuilder = new GlypthBuilder('1', font);
            glypthBuilder.BgClr = Color.FromArgb(255, 0, 0, 0);
            glypthBuilder.FgClr = Color.FromArgb(255, 255, 255, 255);
            glypthBuilder.InnerRect = innerRect;
            glypthBuilder.OuterRect = outerRect;
            var glypth1 = glypthBuilder.Build();
            glypthBuilder.Ch = '2';
            var glypth2 = glypthBuilder.Build();
            var sut = new GlypthCollection() { glypth1, glypth2 };

            sut.SetBackgroundColor(Color.Green);

            //var bmp = sut.ToBitmap();
            //Assert.IsNotNull(bmp);
            //Assert.That(bmp.Width, Is.EqualTo(20));
            //Assert.That(bmp.Height, Is.EqualTo(10));
            //Assert.That(bmp.GetPixel(0, 4), Is.EqualTo(Color.FromArgb(255, 0, 128, 0)));
            //Assert.That(bmp.GetPixel(2, 4), Is.EqualTo(Color.FromArgb(255, 0, 128, 0)));
            //Assert.That(bmp.GetPixel(4, 4), Is.EqualTo(Color.FromArgb(255, 0, 128, 0)));
            //Assert.That(bmp.GetPixel(6, 4), Is.EqualTo(Color.FromArgb(255, 255, 255, 255)));
            //Assert.That(bmp.GetPixel(8, 4), Is.EqualTo(Color.FromArgb(255, 0, 128, 0)));
            //Assert.That(bmp.GetPixel(10, 4), Is.EqualTo(Color.FromArgb(255, 0, 128, 0)));
            //Assert.That(bmp.GetPixel(12, 4), Is.EqualTo(Color.FromArgb(255, 0, 128, 0)));
            //Assert.That(bmp.GetPixel(14, 4), Is.EqualTo(Color.FromArgb(255, 0, 128, 0)));
            //Assert.That(bmp.GetPixel(16, 4), Is.EqualTo(Color.FromArgb(255, 0, 128, 0)));
            //Assert.That(bmp.GetPixel(18, 4), Is.EqualTo(Color.FromArgb(255, 255, 255, 255)));
        }

        [Test]
        public void SetForegroundColorTest()
        {
            var innerRect = new Rectangle(1, 1, 8, 8);
            var outerRect = new Rectangle(0, 0, 10, 10);

            var font = new Font("Arial", 10);
            var glypthBuilder = new GlypthBuilder('1', font);
            glypthBuilder.BgClr = Color.FromArgb(255, 0, 0, 0);
            glypthBuilder.FgClr = Color.FromArgb(255, 255, 255, 255);
            glypthBuilder.InnerRect = innerRect;
            glypthBuilder.OuterRect = outerRect;
            var glypth1 = glypthBuilder.Build();
            glypthBuilder.Ch = '2';
            var glypth2 = glypthBuilder.Build();
            var sut = new GlypthCollection() { glypth1, glypth2 };

            sut.SetForegroundColor(Color.Green);

            //var bmp = sut.ToBitmap();
            //Assert.IsNotNull(bmp);
            //Assert.That(bmp.Width, Is.EqualTo(20));
            //Assert.That(bmp.Height, Is.EqualTo(10));
            //Assert.That(bmp.GetPixel(0, 4), Is.EqualTo(Color.FromArgb(255, 0, 0, 0)));
            //Assert.That(bmp.GetPixel(2, 4), Is.EqualTo(Color.FromArgb(255, 0, 0, 0)));
            //Assert.That(bmp.GetPixel(4, 4), Is.EqualTo(Color.FromArgb(255, 0, 0, 0)));
            //Assert.That(bmp.GetPixel(6, 4), Is.EqualTo(Color.FromArgb(255, 0, 128, 0)));
            //Assert.That(bmp.GetPixel(8, 4), Is.EqualTo(Color.FromArgb(255, 0, 0, 0)));
            //Assert.That(bmp.GetPixel(10, 4), Is.EqualTo(Color.FromArgb(255, 0, 0, 0)));
            //Assert.That(bmp.GetPixel(12, 4), Is.EqualTo(Color.FromArgb(255, 0, 0, 0)));
            //Assert.That(bmp.GetPixel(14, 4), Is.EqualTo(Color.FromArgb(255, 0, 0, 0)));
            //Assert.That(bmp.GetPixel(16, 4), Is.EqualTo(Color.FromArgb(255, 0, 0, 0)));
            //Assert.That(bmp.GetPixel(18, 4), Is.EqualTo(Color.FromArgb(255, 0, 128, 0)));
        }

        [Test]
        public void SetFontTest()
        {
            var innerRect = new Rectangle(1, 1, 8, 8);
            var outerRect = new Rectangle(0, 0, 10, 10);

            var font = new Font("Arial", 10);
            var glypthBuilder = new GlypthBuilder('1', font);
            glypthBuilder.BgClr = Color.FromArgb(255, 0, 0, 0);
            glypthBuilder.FgClr = Color.FromArgb(255, 255, 255, 255);
            glypthBuilder.InnerRect = innerRect;
            glypthBuilder.OuterRect = outerRect;
            var glypth1 = glypthBuilder.Build();
            glypthBuilder.Ch = '2';
            var glypth2 = glypthBuilder.Build();
            var sut = new GlypthCollection() { glypth1, glypth2 };
            font = new Font("Arial", 10, FontStyle.Bold);

            sut.SetFont(font);

            //var bmp = sut.ToBitmap();
            //Assert.IsNotNull(bmp);
            //Assert.That(bmp.Width, Is.EqualTo(20));
            //Assert.That(bmp.Height, Is.EqualTo(10));
            //Assert.That(bmp.GetPixel(0, 4), Is.EqualTo(Color.FromArgb(255, 0, 0, 0)));
            //Assert.That(bmp.GetPixel(2, 4), Is.EqualTo(Color.FromArgb(255, 0, 0, 0)));
            //Assert.That(bmp.GetPixel(4, 4), Is.EqualTo(Color.FromArgb(255, 0, 0, 0)));
            //Assert.That(bmp.GetPixel(6, 4), Is.EqualTo(Color.FromArgb(255, 255, 255, 255)));
            //Assert.That(bmp.GetPixel(7, 4), Is.EqualTo(Color.FromArgb(255, 255, 255, 255)));
            //Assert.That(bmp.GetPixel(8, 4), Is.EqualTo(Color.FromArgb(255, 0, 0, 0)));
            //Assert.That(bmp.GetPixel(10, 4), Is.EqualTo(Color.FromArgb(255, 0, 0, 0)));
            //Assert.That(bmp.GetPixel(12, 4), Is.EqualTo(Color.FromArgb(255, 0, 0, 0)));
            //Assert.That(bmp.GetPixel(14, 4), Is.EqualTo(Color.FromArgb(255, 255, 255, 255)));
            //Assert.That(bmp.GetPixel(16, 4), Is.EqualTo(Color.FromArgb(255, 0, 0, 0)));
            //Assert.That(bmp.GetPixel(18, 4), Is.EqualTo(Color.FromArgb(255, 255, 255, 255)));
        }

        [Test]
        public void RemoveTest()
        {
            var font = new Font("Arial", 10);
            var glypth1 = new Glypth('1', font);
            var glypth2 = new Glypth('2', font);
            var sut = new GlypthCollection() { glypth1, glypth2 };

            sut.Remove(glypth1);

            Assert.That(sut.Count(), Is.EqualTo(1));
        }

        [Test]
        public void IsCollectionTest()
        {
            var font = new Font("Arial", 10);
            var glypth1 = new Glypth('1', font);
            var glypth2 = new Glypth('2', font);
            var sut = new GlypthCollection() { glypth1, glypth2 };

            var res = sut.IsCollection();

            Assert.True(res);
        }
    }
}
