using System;
using UnityEditor;

namespace RPGBase.Utilities
{
    public sealed class ArrayUtilities
    {
        /// <summary>
        /// the one and only instance of the <see cref="ArrayUtilities"/> class.
        /// </summary>
        private static ArrayUtilities instance;
        /// <summary>
        /// Gives access to the singleton instance of <see cref="StringBuilderPool"/>.
        /// </summary>
        public static ArrayUtilities GetInstance()
        {
            if (ArrayUtilities.instance == null)
            {
                ArrayUtilities.instance = new ArrayUtilities();
            }
            return ArrayUtilities.instance;
        }
        private ArrayUtilities() { }
        public T[] ExtendArray<T>(T element, T[] src)
        {
            T[] dest = new T[src.Length + 1];
            Array.Copy(src, dest, src.Length);
            dest[src.Length] = element;
            return dest;
        }
    }
}