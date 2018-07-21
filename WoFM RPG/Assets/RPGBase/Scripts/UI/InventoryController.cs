using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPGBase.Constants;
using RPGBase.Pooled;
using RPGBase.Flyweights;

namespace Assets.Scripts.UI
{
    public class InventoryController : MonoBehaviour
    {
        /// <summary>
        /// The text fields used for displaying inventory information.
        /// </summary>
        [SerializeField]
        private Text TextField;
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        internal void EnterIo(BaseInteractiveObject io)
        {
            if (io != null
                && io.HasIOFlag(IoGlobals.IO_02_ITEM))
            {
                PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
                sb.Append(io.ItemData.Description);
                sb.Append("\n\nblah");
                TextField.text = sb.ToString();
                sb.ReturnToPool();
            }
        }
        internal void ExitIo(BaseInteractiveObject io)
        {
            if (io != null
                && io.HasIOFlag(IoGlobals.IO_02_ITEM))
            {
                if (TextField.text.StartsWith(io.ItemData.Description, StringComparison.OrdinalIgnoreCase))
                {
                    TextField.text = "";
                }
            }
        }
    }
}
