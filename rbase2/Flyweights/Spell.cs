using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Flyweights
{
    class Spell
    {
        /** the caster's reference id. */
        private int caster;
        /** the caster's level. */
        private float casterLevel;
        private bool exists;
        /** any flags that have been set. */
        private long flags = 0;
        private int lastTurnUpdated;
        private long lastUpdated;
        private int longinfo;
        private int longinfo2;
        private Object misc;
        private int target;
        private long timeCreated;
        private long timeToLive;
        private int turnCreated;
        private int turnsToLive;
        private int type;
        /**
         * Adds a flag.
         * @param flag the flag
         */
        public void AddFlag( long flag)
        {
            flags |= flag;
        }
        /** Clears all flags that were set. */
        public void ClearFlags()
        {
            flags = 0;
        }
        /**
         * Gets the exists
         * @return <code>bool</code>
         */
        public bool exists()
        {
            return exists;
        }
        /**
         * Gets the caster
         * @return {@link int}
         */
        public int getCaster()
        {
            return caster;
        }
        /**
         * Gets the caster level.
         * @return {@link float}
         */
        public  float getCasterLevel()
        {
            return casterLevel;
        }
        /**
         * Gets the lastTurnUpdated
         * @return {@link int}
         */
        public int getLastTurnUpdated()
        {
            return lastTurnUpdated;
        }
        /**
         * Gets the lastUpdated
         * @return {@link long}
         */
        public long getLastUpdated()
        {
            return lastUpdated;
        }
        /**
         * Gets the value of the longinfo.
         * @return {@link int}
         */
        public int getLonginfo()
        {
            return longinfo;
        }
        /**
         * Gets the value of the longinfo2.
         * @return {@link int}
         */
        public int getLonginfo2()
        {
            return longinfo2;
        }
        /**
         * Gets the value of the misc.
         * @return {@link Object}
         */
        public Object getMisc()
        {
            return misc;
        }
        /**
         * Gets the target
         * @return {@link int}
         */
        public int getTarget()
        {
            return target;
        }
        /**
         * Gets the timeCreated
         * @return {@link long}
         */
        public long getTimeCreated()
        {
            return timeCreated;
        }
        /**
         * Gets the timeToLive
         * @return {@link long}
         */
        public long getTimeToLive()
        {
            return timeToLive;
        }
        /**
         * Gets the turnCreated
         * @return {@link int}
         */
        public int getTurnCreated()
        {
            return turnCreated;
        }
        /**
         * Gets the turnsToLive
         * @return {@link int}
         */
        public int getTurnsToLive()
        {
            return turnsToLive;
        }
        /**
         * Gets the type
         * @return {@link int}
         */
        public int getType()
        {
            return type;
        }
        /**
         * Determines if the {@link Spell} has a specific flag.
         * @param flag the flag
         * @return true if the {@link Spell} has the flag; false otherwise
         */
        public bool HasFlag( long flag)
        {
            return (flags & flag) == flag;
        }
        /**
         * Removes a flag.
         * @param flag the flag
         */
        public void RemoveFlag( long flag)
        {
            flags &= ~flag;
        }
        /**
         * Sets the caster
         * @param caster the caster to set
         */
        public void setCaster( int caster)
        {
            this.caster = caster;
        }
        /**
         * Sets the caster level.
         * @param val the level to set
         */
        public  void setCasterLevel( float val)
        {
            casterLevel = val;
        }
        /**
         * Sets the exists
         * @param exists the exists to set
         */
        public void setExists( bool exists)
        {
            this.exists = exists;
        }
        /**
         * Sets the lastTurnUpdated
         * @param lastTurnUpdated the lastTurnUpdated to set
         */
        public void setLastTurnUpdated( int lastTurnUpdated)
        {
            this.lastTurnUpdated = lastTurnUpdated;
        }
        /**
         * Sets the lastUpdated
         * @param lastUpdated the lastUpdated to set
         */
        public void setLastUpdated( long lastUpdated)
        {
            this.lastUpdated = lastUpdated;
        }
        /**
         * Sets the value of the longinfo.
         * @param longinfo the value to set
         */
        public void setLonginfo( int longinfo)
        {
            this.longinfo = longinfo;
        }
        /**
         * Sets the value of the longinfo2.
         * @param longinfo2 the value to set
         */
        public void setLonginfo2( int longinfo2)
        {
            this.longinfo2 = longinfo2;
        }
        /**
         * Sets the value of the misc.
         * @param misc the value to set
         */
        public void setMisc( Object misc)
        {
            this.misc = misc;
        }
        /**
         * Sets the target
         * @param target the target to set
         */
        public void setTarget( int target)
        {
            this.target = target;
        }
        /**
         * Sets the timeCreated
         * @param timeCreated the timeCreated to set
         */
        public void setTimeCreated( long timeCreated)
        {
            this.timeCreated = timeCreated;
        }
        /**
         * Sets the timeToLive
         * @param timeToLive the timeToLive to set
         */
        public void setTimeToLive( long timeToLive)
        {
            this.timeToLive = timeToLive;
        }
        /**
         * Sets the turnCreated
         * @param turnCreated the turnCreated to set
         */
        public void setTurnCreated( int turnCreated)
        {
            this.turnCreated = turnCreated;
        }
        /**
         * Sets the turnsToLive
         * @param turnsToLive the turnsToLive to set
         */
        public void setTurnsToLive( int turnsToLive)
        {
            this.turnsToLive = turnsToLive;
        }
        /**
         * Sets the type
         * @param type the type to set
         */
        public void setType( int type)
        {
            this.type = type;
        }
    }
}
