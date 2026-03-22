using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Converter_p1_p2
{
    public class Conver_p_10
    {
        /// <summary>
        /// Целевые системы счисления от 2 до 16
        /// </summary>

        //Преобразовать цифру в число.
        public static double char_To_num(char ch) {
            ch = char.ToUpper(ch);

            if (ch >= '0' && ch <= '9')
            {
                return ch - '0';
            }
            else if (ch >= 'A' && ch <= 'F')
            {
                return ch - 'A' + 10;
            }
            else
            {
                throw new ArgumentException($"Недопустимый символ");
            }
                
        }
        //Преобразовать строку в число
        private static double convert(string P_num, int P, double weight) {
            double  result = 0;

            for (int i = 0; i < P_num.Length; i++)
            {
                double digit = char_To_num(P_num[i]);

                result += digit * weight;

                weight /= P;
            }

            return result;
        }

        //Преобразовать из с.сч. с основанием р 
        //в с.сч. с основанием 10.
        public static double dval(string P_num, int P) {
            if(P < 2 || P > 16)
            {
                throw new ArgumentOutOfRangeException("Система счисления не поддерживается");
            }

            if (string.IsNullOrEmpty(P_num))
            {
                throw new ArgumentException(P_num);
            }

            bool isNegative = false;
            string integerPart = P_num.Trim();

            if (integerPart[0].Equals('-'))
            {
                isNegative = true;
                integerPart = integerPart.Substring(1);
            }
            else if (integerPart[0] == '+')
            {
                integerPart = integerPart.Substring(1);
            }

            string[] parts = integerPart.Split('.');
            string numberPart = parts[0];
            string fractionalPart = "";

            if (parts.Length > 1)
            {
                fractionalPart = parts[1];
            }

            double integerValue = 0;
            if (!string.IsNullOrEmpty(integerPart))
            {
                double weight = Math.Pow(P, numberPart.Length - 1);
                integerValue = convert(numberPart, P ,weight);
            }

            double fractionalValue = 0;
            if (!string.IsNullOrEmpty(fractionalPart))
            {
                double weight = 1.0 / P;
                fractionalValue = convert(fractionalPart, P ,weight);  
            }

            double result = integerValue + fractionalValue;

            if (isNegative)
            {
                result = -result;
            }

            return result;
        }
    }
}