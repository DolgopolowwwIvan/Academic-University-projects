// Тесты для класса TProc (процессор)
using CalculatorFractions;

namespace Tests;

public class TProcTests
{
    [Fact]
    public void Constructor_DefaultState_OperationNotSet()
    {
        var proc = new TProc<TFrac>();
        Assert.Equal(TOprtn.opNone, proc.Operation);
    }

    [Fact]
    public void SetOperation_Add_SetsCorrectOperation()
    {
        var proc = new TProc<TFrac>();
        proc.SetOperation(TOprtn.opAdd);
        Assert.Equal(TOprtn.opAdd, proc.Operation);
    }

    [Fact]
    public void IsOperationSet_AfterSetOperation_ReturnsTrue()
    {
        var proc = new TProc<TFrac>();
        proc.SetOperation(TOprtn.opAdd);
        Assert.True(proc.IsOperationSet());
    }

    [Fact]
    public void IsOperationSet_Default_ReturnsFalse()
    {
        var proc = new TProc<TFrac>();
        Assert.False(proc.IsOperationSet());
    }

    [Fact]
    public void SetLop_Res_SetsLeftOperand()
    {
        var proc = new TProc<TFrac>();
        var value = new TFrac(5, 1);
        proc.SetLop_Res(value);
        Assert.Equal(5, proc.Lop_Res.Numerator);
    }

    [Fact]
    public void SetRop_SetsRightOperand()
    {
        var proc = new TProc<TFrac>();
        var value = new TFrac(3, 4);
        proc.SetRop(value);
        Assert.Equal(3, proc.Rop.Numerator);
        Assert.Equal(4, proc.Rop.Denominator);
    }

    [Fact]
    public void OprtnRun_Add_CalculatesCorrectly()
    {
        var proc = new TProc<TFrac>();
        proc.SetLop_Res(new TFrac(1, 2));
        proc.SetRop(new TFrac(1, 3));
        proc.SetOperation(TOprtn.opAdd);
        proc.OprtnRun();
        Assert.Equal(5, proc.Lop_Res.Numerator);
        Assert.Equal(6, proc.Lop_Res.Denominator);
    }

    [Fact]
    public void OprtnRun_Sub_CalculatesCorrectly()
    {
        var proc = new TProc<TFrac>();
        proc.SetLop_Res(new TFrac(3, 4));
        proc.SetRop(new TFrac(1, 4));
        proc.SetOperation(TOprtn.opSub);
        proc.OprtnRun();
        Assert.Equal(1, proc.Lop_Res.Numerator);
        Assert.Equal(2, proc.Lop_Res.Denominator);
    }

    [Fact]
    public void OprtnRun_Mul_CalculatesCorrectly()
    {
        var proc = new TProc<TFrac>();
        proc.SetLop_Res(new TFrac(1, 2));
        proc.SetRop(new TFrac(2, 3));
        proc.SetOperation(TOprtn.opMul);
        proc.OprtnRun();
        Assert.Equal(1, proc.Lop_Res.Numerator);
        Assert.Equal(3, proc.Lop_Res.Denominator);
    }

    [Fact]
    public void OprtnRun_Div_CalculatesCorrectly()
    {
        var proc = new TProc<TFrac>();
        proc.SetLop_Res(new TFrac(3, 4));
        proc.SetRop(new TFrac(1, 2));
        proc.SetOperation(TOprtn.opDiv);
        proc.OprtnRun();
        Assert.Equal(3, proc.Lop_Res.Numerator);
        Assert.Equal(2, proc.Lop_Res.Denominator);
    }

    [Fact]
    public void FuncRun_Sqr_CalculatesCorrectly()
    {
        var proc = new TProc<TFrac>();
        proc.SetRop(new TFrac(2, 3));
        proc.FuncRun(TFunc.fnSqr);
        Assert.Equal(4, proc.Rop.Numerator);
        Assert.Equal(9, proc.Rop.Denominator);
    }

    [Fact]
    public void FuncRun_Rev_CalculatesCorrectly()
    {
        var proc = new TProc<TFrac>();
        proc.SetRop(new TFrac(3, 4));
        proc.FuncRun(TFunc.fnRev);
        Assert.Equal(4, proc.Rop.Numerator);
        Assert.Equal(3, proc.Rop.Denominator);
    }

