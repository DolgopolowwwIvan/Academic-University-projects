using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTFBook
{
    public class TAbonentList
    {
        private List<TAbonent> FList;

        // Конструктор
        public TAbonentList()
        {
            FList = new List<TAbonent>();
        }

        // Чтение записи по индексу
        public TAbonent ReadRecord(int i)
        {
            if (i >= 0 && i < FList.Count)
                return FList[i];
            return null;
        }

        // Количество записей
        public int RecordCount
        {
            get { return FList.Count; }
        }

        // Добавление записи
        public void AddRecord(TAbonent r)
        {
            FList.Add(r);
        }

        // Поиск записи (возвращает индекс)
        public int FindRecord(TAbonent r)
        {
            for (int i = 0; i < FList.Count; i++)
            {
                if (FList[i].Equal(r))
                    return i;
            }
            return -1;
        }

        // Удаление записи по значению
        public void DeleteRecord(TAbonent r)
        {
            int index = FindRecord(r);
            if (index != -1)
                FList.RemoveAt(index);
        }

        // Удаление записи по индексу
        public void DeleteSelectedRecord(int i)
        {
            if (i >= 0 && i < FList.Count)
                FList.RemoveAt(i);
        }

        // Очистка списка
        public void ClearList()
        {
            FList.Clear();
        }

        // Деструктор
        ~TAbonentList()
        {
            // Финализатор
        }
    }
}
