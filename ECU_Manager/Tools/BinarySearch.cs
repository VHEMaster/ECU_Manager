using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECU_Manager.Tools
{
    public class BinarySearch<T> where T : IComparable
    {
        public static int Find(T[] array, int start_index, int end_index, T element)
        {
            int iterations = 0;
            int limit = (int)((Math.Sqrt(end_index - start_index) + 1.0) * 2.0D);
            while (start_index <= end_index && ++iterations < limit)
            {
                int middle = start_index + ((end_index - start_index) >> 1);
                if (array[middle].CompareTo(element) <= 0 && (middle + 1 >= array.Length || array[middle + 1].CompareTo(element) > 0))
                    return middle;
                if (array[middle + 1].CompareTo(element) <= 0)
                    start_index = middle + 1;
                else if (array[middle].CompareTo(element) >= 0)
                    end_index = middle - 1;
            }
            return -1;
        }
    }
}
