namespace Calculator.Core.Numbers;

/// <summary>
/// Класс для работы с p-ичными числами (система счисления с основанием 2-16)
/// </summary>
public class TPNumber : TANumber
{
    private double _value;
    private int _baseSystem; // Основание системы счисления (2-16)

    public const int MIN_BASE = 2;
    public const int MAX_BASE = 16;
    public const string SEPARATOR = ".";
    public const string ZERO_STRING = "0";

    // Константы команд редактора
    public const uint cZero = 0;
    public const uint cOne = 1;
    public const uint cTwo = 2;
    public const uint cThree = 3;
    public const uint cFour = 4;
    public const uint cFive = 5;
    public const uint cSix = 6;
    public const uint cSeven = 7;
    public const uint cEight = 8;
    public const uint cNine = 9;
    public const uint cSign = 10;
    public const uint cSeparatorFR = 11;
    public const uint cBS = 13;
    public const uint CE = 14;

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

    public TPNumber(double value, int baseSystem) : base(ConvertToBaseString(value, baseSystem))
    {
        _value = value;
        _baseSystem = Math.Max(MIN_BASE, Math.Min(MAX_BASE, baseSystem));
    }

    public TPNumber(TPNumber other) : base(other._stringValue)
    {
        _value = other._value;
        _baseSystem = other._baseSystem;
    }

    /// <summary>
    /// Основание системы счисления
    /// </summary>
    public int BaseSystem
    {
        get => _baseSystem;
        set => _baseSystem = Math.Max(MIN_BASE, Math.Min(MAX_BASE, value));
    }

    /// <summary>
    /// Значение числа в десятичной системе
    /// </summary>
    public double Value
    {
        get => _value;
        set => _value = value;
    }

    public override double ToDouble() => _value;

    public override bool EqZero()
    {
        return Math.Abs(_value) < 1e-10;
    }

    public override TANumber Copy()
    {
        return new TPNumber(this);
    }

    public override TANumber Add(TANumber other)
    {
        if (other is TPNumber pNum)
        {
            double result = _value + pNum._value;
            return new TPNumber(result, _baseSystem);
        }
        else if (other is Frac frac)
        {
            double result = _value + frac.ToDouble();
            return new TPNumber(result, _baseSystem);
        }
        else if (other is TComplex comp)
        {
            double realPart = _value + comp.RealPart.ToDouble();
            return new TComplex(new TPNumber(realPart, _baseSystem), comp.ImaginaryPart.Copy());
        }
        throw new ArgumentException("Неподдерживаемый тип числа для сложения");
    }

    public override TANumber Subtract(TANumber other)
    {
        if (other is TPNumber pNum)
        {
            double result = _value - pNum._value;
            return new TPNumber(result, _baseSystem);
        }
        else if (other is Frac frac)
        {
            double result = _value - frac.ToDouble();
            return new TPNumber(result, _baseSystem);
        }
        else if (other is TComplex comp)
        {
            double realPart = _value - comp.RealPart.ToDouble();
            return new TComplex(new TPNumber(realPart, _baseSystem), comp.ImaginaryPart.Copy());
        }
        throw new ArgumentException("Неподдерживаемый тип числа для вычитания");
    }

    public override TANumber Multiply(TANumber other)
    {
        if (other is TPNumber pNum)
        {
            double result = _value * pNum._value;
            return new TPNumber(result, _baseSystem);
        }
        else if (other is Frac frac)
        {
            double result = _value * frac.ToDouble();
            return new TPNumber(result, _baseSystem);
        }
        else if (other is TComplex comp)
        {
            // (a) * (b + ci) = ab + aci
            double realPart = _value * comp.RealPart.ToDouble();
            double imagPart = _value * comp.ImaginaryPart.ToDouble();
            return new TComplex(new TPNumber(realPart, _baseSystem), new TPNumber(imagPart, _baseSystem));
        }
        throw new ArgumentException("Неподдерживаемый тип числа для умножения");
    }

