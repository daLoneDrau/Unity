using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Flyweights
{
    class InventorySlot
    {
        /** the item occupying the inventory slot. */
        private IO io;
        /** a flag indicating the item is showing and should be rendered. */
        private bool show;
        /**
         * the list of {@link Watcher}s associated with this {@link IoPcData}.
         */
        private  ArrayList<Watcher>    watchers    = new ArrayList<Watcher>();
        /**
         * {@inheritDoc}
         */
        @Override
    public  void addWatcher( Watcher watcher)
        {
            watchers.add(watcher);
        }
        /**
         * Gets the item occupying the inventory slot.
         * @return {@link IO}
         */
        public  IO getIo()
        {
            return io;
        }
        /**
         * Gets the flag indicating the item is showing and should be rendered.
         * @return <code>bool</code>
         */
        public  bool isShow()
        {
            return show;
        }
        /**
         * {@inheritDoc}
         */
        @Override
    public  void notifyWatchers()
        {
            for (int i = 0; i < watchers.size(); i++)
            {
                watchers.get(i).watchUpdated(this);
            }
        }
        /**
         * {@inheritDoc}
         */
        @Override
    public  void removeWatcher( Watcher watcher)
        {
            watchers.remove(watcher);
        }
        /**
         * Sets the item occupying the inventory slot.
         * @param val the val to set
         */
        public  void setIo( IO val)
        {
            io = val;
            notifyWatchers();
        }
        /**
         * Sets the flag indicating the item is showing and should be rendered.
         * @param flag the show to set
         */
        public  void setShow( bool flag)
        {
            show = flag;
            notifyWatchers();
        }
    }
}
