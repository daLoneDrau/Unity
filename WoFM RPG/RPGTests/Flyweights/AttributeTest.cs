using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RPGBase.Flyweights;

namespace RPGTests
{
    [TestClass]
    public class AttributeTest
    {
        /** test abbreviation. */
        private string abbr;
        /** test Attribute. */
        private RPGBase.Flyweights.Attribute att;
        /** test description. */
        private string desc;
        /** test name. */
        private string name;
        [TestInitialize]
        public void BeforeTests()
        {
            abbr = "abbr";
            name = "name";
            desc = "desc";
            att = new RPGBase.Flyweights.Attribute(abbr, name, desc);
        }
        [TestMethod]
        public void CanAdjustModifier()
        {
            // given

            // when
            att.AdjustModifier(2);

            // then
            float expected = 2f;
            float actual = att.Modifier;
            float delta = 0f;
            Assert.AreEqual(expected, actual, delta, "modifier is NOT 2");
        }
        [TestMethod]
        public void CanCalculateFullValue()
        {
            // given

            // when
            att.AdjustModifier(2);
            att.BaseVal = 10;

            // then
            float expected = 12f;
            float actual = att.Full;
            float delta = 0f;
            Assert.AreEqual(expected, actual, delta, "full is NOT 12");
        }
        [TestMethod]
        public void CanClearModifier()
        {
            // given

            // when
            att.AdjustModifier(2);
            att.ClearModifier();

            // then
            float expected = 0f;
            float actual = att.Modifier;
            float delta = 0f;
            Assert.AreEqual(expected, actual, delta, "modifier is NOT 0");
        }
        [TestMethod]
        public void CanConstruct2Strings()
        {
            RPGBase.Flyweights.Attribute att = new RPGBase.Flyweights.Attribute("abbreviation", "name");

            Assert.IsTrue(string.Equals(att.Abbr, "abbreviation", StringComparison.OrdinalIgnoreCase), "abbr is NOT correct");
            Assert.IsTrue(string.Equals(att.DisplayName, "name", StringComparison.OrdinalIgnoreCase), "name is NOT correct");
        }
        [TestMethod]
        public void CanConstructAllStrings()
        {
            // given

            // when
            RPGBase.Flyweights.Attribute att = new RPGBase.Flyweights.Attribute("abbreviation", "name", "description");

            // then
            Assert.IsTrue(string.Equals(att.Abbr, "abbreviation", StringComparison.OrdinalIgnoreCase), "abbr is NOT correct");
            Assert.IsTrue(string.Equals(att.DisplayName, "name", StringComparison.OrdinalIgnoreCase), "name is NOT correct");
            Assert.IsTrue(string.Equals(att.Description, "description", StringComparison.OrdinalIgnoreCase), "description is NOT correct");
        }
        [TestMethod]
        public void CanSetAbbr()
        {
            Assert.AreEqual("abbr", att.Abbr, "abbr is NOT correct");
            att.Abbr = "test";
            Assert.AreEqual("test", att.Abbr, "abbr is NOT correct");
            att.Abbr = "abbr";
            Assert.AreEqual("abbr", att.Abbr, "abbr is NOT correct");
        }
        [TestMethod]
        public void CanSetBase()
        {
            att.BaseVal = 2;
            Assert.AreEqual(2f, att.BaseVal, 0f, "base is NOT 2");
        }
        [TestMethod]
        public void CanSetDesc()
        {
            Assert.AreEqual("desc", att.Description, "description is NOT correct");
            att.Description = "test";
            Assert.AreEqual("test", att.Description, "description is NOT correct");
            att.Description = "desc";
            Assert.AreEqual("desc", att.Description, "description is NOT correct");
        }
        [TestMethod]
        public void CanSetName()
        {
            Assert.AreEqual("name", att.DisplayName, "description is NOT correct");
            att.DisplayName = "test";
            Assert.AreEqual("test", att.DisplayName, "description is NOT correct");
            att.DisplayName = "name";
            Assert.AreEqual("name", att.DisplayName, "description is NOT correct");
        }
        [TestMethod]
        [ExpectedException(typeof(RPGException), "Exception was NOT thrown")]
        public void WillNotConstructNullStringAbbreviation()
        {
            new RPGBase.Flyweights.Attribute(null, "name");
        }
        [TestMethod]
        [ExpectedException(typeof(RPGException), "Exception was NOT thrown")]
        public void WillNotConstructNullStringName()
        {

            new RPGBase.Flyweights.Attribute("abbr", null);
        }
        [TestMethod]
        [ExpectedException(typeof(RPGException), "Exception was NOT thrown")]
        public void WillNotConstructStringNullDesc()
        {

            new RPGBase.Flyweights.Attribute("", "", null);
        }
        [TestMethod]
        [ExpectedException(typeof(RPGException), "Exception was NOT thrown")]
        public void WillNotConstructStringNullName()
        {

            new RPGBase.Flyweights.Attribute("", null);
        }
        [TestMethod]
        [ExpectedException(typeof(RPGException), "Exception was NOT thrown")]
        public void WillNotConstructStringNullName2()
        {

            new RPGBase.Flyweights.Attribute("", null, "");
        }
        [TestMethod]
        [ExpectedException(typeof(RPGException), "Exception was NOT thrown")]
        public void WillNotSetNullStringAbbr()
        {
            att.Abbr = null;
        }
        [TestMethod]
        [ExpectedException(typeof(RPGException), "Exception was NOT thrown")]
        public void WillNotSetNullStringDesc()
        {
            att.Description = null;
        }
        [TestMethod]
        [ExpectedException(typeof(RPGException), "Exception was NOT thrown")]
        public void WillNotSetNullStringName()
        {
            att.DisplayName = null;
        }
    }
}
