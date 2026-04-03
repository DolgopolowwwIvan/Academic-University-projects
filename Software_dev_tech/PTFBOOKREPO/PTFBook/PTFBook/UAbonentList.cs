using System;
using System.Collections.Generic;

namespace PTFBook
{
    public class TAbonentList
    {
        protected List<TAbonent> FList;

        public TAbonentList()
        {
            FList = new List<TAbonent>();
        }

        public TAbonent ReadRecord(int i)
        {
            if (i >= 0 && i < FList.Count)
                return FList[i];
            return null;
        }

        public int Count()
        {
            return FList.Count;
        }

        // Добавление с сортировкой (из проекта товарища)
        public void AddRecord(TAbonent newAbonent)
        {
            FList.Add(newAbonent);
            // Сортировка по имени
            FList.Sort((x, y) => string.Compare(x.Read().FirstName, y.Read().FirstName, StringComparison.OrdinalIgnoreCase));
        }

        public void AddRecord(string name, string number)
        {
            TRec data;
            data.LastName = "";
            data.FirstName = name;
            data.Phone = number;
            data.Address = "";

            TAbonent newAbonent = new TAbonent();
            newAbonent.Write(data);

            FList.Add(newAbonent);
            // Сортировка по имени
            FList.Sort((x, y) => string.Compare(x.Read().FirstName, y.Read().FirstName, StringComparison.OrdinalIgnoreCase));
        }

        public int FindRecord(TAbonent r)
        {
            for (int i = 0; i < FList.Count; i++)
            {
                if (FList[i].IsEqual(r.Read()))
                {
                    return i;
                }
            }
            return -1;
        }

        public void DeleteRecord(TAbonent r)
        {
            int index = FindRecord(r);
            if (index != -1)
            {
                FList.RemoveAt(index);
            }
        }

        public void DeleteAt(int i)
        {
            if (i >= 0 && i < FList.Count)
            {
                FList.RemoveAt(i);
            }
        }

        public void ClearList()
        {
            FList.Clear();
        }
    }
}
