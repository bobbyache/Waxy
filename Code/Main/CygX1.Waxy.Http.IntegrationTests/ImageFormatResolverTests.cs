using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CygX1.Waxy.Http.IntegrationTests
{
    [TestFixture]
    [Category("Fast Units")]
    class ImageFormatResolverTests
    {
        [Test]
        public void ImageFormatResolver_ResolveExtension_AsJPEG_ResolvesTo_JPG()
        {
            string extension = ImageFormatResolver.ResolveExtension("image/jpeg");
            Assert.AreEqual(".jpg", extension);
        }

        [Test]
        public void ImageFormatResolver_ResolveExtension_AsPNG_ResolvesTo_PNG()
        {
            string extension = ImageFormatResolver.ResolveExtension("image/png");
            Assert.AreEqual(".png", extension);
        }

        [Test]
        public void ImageFormatResolver_ResolveExtension_AsBMP_ResolvesTo_BMP()
        {
            string extension = ImageFormatResolver.ResolveExtension("image/bmp");
            Assert.AreEqual(".bmp", extension);
        }

        [Test]
        public void ImageFormatResolver_ResolveExtension_AsGIF_ResolvesTo_GIF()
        {
            string extension = ImageFormatResolver.ResolveExtension("image/gif");
            Assert.AreEqual(".gif", extension);
        }

        //https://stackoverflow.com/questions/13047521/get-file-extension-or-hasextension-type-bool-from-uri-object-c-sharp

        //[Test]
        //public void TestExtension()
        //{
        //    Uri myURI1 = new Uri(@"http://www.somesite.com/");
        //    Uri myURI2 = new Uri(@"http://www.somesite.com/filenoext");
        //    Uri myURI3 = new Uri(@"http://www.somesite.com/filewithext.jpg");
        //    Uri myURI4 = new Uri(@"http://www.somesite.com/filewithext.jpg?q=randomquerystring");

        //    Assert.IsFalse(System.IO.Path.HasExtension(myURI1.AbsoluteUri));
        //    Assert.IsFalse(System.IO.Path.HasExtension(myURI2.AbsoluteUri));
        //    Assert.IsTrue(System.IO.Path.HasExtension(myURI3.AbsoluteUri));
        //    Assert.IsTrue(System.IO.Path.HasExtension(myURI4.AbsoluteUri));
        //}

        //[Test]
        //public void TestExtension_2()
        //{
        //    Uri myURI3 = new Uri(@"http://www.somesite.com/filewithext.jpg");
        //    Uri myURI4 = new Uri(@"http://www.somesite.com/filewithext.jpg?q=randomquerystring");

        //    Assert.AreEqual(".jpg", System.IO.Path.GetExtension(myURI3.AbsoluteUri));
        //    Assert.AreEqual(".jpg", System.IO.Path.GetExtension(myURI4.AbsoluteUri));
        //}
    }
}
