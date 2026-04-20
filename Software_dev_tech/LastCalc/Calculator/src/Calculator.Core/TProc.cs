namespace Calculator.Core;

// Процессор - выполняет арифметические операции и функции
#nullable disable
public class TProc
{
    private TANumber _lopd_Res;
    private TANumber _ropd;
    private TOperation _operation;
    private string _error;

    public TProc()
    {
        _lopd_Res = new TANumber();
        _ropd = new TANumber();
        _operation = TOperation.None;
        _error = string.Empty;
    }

    // Левый операнд и результат
    public TANumber Lopd_Res
    {
        get => _lopd_Res ?? new TANumber();
        set => _lopd_Res = value;
    }

    // Правый операнд
    public TANumber Ropd
    {
        get => _ropd ?? new TANumber();
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
            double left = _lopd_Res.Value;
            double right = _ropd.Value;
            double result = 0;

            switch (_operation)
            {
                case TOperation.Add:
                    result = left + right;
                    break;
                case TOperation.Subtract:
                    result = left - right;
                    break;
                case TOperation.Multiply:
                    result = left * right;
                    break;
                case TOperation.Divide:
                    if (Math.Abs(right) < 1e-10)
                    {
                        _error = "Деление на ноль";
                        return;
                    }
                    result = left / right;
                    break;
                case TOperation.Power:
                    result = Math.Pow(left, right);
                    break;
                case TOperation.Modulo:
                    if (Math.Abs(right) < 1e-10)
                    {
                        _error = "Деление на ноль";
                        return;
                    }
                    result = left % right;
                    break;
                case TOperation.None:
                    result = left;
                    break;
            }

            _lopd_Res.Value = result;
            _error = string.Empty;
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
            double value = _lopd_Res.Value;
            double result = 0;

            switch (func)
            {
                case TFunction.Sin:
                    result = Math.Sin(value);
                    break;
                case TFunction.Cos:
                    result = Math.Cos(value);
                    break;
                case TFunction.Tan:
                    result = Math.Tan(value);
                    break;
                case TFunction.Sqrt:
                    if (value < 0)
                    {
                        _error = "Квадратный корень из отрицательного числа";
                        return;
                    }
                    result = Math.Sqrt(value);
                    break;
                case TFunction.Log:
                    if (value <= 0)
                    {
                        _error = "Логарифм из неположительного числа";
                        return;
                    }
                    result = Math.Log10(value);
                    break;
                case TFunction.Ln:
                    if (value <= 0)
                    {
                        _error = "Натуральный логарифм из неположительного числа";
                        return;
                    }
                    result = Math.Log(value);
                    break;
                case TFunction.Abs:
                    result = Math.Abs(value);
                    break;
                case TFunction.Exp:
                    result = Math.Exp(value);
                    break;
                case TFunction.None:
                    result = value;
                    break;
            }

            _lopd_Res.Value = result;
            _error = string.Empty;
        }
        catch (Exception ex)
        {
            _error = ex.Message;
        }
    }

    // Установить начальное состояние
    public void ReSet()
    {
        _lopd_Res?.Clear();
        _ropd?.Clear();
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
