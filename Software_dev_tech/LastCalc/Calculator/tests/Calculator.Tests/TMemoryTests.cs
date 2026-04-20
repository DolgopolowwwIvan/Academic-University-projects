using Calculator.Core;

namespace Calculator.Tests;

// Тесты для класса TMemory
public class TMemoryTests
{
    [Fact]
    public void Constructor_Default_InitializesEmpty()
    {
        // Инициализация
        var memory = new TMemory();

        // Проверка
        Assert.False(memory.HasValue);
        Assert.Empty(memory.Error);
    }

    [Fact]
    public void Store_SavesValue()
    {
        // Инициализация
        var memory = new TMemory();
        var number = new TANumber(42);

        // Действие
        memory.Store(number);

        // Проверка
        Assert.True(memory.HasValue);
    }

    [Fact]
    public void Store_NullValue_SetsError()
    {
        // Инициализация
        var memory = new TMemory();
        TANumber? number = null;

        // Действие
        memory.Store(number!);

        // Проверка
        Assert.NotEmpty(memory.Error);
    }

    [Fact]
    public void Add_ToEmptyMemory_StoresValue()
    {
        // Инициализация
        var memory = new TMemory();
        var number = new TANumber(10);

        // Действие
        memory.Add(number);

        // Проверка
        Assert.True(memory.HasValue);
        var recalled = memory.Recall();
        Assert.Equal(10, recalled.Value);
    }

    [Fact]
    public void Add_ToExistingMemory_AddsValue()
    {
        // Инициализация
        var memory = new TMemory();
        memory.Store(new TANumber(10));
        var number = new TANumber(5);

        // Действие
        memory.Add(number);

        // Проверка
        var recalled = memory.Recall();
        Assert.Equal(15, recalled.Value);
    }

    [Fact]
    public void Subtract_FromEmptyMemory_StoresNegativeValue()
    {
        // Инициализация
        var memory = new TMemory();
        var number = new TANumber(10);

        // Действие
        memory.Subtract(number);

        // Проверка
        var recalled = memory.Recall();
        Assert.Equal(-10, recalled.Value);
    }

    [Fact]
    public void Subtract_FromExistingMemory_SubtractsValue()
    {
        // Инициализация
        var memory = new TMemory();
        memory.Store(new TANumber(20));
        var number = new TANumber(5);

        // Действие
        memory.Subtract(number);

        // Проверка
        var recalled = memory.Recall();
        Assert.Equal(15, recalled.Value);
    }

    [Fact]
    public void Recall_EmptyMemory_ReturnsZeroAndSetsError()
    {
        // Инициализация
        var memory = new TMemory();

        // Действие
        var result = memory.Recall();

        // Проверка
        Assert.Equal(0, result.Value);
        Assert.NotEmpty(memory.Error);
    }

    [Fact]
    public void Recall_WithValue_ReturnsStoredValue()
    {
        // Инициализация
        var memory = new TMemory();
        memory.Store(new TANumber(99.5));

        // Действие
        var result = memory.Recall();

        // Проверка
        Assert.Equal(99.5, result.Value);
        Assert.Empty(memory.Error);
    }

    [Fact]
    public void Clear_EmptiesMemory()
    {
        // Инициализация
        var memory = new TMemory();
        memory.Store(new TANumber(42));

        // Действие
        memory.Clear();

        // Проверка
        Assert.False(memory.HasValue);
        Assert.Empty(memory.Error);
    }

    [Fact]
    public void GetStateString_Empty_ReturnsEmpty()
    {
        // Инициализация
        var memory = new TMemory();

        // Действие
        var result = memory.GetStateString();

        // Проверка
        Assert.Empty(result);
    }

    [Fact]
    public void GetStateString_WithValue_ReturnsM()
    {
        // Инициализация
        var memory = new TMemory();
        memory.Store(new TANumber(42));

        // Действие
        var result = memory.GetStateString();

        // Проверка
        Assert.Equal("M", result);
    }

    [Fact]
    public void GetValueString_Empty_ReturnsZero()
    {
        // Инициализация
        var memory = new TMemory();

        // Действие
        var result = memory.GetValueString();

        // Проверка
        Assert.Equal("0", result);
    }

    [Fact]
    public void GetValueString_WithValue_ReturnsValue()
    {
        // Инициализация
        var memory = new TMemory();
        memory.Store(new TANumber(123.45));

        // Действие
        var result = memory.GetValueString();

        // Проверка
        Assert.Equal("123.45", result);
    }

    [Fact]
    public void ReSet_ClearsMemory()
    {
        // Инициализация
        var memory = new TMemory();
        memory.Store(new TANumber(42));

        // Действие
        memory.ReSet();

        // Проверка
        Assert.False(memory.HasValue);
    }
}
