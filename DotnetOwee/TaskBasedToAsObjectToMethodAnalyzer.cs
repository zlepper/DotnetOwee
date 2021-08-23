using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DotnetOwee
{
    public class TaskBasedToAsObjectToMethod : DiagnosticAnalyzer
    {
        public override void Initialize(AnalysisContext context)
        {
            throw new System.NotImplementedException();
        }

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; }
    }
}
