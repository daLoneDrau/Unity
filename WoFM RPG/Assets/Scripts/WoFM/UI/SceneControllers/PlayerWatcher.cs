using RPGBase.Flyweights;
using RPGBase.Pooled;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using WoFM.Flyweights;

namespace WoFM.UI.SceneControllers
{
    public class PlayerWatcher : MonoBehaviour, IWatcher
    {
        public Text skill;
        public Text stamina;
        public Text luck;
        public void WatchUpdated(Watchable data)
        {
            WoFMCharacter player = (WoFMCharacter)data;
            PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
            player.ComputeFullStats();
            sb.Append((int)player.GetFullAttributeScore("SKL"));
            sb.Append("/");
            sb.Append((int)player.GetFullAttributeScore("MSK"));
            skill.text = sb.ToString();

            sb.Length = 0;
            sb.Append((int)player.GetFullAttributeScore("STM"));
            sb.Append("/");
            sb.Append((int)player.GetFullAttributeScore("MSTM"));
            stamina.text = sb.ToString();

            sb.Length = 0;
            sb.Append((int)player.GetFullAttributeScore("LUK"));
            sb.Append("/");
            sb.Append((int)player.GetFullAttributeScore("MLK"));
            luck.text = sb.ToString();

            sb.ReturnToPool();
            player = null;
            sb = null;
        }
    }
}
