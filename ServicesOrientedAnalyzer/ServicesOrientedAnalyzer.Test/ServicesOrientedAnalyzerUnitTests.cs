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
        public async Task Class_WithField_Throws()
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
        public async Task Class_WithProperty_Throws()
        {
            var test = @"
public class Echo
{
    public string Message { get; set; } = """";
}";

            var expected = VerifyCS.Diagnostic("ClassWithData")
                .WithSpan(2, 14, 2, 18)
                .WithArguments("Echo", "Message");

            await VerifyCS.VerifyAnalyzerAsync(test, expected);
        }

        [TestMethod]
        public async Task Class_WithStatic_Throws()
        {
            var test = @"
public static class Echo3
{
}";

            var expected = VerifyCS.Diagnostic("DerivedClasses")
                .WithSpan(2, 21, 2, 26)
                .WithArguments("Echo3");

            await VerifyCS.VerifyAnalyzerAsync(test, expected);
        }

        [TestMethod]
        public async Task Class_WithStaticMethod_Throws()
        {
            var test = @"
public class Echo3
{
    public static void Message(){}
}";

            var expected = VerifyCS.Diagnostic("DerivedClassesMethod")
                .WithSpan(2, 14, 2, 19)
                .WithArguments("Echo3", "Message");

            await VerifyCS.VerifyAnalyzerAsync(test, expected);
        }

        [TestMethod]
        public async Task Class_WithVirtualMethod_Throws()
        {
            var test = @"
public class Echo3
{
    public virtual void Message(){}
}";

            var expected = VerifyCS.Diagnostic("DerivedClassesMethod")
                .WithSpan(2, 14, 2, 19)
                .WithArguments("Echo3", "Message");
            await VerifyCS.VerifyAnalyzerAsync(test, expected);
        }

        [TestMethod]
        public async Task Class_WhenAbstract_Throws()
        {
            var test = @"
public abstract class Echo3
{}";

            var expected = VerifyCS.Diagnostic("DerivedClasses")
                .WithSpan(2, 23, 2, 28)
                .WithArguments("Echo3");

            await VerifyCS.VerifyAnalyzerAsync(test, expected);
        }

        // public async Class_Simple_DoesntThrow 
    }
}