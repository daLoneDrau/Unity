using System;
using System.Reflection;

namespace RPGBase.Flyweights
{
    public struct ScriptTimerAction
    {
        /// <summary>
        /// the argument list supplied to the <see cref="MethodInfo"/> being invoked. can be null.
        /// </summary>
        private object[] args;
        /// <summary>
        /// if true, the <see cref="ScriptTimerAction"/> has an existing action.
        /// </summary>
        public bool Exists { get; private set; }
        /// <summary>
        /// the <see cref="MethodInfo"/> invoked on the associated <see cref="object"/>.
        /// </summary>
        private MethodInfo method;
        /** the <see cref="object"/> associated with the <see cref="ScriptTimerAction"/>. */
        private object instance;
        /// <summary>
        /// Creates a new instance of <see cref="ScriptTimerAction"/>.
        /// </summary>
        /// <param name="o">the object having the action applied</param>
        /// <param name="m">the method invoked</param>
        /// <param name="a">any arguments supplied to the method</param>
        public ScriptTimerAction(object o, MethodInfo m, object[] a)
        {
            Exists = true;
            instance = o;
            method = m;
            args = a;
        }
        /// <summary>
        /// Clears the action without processing.
        /// </summary>
        public void Clear()
        {
            Exists = false;
            instance = null;
            method = null;
            args = null;
        }
        /// <summary>
        /// Process the associated action.
        /// </summary>
        public void Process()
        {
            if (Exists)
            {
                Exists = false;
                try
                {
                    method.Invoke(instance, args);
                }
                catch (Exception e)
                {
                    throw new RPGException(ErrorMessage.INTERNAL_ERROR, e);
                }
            }
        }
        /// <summary>
        /// Sets a new action to process.
        /// </summary>
        /// <param name="o">the object having the action applied</param>
        /// <param name="m">the method invoked</param>
        /// <param name="a">ny arguments supplied to the method</param>
        public void Set(object o, MethodInfo m, object[] a)
        {
            Exists = true;
            instance = o;
            method = m;
            args = a;
        }
        /// <summary>
        /// Sets a new action to process.
        /// </summary>
        /// <param name="action">the new action to process</param>
        public void Set(ScriptTimerAction action)
        {
            Exists = true;
            instance = action.instance;
            method = action.method;
            args = action.args;
        }
    }
}
