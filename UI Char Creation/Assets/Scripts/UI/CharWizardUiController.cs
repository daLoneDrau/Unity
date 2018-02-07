using Assets.Scripts.Crypts.Constants;
using Assets.Scripts.Crypts.Flyweights;
using Assets.Scripts.Crypts.Singletons;
using RPGBase.Constants;
using RPGBase.Flyweights;
using RPGBase.Pooled;
using RPGBase.Singletons;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class CharWizardUiController : MonoBehaviour
    {
        /// <summary>
        /// When the wizard first starts, only display the menu panel.
        /// </summary>
        private const int STATE_AWAKE = 0;
        /// <summary>
        /// wizard shows gender options.
        /// </summary>
        private const int STATE_CHAR_CREATION_GENDER = 1;
        /// <summary>
        /// wizard show stats panel.
        /// </summary>
        private const int STATE_CHAR_CREATION_ROLL = 2;
        /// <summary>
        /// the wizard show class menu state.
        /// </summary>
        private const int STATE_CHAR_CREATION_CLASS = 3;
        /// <summary>
        /// the wizard show class menu state.
        /// </summary>
        private const int STATE_CHAR_CREATION_LIFE_EVENTS = 4;
        private int state;
        private int State { get { return state; } set { state = value; ChangeState(); } }
        /// <summary>
        /// the menu displaying class options when creating characters.
        /// </summary>
        [SerializeField]
        private GameObject pnlClassChooser;
        /// <summary>
        /// the menu displaying basic options when creating characters.
        /// </summary>
        [SerializeField]
        private GameObject pnlCreateCharacter;
        /// <summary>
        /// the menu displaying options for generating life events. 
        /// </summary>
        [SerializeField]
        private GameObject pnlLifeEventsMenu;
        /// <summary>
        /// the content for generating life events. 
        /// </summary>
        [SerializeField]
        private GameObject pnlLifeEventsContent;
        /// <summary>
        /// the menu displaying options for choosing the character's gender. 
        /// </summary>
        [SerializeField]
        private GameObject pnlStepGender;
        /// <summary>
        /// the panel displaying the character's stats. 
        /// </summary>
        [SerializeField]
        private GameObject pnlStats;
        /// <summary>
        /// the list of buttons tied to the UI.
        /// </summary>
        [SerializeField]
        private Button[] buttons;
        /// <summary>
        /// the list of stat fields.
        /// </summary>
        [SerializeField]
        private Text[] fields;
        /// <summary>
        /// the <see cref="Text"/> object for the Life Events content header.
        /// </summary>
        [SerializeField]
        private Text txtLeHeader;
        /// <summary>
        /// the <see cref="Text"/> object for the Life Events content description.
        /// </summary>
        [SerializeField]
        private Text txtLeDescription;
        /// <summary>
        /// The sprites used for the gender icons.
        /// </summary>
        [SerializeField]
        private Sprite[] genderSprites;
        /// <summary>
        /// the icon displaying the character's gender. 
        /// </summary>
        [SerializeField]
        private GameObject genderIcon;
        private CryptInteractiveObject heroIo;
        private StatWatcher statWatcher;
        /// <summary>
        /// constant for the gender button.
        /// </summary>
        private const int BTN_GENDER = 0;
        /// <summary>
        /// constant for the roll stats button.
        /// </summary>
        private const int BTN_ROLL_STATS = 1;
        /// <summary>
        /// constant for the choose class button.
        /// </summary>
        private const int BTN_CHOOSE_CLASS = 2;
        /// <summary>
        /// constant for the life events button.
        /// </summary>
        private const int BTN_LIFE_EVENTS = 3;
        /// <summary>
        /// constant for the cancel button.
        /// </summary>
        private const int BTN_CANCEL = 4;
        /// <summary>
        /// constant for the Barbarian button.
        /// </summary>
        private const int BTN_BARBARIAN = 5;
        /// <summary>
        /// constant for the Fighter button.
        /// </summary>
        private const int BTN_FIGHTER = 6;
        /// <summary>
        /// constant for the Sorcerer button.
        /// </summary>
        private const int BTN_SORCERER = 7;
        /// <summary>
        /// constant for the Thief button.
        /// </summary>
        private const int BTN_THIEF = 8;
        /// <summary>
        /// constant for the Homeland button.
        /// </summary>
        private const int BTN_HOMELAND = 9;
        /// <summary>
        /// constant for the Background button.
        /// </summary>
        private const int BTN_BACKGROUND = 10;
        /// <summary>
        /// constant for the Training button.
        /// </summary>
        private const int BTN_TRAINING = 11;
        private Homeland homeland;
        private void Awake()
        {
            UnitySystemConsoleRedirector.Redirect();
            statWatcher = new StatWatcher
            {
                LblStr = fields[0],
                LblDex = fields[1],
                LblCon = fields[2],
                LblInt = fields[3],
                LblWis = fields[4],
                LblCha = fields[5],
                LblAc = fields[6],
                LblHp = fields[7],
                LblLuk = fields[8],
                LblCrp = fields[9],
                LblSan = fields[10],
                LblHit = fields[11],
                LblDmg = fields[12],
                LblMss = fields[13],
                LblAcMod = fields[14],
                LblHPB = fields[15],
                LblLang = fields[16],
                LblCharm = fields[17],
                LblHire = fields[18],
                LblName = fields[19],
                LblLevel = fields[20],
                LblClass = fields[21],
                IconGender = genderIcon,
                SpriteFemale = genderSprites[1],
                SpriteMale = genderSprites[0]
            };
            new CryptInteractive();
            new CryptProject();
            heroIo = ((CryptInteractive)CryptInteractive.Instance).NewHero();
            heroIo.PcData.AddWatcher(statWatcher);
            //State = STATE_AWAKE;
            print("call web service");
            WebServiceClient client = gameObject.GetComponent<WebServiceClient>();
            StartCoroutine(client.LoadHomelands(SetStateAwake));
            print("after co");
        }
        private void SetStateAwake()
        {
            State = STATE_AWAKE;
        }
        private void OnAwake()
        {
            HideAllPanels();
            DisableAllCharacterMenuButtons();
            EnableButton(buttons[BTN_GENDER].GetComponent<Button>());
            // show main menu
            pnlCreateCharacter.SetActive(true);
        }
        private void OnChooseClass()
        {
            HideAllPanels();
            SetCharacterMenuButtons();
            SetStateButton(buttons[BTN_CHOOSE_CLASS]);
            // show main menu
            pnlCreateCharacter.SetActive(true);
            // show class chooser panel
            pnlClassChooser.SetActive(true);
        }
        private void OnChooseLifeEvents()
        {
            print("++++++++++++++++++++++++OnChooseLifeEvents");
            HideAllPanels();
            SetLifeEventMenuButtons();
            if (((CryptCharacter)heroIo.PcData).Homeland == null)
            {
                RollHomeland();
            }
            else if (((CryptCharacter)heroIo.PcData).Homeland != null
                && ((CryptCharacter)heroIo.PcData).Background == null)
            {

            }
            else
            {
                txtLeHeader.text = ((CryptCharacter)heroIo.PcData).Homeland.Name;
                txtLeDescription.text = GetHomelandDescription(((CryptCharacter)heroIo.PcData).Homeland);
            }
            // show life events menu
            pnlLifeEventsMenu.SetActive(true);
            // show life events panel
            pnlLifeEventsContent.SetActive(true);
        }
        private void OnGender()
        {
            HideAllPanels();
            DisableAllCharacterMenuButtons();
            DisableAllClassMenuButtons();
            SetStateButton(buttons[BTN_GENDER]);
            // show main menu
            pnlCreateCharacter.SetActive(true);
            // show gender panel
            pnlStepGender.SetActive(true);
        }
        private void SetStateButton(Button gameObject)
        {
            gameObject.GetComponent<MenuButton>().CurrentState = true;
        }
        private void OnRoll()
        {
            HideAllPanels();
            SetCharacterMenuButtons();
            SetStateButton(buttons[BTN_ROLL_STATS]);
            if (heroIo.PcData.GetBaseAttributeScore("STR") <= 0)
            {
                ReRoll();
            }
            // show main menu
            pnlCreateCharacter.SetActive(true);
            // show stats panel
            pnlStats.SetActive(true);
        }
        /// <summary>
        /// Disables all character menu buttons.
        /// </summary>
        private void DisableAllCharacterMenuButtons()
        {
            for (int i = BTN_CANCEL; i >= 0; i--)
            {
                DisableButton(buttons[i]);
                MenuButton menu = buttons[i].GetComponent<MenuButton>();
                menu.CurrentState = false;
            }
        }
        /// <summary>
        /// Disables all life event menu buttons.
        /// </summary>
        private void DisableAllLifeEventMenuButtons()
        {
            for (int i = BTN_TRAINING; i >= BTN_HOMELAND; i--)
            {
                DisableButton(buttons[i].GetComponent<Button>());
            }
        }
        /// <summary>
        /// Disables all class menu buttons.
        /// </summary>
        private void DisableAllClassMenuButtons()
        {
            for (int i = 8; i >= 5; i--)
            {
                DisableButton(buttons[i].GetComponent<Button>());
            }
        }
        private string GetHomelandDescription(Homeland h)
        {
            PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
            sb.Append(h.Description);
            sb.Append('\n');
            sb.Append('\n');
            /*
            switch (homeland.Modifier[0])
            {
                case CryptEquipGlobals.EQUIP_ELEMENT_STRENGTH:
                    sb.Append("+ ");
                    sb.Append(h.Modifier[1]);
                    sb.Append(" Strength");
                    break;
                case CryptEquipGlobals.EQUIP_ELEMENT_DEXTERITY:
                    sb.Append("+ ");
                    sb.Append(h.Modifier[1]);
                    sb.Append(" Dexterity");
                    break;
                case CryptEquipGlobals.EQUIP_ELEMENT_CONSTITUTION:
                    sb.Append("+ ");
                    sb.Append(h.Modifier[1]);
                    sb.Append(" Constitution");
                    break;
                case CryptEquipGlobals.EQUIP_ELEMENT_INTELLIGENCE:
                    sb.Append("+ ");
                    sb.Append(h.Modifier[1]);
                    sb.Append(" Intelligence");
                    break;
                case CryptEquipGlobals.EQUIP_ELEMENT_WISDOM:
                    sb.Append("+ ");
                    sb.Append(h.Modifier[1]);
                    sb.Append(" Wisdom");
                    break;
                case CryptEquipGlobals.EQUIP_ELEMENT_CHARISMA:
                    sb.Append("+ ");
                    sb.Append(h.Modifier[1]);
                    sb.Append(" Charisma");
                    break;
            }
            */
            string s = sb.ToString();
            sb.ReturnToPool();
            return s;
        }
        /// <summary>
        /// Sets the character menu buttons based on the character's state.
        /// </summary>
        private void SetCharacterMenuButtons()
        {
            DisableAllCharacterMenuButtons();
            EnableButton(buttons[BTN_GENDER].GetComponent<Button>());
            EnableButton(buttons[BTN_ROLL_STATS].GetComponent<Button>());
            if (heroIo.PcData.GetBaseAttributeScore("STR") > 0)
            {
                EnableButton(buttons[BTN_CHOOSE_CLASS].GetComponent<Button>());
            }
            if (((CryptCharacter)heroIo.PcData).Profession > 0)
            {
                EnableButton(buttons[BTN_LIFE_EVENTS].GetComponent<Button>());
            }
            EnableButton(buttons[BTN_CANCEL].GetComponent<Button>());
        }
        /// <summary>
        /// Sets the character menu buttons based on the character's state.
        /// </summary>
        private void SetLifeEventMenuButtons()
        {
            DisableAllLifeEventMenuButtons();
            EnableButton(buttons[BTN_HOMELAND].GetComponent<Button>());
            if (((CryptCharacter)heroIo.PcData).Homeland != null)
            {
                EnableButton(buttons[BTN_BACKGROUND].GetComponent<Button>());
            }
        }
        private void ChangeState()
        {
            switch (State)
            {
                case STATE_AWAKE:
                    OnAwake();
                    break;
                case STATE_CHAR_CREATION_GENDER:
                    OnGender();
                    break;
                case STATE_CHAR_CREATION_ROLL:
                    OnRoll();
                    break;
                case STATE_CHAR_CREATION_CLASS:
                    OnChooseClass();
                    break;
                case STATE_CHAR_CREATION_LIFE_EVENTS:
                    OnChooseLifeEvents();
                    break;
            }
        }
        public void ChooseAbilityScores()
        {
            State = STATE_CHAR_CREATION_ROLL;
        }
        public void ChooseClass()
        {
            State = STATE_CHAR_CREATION_CLASS;
        }
        public void ChooseGender()
        {
            State = STATE_CHAR_CREATION_GENDER;
        }
        public void ChooseLifeEvents()
        {
            State = STATE_CHAR_CREATION_LIFE_EVENTS;
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
        private void HideAllPanels()
        {
            pnlCreateCharacter.SetActive(false);
            pnlLifeEventsMenu.SetActive(false);
            pnlStepGender.SetActive(false);
            pnlStats.SetActive(false);
            pnlClassChooser.SetActive(false);
            pnlLifeEventsContent.SetActive(false);
            pnlLifeEventsMenu.SetActive(false);
        }
        public void ReRoll()
        {
            ((CryptCharacter)heroIo.PcData).NewHeroStepOne();
            DisableAllClassMenuButtons();
            int classes = ((CryptCharacter)heroIo.PcData).GetQualifyingClasses();
            if ((classes & CryptEquipGlobals.CLASS_BARBARIAN) == CryptEquipGlobals.CLASS_BARBARIAN)
            {
                EnableButton(buttons[BTN_BARBARIAN].GetComponent<Button>());
            }
            if ((classes & CryptEquipGlobals.CLASS_FIGHTER) == CryptEquipGlobals.CLASS_FIGHTER)
            {
                EnableButton(buttons[BTN_FIGHTER].GetComponent<Button>());
            }
            if ((classes & CryptEquipGlobals.CLASS_SORCERER) == CryptEquipGlobals.CLASS_SORCERER)
            {
                EnableButton(buttons[BTN_SORCERER].GetComponent<Button>());
            }
            if ((classes & CryptEquipGlobals.CLASS_THIEF) == CryptEquipGlobals.CLASS_THIEF)
            {
                EnableButton(buttons[BTN_THIEF].GetComponent<Button>());
            }
            SetCharacterMenuButtons();
            SetStateButton(buttons[BTN_ROLL_STATS]);
        }
        public void SetClass(int val)
        {
            ((CryptCharacter)heroIo.PcData).Profession = val;
            int hp = 0;
            bool valid = false;
            do
            {
                switch (val)
                {
                    case CryptEquipGlobals.CLASS_BARBARIAN:
                    case CryptEquipGlobals.CLASS_THIEF:
                        hp = Diceroller.Instance.RolldXPlusY(6, 1);
                        break;
                    case CryptEquipGlobals.CLASS_FIGHTER:
                        hp = Diceroller.Instance.RolldXPlusY(6, 2);
                        break;
                    case CryptEquipGlobals.CLASS_SORCERER:
                        hp = Diceroller.Instance.RolldX(6);
                        break;
                }
                heroIo.PcData.SetBaseAttributeScore("MHP", hp);
                heroIo.PcData.ComputeFullStats();
                // make sure hero's HP > 0
                if (hp + heroIo.PcData.GetFullAttributeScore("HPB") > 0)
                {
                    valid = true;
                }
            } while (!valid);
            heroIo.PcData.HealPlayer(9999, true);
            State = STATE_CHAR_CREATION_ROLL;
        }
        public void SetGender(int gender)
        {
            heroIo.PcData.Gender = gender;
            State = STATE_CHAR_CREATION_ROLL;
        }
        public void SetHomeland(int gender)
        {
            ((CryptCharacter)heroIo.PcData).Homeland = homeland;
            State = STATE_CHAR_CREATION_LIFE_EVENTS;
        }
        public void RollHomeland()
        {
            print("---------------------RollHomeland");
            homeland = (Homeland)Diceroller.Instance.GetRandomObject(Homeland.Values());
            txtLeHeader.text = homeland.Name;
            txtLeDescription.text = GetHomelandDescription(homeland);
        }
        // Use this for initialization
        void Start() { }
        // Update is called once per frame
        void Update() { }
    }
}
