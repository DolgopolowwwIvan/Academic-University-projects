using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;

namespace Converter_p1_p2
{
    /// <summary>
    /// Редактор действительных чисел в системе счисления с основанием p (2..16)
    /// </summary>
    public class Editor
    {
        // Поле для хранения редактируемого числа
        private string number = "";

        // Разделитель целой и дробной частей
        private const string delim = ".";

        // Ноль
        private const string zero = "0";

        // Основание системы счисления
        private int p;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="p">Основание системы счисления (2..16)</param>
        public Editor(int p)
        {
            if (p < 2 || p > 16)
                throw new ArgumentException("Основание системы счисления должно быть от 2 до 16");

            this.p = p;
        }

        /// <summary>
        /// Свойство для чтения редактируемого числа
        /// </summary>
        public string Number
        {
            get { return number; }
        }

        /// <summary>
        /// Добавить цифру
        /// </summary>
        /// <param name="n">Цифра от 0 до p-1</param>
        /// <returns>Текущее значение числа</returns>
        public string AddDigit(int n)
        {
            if (n < 0 || n >= p)
                throw new ArgumentException($"Цифра должна быть от 0 до {p - 1}");

            char digit = Conver_10_p.int_to_Char(n);
            number += digit;
            return number;
        }

        /// <summary>
        /// Добавить ноль
        /// </summary>
        /// <returns>Текущее значение числа</returns>
        public string AddZero()
        {
            number += zero;
            return number;
        }

        /// <summary>
        /// Добавить разделитель
        /// </summary>
        /// <returns>Текущее значение числа</returns>
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

        /// <summary>
        /// Удалить символ справа (забой)
        /// </summary>
        /// <returns>Текущее значение числа</returns>
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

        /// <summary>
        /// Очистить редактируемое число
        /// </summary>
        /// <returns>Текущее значение числа</returns>
        public string Clear()
        {
            number = "";
            return number;
        }

        /// <summary>
        /// Получить точность представления (количество знаков после разделителя)
        /// </summary>
        /// <returns>Количество знаков после запятой</returns>
        public int Acc()
        {
            int dotIndex = number.IndexOf(delim);
            if (dotIndex == -1)
                return 0;

            return number.Length - dotIndex - 1;
        }

        /// <summary>
        /// Выполнить команду редактирования
        /// </summary>
        /// <param name="j">Номер команды</param>
        /// <returns>Текущее значение числа</returns>
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
