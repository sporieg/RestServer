using CodingAssignment.DataStore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading;

namespace CodingAssignmentTest.DataStore
{
    [TestClass]
    public class IdGeneratorTest
    {
        IdGenerator gen = new IdGenerator();
        bool[] WasGenned = new bool[20000];

        [TestMethod]
        public void IdGenerationIsThreadSafe()
        {
            var t1 = new Thread(new ThreadStart(Generate));
            var t2 = new Thread(new ThreadStart(Generate));
            t1.Start();
            t2.Start();
            t1.Join();
            t2.Join();
            var missedAny = WasGenned.Any(b => !b);
            Assert.IsFalse(missedAny, "Missed generating an id");

        }

        private void Generate()
        {
            for (int i = 0; i < 10000; i++)
            {
                WasGenned[gen.Next - 1] = true;
            }
        }
    }
}
