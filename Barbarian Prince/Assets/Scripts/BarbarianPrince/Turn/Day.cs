using RPGBase.Flyweights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.BarbarianPrince.Turn
{
    public class Day
    {
        /// <summary>
        /// day one.
        /// </summary>
        public static readonly Day ONE = new Day(1, "1st");
        /// <summary>
        /// day two.
        /// </summary>
        public static readonly Day TWO = new Day(2, "2nd");
        /// <summary>
        /// day three.
        /// </summary>
        public static readonly Day THREE = new Day(3, "3rd");
        /// <summary>
        /// day four.
        /// </summary>
        public static readonly Day FOUR = new Day(4, "4th");
        /// <summary>
        /// day five.
        /// </summary>
        public static readonly Day FIVE = new Day(5, "5th");
        /// <summary>
        /// day six.
        /// </summary>
        public static readonly Day SIX = new Day(6, "6th");
        /// <summary>
        /// day seven.
        /// </summary>
        public static readonly Day SEVEN = new Day(7, "7th");
        public static Day ValueOf(int day)
        {
            Day d = null;
            switch (day)
            {
                case 1:
                    d = ONE;
                    break;
                case 2:
                    d = TWO;
                    break;
                case 3:
                    d = THREE;
                    break;
                case 4:
                    d = FOUR;
                    break;
                case 5:
                    d = FIVE;
                    break;
                case 6:
                    d = SIX;
                    break;
                case 7:
                    d = SEVEN;
                    break;
            }
            if (d == null)
            {
                throw new RPGException(ErrorMessage.BAD_PARAMETERS, "Invalid day");
            }
            return d;
        }
        public const int MAX = 7;
        public int Value { get; private set; }
        public string Adjective { get; private set; }
        private HashSet<Type> NumericTypes = new HashSet<Type> { typeof(int) };
        private Day(int v, string a)
        {
            Value = v;
            Adjective = a;
        }
        public override bool Equals(Object obj)
        {
            bool b = false;
            // Check for null values and compare run-time types.
            if (obj != null)
            {
                if (GetType() == obj.GetType())
                {
                    b = ((Day)obj).Value == Value;
                }
                else if (NumericTypes.Contains(obj.GetType()))
                {
                    b = (int)obj == Value;
                }
            }
            return b;
        }
    }
}
