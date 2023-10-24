using Microsoft.VisualStudio.TestTools.UnitTesting;
using RanR.POC.Helpers;
using RanR.POC.Logging;
using RanR.POC.SolutionDefinitions;
using RanR.POC.SolutionManagement;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace RanR.POC.Tests
{
    [TestClass]
    public class SolutionGeneratorTests
    {
        [TestMethod]
        public void add_generated_solution_and_export_for_arbitrary_number()
        {
            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;
            var solutionRepository = new SolutionRepository(tokenSource);
            BigInteger encryptedFile = 1500;
            BigInteger decryptedFile = 500;
            var path = Path.GetTempPath();
            var filePair = (EncryptedFile: encryptedFile, DecryptedFile: decryptedFile);
            var solutionToAddAndExport = new Solution(filePair, new BigInteger(1500));
            SolutionGenerator.GenerateSolution(solutionRepository, solutionToAddAndExport, tokenSource, FileLogger.Instance, true);
            string exportPath = solutionRepository.ExportSolution(solutionToAddAndExport, path);
            byte[] exportedBytes = File.ReadAllBytes(exportPath);
            File.Delete(exportPath);
            Assert.IsTrue(exportedBytes.Length > 0);
        }

        [TestMethod]
        public void solve_for_unknown_plaintext_with_solution_20180625_simple_files()
        {
            var assembly = Assembly.GetExecutingAssembly();

            var initialConvertedFiles = new List<BigInteger>();
            List<string> initialFilesToProcess = new List<string>
                {
                    "RanR.POC.Tests.Samples._7ZIP.Simple1.txt.7z",
                    "RanR.POC.Tests.Samples._7ZIP.Simple1.txt",
                    "RanR.POC.Tests.Samples._7ZIP.Simple3.txt.7z",
                    "RanR.POC.Tests.Samples._7ZIP.Simple3.txt"
                };

            foreach (var file in initialFilesToProcess)
            {
                using (Stream stream = assembly.GetManifestResourceStream(file))
                {
                    byte[] fileBytes = new byte[stream.Length];
                    stream.Read(fileBytes, 0, fileBytes.Length);
                    initialConvertedFiles.Add(new BigInteger(fileBytes));
                }
            }

            var solutionUnderTest = new Solution((initialConvertedFiles[0], initialConvertedFiles[1]), initialConvertedFiles[2]);

            var unknownPlainText = new BigDecimal(initialConvertedFiles[3], 0);

            var modifier = (unknownPlainText * solutionUnderTest.SolutionGlobals.KnownCiphertext)
                / (solutionUnderTest.SolutionGlobals.TargetCiphertext * solutionUnderTest.SolutionGlobals.KnownPlaintext);

            var verifySolution = (unknownPlainText * solutionUnderTest.SolutionGlobals.KnownCiphertext)
                / (solutionUnderTest.SolutionGlobals.TargetCiphertext * modifier);

            Debug.WriteLine(modifier.ToString());

            Assert.IsTrue(solutionUnderTest.SolutionGlobals.KnownPlaintext == verifySolution);
        }

        [TestMethod]
        public void solve_for_unknown_plaintext_with_solution_20180625_complex_files()
        {
            var assembly = Assembly.GetExecutingAssembly();

            var initialConvertedFiles = new List<BigInteger>();
            List<string> initialFilesToProcess = new List<string>
                {
                    "RanR.POC.Tests.Samples._7ZIP.Complex1.txt.7z",
                    "RanR.POC.Tests.Samples._7ZIP.Complex1.txt",
                    "RanR.POC.Tests.Samples._7ZIP.Complex2.txt.7z",
                    "RanR.POC.Tests.Samples._7ZIP.Complex2.txt"
                };

            foreach (var file in initialFilesToProcess)
            {
                using (Stream stream = assembly.GetManifestResourceStream(file))
                {
                    byte[] fileBytes = new byte[stream.Length];
                    stream.Read(fileBytes, 0, fileBytes.Length);
                    initialConvertedFiles.Add(new BigInteger(fileBytes));
                }
            }

            var solutionUnderTest = new Solution((initialConvertedFiles[0], initialConvertedFiles[1]), initialConvertedFiles[2]);

            var unknownPlainText = new BigDecimal(initialConvertedFiles[3], 0);

            var modifier = (unknownPlainText * solutionUnderTest.SolutionGlobals.KnownCiphertext)
                / (solutionUnderTest.SolutionGlobals.TargetCiphertext * solutionUnderTest.SolutionGlobals.KnownPlaintext);

            var verifySolution = (unknownPlainText * solutionUnderTest.SolutionGlobals.KnownCiphertext)
                / (solutionUnderTest.SolutionGlobals.TargetCiphertext * modifier);

            var result = verifySolution.Truncate(solutionUnderTest.OriginalValues.DecryptedFile.ToString().Length);

            Debug.WriteLine(modifier.ToString());

            Assert.IsTrue(solutionUnderTest.SolutionGlobals.KnownPlaintext == result);
        }

        [TestMethod]
        public void solve_for_x()
        {
            var assembly = Assembly.GetExecutingAssembly();

            var initialConvertedFiles = new List<BigInteger>();
            List<string> initialFilesToProcess = new List<string>
                {
                    "RanR.POC.Tests.Samples._7ZIP.Simple1.txt.7z",
                    "RanR.POC.Tests.Samples._7ZIP.Simple1.txt",
                    "RanR.POC.Tests.Samples._7ZIP.Simple2.txt.7z",
                    "RanR.POC.Tests.Samples._7ZIP.Simple2.txt",
                    "RanR.POC.Tests.Samples._7ZIP.Simple3.txt.7z",
                    "RanR.POC.Tests.Samples._7ZIP.Simple3.txt"
                };

            foreach (var file in initialFilesToProcess)
            {
                using (Stream stream = assembly.GetManifestResourceStream(file))
                {
                    byte[] fileBytes = new byte[stream.Length];
                    stream.Read(fileBytes, 0, fileBytes.Length);
                    initialConvertedFiles.Add(new BigInteger(fileBytes));
                }
            }

            var interval = 0;
            var x = BigDecimal.Parse("1E0") + BigDecimal.Parse("1E-10000");
            var solution = new BigDecimal();
            var kc1 = new BigDecimal(initialConvertedFiles[0], 0);
            var kp1 = new BigDecimal(initialConvertedFiles[1], 0);
            var kc2 = new BigDecimal(initialConvertedFiles[2], 0);
            var kp2 = new BigDecimal(initialConvertedFiles[3], 0);
            var kc3 = new BigDecimal(initialConvertedFiles[4], 0);
            var kp3 = new BigDecimal(initialConvertedFiles[5], 0);

            try
            {
                while (true)
                {
                    interval++;
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();

                    var left = (kp1 * x * kc2) / kc1;
                    var right = (kp1 * x * kc3) / kc1;


                    if (left == kp2 && right == kp3)
                    {
                        solution = x;
                        Debug.WriteLine("Solution Found");
                        break;
                    }
                    else if (left > kp2 && right > kp3)
                    {
                        Debug.WriteLine("X Divided");
                        x = x / 2;
                    }
                    else if (left < kp2 && right < kp3)
                    {
                        Debug.WriteLine("X Multiplied");
                        x = x * 1.5;
                    }
                    else if (left > kp2 && right < kp3
                        || left < kp2 && right > kp3)
                    {
                        x = x + 1;
                        Debug.WriteLine("Mismatch");
                    }
                    if (interval % 500 == 0)
                    {
                        Debug.WriteLine("Current X: " + x);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(String.Concat("Exception found: ", ex.Message));
            }

            Debug.WriteLine(x);
            Assert.IsTrue(x != BigDecimal.Parse("1E0") + BigDecimal.Parse("1E-10000"));
        }
    }
}
