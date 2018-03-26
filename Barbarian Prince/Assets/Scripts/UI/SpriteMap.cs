using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Assets.Scripts.UI
{
    public class SpriteMap : MonoBehaviour
    {
        [SerializeField]
        SpriteEntry[] Map;
        /// <summary>
        /// the one and only instance of the <see cref="Diceroller"/> class.
        /// </summary>
        public static SpriteMap Instance { get; private set; }
        private void Awake()
        {
            Instance = this;
        }
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        public Sprite GetSprite(string name)
        {
            Sprite sprite = null;
            for (int i = Map.Length - 1; i >= 0; i--)
            {
                if (string.Equals(name, Map[i].Name, StringComparison.OrdinalIgnoreCase))
                {
                    sprite = Map[i].Sprite;
                    break;
                }
            }
            return sprite;
        }
    }
    [System.Serializable]
    public struct SpriteEntry
    {
        public string Name;
        public Sprite Sprite;
    }
}
