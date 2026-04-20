// TFrac.cs - Абстрактный тип данных "Простая дробь"
// Original: Абстрактный тип данных простые дроби

using System;

namespace CalculatorFractions
{
    /// <summary>
    /// Класс простой дроби (числитель/знаменатель)
    /// ADT TFrac - Простая дробь (тип TFrac)
    /// Это пара целых чисел: числитель и знаменатель (a/b)
    /// Простые дроби изменяемые
    /// </summary>
    public class TFrac
    {
        private int FNum;       // Числитель (numerator)
        private int FDenom;     // Знаменатель (denominator)

    /// <summary>
    /// Конструктор по умолчанию: 0/1
    /// </summary>
    public TFrac() : this(0, 1) { }

    /// <summary>
    /// Конструктор с параметрами
    /// </summary>
    public TFrac(int num, int denom = 1)
    {
        FNum = num;
        FDenom = denom;
        Reduce();
    }

        /// <summary>
        /// Копирующий конструктор
        /// </summary>
        public TFrac(TFrac other)
        {
            FNum = other.FNum;
            FDenom = other.FDenom;
        }

        /// <summary>
        /// Сокращение дроби
        /// </summary>
        private void Reduce()
        {
            if (FDenom == 0)
            {
                throw new ArgumentException("Denominator cannot be zero");
            }

            // Знаменатель всегда положительный
            if (FDenom < 0)
            {
                FNum = -FNum;
                FDenom = -FDenom;
            }

            // Сокращение на НОД
            int gcd = Gcd(Math.Abs(FNum), Math.Abs(FDenom));
            if (gcd != 0)
            {
                FNum /= gcd;
                FDenom /= gcd;
            }
        }

        /// <summary>
        /// Вычисление НОД
        /// </summary>
        private int Gcd(int a, int b)
        {
            while (b != 0)
            {
                int temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        // Геттеры
        public int Numerator => FNum;
        public int Denominator => FDenom;

        /// <summary>
        /// Установка значения дроби
        /// </summary>
        public void Set(int num, int denom)
        {
            if (denom == 0)
            {
                throw new ArgumentException("Denominator cannot be zero");
            }
            FNum = num;
            FDenom = denom;
            Reduce();
        }

        /// <summary>
        /// Проверка на ноль (0/1)
        /// </summary>
        public bool IsZero() => FNum == 0;

        /// <summary>
        /// Сложение
        /// </summary>
        public TFrac Add(TFrac other)
        {
            int num = FNum * other.FDenom + other.FNum * FDenom;
            int denom = FDenom * other.FDenom;
            return new TFrac(num, denom);
        }

        /// <summary>
        /// Вычитание
        /// </summary>
        public TFrac Sub(TFrac other)
        {
            int num = FNum * other.FDenom - other.FNum * FDenom;
            int denom = FDenom * other.FDenom;
            return new TFrac(num, denom);
        }

        /// <summary>
        /// Умножение
        /// </summary>
        public TFrac Mul(TFrac other)
        {
            int num = FNum * other.FNum;
            int denom = FDenom * other.FDenom;
            return new TFrac(num, denom);
        }

        /// <summary>
        /// Деление
        /// </summary>
        public TFrac Div(TFrac other)
        {
            if (other.FNum == 0)
            {
                throw new ArgumentException("Division by zero fraction");
            }
            int num = FNum * other.FDenom;
            int denom = FDenom * other.FNum;
            return new TFrac(num, denom);
        }

        /// <summary>
        /// Возведение в квадрат (Sqr)
        /// </summary>
        public TFrac Sqr()
        {
            return new TFrac(FNum * FNum, FDenom * FDenom);
        }

        /// <summary>
        /// Обратное значение (Rev) - 1/x
        /// </summary>
        public TFrac Rev()
        {
            if (FNum == 0)
            {
                throw new ArgumentException("Cannot reverse zero fraction");
            }
            return new TFrac(FDenom, FNum);
        }

        /// <summary>
        /// Преобразование в строку
        /// </summary>
        public override string ToString()
        {
            return $"{FNum}/{FDenom}";
        }

        /// <summary>
        /// Преобразование в десятичное число
        /// </summary>
        public double ToDouble()
        {
            return (double)FNum / (double)FDenom;
        }

        /// <summary>
        /// Оператор равенства
        /// </summary>
        public override bool Equals(object obj)
        {
            if (obj is TFrac other)
            {
                return FNum == other.FNum && FDenom == other.FDenom;
            }
            return false;
        }

        /// <summary>
        /// Оператор неравенства
        /// </summary>
        public override int GetHashCode()
        {
            return FNum.GetHashCode() ^ FDenom.GetHashCode();
        }
    }
}
