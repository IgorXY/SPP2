using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using _2;

namespace WeakDelegateTest
{
    [TestClass]
    public class UnitTest1
    {
        private TestClass testClass{ get { return new TestClass(); } }

        [TestMethod]
        public void TestSum()
        {
            var testWeakClass = testClass;
            Delegate weakReference = new WeakDelegate((Action<int, int>)testWeakClass.Sum);
            
            weakReference.DynamicInvoke(5, 6);
            Assert.AreEqual(testWeakClass.IntValue, 11);
        }

    }
}
