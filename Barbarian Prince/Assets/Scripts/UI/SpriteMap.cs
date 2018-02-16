﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Assets.Scripts.UI
{
    public class SpriteMap : MonoBehaviour
    {
        [SerializeField]
        SpriteEntry[] Map;
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