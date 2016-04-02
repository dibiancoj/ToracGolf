using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace UnitTesting
{

    public class CommonUtilities
    {

        // This project can output the Class library as a NuGet Package.
        // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".

        [Fact]
        public void Test()
        {
            Assert.Equal(4, 4);
        }

        [Theory]
        [InlineData(3)]
        [InlineData(5)]
        [InlineData(6)]
        public void MyFirstTheory(int value)
        {
            Assert.True(IsOdd(value));
        }

        bool IsOdd(int value)
        {
            return value % 2 == 1;
        }

    }

}
