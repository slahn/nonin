using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nonin.Tests
{
    [TestFixture]
    public class InvalidControlDigitsTests
    {

        public Random rand = new Random();

        [Test]
        public void ValidNumbersWithOneChangedDigitShouldNotValidate()
        {
            foreach (var testCase in TestData.AllTestCases())
            {                
                for (var idxToMutate = 0; idxToMutate < testCase.Number.Length; ++idxToMutate)
                {
                    var num = testCase.Number.ToCharArray();
                    var oldDigit = (int)(num[idxToMutate] - '0');

                    int newDigit;
                    do
                    {
                        newDigit = rand.Next(0, 9);
                    } while (newDigit == oldDigit);

                    num[idxToMutate] = newDigit.ToString()[0];
                    var changedStr = new string(num);

                    Assert.IsFalse(Nin.IsValid(changedStr), "A Nin with one digit changed should never validate (original {0}, changed {1})", testCase.Number, changedStr);
                }
            }
        }


        [Test]
        public void InvalidControlDigitsShouldNotValidate()
        {
            foreach (var n in TestData.InvalidControlDigits)
            {
                Assert.AreEqual(NinValidation.InvalidControlDigit, Nin.Validate(n));
            }
        }


        [Test]
        public void InvalidControlDigitsShouldThrowOnCreate()
        {
            foreach (var n in TestData.InvalidControlDigits)
            {
                var ex = Assert.Throws<InvalidNinException>(delegate { new Nin(n); }, "Should throw an InvalidNinException when the control digit is not valid");               
                Assert.AreEqual(NinValidation.InvalidControlDigit, ex.Reason, "Unexpected exception reason.");                
            }
        }
    }
}
