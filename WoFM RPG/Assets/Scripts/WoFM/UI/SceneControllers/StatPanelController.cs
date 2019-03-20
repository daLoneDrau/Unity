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
using WoFM.UI.Tooltips;

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
        public InteractiveTooltipWidgetStatbar LuckTooltip;
        /// <summary>
        /// The Skill gauge.
        /// </summary>
        public GameObject SkillGauge;
        public InteractiveTooltipWidgetStatbar SkillTooltip;
        /// <summary>
        /// The Stamina gauge.
        /// </summary>
        public GameObject StaminaGauge;
        public InteractiveTooltipWidgetStatbar StaminaTooltip;
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
            RectTransform rt = LuckGauge.GetComponent<RectTransform>();
            float currentBarLen = rt.anchorMax.x - MinX;
            float realPercent = ioData.GetFullAttributeScore("LUK") / 12f;
            if (realPercent > 1)
            {
                realPercent = 1f;
            }
            float realBarLen = realPercent * barLen;
            if (currentBarLen != realBarLen)
            {
                StartCoroutine(UpdateGuage(LuckGauge, realPercent));
            }
            LuckTooltip.IoId = ioData.GetIo().RefId;
        }
        private void CheckForSkillBarUpdate(IOCharacter ioData)
        {
            RectTransform rt = SkillGauge.GetComponent<RectTransform>();
            float currentBarLen = rt.anchorMax.x - MinX;
            float realPercent = ioData.GetFullAttributeScore("SKL") / 12f;
            if (realPercent > 1)
            {
                realPercent = 1f;
            }
            float realBarLen = realPercent * barLen;
            if (currentBarLen != realBarLen)
            {
                StartCoroutine(UpdateGuage(SkillGauge, realPercent));
            }
            SkillTooltip.IoId = ioData.GetIo().RefId;
        }
        private void CheckForStaminaBarUpdate(IOCharacter ioData)
        {
            RectTransform rt = StaminaGauge.GetComponent<RectTransform>();
            float currentBarLen = rt.anchorMax.x - MinX;
            float realPercent = ioData.Life / 24f;
            if (realPercent > 1)
            {
                realPercent = 1f;
            }
            float realBarLen = realPercent * barLen;
            if (currentBarLen != realBarLen)
            {
                StartCoroutine(UpdateGuage(StaminaGauge, realPercent));
            }
            StaminaTooltip.IoId = ioData.GetIo().RefId;
        }
    }
}
