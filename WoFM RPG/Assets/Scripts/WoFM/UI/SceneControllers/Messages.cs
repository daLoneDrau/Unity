using RPGBase.Pooled;
using RPGBase.Singletons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;

namespace WoFM.UI.SceneControllers
{
    public class Messages : Singleton<Messages>
    {
        /// <summary>
        /// the text field where messages are displayed.
        /// </summary>
        public Text TextField;
        /// <summary>
        /// message level informational.
        /// </summary>
        public const int INFO = 0;
        /// <summary>
        /// message level warning.
        /// </summary>
        public const int WARN = 1;
        private MessageData[] messages = new MessageData[0];
        public void SendMessage(string message, int messageLevel = INFO)
        {
            messages = ArrayUtilities.Instance.ExtendArray(new MessageData(message, messageLevel), messages);
        }
        public void DisplayMessages()
        {
            PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
            // clear messages on a FIFO basis
            for (int i = messages.Length - 1; i >= 0 ; i--)
            {
                sb.Append("(");
                sb.Append(System.DateTime.UtcNow.ToString("HH:mm:ss"));
                sb.Append(") ");
                switch (messages[i].level)
                {
                    case INFO:
                        sb.Append(messages[i].message);
                        break;
                    case WARN:
                        sb.Append("<color=red>");
                        sb.Append(messages[i].message);
                        sb.Append("</color>");
                        break;
                }
                sb.Append("\n");
                messages = ArrayUtilities.Instance.RemoveIndex(i, messages);
                i--;
            }
            sb.Append(TextField.text);
            TextField.text = sb.ToString();
            sb.ReturnToPool();
            sb = null;
        }
        private struct MessageData
        {
            public string message;
            public int level;
            public MessageData(string m, int l)
            {
                message = m;
                level = l;
            }
        }
    }
}
