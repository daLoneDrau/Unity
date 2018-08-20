using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RPGBase.Scripts.UI._2D
{
    public class SpriteMap : MonoBehaviour
    {
        /// <summary>
        /// The list of sprites.
        /// </summary>
        [SerializeField]
        SpriteEntry[] Map;
        /// <summary>
        /// The one and only instance of the <see cref="SpriteMap"/> class.
        /// </summary>
        public static SpriteMap Instance { get; private set; }
        #region MonoBehaviour messages
        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            Instance = this;
            print("setting instance of SpriteMap");
        }
        /// <summary>
        /// Start is called on the frame when a script is enabled just before any of the Update methods are called the first time.
        /// </summary>
        void Start()
        {

        }
        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        private void Update()
        {
        }
        #endregion
        /// <summary>
        /// Gets a specific <see cref="Sprite"/> by name.
        /// </summary>
        /// <param name="name">the name</param>
        /// <returns><see cref="Sprite"/></returns>
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
    [Serializable]
    public struct SpriteEntry
    {
        public string Name;
        public Sprite Sprite;
    }
}
