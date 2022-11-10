using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;
using NUnit.Allure.Core;

using Service;

namespace ConverterServiceTest
{
    [AllureNUnit]
    public  class GlypthTest
    {
        [Test]
        public void SetOuterRectCollisionTest()
        {
            var font = new Font("Arial", 10);
            var sut = new Glypth('1', font);
            var innerRect = new Rectangle(0, 0, 9, 9);
            sut.SetInnerRect(innerRect);
            var outerRect = new Rectangle(1, 1, 10, 10);

            TestDelegate del = () => sut.SetOuterRect(outerRect);

            Assert.Throws<ApplicationException>(() => del());
        }

        [Test]
        public void SetOuterRectDefaultTest()
        {
            var font = new Font("Arial", 10);
            var sut = new Glypth('1', font);
            var innerRect = new Rectangle(0, 0, 1, 1);
            sut.SetInnerRect(innerRect);
            var outerRect = new Rectangle(0, 0, 10, 10);
            sut.SetBackgroundColor(Color.Black);
            sut.SetForegroundColor(Color.White);

            sut.SetOuterRect(outerRect);

            var bmp = sut.ToBitmap();
            Assert.That(bmp.Width, Is.EqualTo(10));
            Assert.That(bmp.Height, Is.EqualTo(10));
            Assert.That(bmp.GetPixel(0, 0), Is.EqualTo(Color.FromArgb(255, 0, 0, 0)));
            Assert.That(bmp.GetPixel(1, 1), Is.EqualTo(Color.FromArgb(255, 0, 0, 0)));
            Assert.That(bmp.GetPixel(2, 2), Is.EqualTo(Color.FromArgb(255, 0, 0, 0)));
            Assert.That(bmp.GetPixel(3, 3), Is.EqualTo(Color.FromArgb(255, 0, 0, 0)));
            Assert.That(bmp.GetPixel(4, 4), Is.EqualTo(Color.FromArgb(255, 0, 0, 0)));
            Assert.That(bmp.GetPixel(5, 5), Is.EqualTo(Color.FromArgb(255, 0, 0, 0)));
            Assert.That(bmp.GetPixel(6, 6), Is.EqualTo(Color.FromArgb(255, 0, 0, 0)));
            Assert.That(bmp.GetPixel(7, 7), Is.EqualTo(Color.FromArgb(255, 0, 0, 0)));
            Assert.That(bmp.GetPixel(8, 8), Is.EqualTo(Color.FromArgb(255, 0, 0, 0)));
            Assert.That(bmp.GetPixel(9, 9), Is.EqualTo(Color.FromArgb(255, 0, 0, 0)));
        }

        [Test]
        public void SetOuterRectSmallWindowTest()
        {
            var font = new Font("Arial", 10);
            var sut = new Glypth('1', font);
            var innerRect = new Rectangle(3, 3, 5, 5);
            sut.SetInnerRect(innerRect);
            var outerRect = new Rectangle(0, 0, 10, 10);
            sut.SetBackgroundColor(Color.Black);
            sut.SetForegroundColor(Color.White);

            sut.SetOuterRect(outerRect);

            var bmp = sut.ToBitmap();
            Assert.That(bmp.Width, Is.EqualTo(10));
            Assert.That(bmp.Height, Is.EqualTo(10));
            Assert.That(bmp.GetPixel(0, 0), Is.EqualTo(Color.FromArgb(255, 0, 0, 0)));
            Assert.That(bmp.GetPixel(1, 1), Is.EqualTo(Color.FromArgb(255, 0, 0, 0)));
            Assert.That(bmp.GetPixel(2, 2), Is.EqualTo(Color.FromArgb(255, 0, 0, 0)));
            Assert.That(bmp.GetPixel(3, 3), Is.EqualTo(Color.FromArgb(255, 0, 0, 0)));
            Assert.That(bmp.GetPixel(4, 4), Is.EqualTo(Color.FromArgb(255, 0, 0, 0)));
            Assert.That(bmp.GetPixel(5, 5), Is.EqualTo(Color.FromArgb(255, 0, 0, 0)));
            Assert.That(bmp.GetPixel(6, 6), Is.EqualTo(Color.FromArgb(255, 255, 255, 255)));
            Assert.That(bmp.GetPixel(7, 7), Is.EqualTo(Color.FromArgb(255, 0, 0, 0)));
            Assert.That(bmp.GetPixel(8, 8), Is.EqualTo(Color.FromArgb(255, 0, 0, 0)));
            Assert.That(bmp.GetPixel(9, 9), Is.EqualTo(Color.FromArgb(255, 0, 0, 0)));
        }

