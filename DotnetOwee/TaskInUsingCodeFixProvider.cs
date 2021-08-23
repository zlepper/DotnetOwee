using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DotnetOwee
{
    public class TaskInUsingCodeFixProvider : CodeFixProvider
    {

        public override FixAllProvider GetFixAllProvider()
        {
            return WellKnownFixAllProviders.BatchFixer;
        }

        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(TaskInUsingAnalyzer.Diag.Id);
        
        public override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

            if (root == null) return;

            var diagnostic = context.Diagnostics.First();

            context.RegisterCodeFix(CodeAction.Create(
                title: "Await task",
                equivalenceKey: "Await task",
                createChangedDocument: _ => AwaitTask(context.Document, diagnostic, root)
            ), diagnostic);
            
        }

        private static Task<Document> AwaitTask(Document document, Diagnostic diagnostic, SyntaxNode root)
        {
            var diagnosticSpan = diagnostic.Location.SourceSpan;

            var expression = root.FindNode(diagnosticSpan).FirstAncestorOrSelf<ExpressionSyntax>();
            if (expression == null)
            {
                return Task.FromResult(document);
            }
            
            var newRoot = root.ReplaceNode(expression, SyntaxFactory.AwaitExpression(expression.WithLeadingTrivia(SyntaxFactory.Space)));

            return Task.FromResult(document.WithSyntaxRoot(newRoot));
        }

    }
}
