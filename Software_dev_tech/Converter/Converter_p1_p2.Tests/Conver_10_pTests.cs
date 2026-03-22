using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Converter_p1_p2;

namespace Converter_p1_p2.Tests
{
    public class Conver_10_pTests
    {

        [Theory]
        [InlineData(-17.875, 16, 3, "-11.E")]      
        [InlineData(165.875, 16, 3, "A5.E")]     
        [InlineData(10.625, 2, 6, "1010.101")]     
        public void DoMethodTest(double number, int baseNum, int precision, string expected)
        {
            string actual = Conver_10_p.Do(number, baseNum, precision);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0, '0')]    
        [InlineData(9, '9')]     
        [InlineData(14, 'E')]    
        public void IntToCharMethodTest(int digit, char expected)
        {
            char actual = Conver_10_p.int_to_Char(digit);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(161, 16, "A1")]    
        [InlineData(10, 2, "1010")]     
        [InlineData(63, 8, "77")]      
        public void IntToPMethodTest(int integer, int baseNum, string expected)
        {
            string actual = Conver_10_p.int_to_P(integer, baseNum);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0.9375, 2, 4, "1111")]    
        [InlineData(0.875, 16, 3, "E")]      
        [InlineData(0.75, 8, 3, "6")]        
        public void FltToPMethodTest(double fraction, int baseNum, int precision, string expected)
        {
            string actual = Conver_10_p.flt_to_P(fraction, baseNum, precision);
            Assert.Equal(expected, actual);
        }

    }
}
