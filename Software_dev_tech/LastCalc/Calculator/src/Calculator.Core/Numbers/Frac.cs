namespace Calculator.Core.Numbers;

/// <summary>
/// Класс для работы с рациональными дробями (простыми дробями)
/// </summary>
public class Frac : TANumber
{
    private long _numerator;   // Числитель
    private long _denominator; // Знаменатель

    public const string SEPARATOR = "/";
    public const string ZERO_STRING = "0";

    public Frac() : base(ZERO_STRING)
    {
        _numerator = 0;
        _denominator = 1;
    }

    public Frac(long numerator, long denominator) : base($"{numerator}/{denominator}")
    {
        if (denominator == 0)
            throw new ArgumentException("Знаменатель не может быть равен нулю");
        
        _numerator = numerator;
        _denominator = denominator;
        Normalize();
    }

    public Frac(string value) : base(value)
    {
        ParseFromString(value);
    }

    public Frac(Frac other) : base(other._stringValue)
    {
        _numerator = other._numerator;
        _denominator = other._denominator;
    }

    /// <summary>
    /// Числитель дроби
    /// </summary>
    public long Numerator
    {
        get => _numerator;
        set
        {
            _numerator = value;
            Normalize();
        }
    }

    /// <summary>
    /// Знаменатель дроби
    /// </summary>
    public long Denominator
    {
        get => _denominator;
        set
        {
            if (value == 0)
                throw new ArgumentException("Знаменатель не может быть равен нулю");
            _denominator = value;
            Normalize();
        }
    }

    /// <summary>
    /// Преобразовать дробь в вещественное число
    /// </summary>
    public double ToDouble()
    {
        return (double)_numerator / _denominator;
    }

    public override double ToDouble() => ToDouble();

    /// <summary>
    /// Проверка, равна ли дробь нулю
    /// </summary>
    public override bool EqZero()
    {
        return _numerator == 0;
    }

    /// <summary>
    /// Копировать дробь
    /// </summary>
    public override TANumber Copy()
    {
        return new Frac(this);
    }

    /// <summary>
    /// Сократить дробь
    /// </summary>
    private void Normalize()
    {
        if (_numerator == 0)
        {
            _denominator = 1;
            return;
        }

        // Переносим знак на числитель
        if (_denominator < 0)
        {
            _numerator = -_numerator;
            _denominator = -_denominator;
        }

        // Сокращаем на наибольший общий делитель
        long gcd = Gcd(Math.Abs(_numerator), _denominator);
        _numerator /= gcd;
        _denominator /= gcd;
    }

