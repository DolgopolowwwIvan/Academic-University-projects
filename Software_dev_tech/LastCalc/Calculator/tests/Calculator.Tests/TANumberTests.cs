using Calculator.Core;

namespace Calculator.Tests;

// Тесты для класса TANumber
public class TANumberTests
{
    [Fact]
    public void Constructor_Default_SetsValueToZero()
    {
        // Инициализация и действие
        var number = new TANumber();

        // Проверка
        Assert.Equal(0, number.Value);
    }

    [Fact]
    public void Constructor_WithValue_SetsValue()
    {
        // Инициализация
        double expectedValue = 42.5;

        // Действие
        var number = new TANumber(expectedValue);

        // Проверка
        Assert.Equal(expectedValue, number.Value);
    }

    [Fact]
    public void Value_Setter_UpdatesValue()
    {
        // Инициализация
        var number = new TANumber();
        double newValue = 100.0;

        // Действие
        number.Value = newValue;

        // Проверка
        Assert.Equal(newValue, number.Value);
    }

    [Fact]
    public void Copy_CreatesNewInstanceWithSameValue()
    {
        // Инициализация
        var original = new TANumber(42.5);

        // Действие
        var copy = original.Copy();

        // Проверка
        Assert.Equal(original.Value, copy.Value);
        Assert.NotSame(original, copy);
    }

    [Fact]
    public void ToString_ReturnsValueAsString()
    {
        // Инициализация
        var number = new TANumber(123.45);

        // Действие
        var result = number.Value.ToString(System.Globalization.CultureInfo.InvariantCulture);

        // Проверка
        Assert.Equal("123.45", result);
    }

    [Fact]
    public void Clear_ResetsValueToZero()
    {
        // Инициализация
        var number = new TANumber(99.9);

        // Действие
        number.Clear();

        // Проверка
        Assert.Equal(0, number.Value);
    }
}
