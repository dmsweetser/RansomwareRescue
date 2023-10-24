using RanR.POC.Logging;
using RanR.POC.SolutionDefinitions;
using RanR.POC.SolutionManagement;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

namespace RanR.POC.Helpers
{
    public class Initializer
    {
        public List<(BigInteger EncryptedFile, BigInteger DecryptedFile)> PairsToSolve { get; set; }
        public BigInteger TargetFile;
        public ILogger OperationLogger { get; set; }
        public string InitialValueForX { get; set; }

        public Initializer()
        {
            PairsToSolve = new List<(BigInteger EncryptedFile, BigInteger DecryptedFile)>();
            OperationLogger = FileLogger.Instance;
            InitialValueForX = "1E0";
        }

        public void AddFiles(List<BigInteger> fileList)
        {
            if (fileList.Count % 2 != 1)
            {
                OperationLogger.LogError("Insufficient number of files present", Environment.StackTrace);
            }
            for (int i = 0; i < fileList.Count - 1; i += 2)
            {
                var filePair = (EncryptedFile: fileList[i], DecryptedFile: fileList[i + 1]);
                PairsToSolve.Add((filePair));
            }
            TargetFile = fileList[fileList.Count - 1];

        }

        public void InitializeSolutionGenerator(CancellationTokenSource tokenSource, SolutionRepository repo)
        {
            foreach (var pair in PairsToSolve)
            {
                Task.Factory.StartNew(() =>
                {
                    while (!repo.SolutionFound)
                    {
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                        GC.Collect();
                        Solution solutionToGenerate = new Solution(pair, TargetFile);
                        SolutionGenerator.GenerateSolution(repo, solutionToGenerate, tokenSource, OperationLogger, initialValueForX: InitialValueForX);
                        repo.AddSolution(solutionToGenerate, SolutionGenerator.ExecuteSolution(solutionToGenerate));
                    };
                });
            }
        }
    }
}
