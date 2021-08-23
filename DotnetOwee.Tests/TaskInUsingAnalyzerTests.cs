using System.IO;
using System.Threading.Tasks;
using JetBrains.Annotations;
using NUnit.Framework;
using Verify = Microsoft.CodeAnalysis.CSharp.Testing.NUnit.CodeFixVerifier<DotnetOwee.TaskInUsingAnalyzer, DotnetOwee.TaskInUsingCodeFixProvider>;
using static Microsoft.CodeAnalysis.Testing.DiagnosticResult;

namespace DotnetOwee.Tests
{
    [TestFixture]
    public class TaskInUsingAnalyzerTests
    {
        [Test]
        public async Task FunctionCallReturningTask()
        {
            var testcode = GetSource("TestData/TaskInUsingAnalyzerData/FunctionCallReturningTask.Broken.cs");
            var expected = Verify.Diagnostic(TaskInUsingAnalyzer.Diag.Id).WithLocation(10, 20);
            var correct = GetSource("TestData/TaskInUsingAnalyzerData/FunctionCallReturningTask.Fixed.cs");
            await Verify.VerifyCodeFixAsync(testcode, expected, correct);
        }
        
        [Test]
        public async Task TaskStoredInVariable()
        {
            var testcode = GetSource("TestData/TaskInUsingAnalyzerData/TaskStoredInVariable.Broken.cs");
            var expected = Verify.Diagnostic(TaskInUsingAnalyzer.Diag.Id).WithLocation(10, 32);
            var correct = GetSource("TestData/TaskInUsingAnalyzerData/TaskStoredInVariable.Fixed.cs");
            await Verify.VerifyCodeFixAsync(testcode, expected, correct);
        }

        private string GetSource([PathReference] string path)
        {
            return File.ReadAllText(path);
        }
    }
}
