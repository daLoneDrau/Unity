using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RPGBase.Flyweights;

namespace RPGTests.Flyweights
{
    [TestClass]
    public class EquipmentItemModifierTest
    {
        private EquipmentItemModifier data;
        [TestInitialize]
        public void BeforeTests()
        {
            data = new EquipmentItemModifier
            {
                Percent = true,
                Special = 1,
                Value = 64
            };
        }
        [TestMethod]
        public void CanInit()
        {
            Assert.IsTrue(data.Percent);
            Assert.AreEqual(1, data.Special);
            Assert.AreEqual(64, data.Value, .001f);
        }
        [TestMethod]
        public void CanClearAll()
        {
            data.Percent = true;
            data.Special=12;
            data.Value=10;
            Assert.IsTrue(data.Percent);
            Assert.AreEqual(12, data.Special);
            Assert.AreEqual(10, data.Value, .001f);
            data.ClearData();
            Assert.IsFalse(data.Percent);
            Assert.AreEqual(0, data.Special);
            Assert.AreEqual(0f, data.Value, .001f);

        }
    }
}
