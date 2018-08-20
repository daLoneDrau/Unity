using System;
using System.Reflection;

namespace RPGBase.Flyweights
{
    public struct ScriptTimerInitializationParameters
    {
        /// <summary>
        /// the argument list supplied to the <see cref="MethodInfo"/> being invoked when the timer completes. can be null.
        /// </summary>
        public object[] Args { get; set; }
        /// <summary>
        /// the flags set on the timer.
        /// </summary>
        public long FlagValues { get; set; }
        /// <summary>
        /// the <see cref="BaseInteractiveObject"/> associated with the timer.
        /// </summary>
        public BaseInteractiveObject Io { get; set; }
        /// <summary>
        /// the <see cref="MethodInfo"/> invoked on the associated <see cref="object"/>.
        /// </summary>
        public MethodInfo Method { get; set; }
        /// <summary>
        /// the number of milliseconds in the timer's cycle.
        /// </summary>
        public int Milliseconds { get; set; }
        /// <summary>
        /// the timer's name.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// the <see cref="object"/> having an action applied when the timer completes.
        /// </summary>
        public object Obj { get; set; }
        /// <summary>
        /// the number of times the timer repeats.
        /// </summary>
        public int RepeatTimes { get; set; }
        /// <summary>
        /// the <see cref="Scriptable"/> associated with the timer.
        /// </summary>
        public Scriptable Script { get; set; }
        /// <summary>
        /// the time when the timer starts.
        /// </summary>
        public long StartTime { get; set; }
        /// <summary>
        /// Clears all parameters.
        /// </summary>
        public void Clear()
        {
            Args = null;
            FlagValues = 0;
            Io = null;
            Method = null;
            Milliseconds = 0;
            Name = null;
            Obj = null;
            RepeatTimes = 0;
            Script = null;
            StartTime = 0;
        }
    }
}
