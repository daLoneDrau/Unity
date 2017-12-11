using System;
using RPGBase.Flyweights;

namespace RPGBase.Singletons
{
    public sealed class ArrayUtilities
    {
        /// <summary>
        /// the singleton instance.
        /// </summary>
        private static ArrayUtilities instance;
        public static ArrayUtilities Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ArrayUtilities();
                }
                return instance;
            }
        }
        private ArrayUtilities() { }
        /// <summary>
        /// Extends an array, adding a new element to the last index.
        /// </summary>
        /// <typeparam name="T">the parameterized type</typeparam>
        /// <param name="element">the new element</param>
        /// <param name="src">the source array</param>
        /// <returns></returns>
        public T[] ExtendArray<T>(T element, T[] src)
        {
            T[] dest = new T[src.Length + 1];
            Array.Copy(src, dest, src.Length);
            dest[src.Length] = element;
            return dest;
        }
        /// <summary>
        /// Removes an element from an array.
        /// </summary>
        /// <typeparam name="T">the parameterized type</typeparam>
        /// <param name="index">the element's index</param>
        /// <param name="src">the source array</param>
        /// <returns><see cref="T"/>[]</returns>
        public T[] RemoveIndex<T>(int index, T[] src)
        {
            T[] dest = new T[src.Length - 1];
            if (index > 0)
            {
                Array.Copy(src, 0, dest, 0, index);
            }
            if (index < src.Length - 1)
            {
                Array.Copy(src, index + 1, dest, index, src.Length - index - 1);
            }
            return dest;
        }
    }
}