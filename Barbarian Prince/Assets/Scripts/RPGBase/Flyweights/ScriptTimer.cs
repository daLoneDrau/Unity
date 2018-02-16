using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPGBase.Flyweights
{
    public class ScriptTimer
    {
        /// <summary>
        /// the action taken when the script timer completes.
        /// </summary>
        public ScriptTimerAction Action { get; set; }
        /// <summary>
        /// the flag indicating whether the timer exists.
        /// </summary>
        public bool Exists { get; set; }
        /** any flags set on the timer. */
        private long flags;
        /// <summary>
        /// the <see cref="BaseInteractiveObject"/> associated with this timer.
        /// </summary>
        public BaseInteractiveObject Io { get; set; }
        /// <summary>
        /// the index of any array the timer is associated with.
        /// </summary>
        public long Longinfo { get; set; }
        /// <summary>
        /// the timer's length in milliseconds.
        /// </summary>
        public long Msecs { get; set; }
        /// <summary>
        /// the timer's name.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// the script associated with the timer.
        /// </summary>
        public Scriptable Script { get; set; }
        /// <summary>
        /// the amount of time passed since the timer was started (timecheck).
        /// </summary>
        public long Tim { get; set; }
        /// <summary>
        /// the number of times the timer repeats.
        /// </summary>
        public long Times { get; set; }
        /// <summary>
        /// if true, the timer is turn-based, otherwise it is millisecond based.
        /// </summary>
        public bool TurnBased { get; set; }
        /**
         * Adds a flag set on the timer..
         * @param flag the flag
         */
        public void AddFlag(long flag)
        {
            flags |= flag;
        }
        /** Clears all flags that were set. */
        public void ClearFlags()
        {
            flags = 0;
        }
        /**
         * Determines if the {@link ScriptTimer} has a specific flag.
         * @param flag the flag
         * @return true if the {@link ScriptTimer} has the flag; false otherwise
         */
        public bool HasFlag(long flag)
        {
            return (flags & flag) == flag;
        }
        /**
         * Removes a flag.
         * @param flag the flag
         */
        public void RemoveFlag(long flag)
        {
            flags &= ~flag;
        }
        /**
         * Sets the timer.
         * @param params the parameters used to set the timer.
         */
        public void Set(ScriptTimerInitializationParameters p)
        {
            Script = p.Script;
            Exists = true;
            Io = p.Io;
            Msecs = p.Milliseconds;
            Name = p.Name;
            Action = new ScriptTimerAction(p.Obj, p.Method, p.Args);
            Tim = p.StartTime;
            Times = p.RepeatTimes;
            ClearFlags();
            AddFlag(p.FlagValues);
        }
    }
}
