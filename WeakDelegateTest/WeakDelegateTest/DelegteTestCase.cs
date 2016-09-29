using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using _2;

namespace WeakDelegateTest
{
    [TestClass]
    public class DelegteTestCase
    {
        private TestClass testClass{ get { return new TestClass(); } set { testClass = value; } }

        [TestMethod]
        public void TestSum()
        {
            var testWeakClass = testClass;
            Delegate weakReference = new WeakDelegate((Action<int, int>)testWeakClass.Sum);
            
            weakReference.DynamicInvoke(5, 6);
            Assert.AreEqual(11, testWeakClass.IntValue);
        }

        [TestMethod]
        public void TestDefaultNullFunc()
        {
            var testWeakClass = testClass;
            Delegate weakReference = new WeakDelegate((Action)testWeakClass.NullFunc);
            weakReference.DynamicInvoke();
            Assert.AreEqual(1, testWeakClass.IntValue);
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

        [TestMethod]
        public void TestThreeSum()
        {
            var testWeakClass = testClass;
            Delegate weakReference = new WeakDelegate((Action<int, string, byte>)testWeakClass.ThreeSum);
            weakReference.DynamicInvoke(1, "2", (byte)3);
            Assert.AreEqual(6, testWeakClass.IntValue);
        }

        private event Func<int> funcIntEvent;
        [TestMethod]
        public void TestWeakDelete()
        {
            funcIntEvent += (Func<int>)new WeakDelegate((Func<int>)testClass.TestWeakDelete);
            Assert.AreEqual(1, funcIntEvent.DynamicInvoke());
            GC.Collect();
            Assert.AreEqual(0, funcIntEvent.DynamicInvoke());
        }
    }
}
