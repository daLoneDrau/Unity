using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace Assets.Scripts.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class HorizontalLayoutHandler : UIBehaviour, IRPGLayoutHandler
    {
        private float lastUpdate = -1f;
        public enum VerticalAlignment
        {
            Upper,
            Middle,
            Lower
        }
        [SerializeField] int spacing;
        [Serializable]
        public class Padding
        {
            public int Left;
            public int Top;
            public int Bottom;
            public int Right;
        }
        [SerializeField] Padding padding;
        [SerializeField] VerticalAlignment VerticalAlign = VerticalAlignment.Upper;
        public virtual void SetLayoutHorizontal() { }
        public virtual void SetLayoutVertical()
        {
            if (gameObject.activeSelf)
            {
                float now = Time.realtimeSinceStartup;
                if (lastUpdate < 0)
                {
                    lastUpdate = now;
                    print("SetLayoutHorizontal::" + now);
                    UpdateRect();
                }
                else if (now - lastUpdate > 1f)
                {
                    lastUpdate = now;
                    print("SetLayoutHorizontal::" + now);
                    UpdateRect();
                }
            }
        }
#if UNITY_EDITOR
        protected override void OnValidate()
        {
            if (gameObject.activeSelf)
            {
                float now = Time.realtimeSinceStartup;
                if (lastUpdate < 0)
                {
                    lastUpdate = now;
                    print("OnValidate::" + now);
                    UpdateRect();
                }
                else if (now - lastUpdate > 1f)
                {
                    lastUpdate = now;
                    print("OnValidate::" + now);
                    UpdateRect();
                }
            }
        }
#endif
        protected override void OnRectTransformDimensionsChange()
        {
            if (gameObject.activeSelf)
            {
                float now = Time.realtimeSinceStartup;
                if (lastUpdate < 0)
                {
                    lastUpdate = now;
                    print("OnRectTransformDimensionsChange::" + now);
                    UpdateRect();
                }
                else if (now - lastUpdate > 1f)
                {
                    lastUpdate = now;
                    print("OnRectTransformDimensionsChange::" + now);
                    UpdateRect();
                }
            }
        }
        Vector2 GetParentSize()
        {
            RectTransform parent = transform.parent as RectTransform;

            return parent == null ? Vector2.zero : parent.rect.size;
        }
        private void Resize(Vector2 size)
        {
            print("Resize(" + size);
            var rectTransform = GetComponent<RectTransform>();
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size.y);
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size.x);
            print("rect::" + rectTransform.rect);
        }
        private float minWidth;
        private float maxHeight;
        private void IterateThroughChildren()
        {
            minWidth = 0;
            maxHeight = 0;
            var rectTransform = GetComponent<RectTransform>();
            foreach (Transform child in rectTransform)
            {
                print("check for layout manager");
                if (!child.gameObject.activeSelf)
                {
                    continue;
                }
                if ((child.gameObject.GetComponent("IRPGLayoutHandler") as IRPGLayoutHandler) != null)
                {
                    print("child has layout manager");
                    var lm = child.gameObject.GetComponent("IRPGLayoutHandler") as IRPGLayoutHandler;
                    print(lm);
                    lm.UpdateRect();
                }
                RectTransform rect = (RectTransform)child;
                print("*************************child");
                print(rect.rect);
                maxHeight = Mathf.Max(maxHeight, rect.rect.height);
                minWidth += rect.rect.width;
            }
            maxHeight += padding.Top + padding.Bottom;
            minWidth += padding.Left + padding.Right;
            minWidth += (rectTransform.childCount - 1) * spacing;
            print(minWidth + "," + maxHeight);
        }
        private void PlaceChildHorizontally(float left, RectTransform child)
        {
            print("PlaceChildHorizontally("+left);
            Vector2 childPivot = child.pivot;
            // move to left should be offset by element width times pivot point
            float childWidth = child.rect.width * (1 - child.pivot.x);
            float myNewX = left + childWidth;
            if (myNewX != child.anchoredPosition.x)
            {
                float diff = 0;
                if (myNewX < child.anchoredPosition.x)
                {
                    // new location is left of current location
                    diff = child.anchoredPosition.x - myNewX;
                    child.anchoredPosition += Vector2.left * diff;
                }
                else
                {
                    // new location is right of current location
                    diff = myNewX - child.anchoredPosition.x;
                    child.anchoredPosition += Vector2.right * diff;
                }
            }
        }
        private void PlaceChildVertically(RectTransform parent, RectTransform child)
        {
            switch (VerticalAlign)
            {
                case VerticalAlignment.Upper:
                    // try to move to top of parent
                    // top is parent height / 2
                    float pHeight = parent.rect.height / 2;
                    // move to top should be offset by element height times pivot point
                    float myHeight = child.rect.height * (1 - child.pivot.y);
                    float myNewY = pHeight - myHeight;
                    if (myNewY != child.anchoredPosition.y)
                    {
                        float diff = 0;
                        if (myNewY != child.anchoredPosition.y)
                        {
                            if (myNewY < child.anchoredPosition.y)
                            {
                                // new location is below current location
                                diff = child.anchoredPosition.y - myNewY;
                                child.anchoredPosition += Vector2.down * diff;
                            }
                            else
                            {
                                // new location is above current location
                                diff = myNewY - child.anchoredPosition.y;
                                child.anchoredPosition += Vector2.up * diff;
                            }
                        }
                    }
                    break;
                case VerticalAlignment.Middle:
                    // try to move to middle of parent
                    // middle is 0
                    pHeight = 0;
                    // move to middle should be offset by element height times pivot point
                    myHeight = child.rect.height * (1 - child.pivot.y);
                    myHeight -= child.rect.height * 0.5F;
                    myNewY = pHeight + myHeight;
                    if (myNewY != child.anchoredPosition.y)
                    {
                        float diff = 0;
                        if (myNewY < child.anchoredPosition.y)
                        {
                            // new location is below current location
                            diff = child.anchoredPosition.y - myNewY;
                            child.anchoredPosition += Vector2.down * diff;
                        }
                        else
                        {
                            // new location is above current location
                            diff = myNewY - child.anchoredPosition.y;
                            child.anchoredPosition += Vector2.up * diff;
                        }
                    }
                    break;
                case VerticalAlignment.Lower:
                    // try to move to bottom of parent
                    // bottom is -parent height / 2
                    pHeight = -parent.rect.height / 2;
                    // move to bottom should be offset by element height times pivot point
                    myHeight = child.rect.height * (1 - child.pivot.y);
                    myNewY = pHeight + myHeight;
                    if (myNewY != child.anchoredPosition.y)
                    {
                        float diff = 0;
                        if (myNewY < child.anchoredPosition.y)
                        {
                            // new location is below current location
                            diff = child.anchoredPosition.y - myNewY;
                            child.anchoredPosition += Vector2.down * diff;
                        }
                        else
                        {
                            // new location is above current location
                            diff = myNewY - child.anchoredPosition.y;
                            child.anchoredPosition += Vector2.up * diff;
                        }
                    }
                    break;
            }
        }
        private void PlaceChildren()
        {
            float left = -minWidth / 2f;
            left += padding.Left;
            var rectTransform = GetComponent<RectTransform>();
            foreach (Transform child in rectTransform)
            {
                var childTransform = child as RectTransform;
                PlaceChildHorizontally(left, childTransform);
                PlaceChildVertically(rectTransform, childTransform);
                print("child placed at " + childTransform.anchoredPosition);
                // move top position down for next child
                left += childTransform.rect.width;
                left += spacing;
            }
        }
        protected override void OnEnable()
        {
            print("+++++++++++++++++++script was enabled");
            // if enabled and parent is enabled, tell parent to update
        }
        protected override void OnDisable()
        {
            print("----------------------script was disabled");
            // if disabled and parent is enabled, tell parent to update
        }
        public void TaskOnClick()
        {
            Debug.Log("You have clicked the button!");
            print(gameObject.activeSelf);
            gameObject.SetActive(!gameObject.activeSelf);
            print(gameObject.activeSelf);
        }
        public void UpdateRect()
        {
            print("UpdateRect");
            IterateThroughChildren();
            Resize(new Vector2(minWidth, maxHeight));
            PlaceChildren();
            // Resize(new Vector2(500, 500));
            /*
            RectTransform rect = (RectTransform)transform;
            Vector2 parentSize = GetParentSize();

            rect.SetInsetAndSizeFromParentEdge(IndentEdgeToRectEdge(m_Edge), parentSize.y + border, parentSize.x - parentSize.y);
            */
        }
        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
