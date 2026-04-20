using Calculator.Core;

namespace Calculator.Tests;

// Тесты для класса TCtrl
public class TCtrlTests
{
    [Fact]
    public void Constructor_Default_InitializesComponents()
    {
        // Инициализация
        var ctrl = new TCtrl();

        // Проверка
        Assert.NotNull(ctrl.Editor);
        Assert.NotNull(ctrl.Processor);
        Assert.NotNull(ctrl.Memory);
        Assert.NotNull(ctrl.Number);
        Assert.Equal(TCtrlState.cStart, ctrl.State);
    }

    [Fact]
    public void ExecuteCalculatorCommand_Digit_UpdatesDisplay()
    {
        // Инициализация
        var ctrl = new TCtrl();
        string buffer = string.Empty;
        string memoryState = string.Empty;

        // Действие
        var result = ctrl.ExecuteCalculatorCommand(TCtrl.CMD_DIGIT_5, ref buffer, ref memoryState);

        // Проверка
        Assert.Equal("5", result);
        Assert.Equal(TCtrlState.cEditing, ctrl.State);
    }

    [Fact]
    public void ExecuteCalculatorCommand_MultipleDigits_AppendsDigits()
    {
        // Инициализация
        var ctrl = new TCtrl();
        string buffer = string.Empty;
        string memoryState = string.Empty;

        // Действие
        ctrl.ExecuteCalculatorCommand(TCtrl.CMD_DIGIT_1, ref buffer, ref memoryState);
        ctrl.ExecuteCalculatorCommand(TCtrl.CMD_DIGIT_2, ref buffer, ref memoryState);
        var result = ctrl.ExecuteCalculatorCommand(TCtrl.CMD_DIGIT_3, ref buffer, ref memoryState);

        // Проверка
        Assert.Equal("123", result);
    }

    [Fact]
    public void ExecuteCalculatorCommand_DecimalPoint_AddsPoint()
    {
        // Инициализация
        var ctrl = new TCtrl();
        string buffer = string.Empty;
        string memoryState = string.Empty;

        // Действие
        ctrl.ExecuteCalculatorCommand(TCtrl.CMD_DIGIT_3, ref buffer, ref memoryState);
        var result = ctrl.ExecuteCalculatorCommand(TCtrl.CMD_DECIMAL_POINT, ref buffer, ref memoryState);

        // Проверка
        Assert.Equal("3.", result);
    }

    [Fact]
    public void ExecuteCalculatorCommand_Add_SetsOperation()
    {
        // Инициализация
        var ctrl = new TCtrl();
        string buffer = string.Empty;
        string memoryState = string.Empty;

        // Действие
        ctrl.ExecuteCalculatorCommand(TCtrl.CMD_DIGIT_5, ref buffer, ref memoryState);
        var result = ctrl.ExecuteCalculatorCommand(TCtrl.CMD_ADD, ref buffer, ref memoryState);

        // Проверка
        Assert.Equal("5", result);
        Assert.Equal(TCtrlState.cOpChange, ctrl.State);
        Assert.Equal(TOperation.Add, ctrl.Processor.Operation);
    }

    [Fact]
    public void ExecuteCalculatorCommand_Equals_PerformsAddition()
    {
        // Инициализация
        var ctrl = new TCtrl();
        string buffer = string.Empty;
        string memoryState = string.Empty;

        // Действие
        ctrl.ExecuteCalculatorCommand(TCtrl.CMD_DIGIT_5, ref buffer, ref memoryState);
        ctrl.ExecuteCalculatorCommand(TCtrl.CMD_ADD, ref buffer, ref memoryState);
        ctrl.ExecuteCalculatorCommand(TCtrl.CMD_DIGIT_3, ref buffer, ref memoryState);
        var result = ctrl.ExecuteCalculatorCommand(TCtrl.CMD_EQUALS, ref buffer, ref memoryState);

        // Проверка
        Assert.Equal("8", result);
        Assert.Equal(TCtrlState.cExpDone, ctrl.State);
    }

