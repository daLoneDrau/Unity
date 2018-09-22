using Assets.Scripts.UI.SimpleJSON;
using RPGBase.Pooled;
using RPGBase.Scripts.UI._2D;
using RPGBase.Singletons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using WoFM.Flyweights;
using WoFM.Singletons;
using WoFM.UI._2D;
using WoFM.UI.SceneControllers;

namespace WoFM.UI.GlobalControllers
{
    /// <summary>
    /// Utility class for managing the game process.  GameController initializes singleton controllers for scripts, IOs, etc... and manages loaded resources, such as text files and the board map.
    /// </summary>
    public class GameController : Singleton<GameController>
    {
        /// <summary>
        /// the next scene playing after the current one finishes
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
        /// the current section of text loaded for reading.
        /// </summary>
        public string textEntry;
        /// <summary>
        /// Transform to hold all our game objects, so they don't clog up the hierarchy.
        /// </summary>
        public Transform triggerHolder;
        // map offset set to 638, 1341, since map cells don't start at 0,0
        public const int MAP_X_OFFSET = 623;
        public const int MAP_Y_OFFSET = 1341;
        /// <summary>
        /// Loads map data from the data repository.
        /// </summary>
        /// <returns><see cref="List"/></returns>
        public List<MapData> LoadMap()
        {
            /********************************************
            * LOAD THE MAP
            /*******************************************/
            List<MapData> mapData = new List<MapData>();
            JSONArray array = mapFile["cells"].AsArray;
            for (int i = 0, li = array.Count; i < li; i++)
            {
                mapData.Add(new MapData(new Vector2(
                    array[i]["x"].AsFloat - MAP_X_OFFSET,
                    MAP_Y_OFFSET - array[i]["y"].AsFloat),
                    array[i]["room"].AsInt,
                    array[i]["type"])
                    );
            }
            array = mapFile["triggers"].AsArray;
            for (int i = 0, li = array.Count; i < li; i++)
            {
                JSONNode node = array[i];
                string type = node["type"];
                int id = node["id"].AsInt;
                float x = node["x"].AsFloat - MAP_X_OFFSET;
                float y = MAP_Y_OFFSET - node["y"].AsFloat;
                GameObject trigger = NewTrigger(id, type);
                WoFMInteractiveObject tio = trigger.GetComponent<WoFMInteractiveObject>();
                tio.Script.SetLocalVariable("x", x);
                tio.Script.SetLocalVariable("y", y);
                if (node["text"] != null)
                {
                    tio.Script.SetLocalVariable("text", (string)node["text"]);
                }
            }
            return mapData;
        }
        /// <summary>
        /// Loads a section of text by name.
        /// </summary>
        /// <param name="entry">the section's name</param>
        public void LoadText(string entry)
        {
            textEntry = textFile[entry];
        }
        /// <summary>
        /// Creates a new player <see cref="GameObject"/> with a <see cref="SpriteRenderer"/> and <see cref="WoFMInteractiveObject"/> components.
        /// </summary>
        /// <returns><see cref="GameObject"/></returns>
        public GameObject NewHero()
        {
            // create player GameObject
            GameObject player = new GameObject
            {
                name = "player"
            };
            // position player outside the screen
            player.transform.position = new Vector3(-1, 0, 0);
            // set player sprite to no shield sprite
            if (SpriteMap.Instance != null)
            {
                SpriteRenderer sr = player.AddComponent<SpriteRenderer>();
                sr.sprite = SpriteMap.Instance.GetSprite("hero_0");
                sr.sortingLayerName = "Units";
                player.layer = LayerMask.NameToLayer("BlockingLayer");
            }
            // add component to handle player moves
            HeroMove hm = player.AddComponent<HeroMove>();
            hm.blockingLayer = 1 << LayerMask.NameToLayer("BlockingLayer");
            // add rigidbody2d and boxcollider2d;
            BoxCollider2D bc = player.AddComponent<BoxCollider2D>() as BoxCollider2D;
            bc.size = new Vector2(0.9f, 0.9f);
            Rigidbody2D r = player.AddComponent<Rigidbody2D>() as Rigidbody2D;
            r.bodyType = RigidbodyType2D.Kinematic;
            // create new IO and assign as component to player object
            WoFMInteractiveObject playerIo = player.AddComponent<WoFMInteractiveObject>() as WoFMInteractiveObject;

            // register the IO as the player and initialize player components
            ((WoFMInteractive)Interactive.Instance).NewHero(playerIo);

            // remove instances for garbage collection
            playerIo = null;

            return player;
        }
        public GameObject NewTrigger(int id, string script)
        {
            PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
            sb.Append("trigger");
            sb.Append(id);
            // create player GameObject
            GameObject trigger = new GameObject
            {

                name = sb.ToString()
            };
            sb.ReturnToPool();
            // position trigger outside the screen
            trigger.transform.position = new Vector3(-1, 0, 0);
            // create new IO and assign as component to player object
            WoFMInteractiveObject triggerIo = trigger.AddComponent<WoFMInteractiveObject>() as WoFMInteractiveObject;
            // register the IO and initialize trigger components
            ((WoFMInteractive)Interactive.Instance).NewTrigger(triggerIo, script);

            // set new trigger as child of trigger holder
            trigger.transform.SetParent(triggerHolder);

            return trigger;
        }
        #region MonoBehaviour messages
        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        void Awake()
        {
            // initialize singleton controllers
            WoFMController.Init();
            WoFMInteractive.Init();
            WoFMScript.Init();
            DontDestroyOnLoad(gameObject);
            // create holder for all triggers
            triggerHolder = new GameObject("Triggers").transform;
            DontDestroyOnLoad(triggerHolder);
            //*******************************//
            // LOAD GAME RESOURCES
            //*******************************//
            // load text file (JSON) version
            string json = System.IO.File.ReadAllText(@"Assets/JS/text.json");
            textFile = JSON.Parse(json);
            LoadText("BACKGROUND");
            json = System.IO.File.ReadAllText(@"Assets/JS/map.json");
            mapFile = JSON.Parse(json);
        }
        /// <summary>
        /// Start is called on the frame when a script is enabled just before any of the Update methods are called the first time.
        /// </summary>
        void Start()
        { }
        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        void Update()
        { }
        #endregion
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
        /// the room in which the tile is located.
        /// </summary>
        public int roomId;
        /// <summary>
        /// the tile type.
        /// </summary>
        public string type;
        /// <summary>
        /// Creates a new instance of <see cref="MapData"/>
        /// </summary>
        /// <param name="v">the tile coordinates</param>
        /// <param name="i">the tile's room</param>
        /// <param name="s">the tile type</param>
        public MapData(Vector2 v, int i, string s)
        {
            coordinates = v;
            roomId = i;
            type = s;
        }
    }
}
