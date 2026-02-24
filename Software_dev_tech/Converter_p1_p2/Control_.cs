using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

namespace Converter_p1_p2
{
    /// <summary>
    /// Состояния конвертера
    /// </summary>
    public enum State
    {
        Редактирование,
        Преобразовано
    }

    /// <summary>
    /// Класс управления для конвертера p1_р2
    /// Координирует действия между интерфейсом, редактором, конвертерами и историей
    /// </summary>
    public class Control_
    {
        // Константы по умолчанию
        private const int DEFAULT_PIN = 10;
        private const int DEFAULT_POUT = 16;
        private const int DEFAULT_ACCURACY = 10;

        // Поля
        private Editor ed;              // Редактор
        private History his;             // История
        private int pin;                 // Основание исходной системы счисления
        private int pout;                // Основание результирующей системы счисления
        private State st;                 // Состояние конвертера
        private int accuracy;             // Точность (число разрядов дробной части)

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public Control_()
        {
            // Инициализация полей начальными значениями
            pin = DEFAULT_PIN;
            pout = DEFAULT_POUT;
            accuracy = DEFAULT_ACCURACY;
            st = State.Редактирование;
            ed = new Editor(pin);  // Создаём редактор для системы счисления pin
            his = new History();
        }

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="pin">Основание исходной системы счисления</param>
        /// <param name="pout">Основание результирующей системы счисления</param>
        /// <param name="accuracy">Точность</param>
        public Control_(int pin, int pout, int accuracy)
        {
            this.pin = pin;
            this.pout = pout;
            this.accuracy = accuracy;
            this.st = State.Редактирование;
            this.ed = new Editor(pin);
            this.his = new History();
        }

        /// <summary>
        /// Свойство для доступа к редактору
        /// </summary>
        public Editor Ed
        {
            get { return ed; }
        }

        /// <summary>
        /// Свойство для доступа к истории
        /// </summary>
        public History His
        {
            get { return his; }
        }

        /// <summary>
        /// Свойство для чтения и записи основания исходной системы счисления (p1)
        /// </summary>
        public int Pin
        {
            get { return pin; }
            set
            {
                if (value < 2 || value > 16)
                    throw new ArgumentException("Основание системы счисления должно быть от 2 до 16");
                pin = value;
                // При изменении основания сбрасываем редактор и состояние
                ed = new Editor(pin);
                st = State.Редактирование;
            }
        }

        /// <summary>
        /// Свойство для чтения и записи основания результирующей системы счисления (p2)
        /// </summary>
        public int Pout
        {
            get { return pout; }
            set
            {
                if (value < 2 || value > 16)
                    throw new ArgumentException("Основание системы счисления должно быть от 2 до 16");
                pout = value;
            }
        }

        /// <summary>
        /// Свойство для чтения и записи точности
        /// </summary>
        public int Accuracy
        {
            get { return accuracy; }
            set { accuracy = value; }
        }

        /// <summary>
        /// Свойство для чтения и записи состояния конвертера
        /// </summary>
        public State St
        {
            get { return st; }
            set { st = value; }
        }

        /// <summary>
        /// Получить текущее число из редактора
        /// </summary>
        public string CurrentNumber
        {
            get { return ed.Number; }
        }

        /// <summary>
        /// Вычисление точности представления результата
        /// </summary>
        /// <returns>Количество знаков после запятой в результате</returns>
        private int CalculateAccuracy()
        {
            // Формула из примера: округление ed.Acc() * log(Pin) / log(Pout) + 0.5
            if (ed.Acc() == 0)
                return 0;

            double result = ed.Acc() * Math.Log(pin) / Math.Log(pout) + 0.5;
            return (int)Math.Round(result);
        }

        /// <summary>
        /// Выполнить команду конвертера
        /// </summary>
        /// <param name="j">Номер команды</param>
        /// <returns>Строка результата (отредактированное число или результат преобразования)</returns>
        public string DoCmnd(int j)
        {
            // Команда преобразования (номер 20 в данном примере, но можно настроить)
            if (j == 20)
            {
                try
                {
                    // Проверяем, что в редакторе есть число
                    if (string.IsNullOrEmpty(ed.Number))
                    {
                        return "Ошибка: не введено число";
                    }

                    // Преобразуем из p1 в 10-чную
                    double decimalValue = Conver_p_10.dval(ed.Number, pin);

                    // Вычисляем точность для результата
                    int resultAccuracy = CalculateAccuracy();
                    if (resultAccuracy == 0)
                        resultAccuracy = accuracy; // Используем заданную точность, если вычисленная равна 0

                    // Преобразуем из 10-чной в p2
                    string result = Conver_10_p.Do(decimalValue, pout, resultAccuracy);

                    // Меняем состояние на "Преобразовано"
                    st = State.Преобразовано;

                    // Добавляем запись в историю
                    his.AddRecord(pin, pout, ed.Number, result);

                    return result;
                }
                catch (Exception ex)
                {
                    return $"Ошибка преобразования: {ex.Message}";
                }
            }
            // Команда смены состояния (если нужно принудительно вернуться в режим редактирования)
            else if (j == 21)
            {
                st = State.Редактирование;
                return ed.Number;
            }
            // Команда очистки истории
            else if (j == 22)
            {
                his.Clear();
                return "История очищена";
            }
            // Команда получения количества записей в истории
            else if (j == 23)
            {
                return $"Записей в истории: {his.Count()}";
            }
            // Команда получения записи из истории по индексу
            else if (j == 24)
            {
                // Для простоты возвращаем последнюю запись
                if (his.Count() > 0)
                {
                    return his[his.Count() - 1];
                }
                else
                {
                    return "История пуста";
                }
            }
            // Все остальные команды - команды редактирования
            else
            {
                // При любой команде редактирования состояние меняется на "Редактирование"
                st = State.Редактирование;

                // Выполняем команду редактора
                string result = ed.DoEdit(j);

                return result;
            }
        }

        /// <summary>
        /// Переключение основания исходной системы счисления
        /// </summary>
        /// <param name="newPin">Новое основание</param>
        public void ChangePin(int newPin)
        {
            Pin = newPin;
        }

        /// <summary>
        /// Переключение основания результирующей системы счисления
        /// </summary>
        /// <param name="newPout">Новое основание</param>
        public void ChangePout(int newPout)
        {
            Pout = newPout;
        }

        /// <summary>
        /// Сброс конвертера в начальное состояние
        /// </summary>
        public void Reset()
        {
            ed.Clear();
            st = State.Редактирование;
        }

        /// <summary>
        /// Получение информации о текущем состоянии
        /// </summary>
        /// <returns>Строка с информацией</returns>
        public string GetStatus()
        {
            return $"Состояние: {st}, p1={pin}, p2={pout}, точность={accuracy}, текущее число: '{ed.Number}'";
        }
    }
}
