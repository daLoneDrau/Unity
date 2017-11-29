using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RPGBase.Flyweights;

namespace RPGTests.Flyweights
{
    [TestClass]
    public class RPGExceptionTest
    {
        [TestMethod]
        public void CanCreate()
        {
            RPGException ex = new RPGException(ErrorMessage.BAD_PARAMETERS, new Exception("msg"));
            Assert.IsTrue(string.Equals(ex.GetDeveloperMessage(), "msg", StringComparison.OrdinalIgnoreCase), "Dev message is not correct");
            Assert.AreEqual(ErrorMessage.BAD_PARAMETERS, ex.GetErrorMessage(), "Error message is not correct");

            ex = new RPGException(ErrorMessage.ILLEGAL_ACCESS, "msg");
            Assert.IsTrue(string.Equals(ex.GetDeveloperMessage(), "msg", StringComparison.OrdinalIgnoreCase), "Dev message is not correct");
            Assert.AreEqual(ErrorMessage.ILLEGAL_ACCESS, ex.GetErrorMessage(), "Error message is not correct");

            ex = new RPGException(ErrorMessage.INTERNAL_ERROR, "dev", new Exception("msg"));
            Assert.IsTrue(string.Equals(ex.GetDeveloperMessage(), "msg", StringComparison.OrdinalIgnoreCase), "Dev message is not correct");
            Assert.AreEqual(ErrorMessage.INTERNAL_ERROR, ex.GetErrorMessage(), "Error message is not correct");
        }
    }
}
