using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Flyweights
{
    class ScriptTimer
    {
        /** the action taken when the script timer completes. */
        private ScriptTimerAction action;
        /** the flag indicating whether the timer exists. */
        private bool exists;
        /** any flags set on the timer. */
        private int flags;
        /** the {@link BaseInteractiveObject} associated with this timer. */
        private BaseInteractiveObject io;
        /** the index of any array the timer is associated with. */
        private long longinfo;
        /** the timer's length in milliseconds. */
        private long msecs;
        /** the timer's name. */
        private String name;
        /** the script associated with the timer. */
        private Scriptable script;
        /** the amount of time passed since the timer was started. */
        private long tim;
        /** the number of times the timer repeats. */
        private long times;
        /** if true, the timer is turn-based, otherwise it is millisecond based. */
        private bool turnBased;
        /**
         * Adds a flag set on the timer..
         * @param flag the flag
         */
        public  void AddFlag( long flag)
        {
            flags |= flag;
        }
        /** Clears all flags that were set. */
        public  void ClearFlags()
        {
            flags = 0;
        }
        /**
         * Gets the flag indicating whether the timer exists.
         * @return <code>bool</code>
         */
        public  bool exists()
        {
            return exists;
        }
        /**
         * Gets the value for the action.
         * @return {@link ScriptTimerAction}
         */
        public  ScriptTimerAction GetAction()
        {
            return action;
        }
        /**
         * Gets the {@link BaseInteractiveObject} associated with this timer.
         * @return {@link BaseInteractiveObject}
         */
        public  BaseInteractiveObject getIo()
        {
            return io;
        }
        /**
         * Gets the index of any array the timer is associated with.
         * @return {@link long}
         */
        public  long getLonginfo()
        {
            return longinfo;
        }
        /**
         * Gets the timer's length in milliseconds.
         * @return {@link long}
         */
        public  long getCycleLength()
        {
            return msecs;
        }
        /**
         * Gets the timer's name.
         * @return {@link String}
         */
        public  String getName()
        {
            return name;
        }
        /**
         * Gets the script associated with the timer.
         * @return {@link Scriptable}<{@link BaseInteractiveObject}>
         */
        public  Scriptable<BaseInteractiveObject> getScript() {
            return script;
        }
        /**
         * Gets the amount of time passed since the timer was started.
         * @return {@link long}
         */
        public  long getLastTimeCheck()
        {
            return tim;
        }
        /**
         * Gets the number of times the timer repeats.
         * @return {@link long}
         */
        public  long getRepeatTimes()
        {
            return times;
        }
        /**
         * Determines if the {@link ScriptTimer} has a specific flag.
         * @param flag the flag
         * @return true if the {@link ScriptTimer} has the flag; false otherwise
         */
        public  bool HasFlag( long flag)
        {
            return (flags & flag) == flag;
        }
        /**
         * Determines whether the timer is turn-based, or millisecond based.
         * @return {@link bool}
         */
        public bool isTurnBased()
        {
            return turnBased;
        }
        /**
         * Removes a flag.
         * @param flag the flag
         */
        public  void RemoveFlag( long flag)
        {
            flags &= ~flag;
        }
        /**
         * Sets the timer.
         * @param params the parameters used to set the timer.
         */
        public  void set( ScriptTimerInitializationParameters params)
        {
            script = (Scriptable) params.getScript();
            exists = true;
            io = (BaseInteractiveObject) params.getIo();
            msecs = params.getMilliseconds();
            name = params.getName();
            action = new ScriptTimerAction(
				params.getObj(),
    				params.getMethod(),
    				params.getArgs());
            tim = params.getStartTime();
            times = params.getRepeatTimes();
            ClearFlags();
            AddFlag(params.getFlagValues());
        }
        /**
         * Sets the action taken when the script timer completes.
         * @param sta the {@link ScriptTimerAction}
         */
        public  void setAction( ScriptTimerAction sta)
        {
            action = sta;
        }
        /**
         * Sets the timer's length in milliseconds.
         * @param val the value to set
         */
        public  void setCycleLength( long val)
        {
            msecs = val;
        }
        /**
         * Sets the flag indicating whether the timer exists.
         * @param flag the flag to set
         */
        public  void setExists( bool flag)
        {
            exists = flag;
        }
        /**
         * Sets the {@link BaseInteractiveObject} associated with this timer.
         * @param val the value to set
         */
        public  void setIo( BaseInteractiveObject val)
        {
            io = val;
        }
        /**
         * Sets the index of any array the timer is associated with.
         * @param val the value to set
         */
        public  void setLonginfo( long val)
        {
            longinfo = val;
        }
        /**
         * Sets the timer's name.
         * @param val the value to set
         */
        public  void setName( String val)
        {
            name = val;
        }
        /**
         * Sets the script associated with the timer.
         * @param val the {@link Scriptable}<{@link BaseInteractiveObject}> to set
         */
        public  void setScript( Scriptable val)
        {
            script = val;
        }
        /**
         * Sets the amount of time passed since the timer was started.
         * @param val the value to set
         */
        public  void setLastTimeCheck( long val)
        {
            tim = val;
        }
        /**
         * Sets the number of times the timer repeats.
         * @param val the value to set
         */
        public  void setRepeatTimes( long val)
        {
            times = val;
        }
        /**
         * Sets whether the timer is turn-based, or millisecond based.
         * @param isTurnBased the new value to set
         */
        public void setTurnBased( bool flag)
        {
            this.turnBased = flag;
        }
    }
}
