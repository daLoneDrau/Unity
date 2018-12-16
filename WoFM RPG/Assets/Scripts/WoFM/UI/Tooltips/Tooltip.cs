using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace WoFM.UI.Tooltips
{
    public class Tooltip:MonoBehaviour
    {
        public static Tooltip Instance { get; private set; }
        /// <summary>
        /// The <see cref="Animator"/> that controls the tooltip display.
        /// </summary>
        public Animator Animator;
        private bool showing;
        #region MonoBehavior
        public void Awake()
        {
            Instance = this;
        }
        public void OnEnable()
        {
        }
        #endregion
        public void Show(RectTransform parent, string text)
        {
            transform.SetAsLastSibling();
            if (Animator != null)
            {
                Animator.StopPlayback();
            }
            gameObject.SetActive(true);
            // change the child's text
            transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = text.Replace("\\n", "\n");
            // get parent's height
            float height = parent.sizeDelta.y;
            // get tooltip height
            RectTransform rt = transform.GetChild(0).GetComponent<RectTransform>();
            float myheight = transform.GetChild(0).GetComponent<RectTransform>().sizeDelta.y;

            // assume orientation is above.
            Canvas c = transform.root.GetComponent<Canvas>();
            Vector3 cPos = c.transform.InverseTransformPoint(parent.position);
            
            // local - parent position - wrong
            // transform.localPosition = parent.position;

            // local - inverse parent position - centered over parent
            // transform.localPosition = c.transform.InverseTransformPoint(parent.position);

            // local - inverse parent position + 1/2 parent height + 1/2 tooltip height. puts tooltip frame at top of parent
            cPos.y += (height / 2) + (myheight / 2);
            transform.localPosition = cPos;
            if (Animator != null)
            {
                Animator.Play("Show Tooltip");
            }
        }
        public void Hide()
        {
            // hide tooltip
            if (Animator != null)
            {
                Animator.Play("Hide Tooltip");
            }
        }
    }
}
