using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Nonin.Tests
{   
    [TestFixture]
    public class OrderingTests
    {        
        [Test]
        public void ConvertToNumber()
        {
            foreach(var n in TestData.RandomNumbers) {
                var fnr = new Nin(n);
                var num = long.Parse(n);

                Assert.AreEqual(num, fnr.ToNumber());
            }
        }


        [Test]
        public void ConvertFromNumber()
        {
            var ex = Assert.Throws<InvalidNinException>(delegate { new Nin(0L); });

            Assert.Contains(ex.Reason, new[] { 
                NinValidation.InvalidDateOfBirth, 
                NinValidation.InvalidCenturyCode,
                NinValidation.InvalidControlDigit}, "The reason 0L is an invalid NIN should not be format or length");
        }

        [Test]
        public void SortOrder()
        {
            var n1 = new Nin("28122418857");
            var n2 = new Nin("13128045818");

            var correctOrder = new List<Nin>() {
                n2, n1
            };

            var nums = new List<Nin>() {
                n1, n2
            };

            nums.Sort();
            CollectionAssert.AreEqual(correctOrder, nums);

            Assert.IsTrue(n2 < n1, "Operator <");
            Assert.IsTrue(n2 <= n1, "Operator <");
            Assert.IsFalse(n1 < n2, "Operator <");
  
            Assert.IsTrue(n1 > n2, "Operator >");
            Assert.IsTrue(n1 >= n2, "Operator >");
            Assert.IsFalse(n2 > n1, "Operator >");
        }

        [Test]
        public void Equality()
        {
            var notEqualStr = TestData.RandomNumbers.First();
            var notEqual = new Nin(notEqualStr);
            
            foreach (var n in TestData.RandomNumbers.Except(new[] { notEqualStr }))
            {


                var a = new Nin(n);
                var b = new Nin(n);

                Assert.AreEqual(a.GetHashCode(), b.GetHashCode(), "HashCode");
                Assert.IsTrue(a.Equals(b), "Equals");
                Assert.IsTrue(b.Equals(a), "Equals");
                Assert.AreEqual(a, b);

                Assert.IsFalse(a.Equals(null), "Equals null");
                Assert.IsTrue(a.CompareTo(null) > 0, "Compare to null");

                Assert.IsTrue(a == b, "Operator ==");
                Assert.IsTrue(b == a, "Operator ==");
                Assert.IsFalse(a == null, "Operator == null");
                Assert.IsFalse(null == a, "Operator == null");

                Assert.IsTrue(a > null, "Operator > null");
                Assert.IsFalse(a < null, "Operator > null");

                Assert.IsTrue(null < a, "Operator < null");
                Assert.IsFalse(null > a, "Operator < null");
                

                Assert.AreEqual(0, a.CompareTo(b), "CompareTo<T>");
                Assert.AreEqual(0, b.CompareTo(a), "CompareTo<T>");

                Assert.AreEqual(0, a.CompareTo((object)b), "CompareTo");
                Assert.AreEqual(0, b.CompareTo((object)a), "CompareTo");

                Assert.AreNotEqual(a, notEqual);
                Assert.AreNotEqual(a.GetHashCode(), notEqual.GetHashCode());
                Assert.IsFalse(a.Equals(notEqual));
                Assert.AreNotEqual(0, a.CompareTo(notEqual));
                Assert.AreNotEqual(0, a.CompareTo((object)notEqual));
            }
        }

        public volatile int _hash = 0;

        [Test]
        public void HashCodeSpeed()
        {
            var iters = 9999;
            var nums = TestData.RandomNumbers.Select(x => new Nin(x)).ToArray();
            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();

            timer.Start();

            var set = new HashSet<int>();

            foreach (var n in nums)
            {                
                set.Add(n.GetHashCode());

                for (var i = 0; i < iters; ++i)
                {
                    _hash = n.GetHashCode();
                }
            }

            timer.Stop();

            var collisions = nums.Length - set.Count;

            Assert.Pass("Completed {0} hashs in {1} MS. {2} collisions.", nums.Length * iters, timer.ElapsedMilliseconds, collisions);
        }


        [Test]
        public void HashCollisions()
        {
            

            var nums = TestData.RandomNumbers.Select(x => new Nin(x)).Concat(TestData.AllTestCases().Select(x => new Nin(x.Number)));

            var count = 0;
            var set = new HashSet<int>();
            var stringSet = new HashSet<int>();
            foreach (var n in nums.Distinct()) {
                count++;
                set.Add(n.GetHashCode());

                stringSet.Add(n.ToString().GetHashCode());
            }

            var collisions = count - set.Count;
            var strCollisions = count - stringSet.Count;
            Assert.Pass("{0} items, {1} collisions ({2} collisions as string)", count, collisions, strCollisions);
        }

    }
}
