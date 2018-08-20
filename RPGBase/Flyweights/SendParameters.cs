using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPGBase.Flyweights
{
    public struct SendParameters
    {
        public static int FIX = 2;
        public static int GROUP = 1;
        public static int IOItemData = 4;
        public static int IONpcData = 8;
        public static int RADIUS = 16;
        public static int ZONE = 32;
        public string EventName { get; set; }
        public object[] EventParameters { get; set; }
        private long flags;
        public string GroupName { get; set; }
        public int Radius { get; set; }
        public string TargetName { get; set; }
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
        public SendParameters(string initParams, string gName, object[] eventParams, string eName, string tName, int rad)
        {
            GroupName = gName;
            EventName = eName;
            TargetName = tName;
            Radius = rad;
            EventParameters = eventParams;
            flags = 0;
            if (initParams != null
                    && initParams.Length > 0)
            {
                String[] split = initParams.Split(' ');
                for (int i = split.Length - 1; i >= 0; i--)
                {
                    if (string.Equals(split[i], "GROUP", StringComparison.OrdinalIgnoreCase))
                    {
                        AddFlag(SendParameters.GROUP);
                    }
                    if (string.Equals(split[i], "FIX", StringComparison.OrdinalIgnoreCase))
                    {
                        AddFlag(SendParameters.FIX);
                    }
                    if (string.Equals(split[i], "IOItemData", StringComparison.OrdinalIgnoreCase))
                    {
                        AddFlag(SendParameters.IOItemData);
                    }
                    if (string.Equals(split[i], "IONpcData", StringComparison.OrdinalIgnoreCase))
                    {
                        AddFlag(SendParameters.IONpcData);
                    }
                    if (string.Equals(split[i], "RADIUS", StringComparison.OrdinalIgnoreCase))
                    {
                        AddFlag(SendParameters.RADIUS);
                    }
                    if (string.Equals(split[i], "ZONE", StringComparison.OrdinalIgnoreCase))
                    {
                        AddFlag(SendParameters.ZONE);
                    }
                }
            }
        }
        /// <summary>
        /// Adds a flag.
        /// </summary>
        /// <param name="flag">the flag</param>
        public void AddFlag(long flag)
        {
            flags |= flag;
        }
        /// <summary>
        /// Determines if the <see cref="BaseInteractiveObject"/> has a specific flag.
        /// </summary>
        /// <param name="flag">the flag</param>
        /// <returns>true if the <see cref="BaseInteractiveObject"/> has the flag; false otherwise</returns>
        public bool HasFlag(long flag)
        {
            return (flags & flag) == flag;
        }
        /// Removes a flag.
        /// </summary>
        /// <param name="flag">the flag</param>
        public void RemoveFlag(long flag)
        {
            flags &= ~flag;
        }
    }
}
