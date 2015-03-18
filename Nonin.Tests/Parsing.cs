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
        public void EdgeCases()
        {
            // Issue #1
            var tc1854 = new TestData.TestCase()
            {
                Number = "03125463265",
                NumberKind = NinKind.BirthNumber,
                DateOfBirth = new DateTime(1854, 12, 3),
                Gender = Gender.Female
            };

            RunTests(tc1854);
        }

        [Test]
        public void AllGeneratedParsingTests()
        {
            foreach (var testcase in TestData.AllTestCases())
            {
                RunTests(testcase);
            }
        }


        private void RunTests(TestData.TestCase testcase)
        {
            var nin = new Nin(testcase.Number);

            TestDateOfBirth(testcase, nin);
            TestNumberType(testcase, nin);
            TestGender(testcase, nin);

            TestLongConversion(testcase, nin);
            TestStringConversion(testcase, nin);
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
