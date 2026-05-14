namespace Calculator.Core;

using Calculator.Core.Numbers;

// Процессор - выполняет арифметические операции и функции для универсальных чисел TANumber
#nullable disable
public class TProc
{
    private TANumber _lopd_Res;
    private TANumber _ropd;
    private TOperation _operation;
    private string _error;

    public TProc()
    {
        _lopd_Res = new TPNumber(0);
        _ropd = new TPNumber(0);
        _operation = TOperation.None;
        _error = string.Empty;
    }

    public TProc(TANumber left, TANumber right)
    {
        _lopd_Res = left ?? new TPNumber(0);
        _ropd = right ?? new TPNumber(0);
        _operation = TOperation.None;
        _error = string.Empty;
    }

    // Левый операнд и результат
    public TANumber Lopd_Res
    {
        get => _lopd_Res ?? new TPNumber(0);
        set => _lopd_Res = value;
    }

    // Правый операнд
    public TANumber Ropd
    {
        get => _ropd ?? new TPNumber(0);
        set => _ropd = value;
    }

    // Операция
    public TOperation Operation
    {
        get => _operation;
        set => _operation = value;
    }

    // Ошибка
    public string Error
    {
        get => _error;
        set => _error = value;
    }

    // Очистить ошибку
    public void ClearError()
    {
        _error = string.Empty;
    }

    // Выполнить операцию
    public void OprtnRun()
    {
        if (_lopd_Res == null || _ropd == null)
        {
            _error = "Операнды не установлены";
            return;
        }

        try
        {
            _error = string.Empty;

            switch (_operation)
            {
                case TOperation.Add:
                    _lopd_Res = _lopd_Res.Add(_ropd);
                    break;
                case TOperation.Subtract:
                    _lopd_Res = _lopd_Res.Subtract(_ropd);
                    break;
                case TOperation.Multiply:
                    _lopd_Res = _lopd_Res.Multiply(_ropd);
                    break;
                case TOperation.Divide:
                    _lopd_Res = _lopd_Res.Divide(_ropd);
                    break;
                case TOperation.Power:
                    _lopd_Res = Power(_lopd_Res, _ropd);
                    break;
                case TOperation.None:
                    // Операция не установлена, ничего не делаем
                    break;
                default:
                    _error = "Неподдерживаемая операция";
                    break;
            }
        }
        catch (DivideByZeroException ex)
        {
            _error = "Деление на ноль";
        }
        catch (Exception ex)
        {
            _error = ex.Message;
        }
    }

    // Выполнить функцию
    public void FuncRun(TFunction func)
    {
        if (_lopd_Res == null)
        {
            _error = "Операнд не установлен";
            return;
        }

        try
        {
            _error = string.Empty;

            switch (func)
            {
                case TFunction.Sin:
                    _lopd_Res = Sin(_lopd_Res);
                    break;
                case TFunction.Cos:
                    _lopd_Res = Cos(_lopd_Res);
                    break;
                case TFunction.Tan:
                    _lopd_Res = Tan(_lopd_Res);
                    break;
                case TFunction.Sqrt:
                    _lopd_Res = Sqrt(_lopd_Res);
                    break;
                case TFunction.Log:
                    _lopd_Res = Log(_lopd_Res);
                    break;
                case TFunction.Ln:
                    _lopd_Res = Ln(_lopd_Res);
                    break;
                case TFunction.Abs:
                    _lopd_Res = Abs(_lopd_Res);
                    break;
                case TFunction.Exp:
                    _lopd_Res = Exp(_lopd_Res);
                    break;
                case TFunction.Sqr:
                    _lopd_Res = _lopd_Res.Sqr();
                    break;
                case TFunction.Reverse:
                    _lopd_Res = _lopd_Res.Reverse();
                    break;
                case TFunction.None:
                    // Функция не установлена, ничего не делаем
                    break;
                default:
                    _error = "Неподдерживаемая функция";
                    break;
            }
        }
        catch (Exception ex)
        {
            _error = ex.Message;
        }
    }

    // Возведение в степень
    private TANumber Power(TANumber baseNum, TANumber exponent)
    {
        double baseVal = baseNum.ToDouble();
        double expVal = exponent.ToDouble();
        
        if (baseVal < 0 && Math.Abs(expVal - Math.Floor(expVal)) > 1e-10)
        {
            throw new ArgumentException("Возведение отрицательного числа в дробную степень");
        }
        
        double result = Math.Pow(baseVal, expVal);
        return new TPNumber(result);
    }

    // Синус
    private TANumber Sin(TANumber num)
    {
        double value = num.ToDouble();
        double result = Math.Sin(value);
        return new TPNumber(result);
    }

    // Косинус
    private TANumber Cos(TANumber num)
    {
        double value = num.ToDouble();
        double result = Math.Cos(value);
        return new TPNumber(result);
    }

    // Тангенс
    private TANumber Tan(TANumber num)
    {
        double value = num.ToDouble();
        double result = Math.Tan(value);
        return new TPNumber(result);
    }

    // Квадратный корень
    private TANumber Sqrt(TANumber num)
    {
        double value = num.ToDouble();
        if (value < 0)
        {
            throw new ArgumentException("Квадратный корень из отрицательного числа");
        }
        double result = Math.Sqrt(value);
        return new TPNumber(result);
    }

    // Логарифм по основанию 10
    private TANumber Log(TANumber num)
    {
        double value = num.ToDouble();
        if (value <= 0)
        {
            throw new ArgumentException("Логарифм из неположительного числа");
        }
        double result = Math.Log10(value);
        return new TPNumber(result);
    }

    // Натуральный логарифм
    private TANumber Ln(TANumber num)
    {
        double value = num.ToDouble();
        if (value <= 0)
        {
            throw new ArgumentException("Натуральный логарифм из неположительного числа");
        }
        double result = Math.Log(value);
        return new TPNumber(result);
    }

    // Модуль
    private TANumber Abs(TANumber num)
    {
        double value = num.ToDouble();
        double result = Math.Abs(value);
        return new TPNumber(result);
    }

    // Экспонента
    private TANumber Exp(TANumber num)
    {
        double value = num.ToDouble();
        double result = Math.Exp(value);
        return new TPNumber(result);
    }

    // Установить начальное состояние с сохранением типа числа
    public void ReSet()
    {
        var type = _lopd_Res?.GetType();
        if (type == NumberType.Fraction)
            _lopd_Res = new Frac(0, 1);
        else if (type == NumberType.Complex)
            _lopd_Res = new TComplex(0, 0);
        else
            _lopd_Res = new TPNumber(0, 10);
        
        _ropd = _lopd_Res.Copy();
        _operation = TOperation.None;
        _error = string.Empty;
    }

    // Установить начальное состояние с явным указанием типа числа
    public void ReSet(NumberType numberType)
    {
        if (numberType == NumberType.Fraction)
            _lopd_Res = new Frac(0, 1);
        else if (numberType == NumberType.Complex)
            _lopd_Res = new TComplex(0, 0);
        else
            _lopd_Res = new TPNumber(0, 10);
        
        _ropd = _lopd_Res.Copy();
        _operation = TOperation.None;
        _error = string.Empty;
    }

    ~TProc()
    {
        _lopd_Res = null;
        _ropd = null;
    }
}
#nullable enable
