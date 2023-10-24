using RanR.POC.SolutionDefinitions;
using System.Numerics;

namespace RanR.POC.SolutionManagement
{
    interface ISolutionRepository
    {
        bool AddSolution(Solution solutionToAdd, BigInteger solutionHash);
        bool RetrieveSolution(BigInteger hash, out Solution solutionToReturn);
    }
}
