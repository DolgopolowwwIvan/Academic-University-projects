using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;

namespace Converter_p1_p2
{

    public class Editor
    {
        private string number = ""; // Поле для хранения редактируемого числа

        private const string delim = "."; // Разделитель целой и дробной частей

        private const string zero = "0";

        private int p; // Основание системы счисления

        public Editor(int p)
        {
            if (p < 2 || p > 16)
                throw new ArgumentException("Основание системы счисления должно быть от 2 до 16");

            this.p = p;
        }

        public string Number
        {
            get { return number; }
        }

        public string AddDigit(int n)
        {
            if (n < 0 || n >= p)
                throw new ArgumentException($"Цифра должна быть от 0 до {p - 1}");

            char digit = Conver_10_p.int_to_Char(n);
            number += digit;
            return number;
        }
        public string AddZero()
        {
            number += zero;
            return number;
        }
        public string AddDelim()
        {
            // Проверяем, есть ли уже разделитель
            if (!number.Contains(delim))
            {
                // Если число пустое или уже есть разделитель в начале, добавляем 0 перед разделителем
                if (string.IsNullOrEmpty(number) || number == "-")
                    number += zero;

                number += delim;
            }
            return number;
        }

        // Удалить символ справа 
        public string Bs()
        {
            if (number.Length > 0)
            {
                number = number.Substring(0, number.Length - 1);

                // Если после удаления остался только минус, удаляем и его
                if (number == "-")
                    number = "";
            }
            return number;
        }
        public string Clear()
        {
            number = "";
            return number;
        }

        /// Получить количество знаков после разделителя
        public int Acc()
        {
            int dotIndex = number.IndexOf(delim);
            if (dotIndex == -1)
                return 0;

            return number.Length - dotIndex - 1;
        }
        public string DoEdit(int j)
        {
            switch (j)
            {
                case 0:
                    return AddDigit(0);
                case 1:
                    return AddDigit(1);
                case 2:
                    return AddDigit(2);
                case 3:
                    return AddDigit(3);
                case 4:
                    return AddDigit(4);
                case 5:
                    return AddDigit(5);
                case 6:
                    return AddDigit(6);
                case 7:
                    return AddDigit(7);
                case 8:
                    return AddDigit(8);
                case 9:
                    return AddDigit(9);
                case 10:
                    return AddDigit(10);
                case 11:
                    return AddDigit(11);
                case 12:
                    return AddDigit(12);
                case 13:
                    return AddDigit(13);
                case 14:
                    return AddDigit(14);
                case 15:
                    return AddDigit(15);
                case 16:
                    return AddZero();
                case 17:
                    return AddDelim();
                case 18:
                    return Bs();
                case 19:
                    return Clear();
                default:
                    return number;
            }
        }
    }
}
