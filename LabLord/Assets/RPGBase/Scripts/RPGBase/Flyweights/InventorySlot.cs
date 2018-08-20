using System;
using System.Collections.Generic;

namespace RPGBase.Flyweights
{
    /// <summary>
    /// 
    /// </summary>
    public class InventorySlot : Watchable
    {
        private BaseInteractiveObject io;
        /// <summary>
        /// the item occupying the inventory slot.
        /// </summary>
        public BaseInteractiveObject Io
        {
            get { return io; }
            set
            {
                io = value;
                NotifyWatchers();
            }
        }
        private bool show;
        /// <summary>
        /// a flag indicating the item is showing and should be rendered.
        /// </summary>
        public bool Show
        {
            get { return show; }
            set
            {
                show = value;
                NotifyWatchers();
            }
        }
        /// <summary>
        /// the list of <see cref="IWatcher"/>s associated with this <see cref="InventorySlot"/>.
        /// </summary>
        private List<IWatcher> watchers = new List<IWatcher>();
        public override void AddWatcher(IWatcher watcher)
        {
            watchers.Add(watcher);
        }
        public override void NotifyWatchers()
        {
            for (int i = watchers.Count - 1; i >= 0; i--)
            {
                watchers[i].WatchUpdated(this);
            }
        }
        public override void RemoveWatcher(IWatcher watcher)
        {
            watchers.Remove(watcher);
        }
    }
}
