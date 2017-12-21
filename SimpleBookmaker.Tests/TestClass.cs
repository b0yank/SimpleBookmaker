namespace SimpleBookmaker.Tests
{
    using Microsoft.AspNetCore.Mvc;

    public class TestClass
    {
        protected const int invalidModelStateTestValue = 666;
        protected const int invalidIdTestValue = -1;
        protected const int validIdTestValue = 1;

        protected TestClass()
        {
            Tests.Initialize();
        }
    }
}
