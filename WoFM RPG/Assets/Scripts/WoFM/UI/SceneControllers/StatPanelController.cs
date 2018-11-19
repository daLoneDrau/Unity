using RPGBase.Flyweights;
using RPGBase.Pooled;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.WoFM.UI.SceneControllers
{
    public class StatPanelController : Singleton<StatPanelController>, IWatcher
    {
        /// <summary>
        /// the length of a full gauge bar.
        /// </summary>
        private float barLen;
        /// <summary>
        /// the minimum x-anchor position of a gauge
        /// </summary>
        public float MinX;
        /// <summary>
        /// the maximum x-anchor position of a gauge
        /// </summary>
        public float MaxX;
        /// <summary>
        /// The Luck gauge.
        /// </summary>
        public GameObject LuckGauge;
        public Text LuckTooltip;
        /// <summary>
        /// The Skill gauge.
        /// </summary>
        public GameObject SkillGauge;
        public Text SkillTooltip;
        /// <summary>
        /// The Stamina gauge.
        /// </summary>
        public GameObject StaminaGauge;
        public Text StaminaTooltip;
        /// <summary>
        /// the time it will take the object to perform a move, in seconds.
        /// </summary>
        public float moveTime;
        /// <summary>
        /// used to make movement calculations more efficient.
        /// </summary>
        private float inverseMoveTime;
        public void Awake()
        {
            // print("statcontroller awake("+ moveTime);
            // store reciprical of moveTime to use multiplaction instead of division
            inverseMoveTime = 1f / moveTime;
            barLen = MaxX - MinX;
        }
        private IEnumerator UpdateGuage(GameObject gauge, float val)
        {
            float realVal = barLen * val;
            realVal += MinX;
            RectTransform rt = gauge.GetComponent<RectTransform>();
            float remaining = realVal - rt.anchorMax.x;
            // while remaining distance still not 0
            while (Mathf.Abs(remaining) > float.Epsilon)
            {
                // find a position proportionally closer to the end based on the move time.
                Vector2 newPosition = Vector2.MoveTowards(new Vector2(rt.anchorMax.x, 1), new Vector2(realVal, 1), inverseMoveTime * Time.deltaTime);
                print("new x " + newPosition.x);
                // update the gauge
                if (newPosition.x > MaxX)
                {
                    rt.anchorMax = new Vector2(MaxX, 1);
                    break;
                }
                else if (newPosition.x < MinX)
                {
                    rt.anchorMax = new Vector2(MinX, 1);
                    break;
                }
                else
                {
                    rt.anchorMax = new Vector2(newPosition.x, 1);
                }
                // re-calculate remaining distance
                remaining = realVal - rt.anchorMax.x;
                // wait one frame before re-evaluating loop condition
                yield return null;
            }
        }
        public void WatchUpdated(Watchable data)
        {
            // data has been updated
            // did skill just change?
            print("got notified of something");
            CheckForLuckBarUpdate((IOCharacter)data);
            CheckForSkillBarUpdate((IOCharacter)data);
            CheckForStaminaBarUpdate((IOCharacter)data);
            // check which value needs to be updated
            /***********************************************************
             * SKILL
             **********************************************************/
        }
        private void CheckForLuckBarUpdate(IOCharacter ioData)
        {
            print("checking luck");
            RectTransform rt = LuckGauge.GetComponent<RectTransform>();
            float currentBarLen = rt.anchorMax.x - MinX;
            float realPercent = ioData.GetFullAttributeScore("LUK") / ioData.GetFullAttributeScore("MLK");
            if (realPercent > 1)
            {
                realPercent = 1f;
            }
            float realBarLen = realPercent * barLen;
            if (currentBarLen != realBarLen)
            {
                StartCoroutine(UpdateGuage(LuckGauge, realPercent));
            }
            PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
            sb.Append((int)ioData.GetFullAttributeScore("LUK"));
            sb.Append("/");
            sb.Append((int)ioData.GetFullAttributeScore("MLK"));
            LuckTooltip.text = sb.ToString();
            sb.ReturnToPool();
        }
        private void CheckForSkillBarUpdate(IOCharacter ioData)
        {
            print("checking skill");
            RectTransform rt = SkillGauge.GetComponent<RectTransform>();
            float currentBarLen = rt.anchorMax.x - MinX;
            float realPercent = ioData.GetFullAttributeScore("SKL") / ioData.GetFullAttributeScore("MSK");
            if (realPercent > 1)
            {
                realPercent = 1f;
            }
            float realBarLen = realPercent * barLen;
            if (currentBarLen != realBarLen)
            {
                StartCoroutine(UpdateGuage(SkillGauge, realPercent));
            }
            PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
            sb.Append((int)ioData.GetFullAttributeScore("SKL"));
            sb.Append("/");
            sb.Append((int)ioData.GetFullAttributeScore("MSK"));
            SkillTooltip.text = sb.ToString();
            sb.ReturnToPool();
        }
        private void CheckForStaminaBarUpdate(IOCharacter ioData)
        {
            print("checking stam");
            RectTransform rt = StaminaGauge.GetComponent<RectTransform>();
            float currentBarLen = rt.anchorMax.x - MinX;
            float realPercent = ioData.Life / ioData.GetFullAttributeScore("MSTM");
            if (realPercent > 1)
            {
                realPercent = 1f;
            }
            float realBarLen = realPercent * barLen;
            if (currentBarLen != realBarLen)
            {
                print("need to update stam");
                StartCoroutine(UpdateGuage(StaminaGauge, realPercent));
            }
            PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
            sb.Append((int)ioData.Life);
            sb.Append("/");
            sb.Append((int)ioData.GetFullAttributeScore("MSTM"));
            StaminaTooltip.text = sb.ToString();
            sb.ReturnToPool();
        }
    }
}
