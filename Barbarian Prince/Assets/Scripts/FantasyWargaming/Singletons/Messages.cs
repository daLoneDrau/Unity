using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.FantasyWargaming.Singletons
{
    public class Messages : MonoBehaviour
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
        private Queue<string> messages;
        public void Add(string msg)
        {
            if (messages == null)
            {
                messages = new Queue<string>();
            }
            messages.Enqueue(msg);
        }
        public bool IsEmpty
        {
            get
            {
                return messages.Count == 0;
            }
        }
        public string Dequeue
        {
            get
            {
                return messages.Dequeue();
            }
        }
    }
}
