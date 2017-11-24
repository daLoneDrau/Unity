using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Flyweights
{
    class ScriptTimerInitializationParameters
    {
        /**
         * the argument list supplied to the {@link Method} being invoked when the
         * timer completes. can be null.
         */
        private Object[] args;
        /** the flags set on the timer. */
        private long flagValues;
        /** the {@link BaseInteractiveObject} associated with the timer. */
        private BaseInteractiveObject io;
        /** the {@link Method} invoked on the associated {@link Object}. */
        private Method method;
        /** the number of milliseconds in the timer's cycle. */
        private int milliseconds;
        /** the timer's name. */
        private String name;
        /** the {@link Object} having an action applied when the timer completes. */
        private Object obj;
        /** the number of times the timer repeats. */
        private int repeatTimes;
        /** the {@link Scriptable} associated with the timer. */
        private Scriptable script;
        /** the time when the timer starts. */
        private long startTime;
        /** Clears all parameters. */
        public void Clear()
        {
            args = null;
            flagValues = 0;
            io = null;
            method = null;
            milliseconds = 0;
            name = null;
            obj = null;
            repeatTimes = 0;
            script = null;
            startTime = 0;
        }
        /**
         * Gets the argument list supplied to the {@link Method} being invoked when
         * the timer completes. can be null.
         * @return {@link Object}
         */
        public Object[] getArgs()
        {
            return args;
        }
        /**
         * Gets the flags to set on the timer.
         * @return {@link long}
         */
        public long getFlagValues()
        {
            return flagValues;
        }
        /**
         * Gets the {@link BaseInteractiveObject} associated with the timer.
         * @return {@link BaseInteractiveObject}
         */
        public BaseInteractiveObject getIo()
        {
            return io;
        }
        /**
         * Gets the {@link Method} invoked on the associated {@link Object}.
         * @return {@link Method}
         */
        public Method getMethod()
        {
            return method;
        }
        /**
         * Gets the number of milliseconds in the timer's cycle.
         * @return {@link int}
         */
        public int getMilliseconds()
        {
            return milliseconds;
        }
        /**
         * Gets the timer's name.
         * @return {@link String}
         */
        public String getName()
        {
            return name;
        }
        /**
         * Gets the {@link Object} having an action applied when the timer
         * completes.
         * @return {@link Object}
         */
        public Object getObj()
        {
            return obj;
        }
        /**
         * Gets the number of times the timer repeats.
         * @return {@link int}
         */
        public int getRepeatTimes()
        {
            return repeatTimes;
        }
        /**
         * Gets the {@link Scriptable} associated with the timer.
         * @return {@link Scriptable<BaseInteractiveObject>}
         */
        public Scriptable getScript()
        {
            return script;
        }
        /**
         * Gets the time when the timer starts.
         * @return {@link long}
         */
        public long getStartTime()
        {
            return startTime;
        }
        /**
         * Sets the argument list supplied to the {@link Method} being invoked when
         * the timer completes. can be null.
         * @param val the new value to set
         */
        public void setArgs( Object[] val)
        {
            args = val;
        }
        /**
         * Sets the flags to set on the timer.
         * @param val the new value to set
         */
        public void setFlagValues( long val)
        {
            flagValues = val;
        }
        /**
         * Sets the {@link BaseInteractiveObject} associated with the timer.
         * @param val the new value to set
         */
        public void setIo( BaseInteractiveObject val)
        {
            io = val;
        }
        /**
         * Sets the {@link Method} invoked on the associated {@link Object}.
         * @param val the new value to set
         */
        public void setMethod( Method val)
        {
            method = val;
        }
        /**
         * Sets the number of milliseconds in the timer's cycle.
         * @param val the new value to set
         */
        public void setMilliseconds( int val)
        {
            milliseconds = val;
        }
        /**
         * Sets the timer's name.
         * @param val the new value to set
         */
        public void setName( String val)
        {
            name = val;
        }
        /**
         * Sets the {@link Object} having an action applied when the timer
         * completes.
         * @param val the new value to set
         */
        public void setObj( Object val)
        {
            obj = val;
        }
        /**
         * Sets the number of times the timer repeats.
         * @param val the new value to set
         */
        public void setRepeatTimes( int val)
        {
            repeatTimes = val;
        }
        /**
         * Sets the {@link Scriptable} associated with the timer.
         * @param val the new value to set
         */
        public void setScript( Scriptable val)
        {
            script = val;
        }
        /**
         * Sets the time when the timer starts.
         * @param val the new value to set
         */
        public void setStartTime( long val)
        {
            startTime = val;
        }
    }
}