        [Test]
        public void CropTest()
        {
            var font = new Font("Arial", 10);
            var sut = new Glypth('1', font);
            var innerRect = new Rectangle(3, 3, 5, 5);
            sut.SetInnerRect(innerRect);
            var outerRect = new Rectangle(0, 0, 10, 10);
            sut.SetBackgroundColor(Color.Black);
            sut.SetForegroundColor(Color.White);
            var newRect = new Rectangle(2, 2, 8, 8);

            sut.Crop(newRect);

            var bmp = sut.ToBitmap();
            Assert.That(bmp.Width, Is.EqualTo(8));
            Assert.That(bmp.Height, Is.EqualTo(8));
            Assert.That(bmp.GetPixel(0, 0), Is.EqualTo(Color.FromArgb(255, 0, 0, 0)));
            Assert.That(bmp.GetPixel(1, 1), Is.EqualTo(Color.FromArgb(255, 0, 0, 0)));
            Assert.That(bmp.GetPixel(2, 2), Is.EqualTo(Color.FromArgb(255, 0, 0, 0)));
            Assert.That(bmp.GetPixel(3, 3), Is.EqualTo(Color.FromArgb(255, 0, 0, 0)));
            Assert.That(bmp.GetPixel(4, 4), Is.EqualTo(Color.FromArgb(255, 255, 255, 255)));
            Assert.That(bmp.GetPixel(5, 5), Is.EqualTo(Color.FromArgb(255, 0, 0, 0)));
            Assert.That(bmp.GetPixel(6, 6), Is.EqualTo(Color.FromArgb(255, 0, 0, 0)));
            Assert.That(bmp.GetPixel(7, 7), Is.EqualTo(Color.FromArgb(255, 0, 0, 0)));
        }

        [Test]
        public void SetBackgroundColorTest()
        {
            var font = new Font("Arial", 10);
            var sut = new Glypth('1', font);
            var innerRect = new Rectangle(3, 3, 5, 5);
            sut.SetInnerRect(innerRect);
            var outerRect = new Rectangle(0, 0, 10, 10);
            sut.SetBackgroundColor(Color.Black);
            sut.SetForegroundColor(Color.White);
            sut.SetOuterRect(outerRect);

            sut.SetBackgroundColor(Color.Green);

            var bmp = sut.ToBitmap();
            Assert.That(bmp.Width, Is.EqualTo(10));
            Assert.That(bmp.Height, Is.EqualTo(10));
            Assert.That(bmp.GetPixel(0, 0), Is.EqualTo(Color.FromArgb(255, 0, 128, 0)));
            Assert.That(bmp.GetPixel(1, 1), Is.EqualTo(Color.FromArgb(255, 0, 128, 0)));
            Assert.That(bmp.GetPixel(2, 2), Is.EqualTo(Color.FromArgb(255, 0, 128, 0)));
            Assert.That(bmp.GetPixel(3, 3), Is.EqualTo(Color.FromArgb(255, 0, 128, 0)));
            Assert.That(bmp.GetPixel(4, 4), Is.EqualTo(Color.FromArgb(255, 0, 128, 0)));
            Assert.That(bmp.GetPixel(5, 5), Is.EqualTo(Color.FromArgb(255, 0, 128, 0)));
            Assert.That(bmp.GetPixel(6, 6), Is.EqualTo(Color.FromArgb(255, 255, 255, 255)));
            Assert.That(bmp.GetPixel(7, 7), Is.EqualTo(Color.FromArgb(255, 0, 128, 0)));
            Assert.That(bmp.GetPixel(8, 8), Is.EqualTo(Color.FromArgb(255, 0, 128, 0)));
            Assert.That(bmp.GetPixel(9, 9), Is.EqualTo(Color.FromArgb(255, 0, 128, 0)));
        }

