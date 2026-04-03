using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTFBook
{
    // Структура записи абонента
    public struct TRec
    {
        public string LastName;   // Фамилия
        public string FirstName;  // Имя
        public string Phone;      // Телефон
        public string Address;    // Адрес
    }

    // Класс Абонент
    public class TAbonent
    {
        private TRec FRec;

        // Конструктор
        public TAbonent()
        {
            FRec = new TRec();
        }

        // Запись
        public void Write(TRec r)
        {
            FRec = r;
        }

        // Чтение
        public TRec Read()
        {
            return FRec;
        }

        // Сравнение по фамилии
        public int Less(TAbonent other)
        {
            return string.Compare(this.FRec.LastName, other.FRec.LastName);
        }

        public bool Equal(TAbonent other)
        {
            return this.FRec.LastName == other.FRec.LastName &&
                   this.FRec.FirstName == other.FRec.FirstName &&
                   this.FRec.Phone == other.FRec.Phone &&
                   this.FRec.Address == other.FRec.Address;
        }

        // Деструктор
        ~TAbonent()
        {
            
        }
    }
}
