using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.BarbarianPrince.Turn
{
    public class TimePhase
    {
        /// <summary>
        /// day one.
        /// </summary>
        public static readonly TimePhase PHASE_00_PRE_ACTION = new TimePhase("Dawn");
        /// <summary>
        /// day two.
        /// </summary>
        public static readonly TimePhase PHASE_01_ACTION = new TimePhase("Noon");
        /// <summary>
        /// day three.
        /// </summary>
        public static readonly TimePhase PHASE_02_POST_ACTION = new TimePhase("Noon");
        /// <summary>
        /// day four.
        /// </summary>
        public static readonly TimePhase PHASE_03_PRE_EVENING = new TimePhase("Noon");
        /// <summary>
        /// day five.
        /// </summary>
        public static readonly TimePhase PHASE_04_EVENING_FOOD = new TimePhase("Dusk");
        /// <summary>
        /// day six.
        /// </summary>
        public static readonly TimePhase PHASE_05_EVENING_LODGING = new TimePhase("Dusk");
        /// <summary>
        /// day seven.
        /// </summary>
        public static readonly TimePhase PHASE_06_POST_EVENING = new TimePhase("Midnight");
        public string Title { get; private set; }
        private TimePhase(string t)
        {
            Title = t;
        }
        /// <summary>
        /// Advances the current position, and get the next Phase.
        /// </summary>
        /// <returns><see cref="TimePhase"/></returns>
        public TimePhase Advance()
        {
            TimePhase t = null;
            if (Equals(PHASE_00_PRE_ACTION))
            {
                t = PHASE_01_ACTION;
            }
            else if (Equals(PHASE_01_ACTION))
            {
                t = PHASE_02_POST_ACTION;
            }
            else if (Equals(PHASE_02_POST_ACTION))
            {
                t = PHASE_03_PRE_EVENING;
            }
            else if (Equals(PHASE_03_PRE_EVENING))
            {
                t = PHASE_04_EVENING_FOOD;
            }
            else if (Equals(PHASE_04_EVENING_FOOD))
            {
                t = PHASE_05_EVENING_LODGING;
            }
            else if (Equals(PHASE_05_EVENING_LODGING))
            {
                t = PHASE_06_POST_EVENING;
            }
            else if (Equals(PHASE_06_POST_EVENING))
            {
                t = PHASE_00_PRE_ACTION;
            }
            return t;
        }
    }
}
