using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeakDelegateTest
{
    class TestClass
    {
        public int IntValue { get; set; }

        public string stringValue { get; set; }

        public void Sum(int x, int y)
        {
            IntValue = x + y;
        }

        public void Mult(int x, string y)
        {
            IntValue = x * int.Parse(y);
        }

        public int Generic(int x, int y, int z)
        {
            return x + y + z;
        }

        public void NullFunc()
        {
            IntValue = 1;
        }

        public void ThreeSum(int x, string y, byte z)
        {
            IntValue = x + int.Parse(y) + z;
        }

        public int TestWeakDelete()
        {
            return 1;
        }
    }
}