    public override TANumber Divide(TANumber other)
    {
        if (other is TPNumber pNum)
        {
            if (Math.Abs(pNum._value) < 1e-10)
                throw new DivideByZeroException("Деление на ноль");
            double result = _value / pNum._value;
            return new TPNumber(result, _baseSystem);
        }
        else if (other is Frac frac)
        {
            if (Math.Abs(frac.ToDouble()) < 1e-10)
                throw new DivideByZeroException("Деление на ноль");
            double result = _value / frac.ToDouble();
            return new TPNumber(result, _baseSystem);
        }
        else if (other is TComplex comp)
        {
            // a / (b + ci) = a(b - ci) / (b² + c²)
            double denom = comp.RealPart.ToDouble() * comp.RealPart.ToDouble() + 
                          comp.ImaginaryPart.ToDouble() * comp.ImaginaryPart.ToDouble();
            if (Math.Abs(denom) < 1e-10)
                throw new DivideByZeroException("Деление на ноль");
            
            double realPart = _value * comp.RealPart.ToDouble() / denom;
            double imagPart = -_value * comp.ImaginaryPart.ToDouble() / denom;
            return new TComplex(new TPNumber(realPart, _baseSystem), new TPNumber(imagPart, _baseSystem));
        }
        throw new ArgumentException("Неподдерживаемый тип числа для деления");
    }

    public override bool EqualsNumber(TANumber other)
    {
        if (other is TPNumber pNum)
        {
            return Math.Abs(_value - pNum._value) < 1e-10;
        }
        else if (other is Frac frac)
        {
            return Math.Abs(_value - frac.ToDouble()) < 1e-10;
        }
        else if (other is TComplex comp)
        {
            return Math.Abs(_value - comp.RealPart.ToDouble()) < 1e-10 && 
                   Math.Abs(comp.ImaginaryPart.ToDouble()) < 1e-10;
        }
        return false;
    }

    public override TANumber Sqr()
    {
        return new TPNumber(_value * _value, _baseSystem);
    }

    public override TANumber Reverse()
    {
        return new TPNumber(-_value, _baseSystem);
    }

    public override string ReadNumberAsString()
    {
        return ConvertToBaseString(_value, _baseSystem);
    }

    public override void WriteNumberFromString(string value)
    {
        ParseFromString(value);
    }

    public override NumberType GetType() => NumberType.Real;

    /// <summary>
    /// Парсинг строки в число с учётом системы счисления
    /// </summary>
    private void ParseFromString(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            _value = 0;
            return;
        }

        string str = value.Trim();
        bool isNegative = str.StartsWith("-");
        if (isNegative) str = str.Substring(1);

        int separatorPos = str.IndexOf(SEPARATOR);
        string intPart, fracPart;

        if (separatorPos >= 0)
        {
            intPart = str.Substring(0, separatorPos);
            fracPart = str.Substring(separatorPos + 1);
        }
        else
        {
            intPart = str;
            fracPart = string.Empty;
        }

        // Парсинг целой части
        _value = ParseBaseStringInternal(intPart, _baseSystem);

        // Парсинг дробной части
        if (!string.IsNullOrEmpty(fracPart))
        {
            _value += ParseFracBaseStringInternal(fracPart, _baseSystem);
        }

        if (isNegative)
            _value = -_value;

