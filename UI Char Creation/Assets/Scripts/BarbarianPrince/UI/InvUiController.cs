using Assets.Scripts.BarbarianPrince.Flyweights;
using Assets.Scripts.BarbarianPrince.Singletons;
using RPGBase.Constants;
using RPGBase.Flyweights;
using RPGBase.Pooled;
using RPGBase.Singletons;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.BarbarianPrince.UI
{
    public class InvUiController : MonoBehaviour
    {
        private void Awake()
        {
            print("awake");
            UnitySystemConsoleRedirector.Redirect();
            new BPProject();
            new BPInteractive();
            new BPScript();
            LoadResources();
            BPInteractiveObject io = null;
            StartCoroutine(BPServiceClient.Instance.GetItemByName("Bonebiter", value => io = value));
            print("after co");
        }
        private void LoadResources()
        {
            print("LoadResources");
            TextAsset textAsset = (TextAsset)Resources.Load("config");
            XmlDocument xmldoc = new XmlDocument();
            print(textAsset);
            xmldoc.LoadXml(textAsset.text);
            XmlNode root = xmldoc.SelectSingleNode("endpoints");
            BPServiceClient.Instance.Endpoint = root.SelectSingleNode("bp_endpoint").InnerText;
            print(BPServiceClient.Instance.Endpoint);
        }
        private void ItemLoaded()
        {
            print("ItemLoaded");
        }
        private void DisableButton(Button button)
        {
            button.interactable = false;
            button.GetComponentInChildren<Text>().color = Color.gray;
            button.GetComponent<MenuButton>().CurrentState = false;
        }
        private void EnableButton(Button button)
        {
            button.interactable = true;
            button.GetComponentInChildren<Text>().color = Color.white;
        }
        // Use this for initialization
        void Start() { }
        // Update is called once per frame
        void Update() {
            // check script timers
            Script.Instance.TimerCheck();
            // if no menus means game is being played
            Script.Instance.EventStackExecute();
        }
    }
}
