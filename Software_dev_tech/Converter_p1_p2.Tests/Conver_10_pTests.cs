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
        [Fact]
        public void DoMethodTest()
        {
            string result = "-11.E";

            string testCase = Conver_10_p.Do(-17.875,16,3);

            Assert.Equal(result, testCase);

        }

        [Fact]
        public void IntToCharMethodTest()
        {
            char result = 'E';

            char testCase = Conver_10_p.int_to_Char(14);

            Assert.Equal(result, testCase);

        }

        [Fact]
        public void IntToPMethodTest()
        {
            string result = "A1";

            string testCase = Conver_10_p.int_to_P(161, 16);

            Assert.Equal(result, testCase);

        }

        [Fact]
        public void FltToPMethodTest()
        {
            string result = "1111";

            string testCase = Conver_10_p.flt_to_P(0.9375, 2, 4);

            Assert.Equal(result, testCase);

        }
    }
}
