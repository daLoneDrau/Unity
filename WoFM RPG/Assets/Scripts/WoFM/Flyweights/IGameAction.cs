using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WoFM.Flyweights
{
    /// <summary>
    /// a <see cref="GameAction"/> is an action that has been taken by an Interactive object and must be resolved
    /// </summary>
    public interface IGameAction
    {
        /// <summary>
        /// Determines if the action has been resolved.
        /// </summary>
        /// <returns></returns>
        bool IsResolved();
        /// <summary>
        /// Executes the action.
        /// </summary>
        void Execute();
    }
}
