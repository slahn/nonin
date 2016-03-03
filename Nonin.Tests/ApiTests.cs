using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nonin.Tests
{
    [TestFixture]
    public class ApiTests
    {
        [Test]
        public void ValidNumberButUnsupportedKindShouldNotValidate()
        {
            var noKinds = new NinKind[0];
            var allKinds = Enum.GetValues(typeof(NinKind)).Cast<NinKind>().ToArray();

            foreach (var test in TestData.AllTestCases())
            {
                var allExceptThis = allKinds.Except(new[] { test.NumberKind }).ToArray();
                
                Assert.IsTrue(Nin.IsValid(test.Number), "Valid number IsValid");
                Assert.AreEqual(NinValidation.Valid, Nin.Validate(test.Number), "Valid number gives Valid result");

                Assert.AreEqual(NinValidation.Valid, Nin.Validate(test.Number, allowedKinds: allKinds), "Valid number should validate when all kinds are specified");
                Assert.AreEqual(NinValidation.Valid, Nin.Validate(test.Number, test.NumberKind), "Valid number should validate when its kind is specified");

                foreach(var kind in allExceptThis)
                {
                    Assert.AreEqual(NinValidation.Valid, Nin.Validate(test.Number, test.NumberKind, kind), "Valid number should validate when its kind is specified");
                    Assert.AreEqual(NinValidation.UnsupportedKind, Nin.Validate(test.Number, kind), "Valid number should give unsupportedkind when its kind is not specified");
                }

                Assert.AreEqual(NinValidation.UnsupportedKind, Nin.Validate(test.Number, allowedKinds: noKinds), "Valid number should give unsupportedkind when no kinds are specified");
                Assert.AreEqual(NinValidation.UnsupportedKind, Nin.Validate(test.Number, allowedKinds: allExceptThis), "Valid number should give unsupportedkind when kind is missing");
            }
        }
    }
}
