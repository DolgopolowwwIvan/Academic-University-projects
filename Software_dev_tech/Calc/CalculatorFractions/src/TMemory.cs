// TMemory.cs - Параметризованный абстрактный тип данных "Память"
// Original: ADT TMemory - Память

using System;

namespace CalculatorFractions
{
    /// <summary>
    /// Состояния памяти
    /// </summary>
    public enum TMemoryState
    {
        msOff,  // Выключена
        msOn    // Включена
    }

    /// <summary>
    /// TMemory - Параметризованный класс памяти для хранения одного числа типа T
    /// Шаблон класса для хранения объекта типа T
    /// </summary>
    public class TMemory<T> where T : class, new()
    {
        private T FNumber;        // Хранимое число
        private TMemoryState FState;  // Состояние памяти

        /// <summary>
        /// Конструктор
        /// Инициализирует поле FNumber объектом типа T со значением по умолчанию
        /// Память устанавливается в состояние "Выключена" (msOff)
        /// </summary>
        public TMemory()
        {
            FNumber = new T();
            FState = TMemoryState.msOff;
        }

        /// <summary>
        /// Записать - записывает объект E в память
        /// Память устанавливается в состояние "Включена" (msOn)
        /// </summary>
        public void Store(T E)
        {
            FNumber = E;
            FState = TMemoryState.msOn;
        }

        /// <summary>
        /// Взять - возвращает копию объекта из памяти
        /// </summary>
        public T Recall()
        {
            return FNumber;
        }

        /// <summary>
        /// Добавить - добавляет объект E к хранимому значению
        /// </summary>
        public void Add(T E)
        {
            // Используем динамику для вызова метода Add у типа T
            dynamic dynNumber = FNumber;
            FNumber = dynNumber.Add(E);
            FState = TMemoryState.msOn;
        }

        /// <summary>
        /// Очистить - сбрасывает память в значение по умолчанию
        /// Память устанавливается в состояние "Выключена" (msOff)
        /// </summary>
        public void Clear()
        {
            FNumber = new T();
            FState = TMemoryState.msOff;
        }

        /// <summary>
        /// Читать состояние памяти - возвращает строковое представление состояния
        /// </summary>
        public string ReadState()
        {
            return FState == TMemoryState.msOn ? "M" : " ";
        }

        /// <summary>
        /// Читать число - возвращает хранимое значение
        /// </summary>
        public T ReadNumber()
        {
            return FNumber;
        }

        /// <summary>
        /// Свойство: состояние памяти (геттер)
        /// </summary>
        public TMemoryState State => FState;

        /// <summary>
        /// Свойство: число (геттер)
        /// </summary>
        public T Number => FNumber;

        /// <summary>
        /// Проверка: память включена
        /// </summary>
        public bool IsOn()
        {
            return FState == TMemoryState.msOn;
        }
    }
}
