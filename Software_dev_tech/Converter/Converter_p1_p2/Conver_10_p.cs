using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Converter_p1_p2
{
    /// <summary>
    /// Целевые системы счисления от 2 до 16
    /// </summary>
    public static class Conver_10_p
    {
        //Преобразовать целое в символ.
        public static char int_to_Char(int n) {
            if (n >= 0 && n <= 9)
                return (char)(n + '0');
            else if (n >= 10 && n <= 15)
            {
                return(char)(n - 10 + 'A');
            }
            else
            {
                throw new ArgumentException("Поддерживаются только значения от 0 до 15");
            }
       
        }

        //Преобразовать десятичное целое в с.сч. с основанием р.
        public static string int_to_P(int n, int p) {
            string result = "";
            while (n > 0)
            {
                int reminder = n % p;
                result = int_to_Char(reminder) + result;
                n /= p;
            }

            return result;
        }
        //Преобразовать десятичную дробь в с.сч. с основанием р.
        public static string flt_to_P(double n, int p, int c) {
            double abs = Math.Abs(n);

            string result = "";

            for (int i = 0; i < c; i++)
            {
                abs *= p;

                int digit = (int)Math.Truncate(abs);
                result += int_to_Char(digit);

                abs -= digit;

                if(abs == 0)
                {
                    break;
                }
            }

            return result;
        }
        //Преобразовать десятичное 
        //действительное число в с.сч. с осн. р.
        public static string Do(double n, int p, int c) {
            bool isNegative = n < 0;
            n = Math.Abs(n);

            string result = "";
           
            int integerPart = (int)Math.Truncate(n);
            double fractionalPart = n - integerPart;

            result += int_to_P(integerPart, p) + "." + flt_to_P(fractionalPart, p, c);

            if (isNegative)
            {
                result = "-" + result;
            }

            return result;
        }   

    }
}

