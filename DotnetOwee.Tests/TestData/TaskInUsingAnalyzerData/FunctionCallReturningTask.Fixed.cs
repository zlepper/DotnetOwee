using System;
using System.Threading.Tasks;

namespace DotnetOwee.Tests.TestData.TaskInUsingAnalyzerData
{
    public class FunctionCallReturningTask
    {
        public async Task DoSomething()
        {
            using (await GetThing())
            {
                // Do something here!!
            }
        }

        private static Task<IDisposable> GetThing()
        {
            throw new NotImplementedException();
        }
    }
    
}
