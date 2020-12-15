using System;
using System.Runtime.CompilerServices;
using Xunit;
using Xunit.Sdk;

namespace CygSoft.Waxy.WvScape.Tests
{
    public class Tests
    {
        
        [Theory]
        [SimpleFile("Sample.txt")]
        public void Test(string text)
        {
            Assert.StartsWith("Name:Kommetjie (WaveScape)", text);
        }

        [Fact]
        public void AssertIt()
        {
            Assert.True(true);
        }

    }
}
