using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RPGBase.Flyweights;
using RPGTests.Singletons;

namespace RPGTests.Flyweights
{
    [TestClass]
    public class IOEquipItemTest
    {
        private IOEquipItem data;
        [TestInitialize]
        public void BeforeTests()
        {
            new TestProjectConstants();
        }
        [TestMethod]
        public void TestMethod1()
        {
        }
    }
}
