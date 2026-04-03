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

        public void AddRecord(string name, string number)
        {
            TRec data;
            data.Name = name;
            data.Number = number;

            TAbonent newAbonent = new TAbonent();
            newAbonent.Write(data);

            FList.Add(newAbonent);

            FList.Sort((x, y) => x.Read().Name.CompareTo(y.Read().Name));
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