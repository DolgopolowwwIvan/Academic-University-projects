using System;

namespace CalculatorFractions
{
    // Состояния памяти
    public enum TMemoryState
    {
        msOff,  // Выключена
        msOn    // Включена
    }

    // TMemory - Параметризованный класс памяти для хранения одного числа типа T
    // Шаблон класса для хранения объекта типа T
    public class TMemory<T> where T : class, new()
    {
        private T FNumber;        // Хранимое число
        private TMemoryState FState;  // Состояние памяти

        // Конструктор
        // Инициализирует поле FNumber объектом типа T со значением по умолчанию
        // Память устанавливается в состояние "Выключена" (msOff)
        public TMemory()
        {
            FNumber = new T();
            FState = TMemoryState.msOff;
        }

        // Записать - записывает объект E в память
        // Память устанавливается в состояние "Включена" (msOn)
        public void Store(T E)
        {
            FNumber = E;
            FState = TMemoryState.msOn;
        }

        // Взять - возвращает копию объекта из памяти
        public T Recall()
        {
            return FNumber;
        }

        // Добавить - добавляет объект E к хранимому значению
        public void Add(T E)
        {
            // Используем динамику для вызова метода Add у типа T
            dynamic dynNumber = FNumber;
            FNumber = dynNumber.Add(E);
            FState = TMemoryState.msOn;
        }

        // Очистить - сбрасывает память в значение по умолчанию
        // Память устанавливается в состояние "Выключена" (msOff)
        public void Clear()
        {
            FNumber = new T();
            FState = TMemoryState.msOff;
        }

        // Читать состояние памяти - возвращает строковое представление состояния
        public string ReadState()
        {
            return FState == TMemoryState.msOn ? "M" : " ";
        }

        // Читать число - возвращает хранимое значение
        public T ReadNumber()
        {
            return FNumber;
        }

        // Свойство: состояние памяти (геттер)
        public TMemoryState State => FState;

        // Свойство: число (геттер)
        public T Number => FNumber;

        // Проверка: память включена
        public bool IsOn()
        {
            return FState == TMemoryState.msOn;
        }
    }
}
