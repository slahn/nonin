using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nonin.Tests
{
    [TestFixture]
    public class Parsing
    {

        [Test]
        public void AllParsingTests()
        {
            foreach (var n in TestData.AllTestCases())
            {
                var f = new Nin(n.Number);

                TestDateOfBirth(n, f);
                TestNumberType(n, f);
                TestGender(n, f);

                TestLongConversion(n, f);
                TestStringConversion(n, f);
            }
        }

        private void TestStringConversion(TestData.TestCase n, Nin f)
        {
            Assert.AreEqual(n.Number, f.ToString(), "ToString in " + n.Number);
        }

        private void TestLongConversion(TestData.TestCase n, Nin f)
        {
            var l = Convert.ToInt64(n.Number);

            Assert.AreEqual(l, f.ToNumber(), "ToNumber in " + n.Number);
        }

        private void TestGender(TestData.TestCase n, Nin f)
        {
            Assert.AreEqual(n.Gender, f.Gender, "Gender in " + n.Number);
        }

        private void TestNumberType(TestData.TestCase n, Nin f)
        {
            Assert.AreEqual(n.NumberKind, f.Kind, "Type in " + n.Number);
        }

        private void TestDateOfBirth(TestData.TestCase n, Nin f)
        {
            Assert.AreEqual(n.DateOfBirth, f.DateOfBirth, "DoB in " + n.Number);
        }

    }
}