    [Fact]
    public void Reset_ClearsOperands()
    {
        var proc = new TProc<TFrac>();
        proc.SetLop_Res(new TFrac(5, 1));
        proc.SetRop(new TFrac(3, 4));
        proc.Reset();
        Assert.Equal(0, proc.Lop_Res.Numerator);
        Assert.Equal(0, proc.Rop.Numerator);
    }

    [Fact]
    public void SetRop_SetsRightOperand()
    {
        var proc = new TProc<TFrac>();
        var value = new TFrac(3, 4);
        proc.SetRop(value);
        Assert.Equal(3, proc.Rop.Num);
        Assert.Equal(4, proc.Rop.Denom);
    }

    [Fact]
    public void OprtnRun_Add_CalculatesCorrectly()
    {
        var proc = new TProc<TFrac>();
        proc.SetLop_Res(new TFrac(1, 2));
        proc.SetRop(new TFrac(1, 3));
        proc.SetOperation(TOprtn.opAdd);
        proc.OprtnRun();
        Assert.Equal(5, proc.Lop_Res.Num);
        Assert.Equal(6, proc.Lop_Res.Denom);
    }

    [Fact]
    public void OprtnRun_Sub_CalculatesCorrectly()
    {
        var proc = new TProc<TFrac>();
        proc.SetLop_Res(new TFrac(3, 4));
        proc.SetRop(new TFrac(1, 4));
        proc.SetOperation(TOprtn.opSub);
        proc.OprtnRun();
        Assert.Equal(1, proc.Lop_Res.Num);
        Assert.Equal(2, proc.Lop_Res.Denom);
    }

    [Fact]
    public void OprtnRun_Mul_CalculatesCorrectly()
    {
        var proc = new TProc<TFrac>();
        proc.SetLop_Res(new TFrac(1, 2));
        proc.SetRop(new TFrac(2, 3));
        proc.SetOperation(TOprtn.opMul);
        proc.OprtnRun();
        Assert.Equal(1, proc.Lop_Res.Num);
        Assert.Equal(3, proc.Lop_Res.Denom);
    }

    [Fact]
    public void OprtnRun_Div_CalculatesCorrectly()
    {
        var proc = new TProc<TFrac>();
        proc.SetLop_Res(new TFrac(3, 4));
        proc.SetRop(new TFrac(1, 2));
        proc.SetOperation(TOprtn.opDiv);
        proc.OprtnRun();
        Assert.Equal(3, proc.Lop_Res.Num);
        Assert.Equal(2, proc.Lop_Res.Denom);
    }

    [Fact]
    public void OprtnRun_DivByZero_ThrowsException()
    {
        var proc = new TProc<TFrac>();
        proc.SetLop_Res(new TFrac(3, 4));
        proc.SetRop(new TFrac(0, 1));
        proc.SetOperation(TOprtn.opDiv);
        Assert.Throws<DivideByZeroException>(() => proc.OprtnRun());
    }

    [Fact]
    public void FuncRun_Sqr_CalculatesCorrectly()
    {
        var proc = new TProc<TFrac>();
        proc.SetRop(new TFrac(2, 3));
        proc.FuncRun(TFunc.fnSqr);
        Assert.Equal(4, proc.Rop.Num);
        Assert.Equal(9, proc.Rop.Denom);
    }

    [Fact]
    public void FuncRun_Rev_CalculatesCorrectly()
    {
        var proc = new TProc<TFrac>();
        proc.SetRop(new TFrac(3, 4));
        proc.FuncRun(TFunc.fnRev);
        Assert.Equal(4, proc.Rop.Num);
        Assert.Equal(3, proc.Rop.Denom);
    }

    [Fact]
    public void FuncRun_Rev_Zero_ThrowsException()
    {
        var proc = new TProc<TFrac>();
        proc.SetRop(new TFrac(0, 1));
        Assert.Throws<DivideByZeroException>(() => proc.FuncRun(TFunc.fnRev));
    }

    [Fact]
    public void Reset_ClearsOperation()
    {
        var proc = new TProc<TFrac>();
        proc.SetOperation(TOprtn.opAdd);
        proc.Reset();
        Assert.Equal(TOprtn.opNone, proc.Operation);
    }

    [Fact]
    public void Reset_ClearsOperands()
    {
        var proc = new TProc<TFrac>();
        proc.SetLop_Res(new TFrac(5, 1));
        proc.SetRop(new TFrac(3, 4));
        proc.Reset();
        Assert.Equal(0, proc.Lop_Res.Num);
        Assert.Equal(0, proc.Rop.Num);
    }
}