    [Fact]
    public void ExecuteCalculatorCommand_Subtract_PerformsSubtraction()
    {
        // Инициализация
        var ctrl = new TCtrl();
        string buffer = string.Empty;
        string memoryState = string.Empty;

        // Действие
        ctrl.ExecuteCalculatorCommand(TCtrl.CMD_DIGIT_1, ref buffer, ref memoryState);
        ctrl.ExecuteCalculatorCommand(TCtrl.CMD_DIGIT_0, ref buffer, ref memoryState);
        ctrl.ExecuteCalculatorCommand(TCtrl.CMD_SUBTRACT, ref buffer, ref memoryState);
        ctrl.ExecuteCalculatorCommand(TCtrl.CMD_DIGIT_4, ref buffer, ref memoryState);
        var result = ctrl.ExecuteCalculatorCommand(TCtrl.CMD_EQUALS, ref buffer, ref memoryState);

        // Проверка
        Assert.Equal("6", result);
    }

    [Fact]
    public void ExecuteCalculatorCommand_Multiply_PerformsMultiplication()
    {
        // Инициализация
        var ctrl = new TCtrl();
        string buffer = string.Empty;
        string memoryState = string.Empty;

        // Действие
        ctrl.ExecuteCalculatorCommand(TCtrl.CMD_DIGIT_6, ref buffer, ref memoryState);
        ctrl.ExecuteCalculatorCommand(TCtrl.CMD_MULTIPLY, ref buffer, ref memoryState);
        ctrl.ExecuteCalculatorCommand(TCtrl.CMD_DIGIT_7, ref buffer, ref memoryState);
        var result = ctrl.ExecuteCalculatorCommand(TCtrl.CMD_EQUALS, ref buffer, ref memoryState);

        // Проверка
        Assert.Equal("42", result);
    }

    [Fact]
    public void ExecuteCalculatorCommand_Divide_PerformsDivision()
    {
        // Инициализация
        var ctrl = new TCtrl();
        string buffer = string.Empty;
        string memoryState = string.Empty;

        // Действие
        ctrl.ExecuteCalculatorCommand(TCtrl.CMD_DIGIT_2, ref buffer, ref memoryState);
        ctrl.ExecuteCalculatorCommand(TCtrl.CMD_DIGIT_0, ref buffer, ref memoryState);
        ctrl.ExecuteCalculatorCommand(TCtrl.CMD_DIVIDE, ref buffer, ref memoryState);
        ctrl.ExecuteCalculatorCommand(TCtrl.CMD_DIGIT_4, ref buffer, ref memoryState);
        var result = ctrl.ExecuteCalculatorCommand(TCtrl.CMD_EQUALS, ref buffer, ref memoryState);

        // Проверка
        Assert.Equal("5", result);
    }

    [Fact]
    public void ExecuteCalculatorCommand_Sin_PerformsFunction()
    {
        // Инициализация
        var ctrl = new TCtrl();
        string buffer = string.Empty;
        string memoryState = string.Empty;

        // Действие
        ctrl.ExecuteCalculatorCommand(TCtrl.CMD_DIGIT_0, ref buffer, ref memoryState);
        var result = ctrl.ExecuteCalculatorCommand(TCtrl.CMD_FUNC_SIN, ref buffer, ref memoryState);

        // Проверка
        Assert.Equal("0", result);
        Assert.Equal(TCtrlState.FunDone, ctrl.State);
    }

    [Fact]
    public void ExecuteCalculatorCommand_Sqrt_PerformsFunction()
    {
        // Инициализация
        var ctrl = new TCtrl();
        string buffer = string.Empty;
        string memoryState = string.Empty;

        // Действие
        ctrl.ExecuteCalculatorCommand(TCtrl.CMD_DIGIT_1, ref buffer, ref memoryState);
        ctrl.ExecuteCalculatorCommand(TCtrl.CMD_DIGIT_6, ref buffer, ref memoryState);
        var result = ctrl.ExecuteCalculatorCommand(TCtrl.CMD_FUNC_SQRT, ref buffer, ref memoryState);

        // Проверка
        Assert.Equal("4", result);
    }