    /// <summary>
    /// Наибольший общий делитель (алгоритм Евклида)
    /// </summary>
    private long Gcd(long a, long b)
    {
        while (b != 0)
        {
            long temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }

    public override TANumber Add(TANumber other)
    {
        if (other is Frac otherFrac)
        {
            // a/b + c/d = (ad + bc) / bd
            long newNumerator = _numerator * otherFrac._denominator + 
                               otherFrac._numerator * _denominator;
            long newDenominator = _denominator * otherFrac._denominator;
            return new Frac(newNumerator, newDenominator);
        }
        else if (other is TPNumber pNum)
        {
            // Преобразуем TPNumber в дробь
            double val = pNum.Value;
            long num = (long)(val * 1000000);
            long den = 1000000;
            return Add(new Frac(num, den));
        }
        else if (other is TComplex comp)
        {
            // Дробь + комплексное = комплексное
            return comp.Add(this);
        }
        throw new ArgumentException("Неподдерживаемый тип числа для сложения");
    }

    public override TANumber Subtract(TANumber other)
    {
        if (other is Frac otherFrac)
        {
            // a/b - c/d = (ad - bc) / bd
            long newNumerator = _numerator * otherFrac._denominator - 
                               otherFrac._numerator * _denominator;
            long newDenominator = _denominator * otherFrac._denominator;
            return new Frac(newNumerator, newDenominator);
        }
        else if (other is TPNumber pNum)
        {
            double val = pNum.Value;
            long num = (long)(val * 1000000);
            long den = 1000000;
            return Subtract(new Frac(num, den));
        }
        else if (other is TComplex comp)
        {
            // Дробь - комплексное = комплексное
            return comp.Subtract(this).Reverse(); // -(comp - frac) = frac - comp
        }
        throw new ArgumentException("Неподдерживаемый тип числа для вычитания");
    }

    public override TANumber Multiply(TANumber other)
    {
        if (other is Frac otherFrac)
        {
            // a/b * c/d = (ac) / (bd)
            long newNumerator = _numerator * otherFrac._numerator;
            long newDenominator = _denominator * otherFrac._denominator;
            return new Frac(newNumerator, newDenominator);
        }
        else if (other is TPNumber pNum)
        {
            double val = pNum.Value;
            long num = (long)(val * 1000000);
            long den = 1000000;
            return Multiply(new Frac(num, den));
        }
        else if (other is TComplex comp)
        {
            // Дробь * комплексное = комплексное
            return comp.Multiply(this);
        }
        throw new ArgumentException("Неподдерживаемый тип числа для умножения");
    }

    public override TANumber Divide(TANumber other)
    {
        if (other is Frac otherFrac)
        {
            if (otherFrac._numerator == 0)
                throw new DivideByZeroException("Деление на ноль");
            // a/b : c/d = (ad) / (bc)
            long newNumerator = _numerator * otherFrac._denominator;
            long newDenominator = _denominator * otherFrac._numerator;
            return new Frac(newNumerator, newDenominator);
        }
        else if (other is TPNumber pNum)
        {
            if (Math.Abs(pNum.Value) < 1e-10)
                throw new DivideByZeroException("Деление на ноль");
            double val = pNum.Value;
            long num = (long)(val * 1000000);
            long den = 1000000;
            return Divide(new Frac(num, den));
        }
        else if (other is TComplex comp)
        {
            if (comp.EqZero())
                throw new DivideByZeroException("Деление на ноль");
            // Дробь / комплексное = комплексное
            return comp.Reciprocal().Multiply(this);
        }
        throw new ArgumentException("Неподдерживаемый тип числа для деления");
    }

    public override bool EqualsNumber(TANumber other)
    {
        if (other is Frac otherFrac)
        {
            return _numerator == otherFrac._numerator && 
                   _denominator == otherFrac._denominator;
        }
        else if (other is TPNumber pNum)
        {
            return Math.Abs(ToDouble() - pNum.Value) < 1e-10;
        }
        else if (other is TComplex comp)
        {
            return Math.Abs(ToDouble() - comp.RealPart.ToDouble()) < 1e-10 && 
                   Math.Abs(comp.ImaginaryPart.ToDouble()) < 1e-10;
        }
        return false;
    }

    public override TANumber Sqr()
    {
        return new Frac(_numerator * _numerator, _denominator * _denominator);
    }

    public override TANumber Reverse()
    {
        return new Frac(-_numerator, _denominator);
    }

    public override string ReadNumberAsString()
    {
        return _stringValue;
    }

    public override void WriteNumberFromString(string value)
    {
        ParseFromString(value);
    }

    public override NumberType GetType() => NumberType.Fraction;

    /// <summary>
    /// Парсинг строки в дробь
    /// </summary>
    private void ParseFromString(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            _numerator = 0;
            _denominator = 1;
            _stringValue = ZERO_STRING;
            return;
        }

        string str = value.Trim();
        
        // Проверка на целое число
        if (!str.Contains(SEPARATOR))
        {
            if (long.TryParse(str, out long num))
            {
                _numerator = num;
                _denominator = 1;
            }
            else
            {
                _numerator = 0;
                _denominator = 1;
            }
            Normalize();
            _stringValue = ToString();
            return;
        }

        // Парсинг дроби a/b
        string[] parts = str.Split(SEPARATOR);
        if (parts.Length == 2)
        {
            if (long.TryParse(parts[0], out long num) && 
                long.TryParse(parts[1], out long den) && den != 0)
            {
                _numerator = num;
                _denominator = den;
                Normalize();
            }
            else
            {
                _numerator = 0;
                _denominator = 1;
            }
        }
        else
        {
            _numerator = 0;
            _denominator = 1;
        }

        _stringValue = ToString();
    }

    public override string ToString()
    {
        if (_numerator == 0)
            return ZERO_STRING;
        
        if (_denominator == 1)
            return _numerator.ToString();
        
        return $"{_numerator}{SEPARATOR}{_denominator}";
    }
}
