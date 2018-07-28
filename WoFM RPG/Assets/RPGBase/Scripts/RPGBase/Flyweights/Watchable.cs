namespace RPGBase.Flyweights
{
    public abstract class Watchable
    {
        public abstract void AddWatcher(IWatcher watcher);
        public abstract void NotifyWatchers();
        public abstract void RemoveWatcher(IWatcher watcher);
    }
}
