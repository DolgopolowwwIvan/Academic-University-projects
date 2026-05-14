namespace Calculator.Core.Numbers;

/// <summary>
/// Класс для работы с рациональными дробями (простыми дробями)
/// </summary>
public class Frac : TANumber
{
    private long _numerator;
    private long _denominator;

    public const string SEPARATOR = "/";
    public const string ZERO_STRING = "0";

    public Frac() : base(ZERO_STRING)
    {
        _numerator = 0;
        _denominator = 1;
    }

    public Frac(long numerator, long denominator) : base($"{numerator}/{denominator}")
    {
        if (denominator == 0) throw new ArgumentException("Знаменатель не может быть равен нулю");
        _numerator = numerator;
        _denominator = denominator;
        Normalize();
    }

    public Frac(string value) : base(value) => ParseFromString(value);

    public Frac(Frac other) : base(other._stringValue)
    {
        _numerator = other._numerator;
        _denominator = other._denominator;
    }

    public long Numerator
    {
        get => _numerator;
        set { _numerator = value; Normalize(); }
    }

    public long Denominator
    {
        get => _denominator;
        set { if (value == 0) throw new ArgumentException("Знаменатель не может быть равен нулю"); _denominator = value; Normalize(); }
    }

    public double ToDoubleValue() => (double)_numerator / _denominator;

    public override double ToDouble() => ToDoubleValue();

    public override bool EqZero() => _numerator == 0;

    public override TANumber Copy() => new Frac(this);

    private void Normalize()
    {
        if (_numerator == 0) { _denominator = 1; _stringValue = ZERO_STRING; return; }
        if (_denominator < 0) { _numerator = -_numerator; _denominator = -_denominator; }
        long gcd = Gcd(Math.Abs(_numerator), _denominator);
        _numerator /= gcd;
        _denominator /= gcd;
        _stringValue = ToString();
    }

    private long Gcd(long a, long b)
    {
        while (b != 0) { long temp = b; b = a % b; a = temp; }
        return a;
    }

    public override TANumber Add(TANumber other)
    {
        if (other is Frac f)
        {
            long newNum = _numerator * f._denominator + f._numerator * _denominator;
            long newDen = _denominator * f._denominator;
            return new Frac(newNum, newDen);
        }
        else if (other is TPNumber p) return Add(new Frac((long)(p.Value * 1000000), 1000000));
        else if (other is TComplex c) return c.Add(this);
        throw new ArgumentException("Неподдерживаемый тип числа");
    }

    public override TANumber Subtract(TANumber other)
    {
        if (other is Frac f)
        {
            long newNum = _numerator * f._denominator - f._numerator * _denominator;
            long newDen = _denominator * f._denominator;
            return new Frac(newNum, newDen);
        }
        else if (other is TPNumber p) return Subtract(new Frac((long)(p.Value * 1000000), 1000000));
        else if (other is TComplex c) return c.Subtract(this).Reverse();
        throw new ArgumentException("Неподдерживаемый тип числа");
    }

    public override TANumber Multiply(TANumber other)
    {
        if (other is Frac f)
        {
            long newNum = _numerator * f._numerator;
            long newDen = _denominator * f._denominator;
            return new Frac(newNum, newDen);
        }
        else if (other is TPNumber p) return Multiply(new Frac((long)(p.Value * 1000000), 1000000));
        else if (other is TComplex c) return c.Multiply(this);
        throw new ArgumentException("Неподдерживаемый тип числа");
    }

    public override TANumber Divide(TANumber other)
    {
        if (other is Frac f)
        {
            if (f._numerator == 0) throw new DivideByZeroException("Деление на ноль");
            long newNum = _numerator * f._denominator;
            long newDen = _denominator * f._numerator;
            return new Frac(newNum, newDen);
        }
        else if (other is TPNumber p)
        {
            if (Math.Abs(p.Value) < 1e-10) throw new DivideByZeroException("Деление на ноль");
            return Divide(new Frac((long)(p.Value * 1000000), 1000000));
        }
        else if (other is TComplex c)
        {
            if (c.EqZero()) throw new DivideByZeroException("Деление на ноль");
            return c.Reciprocal().Multiply(this);
        }
        throw new ArgumentException("Неподдерживаемый тип числа");
    }

    public override bool EqualsNumber(TANumber other)
    {
        if (other is Frac f) return _numerator == f._numerator && _denominator == f._denominator;
        else if (other is TPNumber p) return Math.Abs(ToDoubleValue() - p.Value) < 1e-10;
        else if (other is TComplex c) return Math.Abs(ToDoubleValue() - c.RealPart.ToDouble()) < 1e-10 && Math.Abs(c.ImaginaryPart.ToDouble()) < 1e-10;
        return false;
    }

    public override TANumber Sqr() => new Frac(_numerator * _numerator, _denominator * _denominator);

    public override TANumber Reverse() => new Frac(-_numerator, _denominator);

    public override string ReadNumberAsString() => _stringValue;

    public override void WriteNumberFromString(string value) => ParseFromString(value);

    public override NumberType GetType() => NumberType.Fraction;

    private void ParseFromString(string value)
    {
        if (string.IsNullOrEmpty(value)) { _numerator = 0; _denominator = 1; _stringValue = ZERO_STRING; return; }
        string str = value.Trim();
        if (!str.Contains(SEPARATOR))
        {
            _numerator = long.TryParse(str, out long num) ? num : 0;
            _denominator = 1;
            Normalize();
            _stringValue = ToString();
            return;
        }
        string[] parts = str.Split(SEPARATOR);
        if (parts.Length == 2 && long.TryParse(parts[0], out long num1) && long.TryParse(parts[1], out long den) && den != 0)
        {
            _numerator = num1;
            _denominator = den;
            Normalize();
        }
        else { _numerator = 0; _denominator = 1; }
        _stringValue = ToString();
    }

    public override string ToString()
    {
        if (_numerator == 0) return ZERO_STRING;
        if (_denominator == 1) return _numerator.ToString();
        return $"{_numerator}{SEPARATOR}{_denominator}";
    }
}
