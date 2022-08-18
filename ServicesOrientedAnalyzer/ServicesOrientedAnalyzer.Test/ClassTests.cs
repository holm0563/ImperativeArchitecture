using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS =
    ServicesOrientedAnalyzer.Test.CSharpAnalyzerVerifier<ServicesOrientedAnalyzer.ServicesOrientedAnalyzerAnalyzer>;

namespace ServicesOrientedAnalyzer.Test
{
    [TestClass]
    public class ClassTests
    {
        [TestMethod]
        public async Task Class_Simple_DoesntThrow()
        {
            var test = @"
public interface ITest
{
    void Echo();
}

public class Test:ITest
{
    public void Echo()
    {
    }
}";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        [TestMethod]
        public async Task Class_WithDI_DoesntThrow()
        {
            var test = @"
public interface ITest
{
    void Echo();
}

public class Test : ITest
{
    private readonly ITest _originalService;

    public void Echo()
    {
    }

    public Test(ITest originalService)
    {
        _originalService = originalService;
    }
}}";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        [TestMethod]
        public async Task Class_MissingMethodInterface_Throws()
        {
            var test = @"
public interface ITest
{
}

public class Test:ITest
{
    public void Echo()
    {
    }
}";
            var expected = VerifyCS.Diagnostic("ClassMethodMissingInterface");
            await VerifyCS.VerifyAnalyzerAsync(test, expected);
        }

        [TestMethod]
        public async Task Class_WithField_Throws()
        {
            var test = @"
public interface EchoService{}
public class Echo: EchoService
{
    public int? _repeat;
}";

            var expected = VerifyCS.Diagnostic("ClassWithData");
            await VerifyCS.VerifyAnalyzerAsync(test, expected);
        }

        [TestMethod]
        public async Task Class_WithProperty_Throws()
        {
            var test = @"
public interface EchoService{}
public class Echo: EchoService
{
    public string Message { get; set; } = """";
}";

            var expected = VerifyCS.Diagnostic("ClassWithData")
                .WithArguments("Echo", "Message");

            await VerifyCS.VerifyAnalyzerAsync(test, expected);
        }

        [TestMethod]
        public async Task Class_WithStaticMethod_Throws()
        {
            var test = @"
public interface EchoService{}
public class Echo3: EchoService
{
    public static void Message(){}
}";

            var expected = VerifyCS.Diagnostic("ClassMethodMissingInterface")
                .WithArguments("Echo3", "Message");

            await VerifyCS.VerifyAnalyzerAsync(test, expected);
        }

        [TestMethod]
        public async Task Class_WithVirtualMethod_Throws()
        {
            var test = @"
public interface EchoService{}
public class Echo: EchoService
{
    public virtual void Message(){}
}";

            var expected = VerifyCS.Diagnostic("ClassMethodMissingInterface");

            await VerifyCS.VerifyAnalyzerAsync(test, expected);
        }

        [TestMethod]
        public async Task Class_WhenAbstract_Throws()
        {
            var test = @"
public interface EchoService{}
public abstract class Echo3: EchoService
{}";

            var expected = VerifyCS.Diagnostic("DerivedClasses");

            await VerifyCS.VerifyAnalyzerAsync(test, expected);
        }

        [TestMethod]
        public async Task Class_WithNonDIConstructor_Fails()
        {
            var test = @"
public interface ITest
{
}

public class Test: ITest
{
    public Test(int echo)
    {
    }
}";

            var expected = VerifyCS.Diagnostic("ConstructorNotDI");

            await VerifyCS.VerifyAnalyzerAsync(test, expected);
        }

        [TestMethod]
        public async Task Class_WithMultipleConstructor_Fails()
        {
            var test = @"
public interface ITest
{
}

public class Test: ITest
{
    public Test(ITest echo)
    {
    }

    public Test(ITest echo, ITest echo2)
    {
    }
}";

            var expected = VerifyCS.Diagnostic("ConstructorNotDI");

            await VerifyCS.VerifyAnalyzerAsync(test, expected);
        }
    }
}