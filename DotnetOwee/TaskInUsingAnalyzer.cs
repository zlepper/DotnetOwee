using System;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;

namespace DotnetOwee
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class TaskInUsingAnalyzer : DiagnosticAnalyzer
    {
        public static readonly DiagnosticDescriptor Diag =
            new DiagnosticDescriptor("DOTOW0101", "Task used in using statement",
                "A task is being used as the target of a using statement here. This was probably not intended. You might want to `await` the call task instead?",
                "Usage", DiagnosticSeverity.Warning, true);

        public override void Initialize(AnalysisContext context)
        {
            context.EnableConcurrentExecution();
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);

            context.RegisterOperationAction(AnalyzeSomething, OperationKind.Using, OperationKind.UsingDeclaration);
        }

        private static void AnalyzeSomething(OperationAnalysisContext operationAnalysisContext)
        {
            if (operationAnalysisContext.Operation is IUsingOperation usingOperation)
            {
                if (usingOperation.Resources is IInvocationOperation invocationOperation)
                {
                    var returnType = invocationOperation.Type;
                    if (returnType == null) return;

                    if (IsTask(returnType))
                    {
                        operationAnalysisContext.ReportDiagnostic(Diagnostic.Create(Diag,
                            invocationOperation.Syntax.GetLocation()));
                    }
                }
                else if(usingOperation.Resources is IVariableDeclarationGroupOperation variableDeclarationGroupOperation)
                {
                    foreach (var declaration in variableDeclarationGroupOperation.Declarations)
                    {
                        foreach (var declarator in declaration.Declarators)
                        {
                            if (declarator.Initializer is {} initializer)
                            {
                                if (initializer.Value.Type is {} returnType)
                                {
                                    if (IsTask(returnType))
                                    {
                                        operationAnalysisContext.ReportDiagnostic(Diagnostic.Create(Diag,
                                            initializer.Value.Syntax.GetLocation()));
                                    }
                                }
                            }
                        }
                    }

                    Console.WriteLine(usingOperation);
                }
            }
        }

        private static bool IsTask(ITypeSymbol type)
        {
            return type is
            {
                Name: "Task",
                ContainingNamespace:
                {
                    Name: "Tasks",
                    ContainingNamespace:
                    {
                        Name: "Threading",
                        ContainingNamespace:
                        { Name: "System", ContainingNamespace: { IsGlobalNamespace: true } }
                    }
                }
            };
        }

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Diag);
    }
}
