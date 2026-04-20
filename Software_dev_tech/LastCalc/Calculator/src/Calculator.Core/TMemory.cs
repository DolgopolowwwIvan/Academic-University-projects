namespace Calculator.Core;

// Память калькулятора
#nullable disable
public class TMemory
{
    private TANumber _storedValue;
    private bool _hasValue;
    private string _error;

    public TMemory()
    {
        _storedValue = new TANumber(0);
        _hasValue = false;
        _error = string.Empty;
    }

    // Есть значение в памяти
    public bool HasValue => _hasValue;

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

    // Сохранить значение в память (M+)
    public void Store(TANumber value)
    {
        if (value == null)
        {
            _error = "Пустое значение";
            return;
        }

        _storedValue = value.Copy();
        _hasValue = true;
        _error = string.Empty;
    }

    // Добавить к значению в памяти (M+)
    public void Add(TANumber value)
    {
        if (value == null)
        {
            _error = "Пустое значение";
            return;
        }

        if (!_hasValue)
        {
            _storedValue = value.Copy();
        }
        else
        {
            _storedValue.Value += value.Value;
        }

        _hasValue = true;
        _error = string.Empty;
    }

    // Вычесть из значения в памяти (M-)
    public void Subtract(TANumber value)
    {
        if (value == null)
        {
            _error = "Пустое значение";
            return;
        }

        if (!_hasValue)
        {
            _storedValue = new TANumber(-value.Value);
        }
        else
        {
            _storedValue.Value -= value.Value;
        }

        _hasValue = true;
        _error = string.Empty;
    }

    // Извлечь значение из памяти (MR)
    public TANumber Recall()
    {
        if (!_hasValue || _storedValue == null)
        {
            _error = "Память пуста";
            return new TANumber(0);
        }

        _error = string.Empty;
        return _storedValue.Copy();
    }

    // Очистить память (MC)
    public void Clear()
    {
        _storedValue?.Clear();
        _hasValue = false;
        _error = string.Empty;
    }

    // Получить состояние памяти для отображения
    public string GetStateString()
    {
        return _hasValue ? "M" : string.Empty;
    }

    // Получить значение памяти как строку
    public string GetValueString()
    {
        if (!_hasValue || _storedValue == null)
        {
            return "0";
        }

        return _storedValue.Value.ToString(System.Globalization.CultureInfo.InvariantCulture);
    }

    // Получить число из памяти
    public TANumber GetValue()
    {
        if (!_hasValue || _storedValue == null)
        {
            return new TANumber(0);
        }

        return _storedValue.Copy();
    }

    // Сбросить состояние
    public void ReSet()
    {
        Clear();
    }

    ~TMemory()
    {
        _storedValue = null;
    }
}
#nullable enable
