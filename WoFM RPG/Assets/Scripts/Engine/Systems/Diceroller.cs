using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGBaseCS.Engine.Systems
{
    public sealed class Diceroller
    {
        /// <summary>
        /// the singleton instance.
        /// </summary>
        private static Diceroller instance;
        /// <summary>
        /// Gets the one and only instance of <see cref="Diceroller"/>.
        /// </summary>
        /// <returns><see cref="Diceroller"/></returns>
        public static Diceroller getInstance()
        {
            if (Diceroller.instance == null)
            {
                Diceroller.instance = new Diceroller();
            }
            return Diceroller.instance;
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
        public long getRandomLong()
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
        public Object GetRandomObject(Object[] array)
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
    }
}
