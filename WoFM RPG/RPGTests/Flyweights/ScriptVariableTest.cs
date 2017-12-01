using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RPGBase.Flyweights;
using RPGBase.Constants;
using System.Diagnostics;

namespace RPGTests.Flyweights
{
    [TestClass]
    public class ScriptVariableTest
    {
        private ScriptVariable faVar;
        private ScriptVariable fVar;
        private ScriptVariable iaVar;
        private ScriptVariable iVar;
        private ScriptVariable laVar;
        private ScriptVariable lVar;
        private ScriptVariable tVar;
        private ScriptVariable taVar;
        private ScriptVariable tgaVar;
        [TestInitialize]
        public void BeforeTests()
        {
            tVar = new ScriptVariable("tvar", ScriptConsts.TYPE_G_00_TEXT, "test");
            taVar = new ScriptVariable("taVar", ScriptConsts.TYPE_L_09_TEXT_ARR, new String[] { "test", "2" });
            tgaVar = new ScriptVariable("tgaVar", ScriptConsts.TYPE_G_01_TEXT_ARR, new String[] { "testg" });
            fVar = new ScriptVariable("fVar", ScriptConsts.TYPE_L_10_FLOAT, 1.2f);
            faVar = new ScriptVariable("faVar", ScriptConsts.TYPE_G_03_FLOAT_ARR, new float[] { 0f, 1.3f });
            iVar = new ScriptVariable("ivar", ScriptConsts.TYPE_L_12_INT, 5);
            iaVar = new ScriptVariable("iaVar", ScriptConsts.TYPE_G_05_INT_ARR, new int[] { 1, 2 });
            lVar = new ScriptVariable("lVar", ScriptConsts.TYPE_G_06_LONG, 5L);
            laVar = new ScriptVariable("laVar", ScriptConsts.TYPE_L_15_LONG_ARR, new long[] { 12, 30 });
        }
        [TestMethod]
        [ExpectedException(typeof(RPGException), "Exception was NOT thrown")]
        public void WillNotGetMisnamedTextArray()
        {
            string[] r = fVar.Textaval;
        }

