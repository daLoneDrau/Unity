using Assets.Scripts.UI.SimpleJSON;
using RPGBase.Pooled;
using RPGBase.Singletons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using WoFM.UI.GlobalControllers;

namespace WoFM.UI.SceneControllers
{
    public class IntroController : MonoBehaviour
    {
        private string json;
        private TextGenerator textGen;
        private TextGenerationSettings generationSettings;
        public Button btnBegin;
        public Button btnNext;
        public List<string> words;
        public string text;
        private string textRemaining;
        public string[] linesDisplayed;
        public bool isDebug = true;
        public Text textUi;
        public Canvas canvas;
        private Vector3 scale;
        void Awake()
        {
            btnBegin.interactable = false;
        }
        // Use this for initialization
        void Start()
        {
            btnBegin.interactable = false;
            textGen = new TextGenerator();
            generationSettings = textUi.GetGenerationSettings(textUi.rectTransform.rect.size);
            print(generationSettings.fontSize);
            scale = canvas.transform.localScale;
            float width = textGen.GetPreferredWidth(" ", generationSettings);
            float height = textGen.GetPreferredHeight(" ", generationSettings);
            PrepareText();
            NextText();
            // get canvas local scale
            print("local scale "+scale);
        }
        bool doonce;
        // Update is called once per frame
        void Update()
        {
        }
        private float GetWordWidth(string word)
        {
            float size = 0;
            for (int i = word.Length - 1; i >= 0; i--)
            {
                size += (int)textGen.GetPreferredWidth(word.Substring(i, 1), generationSettings);
            }
            return size;
        }
        private void PrepareText()
        {
            text = GameController.Instance.textEntry;
            words = new List<string>();
            string[] tokenizer = System.Text.RegularExpressions.Regex.Split(text, @"([ \t\r\n\f])");
            for (int i = 0, li = tokenizer.Length; i < li; i++)
            {
                if (tokenizer[i].Length > 0)
                {
                    words.Add(tokenizer[i]);
                }
            }
        }
        public void NextText()
        {
            if (words != null
                    && words.Count > 0)
            {
                linesDisplayed = new string[0];
                if (isDebug)
                {
                    print("sizing text::" + text);
                }

                // all tabs replaced with spaces
                var rectTransform = textUi.GetComponent<RectTransform>();
                float innerWidth = rectTransform.rect.width * scale.x;
                float maxHeight = rectTransform.rect.height * scale.y;
                print(rectTransform.rect);
                // start a line of text
                PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
                // init measurements
                double currentWidth = 0d, currentHeight = 0d;
                string nextWord = "";
                float nextWordLength = 0f;
                float spaceWidth = textGen.GetPreferredWidth(" ", generationSettings), spaceHeight = textGen.GetPreferredHeight(" ", generationSettings);
                print("spaceWidth::" + spaceWidth);
                print("spaceHeight::" + spaceHeight);
                maxHeight -= spaceHeight;
                int i = 0;
                for (int li = words.Count; i < li; i++)
                {
                    if (words[i].StartsWith(" "))
                    {
                        // HANDLE SPACE
                        nextWord = words[i];
                        nextWordLength = spaceWidth;
                        float newWidth = (float)currentWidth + nextWordLength;
                        if (newWidth > innerWidth)
                        {
                            if (isDebug)
                            {
                                print("ending line because " +
                                        "space takes it over width");
                                print(newWidth + " > " + innerWidth);
                                print("current line::" + sb.ToString());
                            }
                            /*******************
                             * END CURRENT LINE
                             ******************/
                            // add line height
                            currentHeight += spaceHeight;
                            print("ending line - height is now " + currentHeight);
                            // add line to lines displayed
                            linesDisplayed = ArrayUtilities.Instance.ExtendArray(sb.ToString(), linesDisplayed);
                            // reset stringbuilder, width, and words
                            sb.Length = 0;
                            currentWidth = 0;
                            nextWord = "";
                            nextWordLength = 0;
                            // check to see if there is room to add more lines
                            if (currentHeight + spaceHeight > maxHeight)
                            {
                                // no more room
                                break;
                            }
                        }
                        else
                        {
                            sb.Append(nextWord);
                            currentWidth = newWidth;
                        }
                    }
                    else if (words[i][0] == '\t')
                    { // TAB
                        nextWord = "    ";
                        nextWordLength = spaceWidth * 4;
                        float newWidth = (float)currentWidth + nextWordLength;
                        if (newWidth > innerWidth)
                        {
                            if (isDebug)
                            {
                                print("ending line because " +
                                        "tab takes it over width");
                                print(newWidth + " > " + innerWidth);
                                print("current line::" + sb.ToString());
                            }
                            /*******************
                             * END CURRENT LINE
                             ******************/
                            // add line height
                            currentHeight += spaceHeight;
                            print("ending line - height is now " + currentHeight);
                            // add line to lines displayed
                            linesDisplayed = ArrayUtilities.Instance.ExtendArray(sb.ToString(), linesDisplayed);
                            // reset stringbuilder, width, and words
                            sb.Length = 0;
                            sb.Append(nextWord);
                            currentWidth = nextWordLength;
                            // check to see if there is room to add more lines
                            if (currentHeight + spaceHeight > maxHeight)
                            {
                                // no more room
                                break;
                            }
                        }
                        else
                        {
                            sb.Append(nextWord);
                            currentWidth = newWidth;
                        }
                    }
                    else if (words[i][0] == '\n')
                    {
                        // NEWLINE
                        if (isDebug)
                        {
                            print("ending line because at NEWLINE");
                            print("current line::" + sb.ToString());
                        }
                        /*******************
                         * END CURRENT LINE
                         ******************/
                        // add line height
                        currentHeight += spaceHeight;
                        print("ending line - height is now " + currentHeight);
                        // add line to lines displayed
                        linesDisplayed = ArrayUtilities.Instance.ExtendArray(sb.ToString(), linesDisplayed);
                        // reset stringbuilder, width, and words
                        sb.Length = 0;
                        currentWidth = 0;
                        nextWord = "";
                        nextWordLength = 0;
                        // check to see if there is room to add more lines
                        if (currentHeight + spaceHeight > maxHeight)
                        {
                            // no more room
                            break;
                        }
                    }
                    else
                    {
                        nextWord = words[i];
                        nextWordLength = GetWordWidth(nextWord);
                        float newWidth = (float)currentWidth + nextWordLength;
                        if (newWidth > innerWidth)
                        {
                            if (isDebug)
                            {
                                print("ending line because " +
                                        "next word '" + nextWord
                                        + "' takes it over width");
                                print(newWidth + " > " + innerWidth);
                                print("current line::" + sb.ToString());
                            }
                            /*******************
                             * END CURRENT LINE
                             ******************/
                            // add line height
                            currentHeight += spaceHeight;
                            print("ending line - height is now " + currentHeight);
                            // add line to lines displayed
                            linesDisplayed = ArrayUtilities.Instance.ExtendArray(sb.ToString(), linesDisplayed);
                            // reset stringbuilder, width, and words
                            sb.Length = 0;
                            sb.Append(nextWord);
                            currentWidth = nextWordLength;
                            // check to see if there is room to add more lines
                            if (currentHeight + spaceHeight > maxHeight)
                            {
                                // no more room
                                break;
                            }
                        }
                        else
                        {
                            sb.Append(words[i]);
                            currentWidth = newWidth;
                        }
                    }
                } // end for
                for (int j = 0; j < i; j++)
                {
                    words.RemoveAt(0);
                }
                if (sb.Length > 0
                    && words.Count == 0)
                { // add the last line
                    if (isDebug)
                    {
                        print("adding last line");
                        print("current line::" + sb.ToString());
                    }
                    /*******************
                     * END CURRENT LINE
                     ******************/
                    // add line height
                    currentHeight += spaceHeight;
                    // add line to lines displayed
                    linesDisplayed = ArrayUtilities.Instance.ExtendArray(sb.ToString(), linesDisplayed);
                    // reset stringbuilder, width, and words
                    sb.Length = 0;
                    currentWidth = 0;
                    nextWord = "";
                    nextWordLength = 0;
                }
            }
            if (isDebug)
            {
                print("end sizetext");
            }
            if (words.Count == 0)
            {
                btnBegin.interactable = true;
                btnNext.interactable = false;
            }
            DisplayText();
        }
        private void DisplayText()
        {
            PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
            for (int i = 0, li = linesDisplayed.Length; i < li; i++)
            {
                sb.Append(linesDisplayed[i]);
                if (i + 1 < li)
                {
                    sb.Append("\n");
                }
            }
            textUi.text = sb.ToString();
            sb.ReturnToPool();
        }
        /// <summary>
        /// Loads the next scene by its index.
        /// </summary>
        public void NextScene()
        {
            SceneManager.LoadScene(GameController.Instance.nextScene);
        }
    }
}