        [Test]
        public void SetForegroundColorTest()
        {
            var font = new Font("Arial", 10);
            var sut = new Glypth('1', font);
            var innerRect = new Rectangle(3, 3, 5, 5);
            sut.SetInnerRect(innerRect);
            var outerRect = new Rectangle(0, 0, 10, 10);
            sut.SetBackgroundColor(Color.Black);
            sut.SetForegroundColor(Color.White);
            sut.SetOuterRect(outerRect);

            sut.SetForegroundColor(Color.Green);

            var bmp = sut.ToBitmap();
            Assert.That(bmp.Width, Is.EqualTo(10));
            Assert.That(bmp.Height, Is.EqualTo(10));
            Assert.That(bmp.GetPixel(0, 0), Is.EqualTo(Color.FromArgb(255, 0, 0, 0)));
            Assert.That(bmp.GetPixel(1, 1), Is.EqualTo(Color.FromArgb(255, 0, 0, 0)));
            Assert.That(bmp.GetPixel(2, 2), Is.EqualTo(Color.FromArgb(255, 0, 0, 0)));
            Assert.That(bmp.GetPixel(3, 3), Is.EqualTo(Color.FromArgb(255, 0, 0, 0)));
            Assert.That(bmp.GetPixel(4, 4), Is.EqualTo(Color.FromArgb(255, 0, 0, 0)));
            Assert.That(bmp.GetPixel(5, 5), Is.EqualTo(Color.FromArgb(255, 0, 0, 0)));
            Assert.That(bmp.GetPixel(6, 6), Is.EqualTo(Color.FromArgb(255, 0, 128, 0)));
            Assert.That(bmp.GetPixel(7, 7), Is.EqualTo(Color.FromArgb(255, 0, 0, 0)));
            Assert.That(bmp.GetPixel(8, 8), Is.EqualTo(Color.FromArgb(255, 0, 0, 0)));
            Assert.That(bmp.GetPixel(9, 9), Is.EqualTo(Color.FromArgb(255, 0, 0, 0)));
        }

        [Test]
        public void SetFontTest()
        {
            var font = new Font("Arial", 10, FontStyle.Regular);
            var sut = new Glypth('1', font);
            var innerRect = new Rectangle(3, 3, 5, 5);
            sut.SetInnerRect(innerRect);
            var outerRect = new Rectangle(0, 0, 10, 10);
            sut.SetOuterRect(outerRect);
            sut.SetBackgroundColor(Color.Black);
            sut.SetForegroundColor(Color.White);
            font = new Font("Arial", 10, FontStyle.Bold);

            sut.SetFont(font);

            var bmp = sut.ToBitmap();
            Assert.That(bmp.Width, Is.EqualTo(10));
            Assert.That(bmp.Height, Is.EqualTo(10));
            Assert.That(bmp.GetPixel(0, 0), Is.EqualTo(Color.FromArgb(255, 0, 0, 0)));
            Assert.That(bmp.GetPixel(1, 1), Is.EqualTo(Color.FromArgb(255, 0, 0, 0)));
            Assert.That(bmp.GetPixel(2, 2), Is.EqualTo(Color.FromArgb(255, 0, 0, 0)));
            Assert.That(bmp.GetPixel(3, 3), Is.EqualTo(Color.FromArgb(255, 0, 0, 0)));
            Assert.That(bmp.GetPixel(4, 4), Is.EqualTo(Color.FromArgb(255, 0, 0, 0)));
            Assert.That(bmp.GetPixel(5, 5), Is.EqualTo(Color.FromArgb(255, 255, 255, 255)));
            Assert.That(bmp.GetPixel(6, 6), Is.EqualTo(Color.FromArgb(255, 255, 255, 255)));
            Assert.That(bmp.GetPixel(7, 7), Is.EqualTo(Color.FromArgb(255, 255, 255, 255)));
            Assert.That(bmp.GetPixel(8, 8), Is.EqualTo(Color.FromArgb(255, 0, 0, 0)));
            Assert.That(bmp.GetPixel(9, 9), Is.EqualTo(Color.FromArgb(255, 0, 0, 0)));
        }

        [Test]
        public void AddTest()
        {
            var font = new Font("Arial", 10, FontStyle.Regular);
            var sut = new Glypth('1', font);
            var toAdd = new Glypth('2', font);

            TestDelegate del = () => sut.Add(toAdd);

            Assert.Throws<ApplicationException>(del);
        }

        [Test]
        public void RemoveTest()
        {
            var font = new Font("Arial", 10, FontStyle.Regular);
            var sut = new Glypth('1', font);
            var toRemove = new Glypth('2', font);

            TestDelegate del = () => sut.Remove(toRemove);

            Assert.Throws<ApplicationException>(del);
        }

        [Test]
        public void IsCollectionTest()
        {
            var font = new Font("Arial", 10, FontStyle.Regular);
            var sut = new Glypth('1', font);

            var res = sut.IsCollection();

            Assert.False(res);
        }

        [Test]
        public void Count()
        {
            var font = new Font("Arial", 10, FontStyle.Regular);
            var sut = new Glypth('1', font);

            TestDelegate del = () => sut.Count();

            Assert.Throws<ApplicationException>(del);
        }
    }
}
