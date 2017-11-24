using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Flyweights
{
    class ScriptTimerAction
    {
        /**
         * the argument list supplied to the {@link Method} being invoked. can be
         * null.
         */
        private Object[] args;
        /** if true, the {@link ScriptTimerAction} has an existing action. */
        private bool exists = false;
        /** the {@link Method} invoked on the associated {@link Object}. */
        private Method method;
        /** the {@link Object} associated with the {@link ScriptTimerAction}. */
        private Object object;
	/**
	 * Creates a new instance of {@link ScriptTimerAction}.
	 * @param o the object having the action applied
	 * @param m the method invoked
	 * @param a any arguments supplied to the method
	 */
	public ScriptTimerAction( Object o,  Method m,  Object[] a)
        {
            exists = true;
            object = o;
            method = m;
            args = a;
        }
        /** Clears the action without processing. */
        public void Clear()
        {
            exists = false;
            object = null;
            method = null;
            args = null;
        }
        /**
         * Determines if the {@link ScriptTimerAction} has an existing action.
         * @return <tt>true</tt> if the {@link ScriptTimerAction} has an existing
         *         action; <tt>false</tt> otherwise
         */
        public bool exists()
        {
            return exists;
        }
        /**
         * Process the associated action.
         * @ if an error occurs
         */
        public void process() 
        {
		if (exists) {
			exists = false;
			try {
				method.invoke(object, args);
			} catch (IllegalAccessException | IllegalArgumentException
					| InvocationTargetException e) {
				throw new RPGException(ErrorMessage.INTERNAL_ERROR, e);
}
		}
	}
	/**
	 * Sets a new action to process.
	 * @param o the object having the action applied
	 * @param m the method invoked
	 * @param a any arguments supplied to the method
	 */
	public void set( Object o,  Method m,  Object[] a)
{
    exists = true;
    object = o;
    method = m;
    args = a;
}
/**
 * Sets a new action to process.
 * @param action the new action to process
 */
public void set( ScriptTimerAction action)
{
    exists = true;
    object = action.object;
    method = action.method;
    args = action.args;
}
    }
}
