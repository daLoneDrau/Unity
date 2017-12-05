using System;
using System.Collections.Generic;
using System.Text;
using RPGBase.Flyweights;

namespace RPGBase.Singletons
{
    public abstract class WebServiceClient
    {
        /// <summary>
        /// the one and only instance of the <see cref="WebServiceClient"/> class.
        /// </summary>
        private static WebServiceClient instance;
        /// <summary>
        /// Gives access to the singleton instance of <see cref="WebServiceClient"/>.
        /// </summary>
        /// <returns><see cref="WebServiceClient"/></returns>
        public static WebServiceClient GetInstance()
        {
            return WebServiceClient.instance;
        }
        /// <summary>
        /// Creates a new instance of <see cref="WebServiceClient"/>.
        /// </summary>
        protected WebServiceClient() { }

        internal abstract BaseInteractiveObject GetItemByName(string item);
    }
}
