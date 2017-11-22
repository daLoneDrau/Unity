﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Flyweights
{
    class StackedEvent
    {
        /** the event name. */
        private String eventname;
        /** flag indicating whether the event still exists. */
        private bool exist;
        /** the IO associated with the event. */
        private IO io;
        /** the event message. */
        private int msg;
        /** the event parameters. */
        private Object[]	params;
	/** the event sender. */
	private IO sender;
        /**
         * Gets the flag indicating whether the event still exists.
         * @return <code>bool</code>
         */
        public  bool exists()
        {
            return exist;
        }
        /**
         * Gets the event name.
         * @return {@link String}
         */
        public  String getEventname()
        {
            return eventname;
        }
        /**
         * Gets the IO associated with the event.
         * @return {@link IO}
         */
        public  IO getIo()
        {
            return io;
        }
        /**
         * Gets the event message.
         * @return {@link int}
         */
        public  int getMsg()
        {
            return msg;
        }
        /**
         * Gets the event parameters.
         * @return {@link Object}[]
         */
        public  Object[] getParams()
        {
            return params;
        }
        /**
         * Gets the event sender.
         * @return {@link IO}
         */
        public  IO getSender()
        {
            return sender;
        }
        /**
         * Sets the event name.
         * @param val the eventname to set
         */
        public  void setEventname( String val)
        {
            this.eventname = val;
        }
        /**
         * Sets the flag indicating whether the event still exists.
         * @param val the exist to set
         */
        public  void setExist( bool val)
        {
            this.exist = val;
        }
        /**
         * Sets the IO associated with the event.
         * @param val the io to set
         */
        public  void setIo( IO val)
        {
            this.io = val;
        }
        /**
         * Sets the event message.
         * @param val the msg to set
         */
        public  void setMsg( int val)
        {
            this.msg = val;
        }
        /**
         * Sets the event parameters.
         * @param val the params to set
         */
        public  void setParams( Object[] val)
        {
            this.params = val;
        }
        /**
         * Sets the event sender.
         * @param val the sender to set
         */
        public  void setSender( IO val)
        {
            this.sender = val;
        }
    }
}
