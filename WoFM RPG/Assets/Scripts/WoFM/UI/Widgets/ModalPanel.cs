using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using WoFM.UI.GlobalControllers;

namespace WoFM.UI.Widgets
{
    public class EventButtonDetails
    {
        public string buttonTitle;
        public Sprite buttonBackground;  // Not implemented
        public UnityAction action;
    }
    public class ModalPanelDetails
    {
        public string title; // Not implemented
        public string content;
        public Sprite iconImage;
        public Sprite panelBackgroundImage; // Not implemented
        public EventButtonDetails button1Details;
        public EventButtonDetails button2Details;
        public EventButtonDetails button3Details;
    }
    public class ModalPanel : MonoBehaviour
    {
        public static ModalPanel Instance { get; private set; }
        /// <summary>
        /// the <see cref="Text"/> instance displaying the message.
        /// </summary>
        public Text content;
        /// <summary>
        /// the <see cref="Image"/> instance displaying the message icon.
        /// </summary>
        public Image icon;
        /// <summary>
        /// the <see cref="Button"/> instance handling 'yes' responses.
        /// </summary>
        public Button button1;
        /// <summary>
        /// the <see cref="Button"/> instance handling 'no' responses.
        /// </summary>
        public Button button2;
        /// <summary>
        /// the <see cref="Button"/> instance handling 'cancel' responses.
        /// </summary>
        public Button button3;
        /// <summary>
        /// the <see cref="Button"/> instance handling 'cancel' responses.
        /// </summary>
        public Button button4;
        public Text button1Text;
        public Text button2Text;
        public Text button3Text;
        #region MonoBehavior
        public void Awake()
        {
            Instance = this;
            gameObject.SetActive(false);
        }
        public void OnEnable()
        {
            // transform.SetAsLastSibling();
            GameSceneController.Instance.CONTROLS_FROZEN = true;
        }
        #endregion
        public void NewChoice(ModalPanelDetails details)
        {
            gameObject.SetActive(true);
            print(this.icon);
            print(this.button1);
            this.icon.gameObject.SetActive(false);
            button1.gameObject.SetActive(false);
            button2.gameObject.SetActive(false);
            button3.gameObject.SetActive(false);

            content.text = details.content;

            if (details.iconImage)
            {
                icon.sprite = details.iconImage;
                icon.gameObject.SetActive(true);
            }

            button1.onClick.RemoveAllListeners();
            if (details.button1Details != null)
            {
                if (details.button1Details.action != null)
                {
                    button1.onClick.AddListener(details.button1Details.action);
                }
                button1Text.text = details.button1Details.buttonTitle;
            }
            button1.onClick.AddListener(ClosePanel);
            button1.gameObject.SetActive(true);

            if (details.button2Details != null)
            {
                button2.onClick.RemoveAllListeners();
                if (details.button2Details.action != null)
                {
                    button2.onClick.AddListener(details.button2Details.action);
                }
                button2.onClick.AddListener(ClosePanel);
                button2Text.text = details.button2Details.buttonTitle;
                button2.gameObject.SetActive(true);
            }

            if (details.button3Details != null)
            {
                button3.onClick.RemoveAllListeners();
                if (details.button3Details.action != null)
                {
                    button3.onClick.AddListener(details.button3Details.action);
                }
                button3.onClick.AddListener(ClosePanel);
                button3Text.text = details.button3Details.buttonTitle;
                button3.gameObject.SetActive(true);
            }
        }
        private void ClosePanel()
        {
            gameObject.SetActive(false);
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            GameSceneController.Instance.CONTROLS_FROZEN = false;
        }
    }
}
