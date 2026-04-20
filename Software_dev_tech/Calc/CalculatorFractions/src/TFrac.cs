using System;

namespace CalculatorFractions
{
    // Класс простой дроби (числитель/знаменатель)
    // ADT TFrac - Простая дробь (тип TFrac)
    // Это пара целых чисел: числитель и знаменатель (a/b)
    // Простые дроби изменяемые
    public class TFrac
    {
        private int FNum;       // Числитель (numerator)
        private int FDenom;     // Знаменатель (denominator)
        private bool FShowAsFraction;  // Режим отображения: "дробь" или "число"

    // Конструктор по умолчанию: 0/1
    public TFrac() : this(0, 1) { }

    // Конструктор с параметрами
    public TFrac(int num, int denom = 1)
    {
        FNum = num;
        FDenom = denom;
        FShowAsFraction = false;  // по умолчанию режим "число"
        Reduce();
    }

        // Копирующий конструктор
        public TFrac(TFrac other)
        {
            FNum = other.FNum;
            FDenom = other.FDenom;
            FShowAsFraction = other.FShowAsFraction;
        }

        // Сокращение дроби
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

        // Вычисление НОД
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

        // Режим отображения: "дробь" (true) или "число" (false)
        // В режиме "дробь" всегда показывается как a/b
        // В режиме "число" целые дроби показываются без /1
        public bool ShowAsFraction
        {
            get => FShowAsFraction;
            set => FShowAsFraction = value;
        }

        // Установка значения дроби
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

        // Проверка на ноль (0/1)
        public bool IsZero() => FNum == 0;

        // Сложение
        public TFrac Add(TFrac other)
        {
            int num = FNum * other.FDenom + other.FNum * FDenom;
            int denom = FDenom * other.FDenom;
            TFrac result = new TFrac(num, denom);
            result.FShowAsFraction = FShowAsFraction;
            return result;
        }

        // Вычитание
        public TFrac Sub(TFrac other)
        {
            int num = FNum * other.FDenom - other.FNum * FDenom;
            int denom = FDenom * other.FDenom;
            TFrac result = new TFrac(num, denom);
            result.FShowAsFraction = FShowAsFraction;
            return result;
        }

        // Умножение
        public TFrac Mul(TFrac other)
        {
            int num = FNum * other.FNum;
            int denom = FDenom * other.FDenom;
            TFrac result = new TFrac(num, denom);
            result.FShowAsFraction = FShowAsFraction;
            return result;
        }

        // Деление
        public TFrac Div(TFrac other)
        {
            if (other.FNum == 0)
            {
                throw new ArgumentException("Division by zero fraction");
            }
            int num = FNum * other.FDenom;
            int denom = FDenom * other.FNum;
            TFrac result = new TFrac(num, denom);
            result.FShowAsFraction = FShowAsFraction;
            return result;
        }

        // Возведение в квадрат (Sqr)
        public TFrac Sqr()
        {
            TFrac result = new TFrac(FNum * FNum, FDenom * FDenom);
            result.FShowAsFraction = FShowAsFraction;
            return result;
        }

        // Обратное значение (Rev) - 1/x
        public TFrac Rev()
        {
            if (FNum == 0)
            {
                throw new ArgumentException("Cannot reverse zero fraction");
            }
            TFrac result = new TFrac(FDenom, FNum);
            result.FShowAsFraction = FShowAsFraction;
            return result;
        }

        // Преобразование в строку с учётом режима отображения
        public override string ToString()
        {
            if (FShowAsFraction)
            {
                return $"{FNum}/{FDenom}";
            }
            
            if (FDenom == 1)
            {
                return FNum.ToString();
            }
            return $"{FNum}/{FDenom}";
        }

        // Преобразование в десятичное число
        public double ToDouble()
        {
            return (double)FNum / (double)FDenom;
        }

        // Оператор равенства
        public override bool Equals(object obj)
        {
            if (obj is TFrac other)
            {
                return FNum == other.FNum && FDenom == other.FDenom;
            }
            return false;
        }

        // Оператор неравенства
        public override int GetHashCode()
        {
            return FNum.GetHashCode() ^ FDenom.GetHashCode();
        }
    }
}
