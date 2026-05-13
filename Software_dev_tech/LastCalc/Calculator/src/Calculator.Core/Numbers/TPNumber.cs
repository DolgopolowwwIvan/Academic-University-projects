namespace Calculator.Core.Numbers;

/// <summary>
/// Класс для работы с p-ичными числами (система счисления с основанием 2-16)
/// </summary>
public class TPNumber : TANumber
{
    private double _value;
    private int _baseSystem;

    public const int MIN_BASE = 2;
    public const int MAX_BASE = 16;
    public const string SEPARATOR = ".";
    public const string ZERO_STRING = "0";

    public TPNumber() : base(ZERO_STRING)
    {
        _value = 0;
        _baseSystem = 10;
    }

    public TPNumber(double value) : base(value.ToString())
    {
        _value = value;
        _baseSystem = 10;
    }

    public TPNumber(string value) : base(value)
    {
        _baseSystem = 10;
        ParseFromString(value);
    }

    public TPNumber(double value, int baseSystem) : base(ConvertToBaseStringInternal(value, baseSystem))
    {
        _value = value;
        _baseSystem = Math.Max(MIN_BASE, Math.Min(MAX_BASE, baseSystem));
    }

    public TPNumber(TPNumber other) : base(other._stringValue)
    {
        _value = other._value;
        _baseSystem = other._baseSystem;
    }

    public int BaseSystem
    {
        get => _baseSystem;
        set => _baseSystem = Math.Max(MIN_BASE, Math.Min(MAX_BASE, value));
    }

    public double Value
    {
        get => _value;
        set => _value = value;
    }

    public override double ToDouble() => _value;

    public override bool EqZero() => Math.Abs(_value) < 1e-10;

    public override TANumber Copy() => new TPNumber(this);

    public override TANumber Add(TANumber other)
    {
        if (other is TPNumber pNum)
            return new TPNumber(_value + pNum._value, _baseSystem);
        else if (other is Frac frac)
            return new TPNumber(_value + frac.ToDouble(), _baseSystem);
        else if (other is TComplex comp)
            return new TComplex(new TPNumber(_value + comp.RealPart.ToDouble(), _baseSystem), comp.ImaginaryPart.Copy());
        throw new ArgumentException("Неподдерживаемый тип числа");
    }

    public override TANumber Subtract(TANumber other)
    {
        if (other is TPNumber pNum)
            return new TPNumber(_value - pNum._value, _baseSystem);
        else if (other is Frac frac)
            return new TPNumber(_value - frac.ToDouble(), _baseSystem);
        else if (other is TComplex comp)
            return new TComplex(new TPNumber(_value - comp.RealPart.ToDouble(), _baseSystem), comp.ImaginaryPart.Copy());
        throw new ArgumentException("Неподдерживаемый тип числа");
    }

    public override TANumber Multiply(TANumber other)
    {
        if (other is TPNumber pNum)
            return new TPNumber(_value * pNum._value, _baseSystem);
        else if (other is Frac frac)
            return new TPNumber(_value * frac.ToDouble(), _baseSystem);
        else if (other is TComplex comp)
            return new TComplex(new TPNumber(_value * comp.RealPart.ToDouble(), _baseSystem), new TPNumber(_value * comp.ImaginaryPart.ToDouble(), _baseSystem));
        throw new ArgumentException("Неподдерживаемый тип числа");
    }

    public override TANumber Divide(TANumber other)
    {
        if (other is TPNumber pNum)
        {
            if (Math.Abs(pNum._value) < 1e-10) throw new DivideByZeroException("Деление на ноль");
            return new TPNumber(_value / pNum._value, _baseSystem);
        }
        else if (other is Frac frac)
        {
            if (Math.Abs(frac.ToDouble()) < 1e-10) throw new DivideByZeroException("Деление на ноль");
            return new TPNumber(_value / frac.ToDouble(), _baseSystem);
        }
        else if (other is TComplex comp)
        {
            double denom = comp.RealPart.ToDouble() * comp.RealPart.ToDouble() + comp.ImaginaryPart.ToDouble() * comp.ImaginaryPart.ToDouble();
            if (Math.Abs(denom) < 1e-10) throw new DivideByZeroException("Деление на ноль");
            double realPart = _value * comp.RealPart.ToDouble() / denom;
            double imagPart = -_value * comp.ImaginaryPart.ToDouble() / denom;
            return new TComplex(new TPNumber(realPart, _baseSystem), new TPNumber(imagPart, _baseSystem));
        }
        throw new ArgumentException("Неподдерживаемый тип числа");
    }

