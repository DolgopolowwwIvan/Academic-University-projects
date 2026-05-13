namespace Calculator.Core.Editors;

/// <summary>
/// Абстрактный базовый класс для редакторов чисел
/// </summary>
public abstract class AEditor
{
    protected string _stringValue;
    protected string _separator;
    protected bool _separatorExists;
    protected string _zeroString;
    protected string _sign;
    protected string _error;

    // Команды управления редактором
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
    public const uint cSeparatorC = 12;
    public const uint cSeparatorFrac = 13;
    public const uint cSeparatorComplex = 14;
    public const uint cBS = 15;
    public const uint CE = 16;

    protected AEditor()
    {
        _stringValue = string.Empty;
        _separator = ".";
        _separatorExists = false;
        _zeroString = "0";
        _sign = string.Empty;
        _error = string.Empty;
    }

    protected AEditor(string zeroString, string separator)
    {
        _stringValue = string.Empty;
        _separator = separator;
        _separatorExists = false;
        _zeroString = zeroString;
        _sign = string.Empty;
        _error = string.Empty;
    }

    /// <summary>
    /// Строковое представление редактируемого числа
    /// </summary>
    public virtual string StringValue
    {
        get => _stringValue;
        set => _stringValue = value ?? string.Empty;
    }

    /// <summary>
    /// Наличие разделителя в числе
    /// </summary>
    public bool SeparatorExists => _separatorExists;

    /// <summary>
    /// Ошибка
    /// </summary>
    public string Error
    {
        get => _error;
        set => _error = value;
    }

    /// <summary>
    /// Очистить ошибку
    /// </summary>
    public void ClearError()
    {
        _error = string.Empty;
    }

    /// <summary>
    /// Проверка, равно ли редактируемое число нулю
    /// </summary>
    public abstract bool IsZero();

    /// <summary>
    /// Добавить разделитель
    /// </summary>
    public abstract string AddSeparator(uint a = 0);

    /// <summary>
    /// Добавить цифру
    /// </summary>
    public abstract string AddDigit(uint a);

    /// <summary>
    /// Изменить знак
    /// </summary>
    public abstract string AddSigne(uint a = 0);

    /// <summary>
    /// Удалить крайний правый символ (BackSpace)
    /// </summary>
    public abstract string BackSpace();

    /// <summary>
    /// Очистить ввод (установить нулевое значение)
    /// </summary>
    public abstract string Clear();

    /// <summary>
    /// Редактировать по команде
    /// </summary>
    public virtual string Edit(uint command)
    {
        return command switch
        {
            >= AEditor.cZero and <= AEditor.cNine => AddDigit(command),
            cSign => AddSigne(),
            cSeparatorFR or cSeparatorC or cSeparatorFrac or cSeparatorComplex => AddSeparator(command),
            cBS => BackSpace(),
            CE => Clear(),
            _ => _stringValue
        };
    }

    /// <summary>
    /// Получить строку редактируемого числа
    /// </summary>
    public virtual string ReadStringAsString()
    {
        return _stringValue;
    }

    /// <summary>
    /// Установить строку редактируемого числа
    /// </summary>
    public virtual void WriteStringAsString(string value)
    {
        _stringValue = value ?? string.Empty;
    }

    /// <summary>
    /// Сбросить состояние
    /// </summary>
    public virtual void ReSet()
    {
        Clear();
    }

    /// <summary>
    /// Установить число (абстрактный метод)
    /// </summary>
    protected abstract void SetNum(string n);

    /// <summary>
    /// Получить число (абстрактный метод)
    /// </summary>
    protected abstract string GetNum();
}
