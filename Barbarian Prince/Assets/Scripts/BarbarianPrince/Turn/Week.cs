using RPGBase.Flyweights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.BarbarianPrince.Turn
{
    public class Week
    {
        /// <summary>
        /// day one.
        /// </summary>
        public static readonly Week ONE = new Week(1, "1st");
        /// <summary>
        /// day two.
        /// </summary>
        public static readonly Week TWO = new Week(2, "2nd");
        /// <summary>
        /// day three.
        /// </summary>
        public static readonly Week THREE = new Week(3, "3rd");
        /// <summary>
        /// day four.
        /// </summary>
        public static readonly Week FOUR = new Week(4, "4th");
        /// <summary>
        /// day five.
        /// </summary>
        public static readonly Week FIVE = new Week(5, "5th");
        /// <summary>
        /// day six.
        /// </summary>
        public static readonly Week SIX = new Week(6, "6th");
        /// <summary>
        /// day seven.
        /// </summary>
        public static readonly Week SEVEN = new Week(7, "7th");
        /// <summary>
        /// day eight.
        /// </summary>
        public static readonly Week EIGHT = new Week(8, "8th");
        /// <summary>
        /// day nine.
        /// </summary>
        public static readonly Week NINE = new Week(9, "9th");
        /// <summary>
        /// day ten.
        /// </summary>
        public static readonly Week TEN = new Week(10, "10th");
        public static Week ValueOf(int week)
        {
            Week w = null;
            switch (week)
            {
                case 1:
                    w = ONE;
                    break;
                case 2:
                    w = TWO;
                    break;
                case 3:
                    w = THREE;
                    break;
                case 4:
                    w = FOUR;
                    break;
                case 5:
                    w = FIVE;
                    break;
                case 6:
                    w = SIX;
                    break;
                case 7:
                    w = SEVEN;
                    break;
                case 8:
                    w = EIGHT;
                    break;
                case 9:
                    w = NINE;
                    break;
                case 10:
                    w = TEN;
                    break;
            }
            if (w == null)
            {
                throw new RPGException(ErrorMessage.BAD_PARAMETERS, "Invalid day");
            }
            return w;
        }
        public int Value { get; private set; }
        public string Adjective { get; private set; }
        private HashSet<Type> NumericTypes = new HashSet<Type> { typeof(int) };
        private Week(int v, string a)
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
                    b = ((Week)obj).Value == Value;
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
