using Assets.Scripts.Crypts.Constants;
using Assets.Scripts.Crypts.Flyweights;
using Assets.Scripts.Crypts.Singletons;
using RPGBase.Constants;
using RPGBase.Flyweights;
using RPGBase.Pooled;
using RPGBase.Singletons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharWizardUiController : MonoBehaviour
{
    /// <summary>
    /// When the wizard first starts, only display the menu panel.
    /// </summary>
    private const int STATE_AWAKE = 0;
    private const int STATE_CHAR_CREATION_GENDER = 1;
    private const int STATE_CHAR_CREATION_ROLL = 2;
    private const int STATE_CHAR_CREATION_CLASS = 3;
    private int state;
    private int State { get { return state; } set { state = value; ChangeState(); } }
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
    /*
    [SerializeField]
    private GameObject pnlMainCharMenu;
    [SerializeField]
    private GameObject pnlMenu;
    [SerializeField]
    private GameObject pnlStepRoll;
    [SerializeField]
    private GameObject pnlStepTwo;
    [SerializeField]
    private GameObject pnlStepThree;
    [SerializeField]
    private GameObject pnlGender;
    [SerializeField]
    private Text[] fields;
    [SerializeField]
    private Sprite[] genderSprites;
    */
    private CryptInteractiveObject heroIo;
    private ButtonWatcher buttonWatcher;
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
    private void OnAwake()
    {
        HideAllPanels();
        DisableAllCharacterMenuButtons();
        EnableButton(buttons[BTN_GENDER]);
        // show main menu
        pnlCreateCharacter.SetActive(true);
    }
    private void OnGender()
    {
        HideAllPanels();
        DisableAllCharacterMenuButtons();
        // show main menu
        pnlCreateCharacter.SetActive(true);
        // show gender panel
        pnlStepGender.SetActive(true);
    }
    /// <summary>
    /// Disables all character menu buttons.
    /// </summary>
    private void DisableAllCharacterMenuButtons()
    {
        for (int i = 4; i >= 0; i--)
        {
            DisableButton(buttons[i]);
        }
    }
    /// <summary>
    /// Sets the character menu buttons based on the character's state.
    /// </summary>
    private void SetCharacterMenuButtons()
    {
        DisableAllCharacterMenuButtons();
        EnableButton(buttons[BTN_GENDER]);
        EnableButton(buttons[BTN_ROLL_STATS]);
        EnableButton(buttons[BTN_CHOOSE_CLASS]);
        if (((CryptCharacter)heroIo.PcData).Profession > 0)
        {
            EnableButton(buttons[BTN_LIFE_EVENTS]);
        }
        EnableButton(buttons[BTN_CANCEL]);
    }
    private void OnRoll()
    {
        HideAllPanels();
        SetCharacterMenuButtons();
        // show main menu
        pnlCreateCharacter.SetActive(true);
        // show stats panel
        pnlStats.SetActive(true);
    }
    private void ChangeState()
    {
        print("ChangeState");
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
                /*
                print(STATE_CHAR_CREATION_ROLL);
                //show character creation panel
                pnlCreateCharacter.SetActive(true);
                // show main character menu
                pnlMainCharMenu.SetActive(true);
                // show roll stats content
                pnlStepRoll.SetActive(true);
                */
                break;
        }
    }
    public void ChooseGender(int gender)
    {
        heroIo.PcData.Gender = gender;
        State = STATE_CHAR_CREATION_ROLL;
    }
    public void ChooseClass()
    {
        State = STATE_CHAR_CREATION_CLASS;
    }
    private void DisableAllButtons()
    {
        DisableButton(buttonWatcher.BtnBarbarian);
        DisableButton(buttonWatcher.BtnFighter);
        DisableButton(buttonWatcher.BtnSorcerer);
        DisableButton(buttonWatcher.BtnThief);

        DisableButton(buttonWatcher.BtnRoll);
        DisableButton(buttonWatcher.BtnLifeEvents);
    }
    private void DisableAllCharMenuButtons()
    {
        DisableButton(buttonWatcher.BtnGender);
        DisableButton(buttonWatcher.BtnRoll);
        DisableButton(buttonWatcher.BtnClass);
        DisableButton(buttonWatcher.BtnLifeEvents);
        DisableButton(buttonWatcher.BtnCharMenuCancel);
    }
    private void DisableButton(Button button)
    {
        button.interactable = false;
        button.GetComponentInChildren<Text>().color = Color.gray;
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
        /*
        pnlMainCharMenu.SetActive(false);
        pnlMenu.SetActive(false);
        pnlStepRoll.SetActive(false);
        pnlStepTwo.SetActive(false);
        pnlStepThree.SetActive(false);
        */
    }
    public void SetClass(int val)
    {
        ((CryptCharacter)heroIo.PcData).Profession = val;
        int hp = 0;
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
        heroIo.PcData.HealPlayer(9999, true);
        /*
        pnlStepRoll.SetActive(true);
        pnlStepTwo.SetActive(false);
        */
    }
    public void ShowGenderPanel()
    {
        State = STATE_CHAR_CREATION_GENDER;
    }
    public void StartCharacterCreation()
    {
        heroIo = ((CryptInteractive)CryptInteractive.Instance).NewHero();
        ReRoll();
        heroIo.PcData.AddWatcher(statWatcher);
        State = STATE_CHAR_CREATION_GENDER;
    }
    private void Awake()
    {
        print("CharWizardUiController.Awake");
        buttonWatcher = new ButtonWatcher
        {
            /*
            BtnBarbarian = buttons[0],
            BtnFighter = buttons[1],
            BtnSorcerer = buttons[2],
            BtnThief = buttons[3],
            BtnGender = buttons[4],
            BtnRoll = buttons[5],
            BtnClass = buttons[6],
            BtnLifeEvents = buttons[7],
            BtnCharMenuCancel = buttons[8]
            */
        };
        statWatcher = new StatWatcher
        {
            /*
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
            PnlGender = pnlGender,
            Female = genderSprites[0],
            Male = genderSprites[1]
            */
        };
        new CryptInteractive();
        new CryptProject();
        heroIo = ((CryptInteractive)CryptInteractive.Instance).NewHero();
        State = STATE_AWAKE;
    }
    // Use this for initialization
    void Start() { }
    // Update is called once per frame
    void Update() { }
    public void ReRoll()
    {
        ((CryptCharacter)heroIo.PcData).NewHeroStepOne();
        DisableButton(buttonWatcher.BtnBarbarian);
        DisableButton(buttonWatcher.BtnFighter);
        DisableButton(buttonWatcher.BtnSorcerer);
        DisableButton(buttonWatcher.BtnThief);
        int classes = ((CryptCharacter)heroIo.PcData).GetQualifyingClasses();
        if ((classes & CryptEquipGlobals.CLASS_BARBARIAN) == CryptEquipGlobals.CLASS_BARBARIAN)
        {
            EnableButton(buttonWatcher.BtnBarbarian);
        }
        if ((classes & CryptEquipGlobals.CLASS_FIGHTER) == CryptEquipGlobals.CLASS_FIGHTER)
        {
            EnableButton(buttonWatcher.BtnFighter);
        }
        if ((classes & CryptEquipGlobals.CLASS_SORCERER) == CryptEquipGlobals.CLASS_SORCERER)
        {
            EnableButton(buttonWatcher.BtnSorcerer);
        }
        if ((classes & CryptEquipGlobals.CLASS_THIEF) == CryptEquipGlobals.CLASS_THIEF)
        {
            EnableButton(buttonWatcher.BtnThief);
        }
    }
}
class ButtonWatcher
{
    public Button BtnBarbarian { get; set; }
    public Button BtnFighter { get; set; }
    public Button BtnLifeEvents { get; set; }
    public Button BtnRoll { get; set; }
    public Button BtnSorcerer { get; set; }
    public Button BtnThief { get; set; }
    public Button BtnGender { get; set; }
    public Button BtnClass { get; set; }
    public Button BtnCharMenuCancel { get; set; }
}
class StatWatcher : Watcher
{
    public Sprite Female { get; set; }
    public Sprite Male { get; set; }
    public GameObject PnlGender { get; set; }
    public Text LblStr { get; set; }
    public Text LblDex { get; set; }
    public Text LblCon { get; set; }
    public Text LblInt { get; set; }
    public Text LblWis { get; set; }
    public Text LblCha { get; set; }
    public Text LblAc { get; set; }
    public Text LblHp { get; set; }
    public Text LblLuk { get; set; }
    public Text LblCrp { get; set; }
    public Text LblSan { get; set; }
    public Text LblHit { get; set; }
    public Text LblDmg { get; set; }
    public Text LblMss { get; set; }
    public Text LblAcMod { get; set; }
    public Text LblHPB { get; set; }
    public Text LblLang { get; set; }
    public Text LblCharm { get; set; }
    public Text LblHire { get; set; }
    public Text LblName { get; set; }
    public Text LblLevel { get; set; }
    public Text LblClass { get; set; }
    public override void WatchUpdated(Watchable data)
    {
        CryptCharacter hero = (CryptCharacter)data;
        hero.ComputeFullStats();
        SetLabel(LblStr, (int)hero.GetFullAttributeScore("STR"));
        SetLabel(LblDex, (int)hero.GetFullAttributeScore("DEX"));
        SetLabel(LblCon, (int)hero.GetFullAttributeScore("CON"));
        SetLabel(LblInt, (int)hero.GetFullAttributeScore("INT"));
        SetLabel(LblWis, (int)hero.GetFullAttributeScore("WIS"));
        SetLabel(LblCha, (int)hero.GetFullAttributeScore("CHA"));
        SetLabel(LblAc, (int)hero.GetFullAttributeScore("AC"));
        SetLabel(LblLuk, (int)hero.GetFullAttributeScore("LUK"));
        SetLabel(LblSan, (int)hero.GetFullAttributeScore("SAN"));
        SetLabel(LblHit, (int)hero.GetFullAttributeScore("HIT"), true);
        SetLabel(LblDmg, (int)hero.GetFullAttributeScore("DMG"), true);
        SetLabel(LblMss, (int)hero.GetFullAttributeScore("MSS"), true);
        SetLabel(LblAcMod, (int)hero.GetFullAttributeScore("ACM"), true);
        SetLabel(LblHPB, (int)hero.GetFullAttributeScore("HPB"), true);
        SetPercentLabel(LblLang, hero.GetFullAttributeScore("LAN"));
        SetPercentLabel(LblCharm, hero.GetFullAttributeScore("CRM"));
        SetLabel(LblHire, (int)hero.GetFullAttributeScore("HIR"));
        if (hero.Profession > 0)
        {
            PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
            sb.Append((int)hero.Life);
            sb.Append("/");
            sb.Append((int)hero.GetMaxLife());
            LblHp.text = sb.ToString();
            sb.ReturnToPool();
        }
        else
        {
            LblHp.text = "";
        }
        if (hero.Profession == 0)
        {
            SetLabel(LblLevel, 0);
            LblClass.text = "          ";
        }
        else
        {
            SetLabel(LblLevel, 1);
            if (hero.Profession == CryptEquipGlobals.CLASS_BARBARIAN)
            {
                LblClass.text = "Barbarian ";
            }
            else if (hero.Profession == CryptEquipGlobals.CLASS_FIGHTER)
            {
                LblClass.text = "Fighter   ";
            }
            else if (hero.Profession == CryptEquipGlobals.CLASS_SORCERER)
            {
                LblClass.text = "Sorcerer  ";
            }
            else if (hero.Profession == CryptEquipGlobals.CLASS_THIEF)
            {
                LblClass.text = "Thief     ";
            }
        }
        LblName.text = "Nameless  ";
        if (hero.Gender == Gender.GENDER_FEMALE)
        {
            Image image = PnlGender.GetComponent<Image>();
            image.sprite = Female;
            //panelRectTransform.sizeDelta.Set((float)xPos + 10, panelRectTransform.sizeDelta.y);
        }
        else if (hero.Gender == Gender.GENDER_MALE)
        {
            Image image = PnlGender.GetComponent<Image>();
            image.sprite = Male;
            //panelRectTransform.sizeDelta.Set((float)xPos + 10, panelRectTransform.sizeDelta.y);
        }
    }
    private void SetLabel(Text text, int val, bool needsPlus = false)
    {
        if (val < 10 && val >= 0)
        {
            PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
            if (needsPlus && val > 0)
            {
                sb.Append("+");
            }
            else if (val == 0)
            {
                sb.Append(" -");
            }
            else
            {
                sb.Append(" ");
            }
            if (val != 0)
            {
                sb.Append(val);
            }
            text.text = sb.ToString();
            sb.ReturnToPool();
        }
        else
        {
            PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
            if (needsPlus && val >= 0)
            {
                sb.Append("+");
            }
            sb.Append(val);
            text.text = sb.ToString();
            sb.ReturnToPool();
        }
    }
    private void SetPercentLabel(Text text, float val, bool needsPlus = false)
    {
        int v = (int)(val * 100f);
        PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
        if (v < 10 && v >= 0)
        {
            sb.Append(" ");
        }
        if (needsPlus && val >= 0)
        {
            sb.Append("+");
        }
        sb.Append(v);
        sb.Append("%");
        text.text = sb.ToString();
        sb.ReturnToPool();
    }
}
