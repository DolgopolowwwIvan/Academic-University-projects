namespace Calculator.Core.Numbers;

/// <summary>
/// Класс для работы с комплексными числами в системе счисления с основанием p
/// </summary>
public class TComplex : TANumber
{
    private TANumber _realPart;   // Действительная часть
    private TANumber _imaginaryPart; // Мнимая часть

    public const string IMAGINARY_SEPARATOR = "i*";
    public const string ZERO_STRING = "0";

    public TComplex() : base(ZERO_STRING)
    {
        _realPart = new TPNumber(0);
        _imaginaryPart = new TPNumber(0);
    }

    public TComplex(TANumber real, TANumber imag) : base(FormatString(real, imag))
    {
        _realPart = real ?? new TPNumber(0);
        _imaginaryPart = imag ?? new TPNumber(0);
    }

    public TComplex(double real, double imag) : base(FormatString(new TPNumber(real), new TPNumber(imag)))
    {
        _realPart = new TPNumber(real);
        _imaginaryPart = new TPNumber(imag);
    }

    public TComplex(string value) : base(value)
    {
        ParseFromString(value);
    }

    public TComplex(TComplex other) : base(other._stringValue)
    {
        _realPart = other._realPart?.Copy() ?? new TPNumber(0);
        _imaginaryPart = other._imaginaryPart?.Copy() ?? new TPNumber(0);
    }

    /// <summary>
    /// Действительная часть
    /// </summary>
    public TANumber RealPart
    {
        get => _realPart;
        set => _realPart = value ?? new TPNumber(0);
    }

    /// <summary>
    /// Мнимая часть
    /// </summary>
    public TANumber ImaginaryPart
    {
        get => _imaginaryPart;
        set => _imaginaryPart = value ?? new TPNumber(0);
    }

    public override double ToDouble()
    {
        // Для комплексных чисел возвращаем модуль
        double real = _realPart.ToDouble();
        double imag = _imaginaryPart.ToDouble();
        return Math.Sqrt(real * real + imag * imag);
    }

    public override bool EqZero()
    {
        return _realPart.EqZero() && _imaginaryPart.EqZero();
    }

    public override TANumber Copy()
    {
        return new TComplex(this);
    }

    public override TANumber Add(TANumber other)
    {
        if (other is TComplex otherComp)
        {
            // (a + bi) + (c + di) = (a+c) + (b+d)i
            TANumber newReal = _realPart.Add(otherComp._realPart);
            TANumber newImag = _imaginaryPart.Add(otherComp._imaginaryPart);
            return new TComplex(newReal, newImag);
        }
        else if (other is TPNumber pNum)
        {
            // (a + bi) + c = (a+c) + bi
            TANumber newReal = _realPart.Add(pNum);
            return new TComplex(newReal, _imaginaryPart.Copy());
        }
        else if (other is Frac frac)
        {
            // (a + bi) + c/d = (a+c/d) + bi
            TANumber newReal = _realPart.Add(frac);
            return new TComplex(newReal, _imaginaryPart.Copy());
        }
        throw new ArgumentException("Неподдерживаемый тип числа для сложения");
    }

    public override TANumber Subtract(TANumber other)
    {
        if (other is TComplex otherComp)
        {
            // (a + bi) - (c + di) = (a-c) + (b-d)i
            TANumber newReal = _realPart.Subtract(otherComp._realPart);
            TANumber newImag = _imaginaryPart.Subtract(otherComp._imaginaryPart);
            return new TComplex(newReal, newImag);
        }
        else if (other is TPNumber pNum)
        {
            // (a + bi) - c = (a-c) + bi
            TANumber newReal = _realPart.Subtract(pNum);
            return new TComplex(newReal, _imaginaryPart.Copy());
        }
        else if (other is Frac frac)
        {
            // (a + bi) - c/d = (a-c/d) + bi
            TANumber newReal = _realPart.Subtract(frac);
            return new TComplex(newReal, _imaginaryPart.Copy());
        }
        throw new ArgumentException("Неподдерживаемый тип числа для вычитания");
    }