        _stringValue = value;
    }

    internal static string ConvertToBaseStringInternal(double value, int baseSystem)
    {
        if (baseSystem == 10)
            return value.ToString(System.Globalization.CultureInfo.InvariantCulture);

        bool isNegative = value < 0;
        double absValue = Math.Abs(value);

        // Целая часть
        long intPart = (long)absValue;
        string intStr = intPart == 0 ? "0" : ConvertToBase(intPart, baseSystem);

        // Дробная часть
        double fracPart = absValue - intPart;
        string fracStr = string.Empty;
        
        if (fracPart > 1e-10)
        {
            fracStr = ConvertFracToBase(fracPart, baseSystem, 10);
        }

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

    /// <summary>
    /// Парсинг строки в число с учётом системы счисления
    /// </summary>
    public static double ParseBaseStringInternal(string str, int baseSystem)
    {
        // Упрощённый парсинг для редактора
        if (string.IsNullOrEmpty(str))
            return 0;

        bool isNegative = str.StartsWith("-");
        if (isNegative) str = str.Substring(1);

        int separatorPos = str.IndexOf(".");
        string intPart = separatorPos >= 0 ? str.Substring(0, separatorPos) : str;
        string fracPart = separatorPos >= 0 && separatorPos < str.Length - 1 ? str.Substring(separatorPos + 1) : string.Empty;

        double result = ParseBaseStringInternal(intPart, baseSystem);
        
        if (!string.IsNullOrEmpty(fracPart))
        {
            result += ParseFracBaseStringInternal(fracPart, baseSystem);
        }

        return isNegative ? -result : result;
    }

    public static double ParseBaseStringFromEditor(string str, int baseSystem)
    {
        return ParseBaseStringInternal(str, baseSystem);
    }

    private static double ParseBaseStringInternal(string str, int baseSystem)
    {
        if (string.IsNullOrEmpty(value))
        {
            _value = 0;
            return;
        }

        string str = value.Trim();
        bool isNegative = str.StartsWith("-");
        if (isNegative) str = str.Substring(1);

        int separatorPos = str.IndexOf(SEPARATOR);
        string intPart, fracPart;

        if (separatorPos >= 0)
        {
            intPart = str.Substring(0, separatorPos);
            fracPart = str.Substring(separatorPos + 1);
        }
        else
        {
            intPart = str;
            fracPart = string.Empty;
        }

        // Парсинг целой части
        _value = ParseBaseString(intPart, _baseSystem);

        // Парсинг дробной части
        if (!string.IsNullOrEmpty(fracPart))
        {
            _value += ParseFracBaseString(fracPart, _baseSystem);
        }

        if (isNegative)
            _value = -_value;

        _stringValue = value;
    }

    private static double ParseBaseStringInternal(string str, int baseSystem)
    {
        const string digits = "0123456789ABCDEF";
        double result = 0;
        double multiplier = 1;

        for (int i = str.Length - 1; i >= 0; i--)
        {
            int digitVal = digits.IndexOf(char.ToUpper(str[i]));
            if (digitVal >= baseSystem || digitVal < 0)
                continue;
            
            result += digitVal * multiplier;
            multiplier *= baseSystem;
        }

        return result;
    }

    private static double ParseFracBaseStringInternal(string str, int baseSystem)
    {
        const string digits = "0123456789ABCDEF";
        double result = 0;
        double multiplier = 1;

        for (int i = str.Length - 1; i >= 0; i--)
        {
            int digitVal = digits.IndexOf(char.ToUpper(str[i]));
            if (digitVal >= baseSystem || digitVal < 0)
                continue; // Пропускаем недопустимые символы
            
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
            if (digitVal >= baseSystem || digitVal < 0)
                continue;
            
            result += digitVal / divisor;
            divisor *= baseSystem;
        }

        return result;
    }

    private static string ParseFracBaseString(string str, int baseSystem)
    {
        const string digits = "0123456789ABCDEF";
        double result = 0;
        double divisor = baseSystem;

        foreach (char c in str)
        {
            int digitVal = digits.IndexOf(char.ToUpper(c));
            if (digitVal >= baseSystem || digitVal < 0)
                continue;
            
            result += digitVal / divisor;
            divisor *= baseSystem;
        }

        return result;
    }

    public override string ToString()
    {
        return ConvertToBaseString(_value, _baseSystem);
    }
}
