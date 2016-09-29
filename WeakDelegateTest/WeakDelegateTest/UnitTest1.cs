using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using _2;

namespace WeakDelegateTest
{
    [TestClass]
    public class UnitTest1
    {
        private TestClass testClass{ get { return new TestClass(); } set { testClass = value; } }

        [TestMethod]
        public void TestSum()
        {
            var testWeakClass = testClass;
            Delegate weakReference = new WeakDelegate((Action<int, int>)testWeakClass.Sum);
            
            weakReference.DynamicInvoke(5, 6);
            Assert.AreEqual(testWeakClass.IntValue, 11);
        }

        [TestMethod]
        public void TestDefaultNullFunc()
        {
            var testWeakClass = testClass;
            Delegate weakReference = new WeakDelegate((Action)testWeakClass.NullFunc);
            weakReference.DynamicInvoke();
            Assert.AreEqual(testWeakClass.IntValue, 1);
        }

        private event Action testEvent;

        [TestMethod]
        public void TestWeakLink()
        {
            testEvent = null;
            var testWeakClass = testClass;
            WeakDelegate weakDelegate = new WeakDelegate((Action)testWeakClass.NullFunc);
            testEvent += (Action)weakDelegate;
            testEvent.DynamicInvoke();
            GC.Collect();
            testEvent.DynamicInvoke();
            Assert.IsTrue(weakDelegate.IsAlive);
        }

        [TestMethod]
        public void TestWeakLinkObjectNull()
        {
            testEvent = null;
            var testWeakClass = testClass;
            WeakDelegate weakDelegate = new WeakDelegate((Action)testWeakClass.NullFunc);
            testEvent += (Action)weakDelegate;
            testEvent.DynamicInvoke();
            testWeakClass = null;
            GC.Collect();
            testEvent.DynamicInvoke();
            Assert.IsFalse(weakDelegate.IsAlive);
        }

    }
}
