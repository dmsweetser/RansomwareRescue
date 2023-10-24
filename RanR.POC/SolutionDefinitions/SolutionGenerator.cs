using Newtonsoft.Json;
using RanR.POC.Helpers;
using RanR.POC.Logging;
using RanR.POC.Operations;
using RanR.POC.SolutionManagement;
using System;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

namespace RanR.POC.SolutionDefinitions
{
    public static class SolutionGenerator
    {

        public static Solution GenerateSolution(SolutionRepository solutionRepository, Solution solutionToGenerate, CancellationTokenSource tokenSource, ILogger logger, bool testmode = false, string initialValueForX = "1E0")
        {
            if (solutionToGenerate.Operations.Count > 0) return null;
            if (testmode)
            {
                solutionToGenerate.Operations.Add(CalculationLibrary.OperationCollection["Solution"]);
                solutionToGenerate.IsValid = true;
                return solutionToGenerate;
            }


            var sequenceGuid = Guid.NewGuid().ToString();
            var solutionToFind = new ConcurrentDictionary<int, BigDecimal>();
            ParallelOptions executionOptions = new ParallelOptions();

            //TODO- fix this so that you can parallelize the solution- currently not working
            executionOptions.MaxDegreeOfParallelism = 1;
            executionOptions.CancellationToken = tokenSource.Token;

            if (initialValueForX == "1E0")
            {
                solutionToFind.TryAdd(0, BigDecimal.Parse("1E0") + BigDecimal.Parse("1E-10000"));
            }
            else
            {
                solutionToFind.TryAdd(0, BigDecimal.Parse(initialValueForX));
            }
            

            Action solveForX = () =>
            {
                //Thread.Sleep(new Random().Next(15));
                var solutionUnderTest = new Solution(solutionToGenerate.OriginalValues, solutionToGenerate.TargetFile);
                var threadGuid = Guid.NewGuid();
                while (!tokenSource.Token.IsCancellationRequested)
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();
                    solutionUnderTest.ResetGlobals();

                    solutionUnderTest.SolutionGlobals.UnknownPlaintext =
                        ((solutionUnderTest.SolutionGlobals.KnownPlaintext * solutionToFind.GetOrAdd(0, BigDecimal.Parse(initialValueForX))) * solutionUnderTest.SolutionGlobals.TargetCiphertext)
                        / solutionUnderTest.SolutionGlobals.KnownCiphertext;

                    solutionUnderTest.SolutionGlobals.VerifiedX =
                        (solutionUnderTest.SolutionGlobals.UnknownPlaintext * solutionUnderTest.SolutionGlobals.KnownCiphertext)
                        / (solutionUnderTest.SolutionGlobals.TargetCiphertext * solutionUnderTest.SolutionGlobals.KnownPlaintext);

                    solutionToFind.AddOrUpdate(0, solutionUnderTest.SolutionGlobals.VerifiedX, (i, x) =>
                    {
                        if (x == solutionUnderTest.SolutionGlobals.VerifiedX + new BigDecimal(1,solutionUnderTest.SolutionGlobals.VerifiedX.Exponent))
                        {
                            solutionToGenerate.Operations.Add(String.Format(CalculationLibrary.OperationCollection["Solution"], x));
                            solutionToGenerate.IsValid = true;
                            tokenSource.Cancel();
                            return x;
                        }
                        return ((x + solutionUnderTest.SolutionGlobals.VerifiedX) / 2);
                    });
                }
            };

            Action[] computationCollection = new Action[1];
            for (int i = 0; i < computationCollection.Length; i++)
            {
                computationCollection[i] = solveForX;
            }

            try
            {
                Parallel.Invoke(executionOptions, computationCollection);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(String.Concat("Exception found: ", ex.Message));
            }
            return solutionToGenerate;
        }

        public static BigInteger ExecuteSolution(Solution solutionToExecute)
        {
            solutionToExecute.ResetGlobals();
            foreach (string op in solutionToExecute.Operations)
            {
                JobCompiler.ExecuteCalculation(op,
                                    CalculationLibrary.AssemblyReferences,
                                    solutionToExecute.SolutionGlobals);
            }

            var result = solutionToExecute.SolutionGlobals.UnknownPlaintext.Truncate(solutionToExecute.TargetFile.ToString().Length);
            result = result + new BigDecimal(1, result.Exponent);
            return result.Mantissa;
        }
    }
}
