// Тесты для класса TMemory (память)

using CalculatorFractions;

namespace CalculatorFractions.Tests;

public class TMemoryTests
{
    #region Начальное состояние

    [Fact]
    public void Constructor_DefaultState_IsOff()
    {
        var memory = new TMemory<TFrac>();
        Assert.Equal("OFF", memory.ReadState());
    }

    #endregion

    #region Store

    [Fact]
    public void Store_Value_TurnsMemoryOn()
    {
        var memory = new TMemory<TFrac>();
        memory.Store(new TFrac(5, 1));
        Assert.Equal("M", memory.ReadState());
    }

    [Fact]
    public void Store_Value_StoresCorrectValue()
    {
        var memory = new TMemory<TFrac>();
        var value = new TFrac(3, 4);
        memory.Store(value);
        var recalled = memory.Recall();
        Assert.Equal(3, recalled.Num);
        Assert.Equal(4, recalled.Denom);
    }

    [Fact]
    public void Store_OverwritesPreviousValue()
    {
        var memory = new TMemory<TFrac>();
        memory.Store(new TFrac(1, 2));
        memory.Store(new TFrac(3, 4));
        var recalled = memory.Recall();
        Assert.Equal(3, recalled.Num);
        Assert.Equal(4, recalled.Denom);
    }

    #endregion

    #region Recall

    [Fact]
    public void Recall_EmptyMemory_ReturnsZeroFraction()
    {
        var memory = new TMemory<TFrac>();
        var recalled = memory.Recall();
        Assert.Equal(0, recalled.Num);
        Assert.Equal(1, recalled.Denom);
    }

    [Fact]
    public void Recall_ReturnsCopy_NotReference()
    {
        var memory = new TMemory<TFrac>();
        var value = new TFrac(3, 4);
        memory.Store(value);
        var recalled = memory.Recall();
        recalled = new TFrac(1, 1); // Изменяем recalled
        var recalled2 = memory.Recall();
        Assert.Equal(3, recalled2.Num); // Оригинал не изменился
    }

    #endregion

    #region Add

    [Fact]
    public void Add_ToEmptyMemory_StoresValue()
    {
        var memory = new TMemory<TFrac>();
        memory.Add(new TFrac(5, 1));
        var recalled = memory.Recall();
        Assert.Equal(5, recalled.Num);
        Assert.Equal(1, recalled.Denom);
    }

    [Fact]
    public void Add_ToExistingValue_AddsCorrectly()
    {
        var memory = new TMemory<TFrac>();
        memory.Store(new TFrac(3, 4));
        memory.Add(new TFrac(1, 4));
        var recalled = memory.Recall();
        Assert.Equal(1, recalled.Num);
        Assert.Equal(1, recalled.Denom);
    }

    #endregion

    #region Clear

    [Fact]
    public void Clear_TurnsMemoryOff()
    {
        var memory = new TMemory<TFrac>();
        memory.Store(new TFrac(5, 1));
        memory.Clear();
        Assert.Equal("OFF", memory.ReadState());
    }

    [Fact]
    public void Clear_ResetsValue()
    {
        var memory = new TMemory<TFrac>();
        memory.Store(new TFrac(5, 1));
        memory.Clear();
        var recalled = memory.Recall();
        Assert.Equal(0, recalled.Num);
        Assert.Equal(1, recalled.Denom);
    }

    #endregion

    #region IsOn

    [Fact]
    public void IsOn_EmptyMemory_ReturnsFalse()
    {
        var memory = new TMemory<TFrac>();
        Assert.False(memory.IsOn());
    }

    [Fact]
    public void IsOn_AfterStore_ReturnsTrue()
    {
        var memory = new TMemory<TFrac>();
        memory.Store(new TFrac(5, 1));
        Assert.True(memory.IsOn());
    }

    [Fact]
    public void IsOn_AfterClear_ReturnsFalse()
    {
        var memory = new TMemory<TFrac>();
        memory.Store(new TFrac(5, 1));
        memory.Clear();
        Assert.False(memory.IsOn());
    }

    #endregion
}
