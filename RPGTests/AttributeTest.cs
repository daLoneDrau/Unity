using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            float actual = att.Modifier;
            float expected = 2;
            float delta = 0f;
            Assert.AreEqual(expected, actual, delta, "modifier is not 2");
        }
        
	[TestMethod]
	public void CanCalculateFullValue() {
		// given

		// when
		att.AdjustModifier(2);
		att.BaseVal= 10;

            // then
            float actual = att.Full;
            float expected = 12;
            float delta = 0f;
            Assert.AreEqual(expected, actual, delta, "full value is not 12");
	}
	[TestMethod]
	public void CanClearModifier() {
		// given

		// when
		att.AdjustModifier(2);
		att.ClearModifier();

            // then
            float actual = att.Modifier;
            float expected = 0;
            float delta = 0f;
            Assert.AreEqual(expected, actual, delta, "modifier is not 0");
	}
	[TestMethod]
	public void CanConstruct2Strings() {
            RPGBase.Flyweights.Attribute att = new RPGBase.Flyweights.Attribute("abbreviation", "name");
            string actual = att.Abbr;
            string expected = "abbreviation";
            Assert.IsTrue(string.Equals(actual, expected, StringComparison.OrdinalIgnoreCase), "abbreviation was not set");
            actual = att.DisplayName;
            expected = "name";
            Assert.IsTrue(string.Equals(actual, expected, StringComparison.OrdinalIgnoreCase), "name was not set");
        }
	[TestMethod]
	public void CanConstructAllStrings() {
            // given

            // when
            RPGBase.Flyweights.Attribute att = new RPGBase.Flyweights.Attribute("abbreviation", "name", "description");

            // then
            string actual = att.Abbr;
            string expected = "abbreviation";
            Assert.IsTrue(string.Equals(actual, expected, StringComparison.OrdinalIgnoreCase), "abbreviation was not set");
            actual = att.DisplayName;
            expected = "name";
            Assert.IsTrue(string.Equals(actual, expected, StringComparison.OrdinalIgnoreCase), "name was not set");
            actual = att.Description;
            expected = "description";
            Assert.IsTrue(string.Equals(actual, expected, StringComparison.OrdinalIgnoreCase), "description was not set");
        }
	[TestMethod]
	public void CanConstructNullAbbreviationNullName() {
            // given

            // when
            RPGBase.Flyweights.Attribute att = new RPGBase.Flyweights.Attribute(null, null);

            // then
            string actual = att.Abbr;
            string expected = null;
            Assert.AreEqual(expected, actual, "abbreviation was not set");

            Assert.IsTrue(string.Equals(actual, expected, StringComparison.OrdinalIgnoreCase), "abbreviation was not set");
            assertTrue("abbreviation", Arrays.equals(att.getAbbr(), (char[]) null));
		assertTrue("name", Arrays.equals(att.getDisplayName(), (char[]) null));
	}
	[TestMethod]
	public void canConstructNullAbbreviationNullNameNullDescription() {
		// given

		// when
		Attribute att =
				new Attribute((char[]) null, (char[]) null, (char[]) null);

		// then
		assertTrue("abbreviation", Arrays.equals(att.getAbbr(), (char[]) null));
		assertTrue("name", Arrays.equals(att.getDisplayName(), (char[]) null));
		assertTrue("description",
				Arrays.equals(att.getDescription(), (char[]) null));
	}
	[TestMethod]
	public void canConstructNullAbbreviationNullNameValidDescription() {
		// given

		// when
		Attribute att = new Attribute((char[]) null, (char[]) null, desc);

		// then
		assertTrue("abbreviation", Arrays.equals(att.getAbbr(), (char[]) null));
		assertTrue("name", Arrays.equals(att.getDisplayName(), (char[]) null));
		assertTrue("description", Arrays.equals(att.getDescription(), desc));
	}
	[TestMethod]
	public void canConstructNullAbbreviationValidName() {
		// given

		// when
		Attribute att = new Attribute((char[]) null, name);

		// then
		assertTrue("abbreviation", Arrays.equals(att.getAbbr(), (char[]) null));
		assertTrue("name", Arrays.equals(att.getDisplayName(), name));
	}
	[TestMethod]
	public void canConstructNullAbbreviationValidNameNullDescription() {
		// given

		// when
		Attribute att = new Attribute((char[]) null, name, (char[]) null);

		// then
		assertTrue("abbreviation", Arrays.equals(att.getAbbr(), (char[]) null));
		assertTrue("name", Arrays.equals(att.getDisplayName(), name));
		assertTrue("description",
				Arrays.equals(att.getDescription(), (char[]) null));
	}
	[TestMethod]
	public void canConstructValidAbbreviationNullName() {
		// given

		// when
		Attribute att = new Attribute(abbr, (char[]) null);

		// then
		assertTrue("abbreviation", Arrays.equals(att.getAbbr(), abbr));
		assertTrue("name", Arrays.equals(att.getDisplayName(), (char[]) null));
	}

	[TestMethod]
	public void canConstructValidAbbreviationNullNameNullDescription() {
		// given

		// when
		Attribute att = new Attribute(abbr, (char[]) null, (char[]) null);

		// then
		assertTrue("abbreviation", Arrays.equals(att.getAbbr(), abbr));
		assertTrue("name", Arrays.equals(att.getDisplayName(), (char[]) null));
		assertTrue("description", Arrays.equals(
				att.getDescription(), (char[]) null));
	}

	[TestMethod]
	public void canSetAbbr() {
		assertTrue("abbreviation",
				Arrays.equals(att.getAbbr(), "abbr".toCharArray()));
		att.setAbbr("test".toCharArray());
		assertTrue("abbreviation",
				Arrays.equals(att.getAbbr(), "test".toCharArray()));
		att.setAbbr("abbr");
		assertTrue("abbreviation",
				Arrays.equals(att.getAbbr(), "abbr".toCharArray()));
	}
	[TestMethod]
	public void canSetBase() {
		att.setBase(2);
		assertEquals("base", att.getBase(), 2, 0f);
	}
	[TestMethod]
	public void canSetDesc() {
		assertTrue("description",
				Arrays.equals(att.getDescription(), "desc".toCharArray()));
		att.setDescription("test".toCharArray());
		assertTrue("description",
				Arrays.equals(att.getDescription(), "test".toCharArray()));
		att.setDescription("desc");
		assertTrue("description",
				Arrays.equals(att.getDescription(), "desc".toCharArray()));
	}

	[TestMethod]
	public void canSetName() {
		assertTrue("name",
				Arrays.equals(att.getDisplayName(), "name".toCharArray()));
		att.setDisplayName("test".toCharArray());
		assertTrue("name",
				Arrays.equals(att.getDisplayName(), "test".toCharArray()));
		att.setDisplayName("name");
		assertTrue("name",
				Arrays.equals(att.getDisplayName(), "name".toCharArray()));
	}

	[TestMethod](expected = RPGException.class)
	public void willNotConstructNullStringAbbreviation()
			throws RPGException {
		new Attribute(null, "name");
	}
	[TestMethod](expected = RPGException.class)
	public void willNotConstructNullStringAbbreviation2()
			throws RPGException {
		new Attribute((String) null, "", "");
	}
	[TestMethod](expected = RPGException.class)
	public void willNotConstructNullStringName()
			throws RPGException {
		new Attribute("abbr", null);
	}
	[TestMethod](expected = RPGException.class)
	public void willNotConstructStringNullDesc()
			throws RPGException {
		new Attribute("", "", (String) null);
	}
	[TestMethod](expected = RPGException.class)
	public void willNotConstructStringNullName()
			throws RPGException {
		new Attribute("", (String) null);
	}
	[TestMethod](expected = RPGException.class)
	public void willNotConstructStringNullName2()
			throws RPGException {
		new Attribute("", (String) null, "");
	}
	[TestMethod](expected = RPGException.class)
	public void willNotSetNullCharAbbr() {
		att.setAbbr((char[]) null);
	}
	[TestMethod](expected = RPGException.class)
	public void willNotSetNullCharDesc() {
		att.setDescription((char[]) null);
	}
	[TestMethod](expected = RPGException.class)
	public void willNotSetNullCharName() {
		att.setDisplayName((char[]) null);
	}
	[TestMethod](expected = RPGException.class)
	public void willNotSetNullStringAbbr() {
		att.setAbbr((String) null);
	}
	[TestMethod](expected = RPGException.class)
	public void willNotSetNullStringDesc() {
		att.setDescription((String) null);
	}
	[TestMethod](expected = RPGException.class)
	public void willNotSetNullStringName() {
		att.setDisplayName((String) null);
	}
    }
}