    public override TANumber Multiply(TANumber other)
    {
        if (other is TComplex otherComp)
        {
            // (a + bi) * (c + di) = (ac - bd) + (ad + bc)i
            TANumber ac = _realPart.Multiply(otherComp._realPart);
            TANumber bd = _imaginaryPart.Multiply(otherComp._imaginaryPart);
            TANumber ad = _realPart.Multiply(otherComp._imaginaryPart);
            TANumber bc = _imaginaryPart.Multiply(otherComp._realPart);
            
            TANumber newReal = ac.Subtract(bd);
            TANumber newImag = ad.Add(bc);
            return new TComplex(newReal, newImag);
        }
        else if (other is TPNumber pNum)
        {
            // (a + bi) * c = ac + bci
            TANumber newReal = _realPart.Multiply(pNum);
            TANumber newImag = _imaginaryPart.Multiply(pNum);
            return new TComplex(newReal, newImag);
        }
        else if (other is Frac frac)
        {
            // (a + bi) * c/d = ac/d + bci/d
            TANumber newReal = _realPart.Multiply(frac);
            TANumber newImag = _imaginaryPart.Multiply(frac);
            return new TComplex(newReal, newImag);
        }
        throw new ArgumentException("Неподдерживаемый тип числа для умножения");
    }

    public override TANumber Divide(TANumber other)
    {
        if (other is TComplex otherComp)
        {
            // (a + bi) / (c + di) = [(a + bi)(c - di)] / (c² + d²)
            // = [(ac + bd) + (bc - ad)i] / (c² + d²)
            
            TANumber c2 = otherComp._realPart.Sqr();
            TANumber d2 = otherComp._imaginaryPart.Sqr();
            TANumber denom = c2.Add(d2);

            if (denom.EqZero())
                throw new DivideByZeroException("Деление на ноль");

            TANumber ac = _realPart.Multiply(otherComp._realPart);
            TANumber bd = _imaginaryPart.Multiply(otherComp._imaginaryPart);
            TANumber bc = _imaginaryPart.Multiply(otherComp._realPart);
            TANumber ad = _realPart.Multiply(otherComp._imaginaryPart);

            TANumber newReal = ac.Add(bd).Divide(denom);
            TANumber newImag = bc.Subtract(ad).Divide(denom);
            return new TComplex(newReal, newImag);
        }
        else if (other is TPNumber pNum)
        {
            if (pNum.EqZero())
                throw new DivideByZeroException("Деление на ноль");
            // (a + bi) / c = a/c + (b/c)i
            TANumber newReal = _realPart.Divide(pNum);
            TANumber newImag = _imaginaryPart.Divide(pNum);
            return new TComplex(newReal, newImag);
        }
        else if (other is Frac frac)
        {
            if (frac.EqZero())
                throw new DivideByZeroException("Деление на ноль");
            // (a + bi) / (c/d) = ad/c + bdi/c
            TANumber newReal = _realPart.Divide(frac);
            TANumber newImag = _imaginaryPart.Divide(frac);
            return new TComplex(newReal, newImag);
        }
        throw new ArgumentException("Неподдерживаемый тип числа для деления");
    }

    public override bool EqualsNumber(TANumber other)
    {
        if (other is TComplex otherComp)
        {
            return _realPart.EqualsNumber(otherComp._realPart) && 
                   _imaginaryPart.EqualsNumber(otherComp._imaginaryPart);
        }
        else if (other is TPNumber pNum)
        {
            return _realPart.EqualsNumber(pNum) && _imaginaryPart.EqZero();
        }
        else if (other is Frac frac)
        {
            return _realPart.EqualsNumber(frac) && _imaginaryPart.EqZero();
        }
        return false;
    }

    public override TANumber Sqr()
    {
        // (a + bi)² = a² - b² + 2abi
        TANumber a2 = _realPart.Sqr();
        TANumber b2 = _imaginaryPart.Sqr();
        TANumber twoAb = _realPart.Multiply(_imaginaryPart).Multiply(new TPNumber(2));
        
        TANumber newReal = a2.Subtract(b2);
        TANumber newImag = twoAb;
        return new TComplex(newReal, newImag);
    }

    public override TANumber Reverse()
    {
        // -(a + bi) = -a - bi
        return new TComplex(_realPart.Reverse(), _imaginaryPart.Reverse());
    }

    /// <summary>
    /// Обратное комплексное число (1/z)
    /// </summary>
    public TANumber Reciprocal()
    {
        // 1/(a + bi) = (a - bi)/(a² + b²)
        TANumber a2 = _realPart.Sqr();
        TANumber b2 = _imaginaryPart.Sqr();
        TANumber denom = a2.Add(b2);

        if (denom.EqZero())
            throw new DivideByZeroException("Деление на ноль");

        TANumber newReal = _realPart.Divide(denom);
        TANumber newImag = _imaginaryPart.Reverse().Divide(denom);
        return new TComplex(newReal, newImag);
    }

