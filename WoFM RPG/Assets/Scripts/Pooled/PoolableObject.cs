using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGBaseCS.Pooled
{
    interface PoolableObject
    {
        /// <summary>
        ///  Initializes the object.
        /// </summary>
        void Init();
        /// <summary>
        ///  Returns the object to the pool.
        /// </summary>
        void ReturnToPool();
    }
}
