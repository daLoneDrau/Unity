using System;
using System.Collections.Generic;
using System.Linq;

namespace RPGBase.Singletons
{
    public sealed class Diceroller
    {
        /// <summary>
        /// the one and only instance of the <see cref="Diceroller"/> class.
        /// </summary>
        private static Diceroller instance;
        /// <summary>
        /// Gives access to the singleton instance of <see cref="Diceroller"/>.
        /// </summary>
        public static Diceroller Instance
        { get
            {
                if (Diceroller.instance == null)
                {
                    Diceroller.instance = new Diceroller();
                }
                return Diceroller.instance;
            }
        }
        /// <summary>
        /// the seed value.
        /// </summary>
        private long mySeed;
        /// <summary>
        /// the random number generator.
        /// </summary>
        private Random random;
        /// <summary>
        /// Hidden constructor.
        /// </summary>
        private Diceroller()
        {
            mySeed = 0;
            random = new Random(unchecked((int)mySeed));
        }
        private void Check()
        {
            long now = DateTime.Now.Ticks;
            if (mySeed != now)
            {
                mySeed = now;
                random = new Random(unchecked((int)mySeed));
            }
        }
        public char GetRandomIndex(char[] array)
        {
            Check();
            return array[Math.Abs(random.Next() % array.Length)];
        }
        public int GetRandomIndex(int[] array)
        {
            Check();
            return array[Math.Abs(random.Next() % array.Length)];
        }
        public long GetRandomLong()
        {
            Check();
            return random.Next();
        }
        public Object GetRandomObject(List<Object> list)
        {
            Check();
            return list[Math.Abs(random.Next() % list.Count())];
        }
        public Object GetRandomObject(Dictionary<Object, Object> map)
        {
            Check();
            List<Object> list = new List<Object>();
            foreach (KeyValuePair<Object, Object> kvp in map)
            {
                list.Add(kvp.Key);
            }
            return map[GetRandomObject(list)];
        }
        public object GetRandomObject(object[] array)
        {
            Check();
            return array[Math.Abs(random.Next() % array.Length)];
        }
        public Object RemoveRandomObject(List<Object> list)
        {
            Check();
            Object o = null;
            if (list.Any())
            {
                o = list[Math.Abs(random.Next() % list.Count)];
                list.Remove(o);
            }
            return o;
        }
        public Object RemoveRandomObject(Dictionary<Object, Object> map)
        {
            Check();
            Object o = null;
            if (map.Any())
            {
                o = this.GetRandomObject(map);
                List<Object> list = new List<Object>();
                foreach (KeyValuePair<Object, Object> kvp in map)
                {
                    list.Add(kvp.Key);
                }
                Object key = GetRandomObject(list);
                o = map[key];
                map.Remove(key);
            }
            return o;
        }
        /// <summary>
        /// Rolls an x-sided die.
        /// </summary>
        /// <param name="x">the # of faces on the die</param>
        /// <returns><see cref="Int32"/></returns>
        public int RolldX(int x)
        {
            Check();
            return Math.Abs(random.Next() % x) + 1;
        }
        /// <summary>
        /// Rolls an x-sided die plus y offset. to roll from 3-8 (d6 + 2) x must be 6, y must be 2.
        /// </summary>
        /// <param name="x">the # of faces on the die</param>
        /// <param name="y">the offset</param>
        /// <returns><see cref="Int32"/></returns>
        public int RolldXPlusY(int x, int y)
        {
            Check();
            return RolldX(x) + y;
        }
        /// <summary>
        /// Gets a positive number between 0.01 and 1.0.
        /// </summary>
        /// <returns></returns>
        public float RollPercent()
        {
            Check();
            float per = (float)random.NextDouble() + 0.1f;
            if (per > 1f)
            {
                per = 1f;
            }
            return per;
        }
        /// <summary>
        /// Gets a y-die roll x number of times. To get a 6d6 roll, x and y must be 6. To get a 3d6 roll, x must be 3, y must be 6.
        /// </summary>
        /// <param name="x">the number of time to roll the die</param>
        /// <param name="y">the # of faces on the die</param>
        /// <returns><see cref="Int32"/></returns>
        public int RollXdY(int x, int y)
        {
            int result = 0;
            for (int i = 0; i < x; i++)
            {
                result += RolldX(y);
            }
            return result;
        }
    }
}