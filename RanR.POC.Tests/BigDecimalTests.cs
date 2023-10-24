using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RanR.POC.Helpers;

namespace RanR.POC.Tests
{
    [TestClass]
    public class BigDecimalTests
    {
        [TestMethod]
        public void add_big_decimal()
        {
            var bigDecimalUnderTest = new BigDecimal(new BigInteger(10), 0);

            bigDecimalUnderTest = bigDecimalUnderTest + 10;

            Assert.IsTrue(bigDecimalUnderTest.Exponent == 1);
        }

        [TestMethod]
        public void multiply_big_decimal()
        {
            var bigDecimalUnderTest = new BigDecimal(new BigInteger(10), 0);

            bigDecimalUnderTest = bigDecimalUnderTest * 10;

            Assert.IsTrue(bigDecimalUnderTest.Exponent == 2);
        }

        [TestMethod]
        public void divide_big_decimal()
        {
            var bigDecimalUnderTest = new BigDecimal(new BigInteger(1), 0);

            bigDecimalUnderTest = bigDecimalUnderTest / 2;

            Assert.IsTrue(bigDecimalUnderTest.Exponent == -1);
        }

        [TestMethod]
        public void subtract_big_decimal()
        {
            var bigDecimalUnderTest = new BigDecimal(new BigInteger(10), 0);

            bigDecimalUnderTest = bigDecimalUnderTest - 10;

            Assert.IsTrue(bigDecimalUnderTest.Exponent == 0);
        }

    }
}