    public override bool EqualsNumber(TANumber other)
    {
        if (other is TPNumber pNum) return Math.Abs(_value - pNum._value) < 1e-10;
        else if (other is Frac frac) return Math.Abs(_value - frac.ToDouble()) < 1e-10;
        else if (other is TComplex comp) return Math.Abs(_value - comp.RealPart.ToDouble()) < 1e-10 && Math.Abs(comp.ImaginaryPart.ToDouble()) < 1e-10;
        return false;
    }

    public override TANumber Sqr() => new TPNumber(_value * _value, _baseSystem);

    public override TANumber Reverse() => new TPNumber(-_value, _baseSystem);

    public override string ReadNumberAsString() => ConvertToBaseStringInternal(_value, _baseSystem);

    public override void WriteNumberFromString(string value) => ParseFromString(value);

    public override NumberType GetType() => NumberType.Real;

    private void ParseFromString(string value)
    {
        if (string.IsNullOrEmpty(value)) { _value = 0; return; }
        string str = value.Trim();
        bool isNegative = str.StartsWith("-");
        if (isNegative) str = str.Substring(1);
        int separatorPos = str.IndexOf(SEPARATOR);
        string intPart = separatorPos >= 0 ? str.Substring(0, separatorPos) : str;
        string fracPart = separatorPos >= 0 && separatorPos < str.Length - 1 ? str.Substring(separatorPos + 1) : string.Empty;
        _value = ParseBaseStringInternal(intPart, _baseSystem);
        if (!string.IsNullOrEmpty(fracPart))
            _value += ParseFracBaseStringInternal(fracPart, _baseSystem);
        if (isNegative) _value = -_value;
        _stringValue = value;
    }

    internal static string ConvertToBaseStringInternal(double value, int baseSystem)
    {
        if (baseSystem == 10) return value.ToString(System.Globalization.CultureInfo.InvariantCulture);
        bool isNegative = value < 0;
        double absValue = Math.Abs(value);
        long intPart = (long)absValue;
        string intStr = intPart == 0 ? "0" : ConvertToBase(intPart, baseSystem);
        double fracPart = absValue - intPart;
        string fracStr = fracPart > 1e-10 ? ConvertFracToBase(fracPart, baseSystem, 10) : string.Empty;
        string result = intStr + (fracStr.Length > 0 ? SEPARATOR + fracStr : string.Empty);
        return isNegative ? "-" + result : result;
    }

    private static string ConvertToBase(long value, int baseSystem)
    {
        const string digits = "0123456789ABCDEF";
        if (value == 0) return "0";
        string result = string.Empty;
        while (value > 0)
        {
            result = digits[(int)(value % baseSystem)] + result;
            value /= baseSystem;
        }
        return result;
    }

    private static string ConvertFracToBase(double value, int baseSystem, int maxDigits)
    {
        const string digits = "0123456789ABCDEF";
        string result = string.Empty;
        for (int i = 0; i < maxDigits && value > 1e-10; i++)
        {
            value *= baseSystem;
            int digit = (int)value;
            result += digits[digit];
            value -= digit;
        }
        return result;
    }

    public static double ParseBaseStringInternal(string str, int baseSystem)
    {
        if (string.IsNullOrEmpty(str)) return 0;
        bool isNegative = str.StartsWith("-");
        if (isNegative) str = str.Substring(1);
        int separatorPos = str.IndexOf(".");
        string intPart = separatorPos >= 0 ? str.Substring(0, separatorPos) : str;
        string fracPart = separatorPos >= 0 && separatorPos < str.Length - 1 ? str.Substring(separatorPos + 1) : string.Empty;
        double result = ParseIntPartInternal(intPart, baseSystem);
        if (!string.IsNullOrEmpty(fracPart))
            result += ParseFracBaseStringInternal(fracPart, baseSystem);
        return isNegative ? -result : result;
    }

    private static double ParseIntPartInternal(string str, int baseSystem)
    {
        const string digits = "0123456789ABCDEF";
        double result = 0;
        double multiplier = 1;
        for (int i = str.Length - 1; i >= 0; i--)
        {
            int digitVal = digits.IndexOf(char.ToUpper(str[i]));
            if (digitVal >= baseSystem || digitVal < 0) continue;
            result += digitVal * multiplier;
            multiplier *= baseSystem;
        }
        return result;
    }

    private static double ParseFracBaseStringInternal(string str, int baseSystem)
    {
        const string digits = "0123456789ABCDEF";
        double result = 0;
        double divisor = baseSystem;
        foreach (char c in str)
        {
            int digitVal = digits.IndexOf(char.ToUpper(c));
            if (digitVal >= baseSystem || digitVal < 0) continue;
            result += digitVal / divisor;
            divisor *= baseSystem;
        }
        return result;
    }

    public override string ToString() => ConvertToBaseStringInternal(_value, _baseSystem);
}
