using RPGBase.Constants;
using RPGBase.Flyweights;
using RPGBase.Pooled;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class StatWatcher : Watcher
    {
        /// <summary>
        /// the game object containing the gender icon.
        /// </summary>
        public GameObject IconGender { get; set; }
        /// <summary>
        /// the Strength label.
        /// </summary>
        public Text LblStr { get; set; }
        /// <summary>
        /// the Dexterity label.
        /// </summary>
        public Text LblDex { get; set; }
        /// <summary>
        /// the Constitution label.
        /// </summary>
        public Text LblCon { get; set; }
        /// <summary>
        /// the Intelligence label.
        /// </summary>
        public Text LblInt { get; set; }
        /// <summary>
        /// the Wisdom label.
        /// </summary>
        public Text LblWis { get; set; }
        /// <summary>
        /// the Charisma label.
        /// </summary>
        public Text LblCha { get; set; }
        /// <summary>
        /// the Armour Class label.
        /// </summary>
        public Text LblAc { get; set; }
        /// <summary>
        /// the Hit Points label.
        /// </summary>
        public Text LblHp { get; set; }
        /// <summary>
        /// the Luck label.
        /// </summary>
        public Text LblLuk { get; set; }
        /// <summary>
        /// the Corruption label.
        /// </summary>
        public Text LblCrp { get; set; }
        /// <summary>
        /// the Sanity label.
        /// </summary>
        public Text LblSan { get; set; }
        /// <summary>
        /// the To Hit label.
        /// </summary>
        public Text LblHit { get; set; }
        /// <summary>
        /// the Damage Bonus label.
        /// </summary>
        public Text LblDmg { get; set; }
        /// <summary>
        /// the Missile Bonus label.
        /// </summary>
        public Text LblMss { get; set; }
        /// <summary>
        /// the AC Modifier label.
        /// </summary>
        public Text LblAcMod { get; set; }
        /// <summary>
        /// the Hit Point Bonus label.
        /// </summary>
        public Text LblHPB { get; set; }
        /// <summary>
        /// the Understand Languages label.
        /// </summary>
        public Text LblLang { get; set; }
        /// <summary>
        /// the Charm label.
        /// </summary>
        public Text LblCharm { get; set; }
        /// <summary>
        /// the Max Hirelings label.
        /// </summary>
        public Text LblHire { get; set; }
        /// <summary>
        /// the Name label.
        /// </summary>
        public Text LblName { get; set; }
        /// <summary>
        /// the Level label.
        /// </summary>
        public Text LblLevel { get; set; }
        /// <summary>
        /// the Class label.
        /// </summary>
        public Text LblClass { get; set; }
        /// <summary>
        /// the sprite icon for female characters.
        /// </summary>
        public Sprite SpriteFemale { get; set; }
        /// <summary>
        /// the sprite icon for male characters.
        /// </summary>
        public Sprite SpriteMale { get; set; }
        public override void WatchUpdated(Watchable data)
        {
            /*
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
            SetLabel(LblCrp, (int)hero.GetFullAttributeScore("CRP"));
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
            if (hero.Gender == Gender.GENDER_FEMALE)
            {
                Image image = IconGender.GetComponent<Image>();
                image.sprite = SpriteFemale;
                image.rectTransform.sizeDelta = new Vector2(10, 16);
            }
            else if (hero.Gender == Gender.GENDER_MALE)
            {
                Image image = IconGender.GetComponent<Image>();
                image.sprite = SpriteMale;
                image.rectTransform.sizeDelta = new Vector2(16, 16);
            }
            LblName.text = "Nameless  ";
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
            */
        }
        /// <summary>
        /// Sets the label's text.
        /// </summary>
        /// <param name="text">the <see cref="Text"/> instance</param>
        /// <param name="val">the value being set</param>
        /// <param name="needsPlus">if true, the value needs a '+' before it</param>
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
        /// <summary>
        /// Sets a label's text as a percent value.
        /// </summary>
        /// <param name="text">the <see cref="Text"/> instance</param>
        /// <param name="val">the value being set</param>
        /// <param name="needsPlus">if true, the value needs a '+' before it</param>
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
}
