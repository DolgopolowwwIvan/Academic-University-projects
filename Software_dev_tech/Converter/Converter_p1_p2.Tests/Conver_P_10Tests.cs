using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Converter_p1_p2;

namespace Converter_p1_p2.Tests
{
    public class Conver_p_10Tests
    {
        [Fact]
        public void char_To_num_Test()
        {
            double result = 10;

            double testCase = Conver_p_10.char_To_num('A');

            Assert.Equal(result, testCase);

        }

        [Theory]
        [InlineData("-A5.E", 16, -165.875)]
        [InlineData("A5.E", 16, 165.875)]
        [InlineData("+A5.E", 16, 165.875)]
        [InlineData("1010.101", 2, 10.625)]
        [InlineData("-1010.101", 2, -10.625)]
        [InlineData("77.7", 8, 63.875)]
        [InlineData("FF", 16, 255.0)]
        [InlineData(".F", 16, 0.9375)]
        [InlineData("001010.1010", 2, 10.625)]
        [InlineData("  A5.E  ", 16, 165.875)]
        [InlineData("1111.1111", 2, 15.9375)]
        [InlineData("FF.FF", 16, 255.99609375)]
        [InlineData("A.01", 16, 10.00390625)]
        [InlineData("0", 16, 0.0)]
        [InlineData("-0", 16, 0.0)]
        [InlineData("110.1", 3, 12.3333333333)]
        [InlineData("132.2", 5, 42.4)]
        [InlineData("12A.9", 12, 178.75)]
        public void dval_Test_VariousInputs(string input, int baseNum, double expected)
        {

            double actual = Conver_p_10.dval(input, baseNum);
    
            Assert.Equal(expected, actual, 5);
        }

    }
}
