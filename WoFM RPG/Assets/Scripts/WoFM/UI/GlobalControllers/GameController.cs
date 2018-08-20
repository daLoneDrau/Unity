using Assets.Scripts.UI.SimpleJSON;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using WoFM.Singletons;

namespace WoFM.UI.GlobalControllers
{
    public class GameController : Singleton<GameController>
    {
        void Awake()
        {
            WoFMController.Init();
            WoFMInteractive.Init();
            WoFMScript.Init();
            DontDestroyOnLoad(gameObject);
            // load text file (JSON) version
            string json = System.IO.File.ReadAllText(@"Assets/JS/text.json");
            textFile = JSON.Parse(json);
            LoadText("BACKGROUND");
            json = System.IO.File.ReadAllText(@"Assets/JS/map.json");
            mapFile = JSON.Parse(json);
        }
        /// <summary>
        /// Loads map data from the data repository.
        /// </summary>
        /// <returns><see cref="List"/></returns>
        public List<MapData> LoadMap()
        {
            List<MapData> mapData = new List<MapData>();
            JSONArray cells = mapFile["cells"].AsArray;
            float xOffset = 638, yOffset = 1341;
            for (int i = 0, li = cells.Count; i < li; i++)
            {
                mapData.Add(new MapData(new Vector2(
                    cells[i]["x"].AsFloat - xOffset,
                    yOffset - cells[i]["y"].AsFloat),
                    cells[i]["type"])
                    );
            }
            return mapData;
        }
        /// <summary>
        /// the next scene playing after the 
        /// </summary>
        public int nextScene;
        /// <summary>
        /// the text file.
        /// </summary>
        private JSONNode textFile;
        /// <summary>
        /// the text file.
        /// </summary>
        private JSONNode mapFile;
        /// <summary>
        /// Loads a section of text by name.
        /// </summary>
        /// <param name="entry">the section's name</param>
        public void LoadText(string entry)
        {
            textEntry = textFile[entry];
        }
        /// <summary>
        /// the current section of text loaded for reading.
        /// </summary>
        public string textEntry;
    }
    /// <summary>
    /// A container for holding map tile information.
    /// </summary>
    public struct MapData
    {
        /// <summary>
        /// the tile coordinates.
        /// </summary>
        public Vector2 coordinates;
        /// <summary>
        /// the tile type.
        /// </summary>
        public string type;
        /// <summary>
        /// Creates a new instance of <see cref="MapData"/>
        /// </summary>
        /// <param name="v">the tile coordinates</param>
        /// <param name="s">the tile type</param>
        public MapData(Vector2 v, string s)
        {
            coordinates = v;
            type = s;
        }
    }
}
