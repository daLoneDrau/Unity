using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RPGBase.Constants;
using RPGBase.Flyweights;

namespace RPGTests.Flyweights
{
    [TestClass]
    public class BaseInteractiveObjectTest
    {
        private TestBaseInteractiveObject testObject;
        [TestInitialize]
        public void BeforeTests()
        {
            testObject = new TestBaseInteractiveObject(0);
        }
        [TestMethod]
        public void TestFlags()
        {
            Assert.IsFalse(testObject.HasBehaviorFlag(1));
            testObject.AddBehaviorFlag(1);
            testObject.AddBehaviorFlag(64);
            Assert.IsTrue(testObject.HasBehaviorFlag(1));
            Assert.IsTrue(testObject.HasBehaviorFlag(64));
            testObject.RemoveBehaviorFlag(64);
            Assert.IsFalse(testObject.HasBehaviorFlag(64));
            Assert.IsTrue(testObject.HasBehaviorFlag(1));
            testObject.ClearBehaviorFlags();


            Assert.IsFalse(testObject.HasGameFlag(1));
            testObject.AddGameFlag(1);
            testObject.AddGameFlag(64);
            Assert.IsTrue(testObject.HasGameFlag(1));
            Assert.IsTrue(testObject.HasGameFlag(64));
            testObject.RemoveGameFlag(64);
            Assert.IsFalse(testObject.HasGameFlag(64));
            Assert.IsTrue(testObject.HasGameFlag(1));
            testObject.ClearGameFlags();


            Assert.IsFalse(testObject.HasIOFlag(1));
            testObject.AddIOFlag(1);
            testObject.AddIOFlag(64);
            Assert.IsTrue(testObject.HasIOFlag(1));
            Assert.IsTrue(testObject.HasIOFlag(64));
            testObject.RemoveIOFlag(64);
            Assert.IsFalse(testObject.HasIOFlag(64));
            Assert.IsTrue(testObject.HasIOFlag(1));
            testObject.ClearIOFlags();
        }
        [TestMethod]
        public void TestGroups()
        {
            Assert.IsFalse(testObject.IsInGroup("test"));
            testObject.AddGroup("test");
            testObject.AddGroup("test");
            testObject.AddGroup("test2");
            Assert.IsTrue(testObject.IsInGroup("test"));
            Assert.IsTrue(testObject.IsInGroup("test2"));
            Assert.AreEqual(2, testObject.GetNumIOGroups());
            Assert.IsTrue(string.Equals("test", testObject.GetIOGroup(0), StringComparison.OrdinalIgnoreCase));
            testObject.RemoveGroup("test");
            Assert.IsFalse(testObject.IsInGroup("test"));
            Assert.AreEqual(1, testObject.GetNumIOGroups());
        }
        [TestMethod]
        public void TestSpells()
        {
            Assert.AreEqual(0, testObject.GetNumberOfSpellsOn());
            testObject.AddSpellOn(1);
            testObject.AddSpellOn(2);
            Assert.AreEqual(2, testObject.GetNumberOfSpellsOn());
            Assert.AreEqual(1, testObject.GetSpellOn(0));
            testObject.RemoveSpellOn(1);
            Assert.AreEqual(1, testObject.GetNumberOfSpellsOn());
            testObject.RemoveAllSpells();
            Assert.AreEqual(0, testObject.GetNumberOfSpellsOn());
        }
        [TestMethod]
        public void TestTypes()
        {
            testObject.AddTypeFlag(EquipmentGlobals.OBJECT_TYPE_DAGGER);
            testObject.AddTypeFlag(EquipmentGlobals.OBJECT_TYPE_SHIELD);
            testObject.AddTypeFlag(EquipmentGlobals.OBJECT_TYPE_FOOD);
        }
        [TestMethod]
        [ExpectedException(typeof(RPGException), "Exception was NOT thrown")]
        public void WillNotSetWeaponType()
        {
            testObject.AddTypeFlag(EquipmentGlobals.OBJECT_TYPE_WEAPON);
        }
        [TestMethod]
        [ExpectedException(typeof(RPGException), "Exception was NOT thrown")]
        public void WillNotSetType22()
        {
            testObject.AddTypeFlag(22);
        }
        [TestMethod]
        public void TestEquals()
        {
            Assert.IsTrue(testObject.Equals(testObject));
            Assert.IsFalse(testObject.Equals(null));
            Assert.IsFalse(testObject.Equals(new Object()));
            Assert.IsTrue(testObject.Equals(new TestBaseInteractiveObject(0)));
        }
        [TestMethod]
        public void TestGettersSetters()
        {
            testObject.Armormaterial = "velcro";
            Assert.IsTrue(string.Equals("velcro", testObject.Armormaterial, StringComparison.OrdinalIgnoreCase));
            testObject.DamageSum = 2.5f;
            Assert.AreEqual(2.5f, testObject.DamageSum, 0.0001f);
        }
    }
}
