using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPGBase.Flyweights
{
    public class StackedEvent
    {
        /// <summary>
        /// the event name.
        /// </summary>
        public string Eventname { get; set; }
        /// <summary>
        /// flag indicating whether the event still exists.
        /// </summary>
        public bool Exists { get; set; }
        /// <summary>
        /// the BaseInteractiveObject associated with the event.
        /// </summary>
        public BaseInteractiveObject Io { get; set; }
        /// <summary>
        /// the event message.
        /// </summary>
        public int Msg { get; set; }
        /// <summary>
        /// the event parameters.
        /// </summary>
        public Object[] Parameters { get; set; }
        /// <summary>
        /// the event sender.
        /// </summary>
        public BaseInteractiveObject Sender { get; set; }
    }
}
