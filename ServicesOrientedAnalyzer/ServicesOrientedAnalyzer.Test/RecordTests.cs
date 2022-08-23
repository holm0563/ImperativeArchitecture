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
   public int Repeat = 1;
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
        public async Task Record_WithPrivateProperty_Fails()
        {
            var test = @"
public record Echo
{
private string Hi { get; set; }
}
";

            var expected = VerifyCS.Diagnostic("NotPublicInRecord");
            await VerifyCS.VerifyAnalyzerAsync(test, expected);
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

        [TestMethod]
        public async Task Record_WithPrivateMethod_Fails()
        {
            var test = @"
public record Echo
{
private void Hi(){}
}
";

            var expected = VerifyCS.Diagnostic("RecordWithMethod");
            await VerifyCS.VerifyAnalyzerAsync(test, expected);
        }

        [TestMethod]
        public async Task Record_WithConstructor_Works()
        {
            var test = @"
public record Test
{
    public Test(int forced)
    {
        
    }
}
";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        [TestMethod]
        public async Task Record_WithInterfaceContructor_Fails()
        {
            var test = @"
public interface ITest
{
}

public record Test
{
    public Test(ITest forced)
    {
    }
}
";

            var expected = VerifyCS.Diagnostic("InterfaceInRecord");
            await VerifyCS.VerifyAnalyzerAsync(test, expected);
        }
    }
}