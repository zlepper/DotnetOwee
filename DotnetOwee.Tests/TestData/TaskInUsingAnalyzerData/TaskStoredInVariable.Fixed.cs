using System;
using System.Threading.Tasks;

namespace DotnetOwee.Tests.TestData.TaskInUsingAnalyzerData
{
    public class TaskStoredInVariable
    {
        public async Task DoSomething()
        {
            using (var thing = await GetThing())
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
