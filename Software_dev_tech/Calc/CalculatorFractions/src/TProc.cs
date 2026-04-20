using System;

namespace CalculatorFractions
{
    // Типы двухоперандных операций
    // Процессор: TOprtn = (None, Add, Sub, Mul, Dvd)
    public enum TOprtn
    {
        opNone,  // Нет операции
        opAdd,   // Сложение (+)
        opSub,   // Вычитание (-)
        opMul,   // Умножение (*)
        opDiv    // Деление (/)
    }

    // Типы однооперандных функций
    public enum TFunc
    {
        fnRev,  // Обратное значение (1/x)
        fnSqr   // Возведение в квадрат
    }

    // TProc - Параметризованный класс процессора для выполнения операций над типом T
    // Выполняет двухоперандные операции и однооперандные функции над значениями типа T
    public class TProc<T> where T : class, new()
    {
        private T FLop_Res;    // Левый операнд и результат
        private T FRop;        // Правый операнд
        private TOprtn FOperation;  // Текущая операция

        // Конструктор
        // Инициализирует поля объектами типа T со значениями по умолчанию
        // Процессор устанавливается в состояние: "операция не установлена" (opNone)
        public TProc()
        {
            FLop_Res = new T();
            FRop = new T();
            FOperation = TOprtn.opNone;
        }

        // Сброс процессора
        // Поля инициализируются значениями по умолчанию, операция сбрасывается
        public void Reset()
        {
            FLop_Res = new T();
            FRop = new T();
            FOperation = TOprtn.opNone;
        }

        // Сброс операции
        // Процессор устанавливается в состояние: "операция не установлена"
        public void OprtnClear()
        {
            FOperation = TOprtn.opNone;
        }

        // Выполнить операцию
        // Выполняет текущую операцию над FRop и FLop_Res, результат в FLop_Res
        public void OprtnRun()
        {
            if (FOperation == TOprtn.opNone) return;

            dynamic dynLop = FLop_Res;
            dynamic dynRop = FRop;

            switch (FOperation)
            {
                case TOprtn.opAdd:
                    FLop_Res = dynLop.Add(dynRop);
                    break;
                case TOprtn.opSub:
                    FLop_Res = dynLop.Sub(dynRop);
                    break;
                case TOprtn.opMul:
                    FLop_Res = dynLop.Mul(dynRop);
                    break;
                case TOprtn.opDiv:
                    FLop_Res = dynLop.Div(dynRop);
                    break;
            }
        }

        // Вычислить функцию
        // Выполняет функцию Func над FRop, результат сохраняется в FRop
        public void FuncRun(TFunc Func)
        {
            dynamic dynRop = FRop;

            switch (Func)
            {
                case TFunc.fnSqr:
                    FRop = dynRop.Sqr();
                    break;
                case TFunc.fnRev:
                    FRop = dynRop.Rev();
                    break;
            }
        }

        // Читать левый операнд
        public T Lop_Res => FLop_Res;

        // Записать левый операнд
        public void SetLop_Res(T value)
        {
            FLop_Res = value;
        }

        // Читать правый операнд
        public T Rop => FRop;

        // Записать правый операнд
        public void SetRop(T value)
        {
            FRop = value;
        }

        // Читать состояние (текущую операцию)
        public TOprtn Operation => FOperation;

        // Записать состояние (установить операцию)
        public void SetOperation(TOprtn value)
        {
            FOperation = value;
        }

        // Проверка: операция установлена
        public bool IsOperationSet()
        {
            return FOperation != TOprtn.opNone;
        }
    }

    // Преобразование операции в строку
    public static class OprtnExtensions
    {
        public static string ToStringValue(this TOprtn oprtn)
        {
            switch (oprtn)
            {
                case TOprtn.opAdd: return "+";
                case TOprtn.opSub: return "-";
                case TOprtn.opMul: return "*";
                case TOprtn.opDiv: return "/";
                default: return "";
            }
        }
    }
}
