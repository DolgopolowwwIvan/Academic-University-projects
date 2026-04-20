using Calculator.Core;

namespace Calculator.Tests;

// Тесты для класса TProc
public class TProcTests
{
    [Fact]
    public void Constructor_Default_InitializesOperands()
    {
        // Инициализация
        var proc = new TProc();

        // Проверка
        Assert.NotNull(proc.Lopd_Res);
        Assert.NotNull(proc.Ropd);
        Assert.Equal(TOperation.None, proc.Operation);
        Assert.Empty(proc.Error);
    }

    [Fact]
    public void OprtnRun_Add_ReturnsCorrectResult()
    {
        // Инициализация
        var proc = new TProc();
        proc.Lopd_Res = new TANumber(10);
        proc.Ropd = new TANumber(5);
        proc.Operation = TOperation.Add;

        // Действие
        proc.OprtnRun();

        // Проверка
        Assert.Equal(15, proc.Lopd_Res.Value);
        Assert.Empty(proc.Error);
    }

    [Fact]
    public void OprtnRun_Subtract_ReturnsCorrectResult()
    {
        // Инициализация
        var proc = new TProc();
        proc.Lopd_Res = new TANumber(10);
        proc.Ropd = new TANumber(5);
        proc.Operation = TOperation.Subtract;

        // Действие
        proc.OprtnRun();

        // Проверка
        Assert.Equal(5, proc.Lopd_Res.Value);
    }

    [Fact]
    public void OprtnRun_Multiply_ReturnsCorrectResult()
    {
        // Инициализация
        var proc = new TProc();
        proc.Lopd_Res = new TANumber(6);
        proc.Ropd = new TANumber(7);
        proc.Operation = TOperation.Multiply;

        // Действие
        proc.OprtnRun();

        // Проверка
        Assert.Equal(42, proc.Lopd_Res.Value);
    }

    [Fact]
    public void OprtnRun_Divide_ReturnsCorrectResult()
    {
        // Инициализация
        var proc = new TProc();
        proc.Lopd_Res = new TANumber(20);
        proc.Ropd = new TANumber(4);
        proc.Operation = TOperation.Divide;

        // Действие
        proc.OprtnRun();

        // Проверка
        Assert.Equal(5, proc.Lopd_Res.Value);
    }

    [Fact]
    public void OprtnRun_DivideByZero_SetsError()
    {
        // Инициализация
        var proc = new TProc();
        proc.Lopd_Res = new TANumber(10);
        proc.Ropd = new TANumber(0);
        proc.Operation = TOperation.Divide;

        // Действие
        proc.OprtnRun();

        // Проверка
        Assert.NotEmpty(proc.Error);
    }

    [Fact]
    public void OprtnRun_Power_ReturnsCorrectResult()
    {
        // Инициализация
        var proc = new TProc();
        proc.Lopd_Res = new TANumber(2);
        proc.Ropd = new TANumber(3);
        proc.Operation = TOperation.Power;

        // Действие
        proc.OprtnRun();

        // Проверка
        Assert.Equal(8, proc.Lopd_Res.Value);
    }

    [Fact]
    public void OprtnRun_Modulo_ReturnsCorrectResult()
    {
        // Инициализация
        var proc = new TProc();
        proc.Lopd_Res = new TANumber(17);
        proc.Ropd = new TANumber(5);
        proc.Operation = TOperation.Modulo;

        // Действие
        proc.OprtnRun();

        // Проверка
        Assert.Equal(2, proc.Lopd_Res.Value);
    }

    [Fact]
    public void FuncRun_Sin_ReturnsCorrectResult()
    {
        // Инициализация
        var proc = new TProc();
        proc.Lopd_Res = new TANumber(0);

        // Действие
        proc.FuncRun(TFunction.Sin);

        // Проверка
        Assert.Equal(0, proc.Lopd_Res.Value, 10);
    }

    [Fact]
    public void FuncRun_Cos_ReturnsCorrectResult()
    {
        // Инициализация
        var proc = new TProc();
        proc.Lopd_Res = new TANumber(0);

        // Действие
        proc.FuncRun(TFunction.Cos);

        // Проверка
        Assert.Equal(1, proc.Lopd_Res.Value, 10);
    }

    [Fact]
    public void FuncRun_Sqrt_ReturnsCorrectResult()
    {
        // Инициализация
        var proc = new TProc();
        proc.Lopd_Res = new TANumber(16);

        // Действие
        proc.FuncRun(TFunction.Sqrt);

        // Проверка
        Assert.Equal(4, proc.Lopd_Res.Value);
    }

    [Fact]
    public void FuncRun_Sqrt_NegativeNumber_SetsError()
    {
        // Инициализация
        var proc = new TProc();
        proc.Lopd_Res = new TANumber(-4);

        // Действие
        proc.FuncRun(TFunction.Sqrt);

        // Проверка
        Assert.NotEmpty(proc.Error);
    }

    [Fact]
    public void FuncRun_Log_ReturnsCorrectResult()
    {
        // Инициализация
        var proc = new TProc();
        proc.Lopd_Res = new TANumber(100);

        // Действие
        proc.FuncRun(TFunction.Log);

        // Проверка
        Assert.Equal(2, proc.Lopd_Res.Value, 10);
    }

    [Fact]
    public void FuncRun_Ln_ReturnsCorrectResult()
    {
        // Инициализация
        var proc = new TProc();
        proc.Lopd_Res = new TANumber(Math.E);

        // Действие
        proc.FuncRun(TFunction.Ln);

        // Проверка
        Assert.Equal(1, proc.Lopd_Res.Value, 10);
    }

    [Fact]
    public void FuncRun_Abs_ReturnsCorrectResult()
    {
        // Инициализация
        var proc = new TProc();
        proc.Lopd_Res = new TANumber(-42);

        // Действие
        proc.FuncRun(TFunction.Abs);

        // Проверка
        Assert.Equal(42, proc.Lopd_Res.Value);
    }

    [Fact]
    public void FuncRun_Exp_ReturnsCorrectResult()
    {
        // Инициализация
        var proc = new TProc();
        proc.Lopd_Res = new TANumber(1);

        // Действие
        proc.FuncRun(TFunction.Exp);

        // Проверка
        Assert.Equal(Math.E, proc.Lopd_Res.Value, 10);
    }

    [Fact]
    public void ReSet_ClearsAllState()
    {
        // Инициализация
        var proc = new TProc();
        proc.Lopd_Res = new TANumber(10);
        proc.Ropd = new TANumber(5);
        proc.Operation = TOperation.Add;

        // Действие
        proc.ReSet();

        // Проверка
        Assert.Equal(0, proc.Lopd_Res.Value);
        Assert.Equal(0, proc.Ropd.Value);
        Assert.Equal(TOperation.None, proc.Operation);
        Assert.Empty(proc.Error);
    }

    [Fact]
    public void Error_Setter_UpdatesError()
    {
        // Инициализация
        var proc = new TProc();
        string errorMessage = "Test error";

        // Действие
        proc.Error = errorMessage;

        // Проверка
        Assert.Equal(errorMessage, proc.Error);
    }

    [Fact]
    public void ClearError_RemovesError()
    {
        // Инициализация
        var proc = new TProc();
        proc.Error = "Test error";

        // Действие
        proc.ClearError();

        // Проверка
        Assert.Empty(proc.Error);
    }
}
