using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS =
    ServicesOrientedAnalyzer.Test.CSharpAnalyzerVerifier<ServicesOrientedAnalyzer.ServicesOrientedAnalyzerAnalyzer>;

namespace ServicesOrientedAnalyzer.Test
{
    [TestClass]
    public class InterfaceTests
    {
        [TestMethod]
        public async Task Interface_NotDefined_Warns()
        {
            var test = @"
interface ITest
{   
}
";

            var expected = VerifyCS.Diagnostic("NotPublic")
                .WithSeverity(DiagnosticSeverity.Warning);
            await VerifyCS.VerifyAnalyzerAsync(test, expected);
        }

        [TestMethod]
        public async Task Interface_Simple_Works()
        {
            var test = @"
public interface ITest
{   
}
";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }
    }
}