    [Fact]
    public void ExecuteCalculatorCommand_Clear_ResetsEditor()
    {
        // Инициализация
        var ctrl = new TCtrl();
        string buffer = string.Empty;
        string memoryState = string.Empty;

        // Действие
        ctrl.ExecuteCalculatorCommand(TCtrl.CMD_DIGIT_5, ref buffer, ref memoryState);
        var result = ctrl.ExecuteCalculatorCommand(TCtrl.CMD_CLEAR, ref buffer, ref memoryState);

        // Проверка
        Assert.Equal("0", result);
        Assert.Equal(TCtrlState.cStart, ctrl.State);
    }

    [Fact]
    public void ExecuteCalculatorCommand_ClearAll_ResetsAll()
    {
        // Инициализация
        var ctrl = new TCtrl();
        string buffer = string.Empty;
        string memoryState = string.Empty;

        // Действие
        ctrl.ExecuteCalculatorCommand(TCtrl.CMD_DIGIT_5, ref buffer, ref memoryState);
        ctrl.ExecuteCalculatorCommand(TCtrl.CMD_ADD, ref buffer, ref memoryState);
        var result = ctrl.ExecuteCalculatorCommand(TCtrl.CMD_CLEAR_ALL, ref buffer, ref memoryState);

        // Проверка
        Assert.Equal("0", result);
        Assert.Equal(TCtrlState.cStart, ctrl.State);
    }

    [Fact]
    public void ExecuteCalculatorCommand_MemoryStore_StoresValue()
    {
        // Инициализация
        var ctrl = new TCtrl();
        string buffer = string.Empty;
        string memoryState = string.Empty;

        // Действие
        ctrl.ExecuteCalculatorCommand(TCtrl.CMD_DIGIT_4, ref buffer, ref memoryState);
        ctrl.ExecuteCalculatorCommand(TCtrl.CMD_DIGIT_2, ref buffer, ref memoryState);
        ctrl.ExecuteCalculatorCommand(TCtrl.CMD_MEMORY_STORE, ref buffer, ref memoryState);

        // Проверка
        Assert.True(ctrl.Memory.HasValue);
        Assert.Equal("M", memoryState);
    }

    [Fact]
    public void ExecuteCalculatorCommand_MemoryRecall_RetrievesValue()
    {
        // Инициализация
        var ctrl = new TCtrl();
        string buffer = string.Empty;
        string memoryState = string.Empty;

        // Действие
        ctrl.ExecuteCalculatorCommand(TCtrl.CMD_DIGIT_1, ref buffer, ref memoryState);
        ctrl.ExecuteCalculatorCommand(TCtrl.CMD_DIGIT_0, ref buffer, ref memoryState);
        ctrl.ExecuteCalculatorCommand(TCtrl.CMD_MEMORY_STORE, ref buffer, ref memoryState);
        ctrl.ExecuteCalculatorCommand(TCtrl.CMD_CLEAR, ref buffer, ref memoryState);
        var result = ctrl.ExecuteCalculatorCommand(TCtrl.CMD_MEMORY_RECALL, ref buffer, ref memoryState);

        // Проверка
        Assert.Equal("10", result);
    }

    [Fact]
    public void ExecuteCalculatorCommand_MemoryClear_EmptiesMemory()
    {
        // Инициализация
        var ctrl = new TCtrl();
        string buffer = string.Empty;
        string memoryState = string.Empty;

        // Действие
        ctrl.ExecuteCalculatorCommand(TCtrl.CMD_DIGIT_5, ref buffer, ref memoryState);
        ctrl.ExecuteCalculatorCommand(TCtrl.CMD_MEMORY_STORE, ref buffer, ref memoryState);
        ctrl.ExecuteCalculatorCommand(TCtrl.CMD_MEMORY_CLEAR, ref buffer, ref memoryState);

        // Проверка
        Assert.False(ctrl.Memory.HasValue);
        Assert.Equal("", memoryState);
    }

