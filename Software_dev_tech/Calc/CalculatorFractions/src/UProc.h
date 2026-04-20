// UProc.h - Параметризованный абстрактный тип данных "Процессор"
// Original: ADT TProc - Процессор

#ifndef UPROC_H
#define UPROC_H

#include <string>

// Типы двухоперандных операций
// Original: TOprtn = (None, Add, Sub, Mul, Dvd)
enum TOprtn {
    opNone,  // Нет операции
    opAdd,   // Сложение (+)
    opSub,   // Вычитание (-)
    opMul,   // Умножение (*)
    opDiv    // Деление (/)
};

// Типы однооперандных функций
// Original: TFunc = (Rev, Sqr)
enum TFunc {
    fnRev,  // Обратное значение (1/x)
    fnSqr   // Возведение в квадрат
};

// Преобразование операции в строку
inline std::string OprtnToString(TOprtn oprtn) {
    switch (oprtn) {
        case opAdd: return "+";
        case opSub: return "-";
        case opMul: return "*";
        case opDiv: return "/";
        default: return "";
    }
}

// TProc - Параметризованный класс процессора для выполнения операций над типом T
// Выполняет двухоперандные операции и однооперандные функции над значениями типа T
template <class T>
class TProc {
private:
    T FLop_Res;    // Левый операнд и результат
    T FRop;        // Правый операнд
    TOprtn FOperation;  // Текущая операция

public:
    // Конструктор
    // Инициализирует поля объектами типа T со значениями по умолчанию
    // Процессор устанавливается в состояние: "операция не установлена" (opNone)
    TProc() : FLop_Res(), FRop(), FOperation(opNone) {}

    // Деструктор
    ~TProc() {}

    // Сброс процессора
    // Поля инициализируются значениями по умолчанию, операция сбрасывается
    void Reset() {
        FLop_Res = T();
        FRop = T();
        FOperation = opNone;
    }

    // Сброс операции
    // Процессор устанавливается в состояние: "операция не установлена"
    void OprtnClear() {
        FOperation = opNone;
    }

    // Выполнить операцию
    // Выполняет текущую операцию над FRop и FLop_Res, результат в FLop_Res
    void OprtnRun() {
        if (FOperation == opNone) return;

        switch (FOperation) {
            case opAdd:
                FLop_Res = FLop_Res.Add(FRop);
                break;
            case opSub:
                FLop_Res = FLop_Res.Sub(FRop);
                break;
            case opMul:
                FLop_Res = FLop_Res.Mul(FRop);
                break;
            case opDiv:
                FLop_Res = FLop_Res.Div(FRop);
                break;
            default:
                break;
        }
        // Операция не сбрасывается для возможности повторного выполнения
    }

    // Вычислить функцию
    // Выполняет функцию Func над FRop, результат сохраняется в FRop
    void FuncRun(TFunc Func) {
        switch (Func) {
            case fnSqr:
                FRop = FRop.Sqr();
                break;
            case fnRev:
                FRop = FRop.Rev();
                break;
            default:
                break;
        }
    }

    // Читать левый операнд
    T GetLop_Res() const {
        return FLop_Res;
    }

    // Записать левый операнд
    void SetLop_Res(const T& Operand) {
        FLop_Res = Operand;
    }

    // Читать правый операнд
    T GetRop() const {
        return FRop;
    }

    // Записать правый операнд
    void SetRop(const T& Operand) {
        FRop = Operand;
    }

    // Читать состояние (текущую операцию)
    TOprtn GetOperation() const {
        return FOperation;
    }

    // Записать состояние (установить операцию)
    void SetOperation(TOprtn Oprtn) {
        FOperation = Oprtn;
    }

    // Свойство: левый операнд-результат
    T Lop_Res() const { return FLop_Res; }
    void Lop_Res(const T& value) { FLop_Res = value; }

    // Свойство: правый операнд
    T Rop() const { return FRop; }
    void Rop(const T& value) { FRop = value; }

    // Свойство: операция
    TOprtn Operation() const { return FOperation; }
    void Operation(TOprtn value) { FOperation = value; }

    // Проверка: операция установлена
    bool IsOperationSet() const {
        return FOperation != opNone;
    }
};

#endif // UPROC_H
