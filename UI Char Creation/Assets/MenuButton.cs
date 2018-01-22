using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{
    [SerializeField]
    private GameObject decoration;
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
        print("OnMouseEnter");
        if (gameObject.GetComponent<Button>().interactable == true)
        {
            print("active");
            decoration.SetActive(true);
        }
        else
        {
            print("intive");
            decoration.SetActive(false);
        }
    }

    void OnMouseExit()
    {
        print("OnMouseExit");
        decoration.SetActive(false);
    }
}
