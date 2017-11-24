using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPGBase.Flyweights
{
    public interface IScriptAction
    {
        /**
         * Executes the script action.
         * @ if an error occurs
         */
        void Execute();
        /**
         * Sets the {@link Scriptable} associated with the action.
         * @param script the script
         */
        void SetScript(Scriptable script);
    }
}
