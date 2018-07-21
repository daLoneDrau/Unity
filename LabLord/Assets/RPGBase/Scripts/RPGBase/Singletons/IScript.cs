using RPGBase.Flyweights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPGBase.Singletons
{
    public interface IScript
    {
        void ClearAdditionalEventStacks();
        void ClearAdditionalEventStacksForIO(BaseInteractiveObject io);
        void DestroyScriptTimers();
        void EventStackInit();
        void ExecuteAdditionalStacks();
        ScriptTimer GetScriptTimer(int id);
        ScriptTimer[] GetScriptTimers();
        StackedEvent GetStackedEvent(int index);
        void InitScriptTimers();
        void SetScriptTimer(int index, ScriptTimer timer);
    }
}
