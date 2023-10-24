using RanR.POC.Helpers;
using System.Collections.Generic;
using System.Numerics;
using System.Reflection;

namespace RanR.POC.Operations
{
    public static class CalculationLibrary
    {
        public static Dictionary<string, string> OperationCollection = new Dictionary<string, string>
        {
            //2018-06-21 NO GOOD
            //{ "ReduceByModulusOfConstant", "A = A % System.Numerics.BigInteger.Parse(\"{0}\");" },
            //{ "ReduceByModulusOfDifferenceBetweenConstantAndGlobalA", "A = A % System.Numerics.BigInteger.Abs(A - System.Numerics.BigInteger.Parse(\"{0}\"));" },
            //{ "MultiplyByConstant", "A = A * System.Numerics.BigInteger.Parse(\"{0}\");" },
            //{ "SubtractConstant", "A = A - System.Numerics.BigInteger.Parse(\"{0}\");" },
            //{ "AddConstant", "A = A + System.Numerics.BigInteger.Parse(\"{0}\");" }

            //2018-06-24 NO GOOD
            //{ "Solution_20180621", "UnknownPlaintext = (KnownPlaintext * System.Numerics.BigInteger.Parse(\"{0}\") * TargetCiphertext) / KnownCiphertext;" },
            //{ "Verification_20180621", "TargetCiphertext = (UnknownPlaintext * KnownCiphertext) / (KnownPlaintext * System.Numerics.BigInteger.Parse(\"{0}\"));" }

            //2018-06-25 NO GOOD
            //{ "Solution_20180624", "UnknownPlaintext = (KnownPlaintext * System.Numerics.BigInteger.Parse(\"{0}\") * TargetCiphertext) / KnownCiphertext;" },
            //{ "Verification_20180624", "KnownPlaintext = (UnknownPlaintext * KnownCiphertext) / (TargetCiphertext * System.Numerics.BigInteger.Parse(\"{0}\"));" }

            //2018-06-28 NO GOOD
            //{ "Solution_20180628", "UnknownPlaintext = (KnownPlaintext * RanR.POC.Helpers.BigDecimal.Parse(\"{0}\") * TargetCiphertext) / KnownCiphertext;" },
            //{ "Verification_20180628", "KnownPlaintext = (UnknownPlaintext * KnownCiphertext) / (TargetCiphertext * RanR.POC.Helpers.BigDecimal.Parse(\"{0}\"));" }

            { "Solution", "UnknownPlaintext = (KnownPlaintext * RanR.POC.Helpers.BigDecimal.Parse(\"{0}\") * TargetCiphertext) / KnownCiphertext;" },
            { "VerificationOfX", "VerifiedX = (UnknownPlaintext * KnownCiphertext) / (TargetCiphertext * KnownPlaintext);" },
            { "VerificationOfKnownPlaintext", "KnownPlaintext = (UnknownPlaintext * KnownCiphertext) / (TargetCiphertext * RanR.POC.Helpers.BigDecimal.Parse(\"{0}\"));" }

        };

        public static List<Assembly> AssemblyReferences = new List<Assembly>
            {
                typeof(BigDecimal).Assembly
            };
    }
}