        [TestMethod]
        [ExpectedException(typeof(RPGException), "Exception was NOT thrown")]
        public void WillNotGetMisnamedTextArrayByIndex()
        {
            string s = fVar.Textaval[1];
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException), "Exception was NOT thrown")]
        public void WillNotGetInvalidTextArrayByIndex()
        {
            string s = taVar.Textaval[5];
        }
        [TestMethod]
        public void CanGetTextArray()
        {
            Assert.AreEqual("test", taVar.Textaval[0]);
            Assert.AreEqual("2", taVar.Textaval[1]);
            Assert.AreEqual("testg", tgaVar.Textaval[0]);
            taVar.Set(1, 33);
            Assert.AreEqual("33", taVar.Textaval[1]);
            Console.WriteLine(taVar.Name);
            taVar = new ScriptVariable(taVar);
            Console.WriteLine("4");
            Assert.AreEqual("33", taVar.Textaval[1]);
            Console.WriteLine("5");
        }
        [TestMethod]
        public void CanCreate()
        {
            float delta = .001f;
            Assert.IsNotNull(tVar);
            Assert.AreEqual("tvar", tVar.Name);
            Assert.AreEqual("test", tVar.Text);
            tVar.Set("test2");
            tVar = new ScriptVariable(tVar);
            Assert.AreEqual("test2", tVar.Text);
            Assert.AreEqual(ScriptConsts.TYPE_G_00_TEXT, tVar.Type);
            tVar.Clear();
            Assert.IsNull(tVar.Text);

            Assert.IsNotNull(fVar);
            Assert.AreEqual(1.2f, fVar.Fval, delta);
            fVar.Set(1f);
            Assert.AreEqual(1f, fVar.Fval, delta);

            Assert.IsNotNull(faVar);
            Assert.IsNotNull(faVar.Faval);
            Assert.AreEqual(1.3f, faVar.Faval[1], delta);
            faVar.Set(new float[] { 0.5f });
            faVar.Set(2f);
            faVar = new ScriptVariable(faVar);
            Assert.AreEqual(2f, faVar.Faval[1], delta);

            Assert.IsNotNull(iVar);
            Assert.AreEqual(5, iVar.Ival);
            iVar.Set(0);
            Assert.AreEqual(0, iVar.Ival);

            Assert.IsNotNull(iaVar);
            Assert.IsNotNull(iaVar.Iaval);
            Assert.AreEqual(2, iaVar.Iaval[1]);
            iaVar.Set(new int[] { 1 });
            iaVar.Set(2);
            iaVar = new ScriptVariable(iaVar);
            Assert.AreEqual(2, iaVar.Iaval[1]);

            Assert.IsNotNull(lVar);
            Assert.AreEqual(5L, lVar.Lval);
            lVar.Set(31L);
            Assert.AreEqual(31L, lVar.Lval);

            Assert.IsNotNull(laVar);
            Assert.IsNotNull(laVar.Laval);
            Assert.AreEqual(30L, laVar.Laval[1]);
            lVar = new ScriptVariable(lVar);
            laVar = new ScriptVariable(laVar);
        }
        [TestMethod]
        [ExpectedException(typeof(RPGException), "Exception was NOT thrown")]
        public void WillNotCloneFromNull()
        {
            fVar = new ScriptVariable(null);
        }
        [TestMethod]
        public void CanSetFields()
        {
            fVar.Set(null);
            const float delta = .001f;
            Assert.AreEqual(0f, fVar.Fval, delta);
            fVar.Type = ScriptConsts.TYPE_G_02_FLOAT;
            fVar.Set(2.5);
            Assert.AreEqual(2.5f, fVar.Fval, delta);
            fVar.Set(0);
            Assert.AreEqual(0f, fVar.Fval, delta);
            fVar.Set(.5f);
            Assert.AreEqual(.5f, fVar.Fval, delta);
            fVar.Set("0.5");
            Assert.AreEqual(.5f, fVar.Fval, delta);

            faVar.Set(null);
            Assert.AreEqual(0, faVar.Faval.Length);
            faVar.Set(new float[] { .5f });
            Assert.AreEqual(.5f, faVar.Faval[0], delta);
            faVar.Set(0.5f);
            Assert.AreEqual(2, faVar.Faval.Length);
            Assert.AreEqual(.5f, faVar.Faval[1], delta);
            faVar.Set(2.2);
            Assert.AreEqual(2.2f, faVar.Faval[2], delta);
            faVar.Set(3);
            Assert.AreEqual(3f, faVar.Faval[3], delta);
            faVar.Set("4");
            Assert.AreEqual(4f, faVar.Faval[4], delta);
            faVar.Set(0, 1f);
            Assert.AreEqual(1f, faVar.Faval[0], delta);
            faVar.Set(1, 1.1);
            Assert.AreEqual(1.1f, faVar.Faval[1], delta);
            faVar.Set(20, 20);
            Assert.AreEqual(20f, faVar.Faval[20], delta);
            faVar.Set(20, "20.22");
            Assert.AreEqual(20.22f, faVar.Faval[20], delta);

            iVar.Set(null);
            Assert.AreEqual(0, iVar.Ival);
            iVar.Set(2.5);
            Assert.AreEqual(2, iVar.Ival);
            iVar.Set(0);
            Assert.AreEqual(0, iVar.Ival);
            iVar.Set(.5f);
            Assert.AreEqual(0, iVar.Ival);
            iVar.Set("20");
            Assert.AreEqual(20, iVar.Ival);

            iaVar.Set(null);
            Assert.AreEqual(0, iaVar.Iaval.Length);
            iaVar.Set(new int[] { 5 });
            Assert.AreEqual(5, iaVar.Iaval[0]);
            iaVar.Set(0.5f);
            Assert.AreEqual(0, iaVar.Iaval[1]);
            iaVar.Set(2.2);
            Assert.AreEqual(2, iaVar.Iaval[2]);
            iaVar.Set(3);
            Assert.AreEqual(3, iaVar.Iaval[3]);
            iaVar.Set("4");
            Assert.AreEqual(4, iaVar.Iaval[4]);
            iaVar.Set(0, 1f);
            Assert.AreEqual(1, iaVar.Iaval[0]);
            iaVar.Set(1, 1.1);
            Assert.AreEqual(1, iaVar.Iaval[1]);
            iaVar.Set(20, 20);
            Assert.AreEqual(20, iaVar.Iaval[20]);
            iaVar.Set(20, "20");
            Assert.AreEqual(20, iaVar.Iaval[20]);

            lVar.Set(null);
            Assert.AreEqual(0L, lVar.Lval);
            lVar.Set(2.5);
            Assert.AreEqual(2L, lVar.Lval);
            lVar.Set(0);
            Assert.AreEqual(0L, lVar.Lval);
            lVar.Set(.5f);
            Assert.AreEqual(0L, lVar.Lval);
            lVar.Set(2234l);
            Assert.AreEqual(2234L, lVar.Lval);
            lVar.Set("55555334556");
            Assert.AreEqual(55555334556L, lVar.Lval);

            laVar.Set(null);
            Assert.AreEqual(0, laVar.Laval.Length);
            laVar.Set(new long[] { 5 });
            Assert.AreEqual(5L, laVar.Laval[0]);
            laVar.Set(0.5f);
            Assert.AreEqual(0L, laVar.Laval[1]);
            laVar.Set(2.2);
            Assert.AreEqual(2L, laVar.Laval[2]);
            laVar.Set(3);
            Assert.AreEqual(3L, laVar.Laval[3]);
            laVar.Set(4L);
            Assert.AreEqual(4L, laVar.Laval[4]);
            laVar.Set(5L);
            Assert.AreEqual(5L, laVar.Laval[5]);
            laVar.Set(0, 1f);
            Assert.AreEqual(1L, laVar.Laval[0]);
            laVar.Set(1, 1.1);
            Assert.AreEqual(1L, laVar.Laval[1]);
            laVar.Set(20, 20);
            Assert.AreEqual(20L, laVar.Laval[20]);
            laVar.Set(20, 20L);
            Assert.AreEqual(20L, laVar.Laval[20]);
            laVar.Set(20, "20");
            Assert.AreEqual(20L, laVar.Laval[20]);

            tVar.Set(1);
            Assert.AreEqual("1", tVar.Text);
            tVar.Set("test");
            Assert.AreEqual("test", tVar.Text);
            tVar.Set(null);
            Assert.IsNull(tVar.Text);
            tVar.Name = "tVar";
            Assert.AreEqual("tVar", tVar.Name);
        }
        [TestMethod]
        [ExpectedException(typeof(RPGException), "Exception was NOT thrown")]
        public void WillNotCreateEmptyName()
        {
            new ScriptVariable("", 255, null);
        }
        [TestMethod]
        [ExpectedException(typeof(RPGException), "Exception was NOT thrown")]
        public void WillNotCreateNullInvalidType()
        {
            new ScriptVariable("test", 255, null);
        }
        [TestMethod]
        [ExpectedException(typeof(RPGException), "Exception was NOT thrown")]
        public void WillNotCreateNullName()
        {
            new ScriptVariable(null, 255, null);
        }
        [TestMethod]
        [ExpectedException(typeof(RPGException), "Exception was NOT thrown")]
        public void WillNotGetWrongTypeF()
        {
            var f = fVar.Faval;
        }
        [TestMethod]
        [ExpectedException(typeof(RPGException), "Exception was NOT thrown")]
        public void WillNotGetWrongTypeFA()
        {
            var f = faVar.Fval;
        }
        [TestMethod]
        [ExpectedException(typeof(RPGException), "Exception was NOT thrown")]
        public void WillNotGetWrongTypeI()
        {
            var f = iVar.Iaval;
        }
        [TestMethod]
        [ExpectedException(typeof(RPGException), "Exception was NOT thrown")]
        public void WillNotGetWrongTypeIA()
        {
            var f = iaVar.Ival;
        }
        [TestMethod]
        [ExpectedException(typeof(RPGException), "Exception was NOT thrown")]
        public void WillNotGetWrongTypeL()
        {
            var f = lVar.Laval;
        }
        [TestMethod]
        [ExpectedException(typeof(RPGException), "Exception was NOT thrown")]
        public void WillNotGetWrongTypeLA()
        {
            var f = laVar.Text;
        }

        [TestMethod]
        [ExpectedException(typeof(RPGException), "Exception was NOT thrown")]
        public void WillNotGetWrongTypeT()
        {
            var f = tVar.Lval;
        }

        [TestMethod]
        [ExpectedException(typeof(RPGException), "Exception was NOT thrown")]
        public void WillNotSetInvalidFloat()
        {
            fVar.Set(new Object());
        }
        [TestMethod]
        [ExpectedException(typeof(RPGException), "Exception was NOT thrown")]
        public void WillNotSetInvalidFloatArray()
        {
            faVar.Set(221L);
        }
        [TestMethod]
        [ExpectedException(typeof(RPGException), "Exception was NOT thrown")]
        public void WillNotSetInvalidFloatArray2()
        {
            faVar.Set(new Object());
        }
        [TestMethod]
        [ExpectedException(typeof(RPGException), "Exception was NOT thrown")]
        public void WillNotSetInvalidFloatArrayIndex()
        {
            faVar.Set(0, null);
        }
        [TestMethod]
        [ExpectedException(typeof(RPGException), "Exception was NOT thrown")]
        public void WillNotSetInvalidFloatArrayIndex2()
        {
            faVar.Set(0, "a");
        }
        [TestMethod]
        [ExpectedException(typeof(RPGException), "Exception was NOT thrown")]
        public void WillNotSetInvalidFloatString()
        {
            fVar.Set("a");
        }
        [TestMethod]
        [ExpectedException(typeof(RPGException), "Exception was NOT thrown")]
        public void WillNotSetInvalidInt()
        {
            iVar.Set(221L);
        }
        [TestMethod]
        [ExpectedException(typeof(RPGException), "Exception was NOT thrown")]
        public void WillNotSetInvalidIntArray()
        {
            iaVar.Set(221L);
        }
        [TestMethod]
        [ExpectedException(typeof(RPGException), "Exception was NOT thrown")]
        public void WillNotSetInvalidIntArrayIndex()
        {
            iaVar.Set(0, null);
        }
        [TestMethod]
        [ExpectedException(typeof(RPGException), "Exception was NOT thrown")]
        public void WillNotSetInvalidIntArrayIndex2()
        {
            iaVar.Set(0, "a");
        }
        [TestMethod]
        [ExpectedException(typeof(RPGException), "Exception was NOT thrown")]
        public void WillNotSetInvalidIntString()
        {
            iVar.Set("a");
        }
        [TestMethod]
        [ExpectedException(typeof(RPGException), "Exception was NOT thrown")]
        public void WillNotSetInvalidLong()
        {
            lVar.Set(new Object());
        }
        [TestMethod]
        [ExpectedException(typeof(RPGException), "Exception was NOT thrown")]
        public void WillNotSetInvalidLongArray()
        {
            laVar.Set(new int[] { });
        }
        [TestMethod]
        [ExpectedException(typeof(RPGException), "Exception was NOT thrown")]
        public void WillNotSetInvalidLongArrayIndex()
        {
            laVar.Set(0, null);
        }
        [TestMethod]
        [ExpectedException(typeof(RPGException), "Exception was NOT thrown")]
        public void WillNotSetInvalidLongArrayIndex2()
        {
            laVar.Set(0, "a");
        }
        [TestMethod]
        [ExpectedException(typeof(RPGException), "Exception was NOT thrown")]
        public void WillNotSetInvalidLongString()
        {
            lVar.Set("a");
        }
        [TestMethod]
        [ExpectedException(typeof(RPGException), "Exception was NOT thrown")]
        public void WillNotSetInvalidNameEmpty()
        {
            laVar.Name = "";
        }
        [TestMethod]
        [ExpectedException(typeof(RPGException), "Exception was NOT thrown")]
        public void WillNotSetInvalidNameNull()
        {
            laVar.Name = null;
        }
        [TestMethod]
        [ExpectedException(typeof(RPGException), "Exception was NOT thrown")]
        public void WillNotSetInvalidType()
        {
            laVar.Type = -1;
        }
        [TestMethod]
        [ExpectedException(typeof(RPGException), "Exception was NOT thrown")]
        public void WillNotSetInvalidType2()
        {
            laVar.Type = 16;
        }
        [TestMethod]
        [ExpectedException(typeof(RPGException), "Exception was NOT thrown")]
        public void WillNotSetNonArray()
        {
            lVar.Set(0, 14);
        }
    }
}
