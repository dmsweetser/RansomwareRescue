using Newtonsoft.Json;
using RanR.POC.SolutionDefinitions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Threading;

namespace RanR.POC.SolutionManagement
{
    public class SolutionRepository : ISolutionRepository
    {
        public Solution IdentifiedSolution { get; private set; }
        private Dictionary<BigInteger, Solution> SolutionList;
        public bool SolutionFound;
        private readonly object RepoLock = new object();

        public CancellationTokenSource CancelOperationsToken { get; set; }

        public SolutionRepository(CancellationTokenSource token)
        {
            SolutionList = new Dictionary<BigInteger, Solution>();
            CancelOperationsToken = token;
        }

        public void ResetRepository()
        {
            IdentifiedSolution = null;
            SolutionList = new Dictionary<BigInteger, Solution>();
            SolutionFound = false;
        }

        public bool AddSolution(Solution solutionToAdd, BigInteger solutionHash)
        {
            lock (RepoLock)
            {
                if (solutionToAdd.IsValid && SolutionList.TryAdd(solutionHash, solutionToAdd))
                {
                    SolutionFound = true;
                    IdentifiedSolution = solutionToAdd;
                    ExportSolution(solutionToAdd);
                    CancelOperationsToken.Cancel();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool RetrieveSolution(BigInteger hash, out Solution solutionToReturn)
        {
            return SolutionList.TryGetValue(hash, out solutionToReturn);
        }

        public string ExportSolution(Solution solutionToExport, string pathToExport = "")
        {
            if (pathToExport == "")
            {
                pathToExport = System.Environment.CurrentDirectory;
            }

            string exportedFileName = String.Format("{0}Solution_{1:YYYYMMddHHmmss}_{2}_{3}.json"
                , pathToExport, DateTime.Now, solutionToExport.OriginalValues.EncryptedFile.GetHashCode(),
                solutionToExport.OriginalValues.DecryptedFile.GetHashCode());

            File.WriteAllText(exportedFileName, JsonConvert.SerializeObject(solutionToExport));
            return exportedFileName;
        }
    }
}
