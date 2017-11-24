using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Flyweights
{
    class SendParameters
    {
        public static int FIX = 2;
        public static int GROUP = 1;
        public static int IOItemData = 4;
        public static int IONpcData = 8;
        public static int RADIUS = 16;
        public static int ZONE = 32;
        private char[] eventName;
        private Object[] eventParameters;
        private long flags;
        private char[] groupName;
        private int radius;
        private char[] targetName;
        /**
         * Creates a new instance of {@link SendParameters}.
         * @param initParams the initialization parameters, such as GROUP, RADIUS,
         *            etc.
         * @param gName the group name
         * @param eventParams the event parameters
         * @param eName the event name
         * @param tName the target name
         * @param rad the radius
         */
        public SendParameters( String initParams,  String gName,
                 Object[] eventParams,  String eName,
                 String tName,  int rad)
        {
            setGroupName(gName);
            setEventName(eName);
            setTargetName(tName);
            radius = rad;
            eventParameters = eventParams;
            if (initParams != null
                    && initParams.Length() > 0)
            {
                String[] split = initParams.split(" ");
                for (int i = split.Length - 1; i >= 0; i--)
                {
                    if (split[i].equalsIgnoreCase("GROUP"))
                    {
                        addFlag(SendParameters.GROUP);
                    }
                    if (split[i].equalsIgnoreCase("FIX"))
                    {
                        addFlag(SendParameters.FIX);
                    }
                    if (split[i].equalsIgnoreCase("IOItemData"))
                    {
                        addFlag(SendParameters.IOItemData);
                    }
                    if (split[i].equalsIgnoreCase("IONpcData"))
                    {
                        addFlag(SendParameters.IONpcData);
                    }
                    if (split[i].equalsIgnoreCase("RADIUS"))
                    {
                        addFlag(SendParameters.RADIUS);
                    }
                    if (split[i].equalsIgnoreCase("ZONE"))
                    {
                        addFlag(SendParameters.ZONE);
                    }
                }
            }
        }
        /**
         * Adds a flag.
         * @param flag the flag
         */
        public void addFlag( long flag)
        {
            flags |= flag;
        }
        /**
         * @return the speechName
         */
        public String getEventName()
        {
            String s = null;
            if (eventName != null)
            {
                s = new String(eventName);
            }
            return s;
        }
        /**
         * @return the eventParameters
         */
        public Object[] getEventParameters()
        {
            return eventParameters;
        }
        /**
         * @return the speechName
         */
        public String getGroupName()
        {
            String s = null;
            if (groupName != null)
            {
                s = new String(groupName);
            }
            return s;
        }
        /**
         * @return the radius
         */
        public int getRadius()
        {
            return radius;
        }
        /**
         * @return the speechName
         */
        public String getTargetName()
        {
            String s = null;
            if (targetName != null)
            {
                s = new String(targetName);
            }
            return s;
        }
        /**
         * Determines if the {@link BaseInteractiveObject} has a specific flag.
         * @param flag the flag
         * @return true if the {@link BaseInteractiveObject} has the flag; false
         *         otherwise
         */
        public  bool hasFlag( long flag)
        {
            return (flags & flag) == flag;
        }
        /**
         * Removes a flag.
         * @param flag the flag
         */
        public  void removeFlag( long flag)
        {
            flags &= ~flag;
        }
        /**
         * @param val the value to set
         */
        public void setEventName(String val)
        {
            if (val != null)
            {
                eventName = val;
            }
        }
        /**
         * @param eventParameters the eventParameters to set
         */
        public void setEventParameters(Object[] eventParameters)
        {
            this.eventParameters = eventParameters;
        }
        /**
         * @param val the value to set
         */
        public void setGroupName(String val)
        {
            if (val != null)
            {
                groupName = val;
            }
        }
        /**
         * @param radius the radius to set
         */
        public void setRadius(int radius)
        {
            this.radius = radius;
        }
        /**
         * @param val the value to set
         */
        public void setTargetName(String val)
        {
            if (val != null)
            {
                targetName = val;
            }
        }
    }
}
