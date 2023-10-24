using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RanR.POC.Helpers;
using RanR.POC.Logging;
using RanR.POC.Operations;
using RanR.POC.SolutionDefinitions;

namespace RanR.POC.Tests
{
    [TestClass]
    public class JobCompilerTests
    {
        [TestMethod]
        public void execute_full_calculation_library()
        {
            var solutionToTest = new Solution((new BigInteger(1500), new BigInteger(1000)), new BigInteger(1500));
            BigInteger constant = 1000;

            foreach (var op in CalculationLibrary.OperationCollection.Keys)
            {
                JobCompiler.ExecuteCalculation(
                                    String.Format(CalculationLibrary.OperationCollection[op], constant),
                                    CalculationLibrary.AssemblyReferences,
                                    solutionToTest.SolutionGlobals);
            }
            Assert.IsTrue(solutionToTest.SolutionGlobals.TargetCiphertext == new BigDecimal(solutionToTest.TargetFile, 0));
        }
    }
}
