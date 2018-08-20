using RPGBase.Flyweights;
using RPGBase.Pooled;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using LabLord.Flyweights;

namespace LabLord.UI.SceneControllers
{
    public class PlayerWatcher : MonoBehaviour, IWatcher
    {
        /// <summary>
        /// the text field dis0laying the STR attribute.
        /// </summary>
        public Text str;
        /// <summary>
        /// the text field dis0laying the STR attribute.
        /// </summary>
        public Text dex;
        /// <summary>
        /// the text field dis0laying the STR attribute.
        /// </summary>
        public Text con;
        /// <summary>
        /// the text field dis0laying the STR attribute.
        /// </summary>
        public Text Int;
        /// <summary>
        /// the text field dis0laying the STR attribute.
        /// </summary>
        public Text wis;
        /// <summary>
        /// the text field dis0laying the STR attribute.
        /// </summary>
        public Text cha;
        public void WatchUpdated(Watchable data)
        {
            LabLordCharacter player = (LabLordCharacter)data;
            PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
            player.ComputeFullStats();
            sb.Append((int)player.GetFullAttributeScore("STR"));
            str.text = sb.ToString();

            sb.Length = 0;
            player.ComputeFullStats();
            sb.Append((int)player.GetFullAttributeScore("DEX"));
            dex.text = sb.ToString();

            sb.Length = 0;
            player.ComputeFullStats();
            sb.Append((int)player.GetFullAttributeScore("CON"));
            con.text = sb.ToString();

            sb.Length = 0;
            player.ComputeFullStats();
            sb.Append((int)player.GetFullAttributeScore("INT"));
            Int.text = sb.ToString();

            sb.Length = 0;
            player.ComputeFullStats();
            sb.Append((int)player.GetFullAttributeScore("WIS"));
            wis.text = sb.ToString();

            sb.Length = 0;
            player.ComputeFullStats();
            sb.Append((int)player.GetFullAttributeScore("CHA"));
            cha.text = sb.ToString();

            sb.ReturnToPool();
            player = null;
            sb = null;
        }
    }
}
