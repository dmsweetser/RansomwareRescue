using RanR.POC.Helpers;
using RanR.POC.Logging;
using RanR.POC.SolutionDefinitions;
using RanR.POC.SolutionManagement;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

namespace RanR.POC
{
    class Program
    {
        static void Main(string[] args)
        {
            var initializer = new Initializer();
            var convertedFiles = new List<BigInteger>();

            if (args.Length < 4 && args.Length % 2 != 0)
            {
                FileLogger.Instance.LogError("Insufficient number of files. Please provide pairs of unencrypted/encrypted files for analysis. The first file should be encrypted and the second should be the unencrypted counterpart.", "");
                return;
            }

            foreach (var file in args)
            {
                try
                {
                    convertedFiles.Add(new BigInteger(File.ReadAllBytes(file)));
                }
                catch (FileNotFoundException ex)
                {
                    FileLogger.Instance.LogError("File(s) not found.\r\nPlease check path and try again.", ex.StackTrace);
                    return;
                }
            }

            initializer.AddFiles(convertedFiles);
            
            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;
            var solutionRepo = new SolutionRepository(tokenSource);
            initializer.InitializeSolutionGenerator(tokenSource, solutionRepo);

            Console.WriteLine("Calculation Initialized\r\nPress 'x' to Cancel");
            while (true)
            {
                if (Console.ReadLine() == "x")
                {
                    tokenSource.Cancel();
                    return;
                }
            }
        }
    }
}
