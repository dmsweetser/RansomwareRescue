using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using RanR.POC.Operations;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Reflection;

namespace RanR.POC.Helpers
{
    //Reference https://msdn.microsoft.com/en-us/magazine/mt808499.aspx
    //Reference https://gsferreira.com/archive/2016/02/the-shining-new-csharp-scripting-api/
    public static class JobCompiler
    {
        public static void ExecuteCalculation(string operationToExecute, List<Assembly> assemblyReferences, Globals globals)
        {
            CSharpScript.RunAsync(operationToExecute, 
                ScriptOptions.Default.WithReferences(assemblyReferences),
                globals: globals);
        }
    }
}