using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{
    [SerializeField]
    private GameObject decoration;
    /// <summary>
    /// If true, the menu button shows the current state of the view, and the decoration needs to show at all times; otherwise the decoration shows only when hovered.
    /// </summary>
    private bool currentState = false;
    public bool CurrentState
    {
        get { return currentState; }
        set
        {
            currentState = value;
            if (currentState)
            {
                if (!decoration.activeSelf)
                {
                    decoration.SetActive(true);
                }
                Text text = gameObject.GetComponentInChildren<Text>();
                if (text.fontStyle != FontStyle.Bold)
                {
                    text.fontStyle = FontStyle.Bold;
                    text.color = Color.white;
                }
            }
            else
            {
                if (decoration.activeSelf)
                {
                    decoration.SetActive(false);
                }
                Text text = gameObject.GetComponentInChildren<Text>();
                if (text.fontStyle == FontStyle.Bold)
                {
                    text.fontStyle = FontStyle.Normal;
                }
            }
        }
    }
    void Awake()
    {
        decoration.SetActive(false);
        EventTrigger.Entry eventtype = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerEnter
        };
        eventtype.callback.AddListener((eventData) => { OnMouseEnter(); });

        gameObject.AddComponent<EventTrigger>();
        gameObject.GetComponent<EventTrigger>().triggers.Add(eventtype);

        eventtype = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerExit
        };
        eventtype.callback.AddListener((eventData) => { OnMouseExit(); });
        gameObject.GetComponent<EventTrigger>().triggers.Add(eventtype);
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnMouseEnter()
    {
        if (CurrentState)
        {
        }
        else
        {
            Image decoImage = decoration.GetComponent<Image>();
            if (gameObject.GetComponent<Button>().interactable == true)
            {
                decoration.SetActive(true);
            }
            else
            {
                decoration.SetActive(false);
            }
        }
    }
    void OnMouseExit()
    {
        if (!CurrentState)
        {
            decoration.SetActive(false);
        }
    }
}
