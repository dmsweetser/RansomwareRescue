using RanR.POC.Helpers;

namespace RanR.POC.Operations
{
    public class Globals
    {
        public BigDecimal KnownPlaintext { get; set; }
        public BigDecimal KnownCiphertext { get; set; }
        public BigDecimal TargetCiphertext { get; set; }
        public BigDecimal UnknownPlaintext { get; set; }
        public BigDecimal VerifiedX { get; set; }
    }
}