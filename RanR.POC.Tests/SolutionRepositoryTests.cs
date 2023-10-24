using Microsoft.VisualStudio.TestTools.UnitTesting;
using RanR.POC.Logging;
using RanR.POC.SolutionDefinitions;
using RanR.POC.SolutionManagement;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;
using System.Threading;

namespace RanR.POC.Tests
{
    [TestClass]
    public class SolutionRepositoryTests
    {
        [TestMethod]
        public void add_generated_solution_twice()
        {
            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;
            var solutionRepository = new SolutionRepository(tokenSource);
            BigInteger encryptedFile = 1500;
            BigInteger decryptedFile = 500;
            var filePair = (EncryptedFile: encryptedFile, DecryptedFile: decryptedFile);
            var solutionToAddAndExport = new Solution(filePair, new BigInteger(double.MaxValue));
            solutionRepository.AddSolution(solutionToAddAndExport, SolutionGenerator.ExecuteSolution(solutionToAddAndExport));
            Assert.IsFalse(solutionRepository.AddSolution(solutionToAddAndExport, SolutionGenerator.ExecuteSolution(solutionToAddAndExport)));
        }
    }
}
