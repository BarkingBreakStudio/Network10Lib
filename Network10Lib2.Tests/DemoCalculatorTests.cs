using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Network10Lib;
using Xunit;

namespace Network10Lib2.Tests
{
    public  class DemoCalculatorTests
    {
        [Fact]
        public void Add_SimpleValueSHouldCalculate()
        {
            float expected = 5;

            float actual = DemoCalculator.Add(3,2);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Multiplay_SimpleValueSHouldCalculate()
        {
            float expected = 50;

            float actual = DemoCalculator.Multiplay(5, 10);

            Assert.Equal(expected, actual);
        }

    }
}