    [Fact]
    public void ExecuteCalculatorCommand_MemoryAdd_AddsToMemory()
    {
        // Инициализация
        var ctrl = new TCtrl();
        string buffer = string.Empty;
        string memoryState = string.Empty;

        // Действие
        ctrl.ExecuteCalculatorCommand(TCtrl.CMD_DIGIT_1, ref buffer, ref memoryState);
        ctrl.ExecuteCalculatorCommand(TCtrl.CMD_DIGIT_0, ref buffer, ref memoryState);
        ctrl.ExecuteCalculatorCommand(TCtrl.CMD_MEMORY_STORE, ref buffer, ref memoryState);
        ctrl.ExecuteCalculatorCommand(TCtrl.CMD_DIGIT_5, ref buffer, ref memoryState);
        ctrl.ExecuteCalculatorCommand(TCtrl.CMD_MEMORY_ADD, ref buffer, ref memoryState);

        // Проверка
        var recalled = ctrl.Memory.Recall();
        Assert.Equal(15.0, recalled.Value, 1);
    }

    [Fact]
    public void ExecuteCalculatorCommand_Paste_RestoresValue()
    {
        // Инициализация
        var ctrl = new TCtrl();
        string buffer = "42";
        string memoryState = string.Empty;

        // Действие
        ctrl.ExecuteCalculatorCommand(TCtrl.CMD_PASTE, ref buffer, ref memoryState);

        // Проверка
        Assert.Equal(42, ctrl.Number.Value);
    }

    [Fact]
    public void ExecuteCalculatorCommand_Copy_StoresInBuffer()
    {
        // Инициализация
        var ctrl = new TCtrl();
        string buffer = string.Empty;
        string memoryState = string.Empty;

        // Действие
        ctrl.ExecuteCalculatorCommand(TCtrl.CMD_DIGIT_1, ref buffer, ref memoryState);
        ctrl.ExecuteCalculatorCommand(TCtrl.CMD_DIGIT_2, ref buffer, ref memoryState);
        ctrl.ExecuteCalculatorCommand(TCtrl.CMD_COPY, ref buffer, ref memoryState);

        // Проверка
        Assert.Equal(42, ctrl.Number.Value);
    }

    [Fact]
    public void ExecuteCalculatorCommand_ChainOperations_CalculatesCorrectly()
    {
        // Инициализация
        var ctrl = new TCtrl();
        string buffer = string.Empty;
        string memoryState = string.Empty;

        // Действие
        ctrl.ExecuteCalculatorCommand(TCtrl.CMD_DIGIT_2, ref buffer, ref memoryState);
        ctrl.ExecuteCalculatorCommand(TCtrl.CMD_ADD, ref buffer, ref memoryState);
        ctrl.ExecuteCalculatorCommand(TCtrl.CMD_DIGIT_3, ref buffer, ref memoryState);
        ctrl.ExecuteCalculatorCommand(TCtrl.CMD_MULTIPLY, ref buffer, ref memoryState);
        ctrl.ExecuteCalculatorCommand(TCtrl.CMD_DIGIT_4, ref buffer, ref memoryState);
        var result = ctrl.ExecuteCalculatorCommand(TCtrl.CMD_EQUALS, ref buffer, ref memoryState);

        // Проверка
        Assert.Equal("20", result); // (2 + 3) * 4 = 20
    }

    [Fact]
    public void State_Property_UpdatesState()
    {
        // Инициализация
        var ctrl = new TCtrl();

        // Действие
        ctrl.State = TCtrlState.cError;

        // Проверка
        Assert.Equal(TCtrlState.cError, ctrl.State);
    }
}
