using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RPGBase.Singletons;
using System.Collections.Generic;

namespace RPGTests.Singletons
{
    [TestClass]
    public class DicerollerTest
    {
        [TestMethod]
        public void CanGetRandom()
        {
            char[] ca = new char[] { 't', 'e', 's', 't' };
            for (int i = 255; i >= 0; i--)
            {
                char c = Diceroller.Instance.GetRandomIndex(ca);
                Assert.IsTrue(c == ca[0] || c == ca[1] || c == ca[2] || c == ca[3]);
            }
            int[] ia = new int[] { 1, 2, 3, 4 };
            for (int i = 255; i >= 0; i--)
            {
                int c = Diceroller.Instance.GetRandomIndex(ia);
                Assert.IsTrue(c == ia[0] || c == ia[1] || c == ia[2] || c == ia[3]);
            }
            for (int i = 255; i >= 0; i--)
            {
                long l = Diceroller.Instance.GetRandomLong();
                Assert.IsFalse(string.Equals("", l.ToString(), StringComparison.OrdinalIgnoreCase));
            }
            List<object> ol = new List<object>
            {
                new Object(),
                new Object(),
                new Object()
            };
            for (int i = 255; i >= 0; i--)
            {
                Object o = Diceroller.Instance.GetRandomObject(ol);
                Assert.IsTrue(ol.Contains(o));
            }
            Dictionary<object, object> om = new Dictionary<Object, Object>
            {
                { new Object(), new Object() },
                { new Object(), new Object() },
                { new Object(), new Object() }
            };
            for (int i = 255; i >= 0; i--)
            {
                Object o = Diceroller.Instance.GetRandomObject(om);
                Assert.IsTrue(om.ContainsValue(o));
            }
            Object[] oa = new Object[] { new Object(), new Object(), new Object() };
            for (int i = 255; i >= 0; i--)
            {
                Object o = Diceroller.Instance.GetRandomObject(oa);
            }
            for (int i = ol.Count; i > 0; i--)
            {
                Object o = Diceroller.Instance.RemoveRandomObject(ol);
                Assert.IsFalse(ol.Contains(o));
            }
            Assert.AreEqual(0, ol.Count);
            for (int i = om.Count; i > 0; i--)
            {
                Object o = Diceroller.Instance.RemoveRandomObject(om);
                Assert.IsFalse(om.ContainsValue(o));
            }
            Assert.AreEqual(0, om.Count);
            for (int i = 255; i >= 0; i--)
            {
                float p = Diceroller.Instance.RollPercent();
                Assert.IsTrue(p >= 0.0f && p <= 1.0f);
            }
            for (int i = 255; i >= 0; i--)
            {
                int d = Diceroller.Instance.RolldX(6);
                Assert.IsTrue(d >= 1 && d <= 6);
            }
            for (int i = 255; i >= 0; i--)
            {
                int d = Diceroller.Instance.RollXdY(3, 6);
                Assert.IsTrue(d >= 3 && d <= 18);
            }
            for (int i = 255; i >= 0; i--)
            {
                int d = Diceroller.Instance.RolldXPlusY(4, 1);
                Assert.IsTrue(d >= 2 && d <= 5);
            }
        }
    }
}
