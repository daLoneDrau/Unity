using Assets.Scripts.UI.SimpleJSON;
using Assets.Scripts.WoFM.UI.SceneControllers;
using RPGBase.Flyweights;
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
        //****************************
        // HOLDERS FOR GAME OBJECTS
        //****************************
        /// <summary>
        /// Transform to hold all our game objects, so they don't clog up the hierarchy.
        /// </summary>
        public Transform doorHolder;
        /// <summary>
        /// Transform to hold all our game objects, so they don't clog up the hierarchy.
        /// </summary>
        public Transform mobHolder;
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
            print("*****************************loadmap");
            /********************************************
            * LOAD THE MAP
            /*******************************************/
            List<MapData> mapData = new List<MapData>();
            JSONArray array = mapFile["cells"].AsArray;
            for (int i = 0, li = array.Count; i < li; i++)
            {
                int[] roomArr;
                if (array[i]["room"].IsArray)
                {
                    roomArr = new int[array[i]["room"].Count];
                    for (int j = 0, lj = array[i]["room"].Count; j < lj; j++)
                    {
                        roomArr[j] = array[i]["room"][j].AsInt;
                    }
                }
                else
                {
                    roomArr = new int[] { array[i]["room"].AsInt };
                }
                mapData.Add(new MapData(new Vector2(
                    array[i]["x"].AsFloat - MAP_X_OFFSET,
                    MAP_Y_OFFSET - array[i]["y"].AsFloat),
                    roomArr,
                    array[i]["type"])
                    );
            }
            /********************************************
            * LOAD TRIGGERS
            /*******************************************/
            array = mapFile["triggers"].AsArray;
            for (int i = 0, li = array.Count; i < li; i++)
            {
                JSONNode node = array[i];
                string type = node["type"];
                int id = node["id"].AsInt;
                print("loading trigger " + id);
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
                if (node["needs_modal"] != null)
                {
                    tio.Script.SetLocalVariable("needs_modal", node["needs_modal"].AsInt);
                }
                if (node["modal_sprite"] != null)
                {
                    tio.Script.SetLocalVariable("modal_sprite", (string)node["modal_sprite"]);
                }
            }

            array = mapFile["doors"].AsArray;
            for (int i = 0, li = array.Count; i < li; i++)
            {
                JSONNode node = array[i];
                string type = node["type"];
                int id = node["id"].AsInt;
                float x = node["x"].AsFloat - MAP_X_OFFSET;
                float y = MAP_Y_OFFSET - node["y"].AsFloat;
                bool locked = node["locked"].AsBool;
                GameObject door = NewDoor(id, type);
                WoFMInteractiveObject tio = door.GetComponent<WoFMInteractiveObject>();
                tio.Script.SetLocalVariable("x", x);
                tio.Script.SetLocalVariable("y", y);
                tio.Script.SetLocalVariable("bashable", node["bashable"].AsInt);
                tio.Script.SetLocalVariable("keyed", node["keyed"].AsInt);
                tio.Script.SetLocalVariable("locked", node["locked"].AsInt);
                tio.Script.SetLocalVariable("open", 0);
                tio.Script.SetLocalVariable("room", node["room"].AsInt);
            }
            return mapData;
        }
        #region TEXT
        /// <summary>
        /// Gets a text entry.
        /// </summary>
        /// <param name="entry">the name of the entry</param>
        /// <returns><see cref="string"/></returns>
        public string GetText(string entry)
        {
            return textFile[entry];
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
        /// Sets a text entry.
        /// </summary>
        /// <param name="entry">the name of the text entry</param>
        /// <param name="newText">the new text</param>
        public void SetText(string entry, string newText)
        {
            textFile[entry] = newText;
        }
        #endregion
        #region IO GENERATION
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
            /********************************************************
             * SETUP SPRITE_RENDERER COMPONENT
            /*******************************************************/
            if (SpriteMap.Instance != null)
            {
                SpriteRenderer sr = player.AddComponent<SpriteRenderer>();
                // set player sprite to no shield sprite
                sr.sprite = SpriteMap.Instance.GetSprite("hero_0");
                sr.sortingLayerName = "Units";
                player.layer = LayerMask.NameToLayer("BlockingLayer");
            }
            /********************************************************
             * SETUP HERO_MOVE COMPONENT
            /*******************************************************/
            HeroMove hm = player.AddComponent<HeroMove>();
            hm.blockingLayer = 1 << LayerMask.NameToLayer("BlockingLayer");
            /********************************************************
             * SETUP BOX_COLLIDER_2D COMPONENT
            /*******************************************************/
            BoxCollider2D bc = player.AddComponent<BoxCollider2D>() as BoxCollider2D;
            bc.size = new Vector2(0.9f, 0.9f);
            /********************************************************
             * SETUP RIGID_BODY COMPONENT
            /*******************************************************/
            Rigidbody2D r = player.AddComponent<Rigidbody2D>() as Rigidbody2D;
            r.bodyType = RigidbodyType2D.Kinematic;
            /********************************************************
             * SETUP WOFM_INTERACTIVE_OBJECT COMPONENT
            /*******************************************************/
            WoFMInteractiveObject playerIo = player.AddComponent<WoFMInteractiveObject>() as WoFMInteractiveObject;
            /********************************************************
             * REGISTER THE IO AND INITIALIZE INTERACTIVE COMPONENTS
            /*******************************************************/
            ((WoFMInteractive)Interactive.Instance).NewHero(playerIo);
            /********************************************************
             * SETUP WATCHERS
            /*******************************************************/
            if (StatPanelController.Instance != null)
            {
                playerIo.PcData.AddWatcher(StatPanelController.Instance);
            }

            // remove instances for garbage collection
            playerIo = null;
            print("created player - " + player.GetComponent<WoFMInteractiveObject>().RefId);
            print(((WoFMInteractive)Interactive.Instance).PlayerId);
            return player;
        }
        /// <summary>
        /// Creates a new mob <see cref="GameObject"/> with a <see cref="SpriteRenderer"/> and <see cref="WoFMInteractiveObject"/> components.
        /// </summary>
        /// <returns><see cref="GameObject"/></returns>
        public GameObject NewMob(string mobName, string sprite, Scriptable script)
        {
            // create player GameObject
            GameObject mob = new GameObject
            {
                name = mobName
            };
            // position player outside the screen
            mob.transform.position = new Vector3(-1, 0, 0);
            /********************************************************
             * SETUP SPRITE_RENDERER COMPONENT
            /*******************************************************/
            if (SpriteMap.Instance != null)
            {
                SpriteRenderer sr = mob.AddComponent<SpriteRenderer>();
                // set player sprite to no shield sprite
                sr.sprite = SpriteMap.Instance.GetSprite(sprite);
                sr.sortingLayerName = "Units";
                mob.layer = LayerMask.NameToLayer("BlockingLayer");
            }
            /********************************************************
             * SETUP MOB_MOVE COMPONENT
            /*******************************************************/
            MobMove move = mob.AddComponent<MobMove>();
            move.blockingLayer = 1 << LayerMask.NameToLayer("BlockingLayer");
            /********************************************************
             * SETUP BOX_COLLIDER_2D COMPONENT
            /*******************************************************/
            BoxCollider2D bc = mob.AddComponent<BoxCollider2D>() as BoxCollider2D;
            bc.size = new Vector2(0.9f, 0.9f);
            /********************************************************
             * SETUP RIGID_BODY COMPONENT
            /*******************************************************/
            Rigidbody2D r = mob.AddComponent<Rigidbody2D>() as Rigidbody2D;
            r.bodyType = RigidbodyType2D.Kinematic;
            /********************************************************
             * SETUP WOFM_INTERACTIVE_OBJECT COMPONENT
            /*******************************************************/
            WoFMInteractiveObject mobIo = mob.AddComponent<WoFMInteractiveObject>() as WoFMInteractiveObject;
            mobIo.Sprite = SpriteMap.Instance.GetSprite(sprite);
            /********************************************************
             * REGISTER THE IO AND INITIALIZE INTERACTIVE COMPONENTS
            /*******************************************************/
            ((WoFMInteractive)Interactive.Instance).NewMob(mobIo, script);
            /********************************************************
             * SETUP WATCHERS
            /*******************************************************/

            // remove instances for garbage collection
            mobIo = null;
            // set new trigger as child of mob holder
            mob.transform.SetParent(mobHolder);
            print("created mob - " + mob.GetComponent<WoFMInteractiveObject>().RefId);
            return mob;
        }
        public GameObject NewDoor(int id, string script)
        {
            PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
            sb.Append("door");
            sb.Append(id);
            // create player GameObject
            GameObject door = new GameObject
            {
                name = sb.ToString()
            };
            sb.ReturnToPool();
            /********************************************************
             * SETUP BLOCKER COMPONENT
            /*******************************************************/
            Blocker b = door.AddComponent<Blocker>();
            b.Type = Blocker.DOOR;
            /********************************************************
             * SETUP BOX_COLLIDER_2D COMPONENT
            /*******************************************************/
            BoxCollider2D bc = door.AddComponent<BoxCollider2D>() as BoxCollider2D;
            bc.size = new Vector2(1f, 1f);
            /********************************************************
             * SETUP SPRITE_RENDERER COMPONENT
            /*******************************************************/
            if (SpriteMap.Instance != null)
            {
                SpriteRenderer sr = door.AddComponent<SpriteRenderer>();
                sr.sprite = SpriteMap.Instance.GetSprite("door_0");
                sr.sortingLayerName = "Units";
                door.layer = LayerMask.NameToLayer("BlockingLayer");
            }
            /********************************************************
             * SETUP INTERACTIVE_OBJECT COMPONENT
            /*******************************************************/
            // create new IO and assign as component to door object
            WoFMInteractiveObject doorIo = door.AddComponent<WoFMInteractiveObject>() as WoFMInteractiveObject;
            // register the IO and initialize trigger components
            ((WoFMInteractive)Interactive.Instance).NewDoor(doorIo, script);
            /********************************************************
             * FINISH SETUP
            /*******************************************************/
            // position door outside the screen
            door.transform.position = new Vector3(-1, 0, 0);
            // set new trigger as child of trigger holder
            door.transform.SetParent(doorHolder);

            return door;
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
        #endregion
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
            // create holder for all doors
            doorHolder = new GameObject("Doors").transform;
            DontDestroyOnLoad(doorHolder);
            // create holder for all mobs
            mobHolder = new GameObject("Mobs").transform;
            DontDestroyOnLoad(mobHolder);
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
        public int[] roomId;
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
        public MapData(Vector2 v, int[] i, string s)
        {
            coordinates = v;
            roomId = i;
            type = s;
        }
    }
}