    /// <summary>
    /// Модуль комплексного числа
    /// </summary>
    public TANumber Modulus()
    {
        // |a + bi| = √(a² + b²)
        TANumber a2 = _realPart.Sqr();
        TANumber b2 = _imaginaryPart.Sqr();
        TANumber sum = a2.Add(b2);
        
        // Для TPNumber можно извлечь корень
        if (sum is TPNumber pNum)
        {
            double val = Math.Sqrt(pNum.Value);
            return new TPNumber(val, pNum.BaseSystem);
        }
        
        return sum;
    }

    public override string ReadNumberAsString()
    {
        return FormatString(_realPart, _imaginaryPart);
    }

    public override void WriteNumberFromString(string value)
    {
        ParseFromString(value);
    }

    public override NumberType GetType() => NumberType.Complex;

    /// <summary>
    /// Форматирование комплексного числа в строку вида "a + bi*" или "a - bi*"
    /// </summary>
    private static string FormatString(TANumber real, TANumber imag)
    {
        string realStr = real.ReadNumberAsString();
        string imagStr = imag.ReadNumberAsString();
        
        // Если мнимая часть ноль — возвращаем только действительную
        if (imag.EqZero())
            return realStr;
        
        // Определяем знак мнимой части
        bool imagNegative = imagStr.StartsWith("-");
        if (imagNegative)
            imagStr = imagStr.Substring(1);
        
        string sign = imagNegative ? " - " : " + ";
        return $"{realStr}{sign}{imagStr}{IMAGINARY_SEPARATOR}";
    }

    /// <summary>
    /// Парсинг строки в комплексное число
    /// Поддерживает форматы: "a", "-a", "a + bi*", "a - bi*", "a+bi*", "a-bi*"
    /// </summary>
    private void ParseFromString(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            _realPart = new TPNumber(0);
            _imaginaryPart = new TPNumber(0);
            _stringValue = ZERO_STRING;
            return;
        }

        string str = value.Trim();
        
        // Проверка на наличие мнимой части
        int imagSepIndex = str.IndexOf(IMAGINARY_SEPARATOR);
        
        if (imagSepIndex < 0)
        {
            // Только действительная часть
            _realPart = new TPNumber(str);
            _imaginaryPart = new TPNumber(0);
            _stringValue = value;
            return;
        }

        // Отрезаем всё после i* (если есть что-то ещё)
        string beforeImag = str.Substring(0, imagSepIndex).TrimEnd();

        // Ищем последний " + " или " - " — разделитель действительной и мнимой части
        int lastPlus = beforeImag.LastIndexOf(" + ");
        int lastMinus = beforeImag.LastIndexOf(" - ");
        int signPos = Math.Max(lastPlus, lastMinus);

        if (signPos > 0)
        {
            // Формат с пробелами: "a + bi*" / "a - bi*"
            string realPartStr = beforeImag.Substring(0, signPos).Trim();
            string imagPartStr = beforeImag.Substring(signPos + 3).Trim();

            if (lastMinus > lastPlus)
                imagPartStr = "-" + imagPartStr;

            _realPart = new TPNumber(realPartStr);
            _imaginaryPart = new TPNumber(imagPartStr);
            _stringValue = value;
            return;
        }
            
        // Fallback: формат без пробелов "a-bi*" или "a+bi*"
        int lastMinusCompact = beforeImag.LastIndexOf('-');
        int lastPlusCompact = beforeImag.LastIndexOf('+');
        int compactSignPos = Math.Max(lastMinusCompact, lastPlusCompact);

        if (compactSignPos > 0)
        {
            string realPartStr = beforeImag.Substring(0, compactSignPos).Trim();
            string imagPartStr = beforeImag.Substring(compactSignPos + 1).Trim();

            if (lastMinusCompact > lastPlusCompact)
                imagPartStr = "-" + imagPartStr;

            _realPart = new TPNumber(realPartStr);
            _imaginaryPart = new TPNumber(imagPartStr);
            _stringValue = value;
            return;
        }

        // Нет разделителя — всё в действительной части (например, "9i*" — мнимая часть без действительной)
        if (beforeImag.Length > 0)
        {
            _realPart = new TPNumber(0);
            _imaginaryPart = new TPNumber(beforeImag);
        }
        else
        {
            _realPart = new TPNumber(0);
            _imaginaryPart = new TPNumber(0);
        }

        _stringValue = value;
    }

    public override string ToString()
    {
        return FormatString(_realPart, _imaginaryPart);
    }
}
