// This software is released under the BSD License.
// See LICENSE file for details.

using NUnit.Framework;
using System.IO;

namespace LEB128
{
    [TestFixture]
    public class LEB128Tests
    {
        [TestCase]
        public void TestLEB128Unsigned () {

            TestUnsigned(0UL, 0x00);
            TestUnsigned(1UL, 0x01);

            TestUnsigned(127UL, 0x7f);
            TestUnsigned(128UL, 0x80, 0x01);
            TestUnsigned(129UL, 0x81, 0x01);
            TestUnsigned(255UL, 0xff, 0x01);
            TestUnsigned(256UL, 0x80, 0x02);
            TestUnsigned(257UL, 0x81, 0x02);
            TestUnsigned(383UL, 0xff, 0x02);
            TestUnsigned(384UL, 0x80, 0x03);
            TestUnsigned(511UL, 0xff, 0x03);
            TestUnsigned(512UL, 0x80, 0x04);

            TestUnsigned(0x007fUL, 0x7f);
            TestUnsigned(0x0080UL, 0x80, 0x01);
            TestUnsigned(0x0081UL, 0x81, 0x01);
            TestUnsigned(0x00ffUL, 0xff, 0x01);
            TestUnsigned(0x0100UL, 0x80, 0x02);
            TestUnsigned(0x0101UL, 0x81, 0x02);
            TestUnsigned(0x017fUL, 0xff, 0x02);
            TestUnsigned(0x0180UL, 0x80, 0x03);
            TestUnsigned(0x01ffUL, 0xff, 0x03);
            TestUnsigned(0x0200UL, 0x80, 0x04);

            TestUnsigned(624485, 0xe5, 0x8e, 0x26); // test value from wikipedia :)

            TestUnsigned(0x7fffffffUL, 0xff, 0xff, 0xff, 0xff, 0x07);

            TestUnsigned(ulong.MinValue, 0x00);
            TestUnsigned(ulong.MaxValue, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0x01);
        }


        [TestCase]
        public void TestLEB128Signed () {

            TestSigned(0L, 0x00);
            TestSigned(1L, 0x01);
            TestSigned(2L, 0x02);

            TestSigned(62L, 0x3e);
            TestSigned(63L, 0x3f);
            TestSigned(64L, 0xc0, 0x00);
            TestSigned(65L, 0xc1, 0x00);
            TestSigned(66L, 0xc2, 0x00);

            TestSigned(127L, 0xff, 0x00);
            TestSigned(128L, 0x80, 0x01);
            TestSigned(129L, 0x81, 0x01);

            TestSigned(-1L, 0x7f);
            TestSigned(-2L, 0x7e);

            TestSigned(-62L, 0x42);
            TestSigned(-63L, 0x41);
            TestSigned(-64L, 0x40);
            TestSigned(-65L, 0xbf, 0x7f);
            TestSigned(-66L, 0xbe, 0x7f);

            TestSigned(-127L, 0x81, 0x7f);
            TestSigned(-128L, 0x80, 0x7f);
            TestSigned(-129L, 0xff, 0x7e);

            TestSigned(-123456L, 0xc0, 0xbb, 0x78); // test value from wikipedia :)

            TestSigned(long.MinValue, 0x80, 0x80, 0x80, 0x80, 0x80, 0x80, 0x80, 0x80, 0x80, 0x7f);
            TestSigned(long.MaxValue, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0x00);
        }

        private void TestSigned (long value, params byte[] bytes) {
            using var ms = new MemoryStream();
            ms.WriteLEB128Signed(value);
            ms.Position = 0;
            Assert.AreEqual(value, ms.ReadLEB128Signed());
            TestStreamBytes(ms, bytes);
        }

        private void TestUnsigned (ulong value, params byte[] bytes) {
            using var ms = new MemoryStream();
            ms.WriteLEB128Unsigned(value);
            ms.Position = 0;
            Assert.AreEqual(value, ms.ReadLEB128Unsigned());
            TestStreamBytes(ms, bytes);
        }

        private void TestStreamBytes (MemoryStream ms, params byte[] values) {
            ms.Position = 0;
            var buf = ms.ToArray();
            CollectionAssert.AreEqual(buf, values);
            Assert.AreEqual(values.Length, buf.Length);
        }
    }
}
