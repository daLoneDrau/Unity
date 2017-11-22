using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Flyweights
{
    class BehaviourData
    {
        /** the list of animations for each behavior. */
        private String[] animations;
        /** the parameter applied to a behavior. */
        private float behaviorParam;
        /** the behavior flag that has been set. */
        private int behaviour;
        /** flag indicating whether the behavior exists. */
        private bool exists;
        /** the movement mode. */
        private int moveMode;
        /** tactics for the behavior; e.g., 0=none, 1=side, 2=side + back, etc... */
        private int tactics;
        /** the behavior target. */
        private int target;
        // ANIM_USE animlayer[MAX_ANIM_LAYERS];
        /**
         * Gets the flag indicating whether the behavior exists.
         * @return <code>bool</code>
         */
        public bool exists()
        {
            return exists;
        }
        /**
         * Gets the parameter applied to a behavior.
         * @return {@link float}
         */
        public float getBehaviorParam()
        {
            return behaviorParam;
        }
        /**
         * Gets the behavior flag that has been set.
         * @return {@link int}
         */
        public int getBehaviour()
        {
            return behaviour;
        }
        /**
         * Gets the movement mode.
         * @return {@link int}
         */
        public int getMoveMode()
        {
            return moveMode;
        }
        /**
         * Gets the tactics for the behavior.
         * @return {@link int}
         */
        public int getTactics()
        {
            return tactics;
        }
        /**
         * Gets the behavior target.
         * @return {@link int}
         */
        public int getTarget()
        {
            return target;
        }
        /**
         * Sets the parameter applied to a behavior.
         * @param val the parameter to set
         */
        public void setBehaviorParam( float val)
        {
            behaviorParam = val;
        }
        /**
         * Sets the behavior flag that has been set.
         * @param val the new value to set
         */
        public void setBehaviour( int val)
        {
            behaviour = val;
        }
        /**
         * Sets the flag indicating whether the behavior exists.
         * @param val the flag to set
         */
        public void setExists( bool val)
        {
            exists = val;
        }
        /**
         * Sets the movement mode.
         * @param val the mode to set
         */
        public void setMovemode( int val)
        {
            moveMode = val;
        }
        /**
         * Sets the tactics for the behavior.
         * @param val the value to set
         */
        public void setTactics( int val)
        {
            tactics = val;
        }
        /**
         * Sets the behavior target.
         * @param val the value to set
         */
        public void setTarget( int val)
        {
            target = val;
        }
    }
}
