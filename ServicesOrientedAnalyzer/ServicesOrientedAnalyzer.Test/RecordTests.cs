using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS =
    ServicesOrientedAnalyzer.Test.CSharpAnalyzerVerifier<ServicesOrientedAnalyzer.ServicesOrientedAnalyzerAnalyzer>;

namespace ServicesOrientedAnalyzer.Test
{
    [TestClass]
    public class RecordTests
    {
        [TestMethod]
        public async Task Record_Simple_Works()
        {
            var test = @"
public record Echo
{}
";


            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        [TestMethod]
        public async Task Record_WithField_Works()
        {
            var test = @"
public record Echo
{
   [Range(0, MaxRepeat)] public int Repeat = 1;
}
";


            await VerifyCS.VerifyAnalyzerAsync(test);
        }


        [TestMethod]
        public async Task Record_WithProperty_Works()
        {
            var test = @"
public record Echo
{
public string Hi { get; set; }
}
";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        [TestMethod]
        public async Task Record_WithMethod_Fails()
        {
            var test = @"
public record Echo
{
public void Hi(){}
}
";

            var expected = VerifyCS.Diagnostic("RecordWithMethod");
            await VerifyCS.VerifyAnalyzerAsync(test, expected);
        }

        //with ctors works
        //with private property fails
        //with private field fails
    }
}