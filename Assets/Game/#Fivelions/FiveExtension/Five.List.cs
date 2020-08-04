using System.Linq;
using System.Collections.Generic;

namespace Five.List
{
    public static class ListExtensions
    {
        public static List<T> Swap<T>(this List<T> list, int indexA, int indexB)
        {
            T temp = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = temp;
            //Debug.Log("List: " + list + "Swap: " + indexA + " With " + indexB);
            return list;
        }

        public static List<T> MergeWith<T>(this List<T> @this, List<T> listMerge)
        {
            foreach (T t in listMerge)
                if (!@this.Contains(t))
                    @this.Add(t);
            return @this;
        }

        public static List<T> MergesWith<T>(this List<T> @this, params List<T>[] listMerges)
        {
            foreach (var t in listMerges)
                @this.MergeWith<T>(t);
            return @this;
        }

        public static bool IsSameWith<T>(this List<T> @this, params List<T>[] listCompare)
        {
            for (int i = 0; i < listCompare.Count(); i++)
            {
                for (int j = 0; j < listCompare[i].Count; j++)
                {
                    if (listCompare[i][j].GetHashCode() != @this[j].GetHashCode())
                        return false;
                }
            }
            return true;
        }

        public static List<T> CloneList<T>(this List<T> @this)
        {
            List<T> listClone = new List<T>();
            listClone.AddRange(@this);
            return listClone;
        }
    }
}