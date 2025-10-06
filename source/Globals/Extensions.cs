using System;
using System.Collections.Generic;

namespace Globals
{
    public static class Extensions
    {
        // based on: https://stackoverflow.com/a/1262619/15783929
        public static void Shuffle<T>(this IList<T> list)
        {
            var n = list.Count;
            while (n > 1)
            {
                n--;
                int k = ThreadSafeRandom.ThisThreadsRandom.Next(n + 1);
                (list[n], list[k]) = (list[k], list[n]);
            }
        }

        public static List<T> Move<T>(this IList<T> list, int first, int last)
        {
            var moved = new List<T>();
            for (int i = first; i < last; i++)
            {
                moved.Add(list[first]);
                list.RemoveAt(first);
            }
            return moved;
        }
    }

    public static class ThreadSafeRandom
    {
        [ThreadStatic] private static Random Local;

        public static Random ThisThreadsRandom
        {
            get { return Local ??= new Random(unchecked(Environment.TickCount * 31 + Environment.CurrentManagedThreadId)); }
        }
    }
}
