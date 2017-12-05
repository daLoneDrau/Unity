namespace RPGBase.Flyweights
{
    public abstract class Watcher
    {
        /// <summary>
        /// Updates 
        /// </summary>
        /// <param name="data">the data instance</param>
        public abstract void WatchUpdated(Watchable data);
    }
}
