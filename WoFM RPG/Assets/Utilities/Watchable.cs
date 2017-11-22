using UnityEngine;

public abstract class Watchable : MonoBehaviour {
    public abstract void AddWatcher(Watcher watcher);
    public abstract void NotifyWatchers();
    public abstract void RemoveWatcher( Watcher watcher);
}
