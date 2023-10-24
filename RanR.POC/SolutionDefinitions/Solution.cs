using RanR.POC.Helpers;
using RanR.POC.Operations;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace RanR.POC.SolutionDefinitions
{
    [Serializable]
    public class Solution
    {
        public List<string> Operations { get; private set; }
        public readonly (BigInteger EncryptedFile, BigInteger DecryptedFile) OriginalValues;
        public readonly BigInteger TargetFile;
        public Globals SolutionGlobals { get; set; }
        public bool IsValid { get; set; }

        public Solution() { }

        public Solution((BigInteger EncryptedFile, BigInteger DecryptedFile) original, BigInteger targetFile)
        {
            OriginalValues = original;
            Operations = new List<string>();
            TargetFile = targetFile;
            SolutionGlobals = new Globals();
            ResetGlobals();
        }

        public void ResetGlobals()
        {
            SolutionGlobals.KnownPlaintext = new BigDecimal(OriginalValues.DecryptedFile, 0);
            SolutionGlobals.KnownCiphertext = new BigDecimal(OriginalValues.EncryptedFile, 0);
            SolutionGlobals.TargetCiphertext = new BigDecimal(TargetFile, 0);
            SolutionGlobals.UnknownPlaintext = 0;
            SolutionGlobals.VerifiedX = 0;
        }

        public void AddOperationSequence(List<string> opListToAdd)
        {
            Operations.AddRange(opListToAdd);
        }
    }
}