namespace Calculator.Core.Numbers;

/// <summary>
/// Абстрактный базовый класс для всех типов чисел в универсальном калькуляторе
/// </summary>
public abstract class TANumber
{
    protected string _stringValue;

    protected TANumber()
    {
        _stringValue = "0";
    }

    protected TANumber(string value)
    {
        _stringValue = value ?? "0";
    }

    /// <summary>
    /// Строковое представление числа
    /// </summary>
    public virtual string StringValue
    {
        get => _stringValue;
        set => _stringValue = value ?? "0";
    }

    /// <summary>
    /// Проверка, равно ли число нулю
    /// </summary>
    public abstract bool EqZero();

    /// <summary>
    /// Копировать число
    /// </summary>
    public abstract TANumber Copy();

    /// <summary>
    /// Сложить с другим числом
    /// </summary>
    public abstract TANumber Add(TANumber other);

    /// <summary>
    /// Вычесть другое число
    /// </summary>
    public abstract TANumber Subtract(TANumber other);

    /// <summary>
    /// Перемножить с другим числом
    /// </summary>
    public abstract TANumber Multiply(TANumber other);

    /// <summary>
    /// Поделить на другое число
    /// </summary>
    public abstract TANumber Divide(TANumber other);

    /// <summary>
    /// Проверка равенства с другим числом
    /// </summary>
    public abstract bool EqualsNumber(TANumber other);

    /// <summary>
    /// Квадрат числа
    /// </summary>
    public abstract TANumber Sqr();

    /// <summary>
    /// Обратное число (со знаком минус)
    /// </summary>
    public abstract TANumber Reverse();

    /// <summary>
    /// Получить значение в формате строки
    /// </summary>
    public abstract string ReadNumberAsString();

    /// <summary>
    /// Установить значение из строки
    /// </summary>
    public abstract void WriteNumberFromString(string value);

    /// <summary>
    /// Получить тип числа
    /// </summary>
    public abstract NumberType GetType();

    /// <summary>
    /// Преобразовать в вещественное число (для отображения)
    /// </summary>
    public abstract double ToDouble();

    public override string ToString()
    {
        return _stringValue;
    }
}

/// <summary>
/// Типы чисел в универсальном калькуляторе
/// </summary>
public enum NumberType
{
    Real,        // Вещественные p-ичные числа
    Fraction,    // Рациональные дроби
    Complex      // Комплексные числа
}

