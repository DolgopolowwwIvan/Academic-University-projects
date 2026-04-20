namespace Calculator.Core;

// Базовый класс для представления числа в калькуляторе
#nullable disable
public class TANumber
{
    protected double _value;

    public TANumber()
    {
        _value = 0;
    }

    public TANumber(double value)
    {
        _value = value;
    }

    // Значение числа
    public virtual double Value
    {
        get => _value;
        set => _value = value;
    }

    // Копировать число
    public virtual TANumber Copy()
    {
        return new TANumber(_value);
    }

    public override string ToString()
    {
        return _value.ToString();
    }

    // Очистить значение
    public virtual void Clear()
    {
        _value = 0;
    }
}
#nullable enable
