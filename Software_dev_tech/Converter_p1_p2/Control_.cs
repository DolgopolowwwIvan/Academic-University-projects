using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

namespace Converter_p1_p2
{

    public enum State
    {
        Редактирование,
        Преобразовано
    }

    public class Control_
    {
        // Константы по умолчанию
        private const int DEFAULT_PIN = 10;
        private const int DEFAULT_POUT = 16;
        private const int DEFAULT_ACCURACY = 10;

        private Editor ed;              // Редактор
        private History his;             // История
        private int pin;                 // Основание исходной системы счисления
        private int pout;                // Основание результирующей системы счисления
        private State st;                 // Состояние конвертера
        private int accuracy;             // Точность 
        public Control_()
        {

            pin = DEFAULT_PIN;
            pout = DEFAULT_POUT;
            accuracy = DEFAULT_ACCURACY;
            st = State.Редактирование;
            ed = new Editor(pin);  // Создаём редактор для системы счисления pin
            his = new History();
        }

        public Control_(int pin, int pout, int accuracy)
        {
            this.pin = pin; // Основание исходной системы счисления
            this.pout = pout; // Основание результирующей системы счисления
            this.accuracy = accuracy; // Точность
            this.st = State.Редактирование;
            this.ed = new Editor(pin);
            this.his = new History();
        }

        public Editor Ed
        {
            get { return ed; }
        }

        public History His
        {
            get { return his; }
        }

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

        public int Accuracy
        {
            get { return accuracy; }
            set { accuracy = value; }
        }

        public State St
        {
            get { return st; }
            set { st = value; }
        }

        // Получить текущее число из редактора
        public string CurrentNumber
        {
            get { return ed.Number; }
        }


        // Вычисление точности представления результата
        private int CalculateAccuracy()
        {
            // Формула: ed.Acc() * log(Pin) / log(Pout) + 0.5
            if (ed.Acc() == 0)
                return 0;

            double result = ed.Acc() * Math.Log(pin) / Math.Log(pout) + 0.5;
            return (int)Math.Round(result);
        }

        public string DoCmnd(int j)
        {
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

                    st = State.Преобразовано;

                    his.AddRecord(pin, pout, ed.Number, result);

                    return result;
                }
                catch (Exception ex)
                {
                    return $"Ошибка преобразования: {ex.Message}";
                }
            }
            // Команда смены состояния
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
            else
            {
                // При любой команде редактирования состояние меняется на "Редактирование"
                st = State.Редактирование;

                // Выполняем команду редактора
                string result = ed.DoEdit(j);

                return result;
            }
        }

        // Переключение основания исходной системы счисления
        public void ChangePin(int newPin)
        {
            Pin = newPin;
        }

        // Переключение основания результирующей системы счисления
        public void ChangePout(int newPout)
        {
            Pout = newPout;
        }

        public void Reset()
        {
            ed.Clear();
            st = State.Редактирование;
        }

        // Получение информации о текущем состоянии
        public string GetStatus()
        {
            return $"Состояние: {st}, p1={pin}, p2={pout}, точность={accuracy}, текущее число: '{ed.Number}'";
        }
    }
}
