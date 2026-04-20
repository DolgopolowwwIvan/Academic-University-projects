// TProc.cs - Параметризованный абстрактный тип данных "Процессор"
// Original: ADT TProc - Процессор

using System;

namespace CalculatorFractions
{
    /// <summary>
    /// Типы двухоперандных операций
    /// Original: TOprtn = (None, Add, Sub, Mul, Dvd)
    /// </summary>
    public enum TOprtn
    {
        opNone,  // Нет операции
        opAdd,   // Сложение (+)
        opSub,   // Вычитание (-)
        opMul,   // Умножение (*)
        opDiv    // Деление (/)
    }

    /// <summary>
    /// Типы однооперандных функций
    /// Original: TFunc = (Rev, Sqr)
    /// </summary>
    public enum TFunc
    {
        fnRev,  // Обратное значение (1/x)
        fnSqr   // Возведение в квадрат
    }

    /// <summary>
    /// TProc - Параметризованный класс процессора для выполнения операций над типом T
    /// Выполняет двухоперандные операции и однооперандные функции над значениями типа T
    /// </summary>
    public class TProc<T> where T : class, new()
    {
        private T FLop_Res;    // Левый операнд и результат
        private T FRop;        // Правый операнд
        private TOprtn FOperation;  // Текущая операция

        /// <summary>
        /// Конструктор
        /// Инициализирует поля объектами типа T со значениями по умолчанию
        /// Процессор устанавливается в состояние: "операция не установлена" (opNone)
        /// </summary>
        public TProc()
        {
            FLop_Res = new T();
            FRop = new T();
            FOperation = TOprtn.opNone;
        }

        /// <summary>
        /// Сброс процессора
        /// Поля инициализируются значениями по умолчанию, операция сбрасывается
        /// </summary>
        public void Reset()
        {
            FLop_Res = new T();
            FRop = new T();
            FOperation = TOprtn.opNone;
        }

        /// <summary>
        /// Сброс операции
        /// Процессор устанавливается в состояние: "операция не установлена"
        /// </summary>
        public void OprtnClear()
        {
            FOperation = TOprtn.opNone;
        }

        /// <summary>
        /// Выполнить операцию
        /// Выполняет текущую операцию над FRop и FLop_Res, результат в FLop_Res
        /// </summary>
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

        /// <summary>
        /// Вычислить функцию
        /// Выполняет функцию Func над FRop, результат сохраняется в FRop
        /// </summary>
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

        /// <summary>
        /// Читать левый операнд
        /// </summary>
        public T Lop_Res => FLop_Res;

        /// <summary>
        /// Записать левый операнд
        /// </summary>
        public void SetLop_Res(T value)
        {
            FLop_Res = value;
        }

        /// <summary>
        /// Читать правый операнд
        /// </summary>
        public T Rop => FRop;

        /// <summary>
        /// Записать правый операнд
        /// </summary>
        public void SetRop(T value)
        {
            FRop = value;
        }

        /// <summary>
        /// Читать состояние (текущую операцию)
        /// </summary>
        public TOprtn Operation => FOperation;

        /// <summary>
        /// Записать состояние (установить операцию)
        /// </summary>
        public void SetOperation(TOprtn value)
        {
            FOperation = value;
        }

        /// <summary>
        /// Проверка: операция установлена
        /// </summary>
        public bool IsOperationSet()
        {
            return FOperation != TOprtn.opNone;
        }
    }

    /// <summary>
    /// Преобразование операции в строку
    /// </summary>
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
