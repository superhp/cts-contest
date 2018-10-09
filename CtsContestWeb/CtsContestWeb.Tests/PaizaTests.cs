using System.IO;
using System.Threading.Tasks;
using CtsContestWeb.Communication;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CtsContestWeb.Tests
{
    [TestClass]
    public class PaizaTests
    {
        [TestMethod]
        public async Task ShouldSolveProblem()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var configuration = builder.Build();

            var compiler = new PaizaCompiler(configuration);

            var result = await compiler.Compile(TaskData.GetTestTask(), "print 2", 5);

            Assert.AreEqual(true, result.Compiled);
            Assert.AreEqual(true, result.ResultCorrect);
        }

        [TestMethod]
        public async Task ShouldNotSolveProblem()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var configuration = builder.Build();

            var compiler = new PaizaCompiler(configuration);

            var result = await compiler.Compile(TaskData.GetTestTask(), "print 5", 5);

            Assert.AreEqual(true, result.Compiled);
            Assert.AreEqual(false, result.ResultCorrect);
        }
    }
}