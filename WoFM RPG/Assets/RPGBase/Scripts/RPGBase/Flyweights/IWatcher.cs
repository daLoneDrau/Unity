namespace RPGBase.Flyweights
{
    public interface IWatcher
    {
        /// <summary>
        /// Updates 
        /// </summary>
        /// <param name="data">the data instance</param>
        void WatchUpdated(Watchable data);
    }
}
