using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS =
    ServicesOrientedAnalyzer.Test.CSharpAnalyzerVerifier<ServicesOrientedAnalyzer.ServicesOrientedAnalyzerAnalyzer>;

namespace ServicesOrientedAnalyzer.Test
{
    [TestClass]
    public class ServicesOrientedAnalyzerUnitTest
    {
        [TestMethod]
        public async Task Test_ClassWithField_Throws()
        {
            var test = @"
public class Echo
{
    private int? _repeat;
}";

            var expected = VerifyCS.Diagnostic("ClassWithData")
                .WithSpan(2, 14, 2, 18)
                .WithArguments("Echo", "_repeat");
            await VerifyCS.VerifyAnalyzerAsync(test, expected);
        }

        [TestMethod]
        public async Task Test_ClassWithProperty_Throws()
        {
            var test = @"
public class Echo
{
    public virtual string Message { get; set; } = """";
}";

            var expected = VerifyCS.Diagnostic("ClassWithData")
                .WithSpan(2, 14, 2, 18)
                .WithArguments("Echo", "Message");
            var expected2 = VerifyCS.Diagnostic("DerivedClasses")
                .WithSpan(2, 14, 2, 18)
                .WithArguments("Echo", "Message");
            await VerifyCS.VerifyAnalyzerAsync(test, expected, expected2);
        }
    }
}