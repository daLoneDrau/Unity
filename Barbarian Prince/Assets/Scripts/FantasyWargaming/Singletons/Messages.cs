using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.FantasyWargaming.Singletons
{
    public class Messages: MonoBehaviour
    {
        private static Messages instance;
        /// <summary>
        /// the singleton instance.
        /// </summary>
        public static Messages Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject go = new GameObject
                    {
                        name = "Messages"
                    };
                    instance = go.AddComponent<Messages>();
                }
                return instance;
            }
        }
        private Stack<string> messages;
        public void Add(string msg)
        {
            if (messages == null)
            {
                messages = new Stack<string>();
            }
            messages.Push(msg);
        }
        public bool IsEmpty()
        {
            return messages.Count == 0;
        }
        public string Pop()
        {
            return messages.Pop();
        }
    }
}
