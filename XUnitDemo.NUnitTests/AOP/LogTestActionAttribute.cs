using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace XUnitDemo.NUnitTests.AOP
{
    public class LogTestActionAttribute : TestActionAttribute
    {
        public override void BeforeTest(ITest test)
        {
            System.Diagnostics.Debug.WriteLine($"{nameof(BeforeTest)}-ClassName:{test.ClassName};Fixture:{test.Fixture};FullName:{test.FullName};MethodName:{test.MethodName};Name:{test.Name}");
            base.BeforeTest(test);
        }

        public override void AfterTest(ITest test)
        {
            System.Diagnostics.Debug.WriteLine($"{nameof(AfterTest)}-ClassName:{test.ClassName};Fixture:{test.Fixture};FullName:{test.FullName};MethodName:{test.MethodName};Name:{test.Name}");
            base.AfterTest(test);
        }
    }
}
