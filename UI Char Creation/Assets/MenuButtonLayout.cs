using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


[RequireComponent(typeof(RectTransform))]
public class MenuButtonLayout : UIBehaviour, ILayoutSelfController
{
    public enum Edge
    {
        Left,
        Right
    }
    [SerializeField] Edge m_Edge = Edge.Left;
    [SerializeField] float border;
    public virtual void SetLayoutHorizontal()
    {
        UpdateRect();
    }
    public virtual void SetLayoutVertical() { }
#if UNITY_EDITOR
    protected override void OnValidate()
    {
        print("OnValidate");
        UpdateRect();
    }
#endif
    protected override void OnRectTransformDimensionsChange()
    {
        print("OnRectTransformDimensionsChange");
        UpdateRect();
    }
    Vector2 GetParentSize()
    {
        RectTransform parent = transform.parent as RectTransform;

        return parent == null ? Vector2.zero : parent.rect.size;
    }
    RectTransform.Edge IndentEdgeToRectEdge(Edge edge)
    {
        return edge == Edge.Left ? RectTransform.Edge.Left : RectTransform.Edge.Right;
    }
    void UpdateRect()
    {
        print("UpdateRect2");
        var rectTransform = GetComponent<RectTransform>();
        RectTransform parentTransform = rectTransform.parent as RectTransform;
        Vector2 parentPivot = parentTransform.pivot;
        Vector2 myPivot = rectTransform.pivot;
        // try to move to top of parent
        // top is parent height / 2
        float pHeight = parentTransform.rect.height / 2;
        // move to top should be offset by 1/2 element height (assuming element pivot is its middle)
        float myHeight = rectTransform.rect.height / 2;
        float myNewY = pHeight - myHeight;
        if (myNewY != rectTransform.anchoredPosition.y)
        {
            print(myNewY + "!=" + rectTransform.anchoredPosition.y);
            rectTransform.anchoredPosition += Vector2.up * (pHeight - myHeight - rectTransform.anchoredPosition.y);
            print("child anchor position after move");
            print(rectTransform.anchoredPosition);
        }
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
