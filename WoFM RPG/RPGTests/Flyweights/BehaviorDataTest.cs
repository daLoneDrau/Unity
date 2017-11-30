using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RPGBase.Flyweights;

namespace RPGTests.Flyweights
{
    [TestClass]
    public class BehaviorDataTest
    {
        [TestMethod]
        public void CanInit()
        {
            BehaviourData data = new BehaviourData();
            Assert.IsNotNull(data);
            data.BehaviorParam = 2.5f;
            Assert.AreEqual(2.5f, data.BehaviorParam, .001f);
            data.Behaviour = 5;
            Assert.AreEqual(5, data.Behaviour);
            data.Exists=true;
            Assert.IsTrue(data.Exists);
            data.MoveMode = 3;
            Assert.AreEqual(3, data.MoveMode);
            data.Tactics = 4;
            Assert.AreEqual(4, data.Tactics);
            data.Target = 25;
            Assert.AreEqual(25, data.Target);
        }
    }
}
