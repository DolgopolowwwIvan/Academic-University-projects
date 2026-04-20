namespace Calculator.Core;

// Состояния калькулятора
public enum TCtrlState
{
    cStart = 0,        // Начальное
    cEditing = 1,      // Ввод и редактирование
    FunDone = 2,       // Функция выполнена
    cValDone = 3,      // Значение введено
    cExpDone = 4,      // Выражение вычислено
    cOpChange = 5,     // Операция изменена
    cError = 6         // Ошибка
}
