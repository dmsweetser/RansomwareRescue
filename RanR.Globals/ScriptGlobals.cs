using System;
using System.Numerics;

namespace RanR.Globals
{
    public class ScriptGlobals
    {
        public BigInteger KnownPlaintext { get; set; }
        public BigInteger KnownCiphertext { get; set; }
        public BigInteger TargetCiphertext { get; set; }
        public BigInteger UnknownPlaintext { get; set; }
    }
}
