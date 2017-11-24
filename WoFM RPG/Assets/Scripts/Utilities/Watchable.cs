using UnityEngine;

namespace RPGBase.Utilities
{
    public abstract class Watchable
    {
        public abstract void AddWatcher(Watcher watcher);
        public abstract void NotifyWatchers();
        public abstract void RemoveWatcher(Watcher watcher);
    }
}
