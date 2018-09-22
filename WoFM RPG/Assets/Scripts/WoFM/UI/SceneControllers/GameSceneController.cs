using RPGBase.Constants;
using RPGBase.Flyweights;
using RPGBase.Graph;
using RPGBase.Scripts.UI._2D;
using RPGBase.Singletons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using WoFM.Flyweights;
using WoFM.Flyweights.Actions;
using WoFM.Singletons;
using WoFM.UI._2D;
using WoFM.UI.SceneControllers;

namespace WoFM.UI.GlobalControllers
{
    public class GameSceneController : Singleton<GameSceneController>
    {
        public ParticleSystem bonker;
        public ParticleSystem snorer;
        public ParticleSystem helper;
        public ParticleSystem snorter;
        /// <summary>
        /// the GAME is playing
        /// </summary>
        public const int STATE_GAME = 0;
        /// <summary>
        /// the field for the current state the game is in.
        /// </summary>
        private int currentState = STATE_GAME;
        /// <summary>
        /// the property for the current state the game is in.
        /// </summary>
        public int CurrentState { get { return currentState; } }
        public bool CONTROLS_FROZEN = false;
        private bool doonce = false;
        /// <summary>
        /// the last state the game was in.
        /// </summary>
        private int lastState;
        /// <summary>
        /// the next state to go into after leaving the current state.
        /// </summary>
        private int nextState;
        /// <summary>
        /// the list of rooms the player has explored.
        /// </summary>
        private Dictionary<int, bool> roomsVisited = new Dictionary<int, bool>() { { 1, true } };
        /// <summary>
        /// List of actions that must be completed before returning control to the player.
        /// </summary>
        private IGameAction[] MustCompleteActions = new IGameAction[0];
        public void AddMustCompleteAction(IGameAction action)
        {
            MustCompleteActions = ArrayUtilities.Instance.ExtendArray(action, MustCompleteActions);
        }
        protected GameSceneController() { } // guarantee this will be always a singleton only - can't use the constructor!
                                            // Use this for initialization
        #region MonoBehaviour messages
        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        private void Awake() { }
        /// <summary>
        /// Start is called on the frame when a script is enabled just before any of the Update methods are called the first time.
        /// </summary>
        private void Start()
        {
            bool playerCreated = true;
            GameObject player;
            try
            {
                ((WoFMInteractive)Interactive.Instance).GetPlayerIO();
            }
            catch (RPGException ex)
            {
                playerCreated = false;
            }
            // does the Player object exist?
            if (!playerCreated)
            {
                // create player object
                player = GameController.Instance.NewHero();
                // get IO component
                WoFMInteractiveObject playerIo = player.GetComponent<WoFMInteractiveObject>();
                // re-initialize player stats
                Script.Instance.SendInitScriptEvent(playerIo);
            }
            player = ((WoFMInteractive)Interactive.Instance).GetPlayerIO().gameObject;
            // add particle system bonk
            player.GetComponent<HeroMove>().bonker = bonker;
            if (player.GetComponent<SpriteRenderer>() == null)
            {
                player.AddComponent<SpriteRenderer>();
                player.GetComponent<SpriteRenderer>().sprite = SpriteMap.Instance.GetSprite("hero_0");
                player.GetComponent<SpriteRenderer>().sortingLayerName = "Units";
                player.layer = LayerMask.NameToLayer("BlockingLayer");
                player.GetComponent<HeroMove>().blockingLayer= 1<<LayerMask.NameToLayer("BlockingLayer");
            }
            // teleport the player to the bottom of the screen
            AddMustCompleteAction(new TeleportAction(((WoFMInteractive)Interactive.Instance).GetPlayerIO(), new Vector2(641 - GameController.MAP_X_OFFSET, GameController.MAP_Y_OFFSET - 1341)));
        }
        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        private void Update()
        {
            if (!doonce)
            {
                doonce = true;
                WeightedGraphEdge[] path = WorldController.Instance.GetLandPath(new Vector2(641 - GameController.MAP_X_OFFSET, GameController.MAP_Y_OFFSET - 1340), new Vector2(641 - GameController.MAP_X_OFFSET, GameController.MAP_Y_OFFSET - 1339));
                for (int i = path.Length - 1; i >= 0; i--)
                {
                    // get tile destination
                    Vector2 node = WorldController.Instance.GetNodeCoordinatesFromId(path[i].To);
                    AddMustCompleteAction(new MoveIoUninterruptedAction(((WoFMInteractive)Interactive.Instance).GetPlayerIO(), node));
                }
            }
            // what state are we in?
            switch (currentState)
            {
                case STATE_GAME:
                    GameState();
                    break;
            }
            // if game state
        }
        #endregion
        private void GameState()
        {
            // are there any actions that need to be run?
            if (MustCompleteActions.Length > 0)
            {
                CONTROLS_FROZEN = true;
                // complete actions and then run another update
                for (int i = 0; i < MustCompleteActions.Length; i++)
                {
                    // execute the action
                    MustCompleteActions[i].Execute();
                    if (MustCompleteActions[i].IsResolved())
                    {
                        // if action is resolved, remove the action from the list
                        MustCompleteActions = ArrayUtilities.Instance.RemoveIndex(i, MustCompleteActions);
                        i--;
                    }
                    else
                    {
                        // don't start a new action until previous resolves
                        break;
                    }
                }
            }
            else
            {
            }
            // are we listening for device input
            if (!CONTROLS_FROZEN)
            {
                if (Input.anyKey)
                {
                    // freeze the controls until the action is over
                    CONTROLS_FROZEN = true;
                    bool actionable = false;
                    int x = 0, y = 0;
                    // find out which key
                    if (Input.GetKeyDown("up"))
                    {
                        print("up arrow key was pressed");
                        actionable = true;
                        y++;
                    }
                    if (Input.GetKeyDown("down"))
                    {
                        print("down arrow key was pressed");
                        actionable = true;
                        y--;
                    }
                    if (Input.GetKeyDown("left"))
                    {
                        print("left arrow key was pressed");
                        actionable = true;
                        x--;

                    }
                    if (Input.GetKeyDown("right"))
                    {
                        print("right arrow key was pressed");
                        actionable = true;
                        x++;
                    }
                    if (!actionable)
                    {
                        // key pressed wasn't valid. turn controls back on
                        CONTROLS_FROZEN = false;
                    }
                    else
                    {
                        if (x != 0
                            || y != 0)
                        {
                            print("try move " + x + "," + y);
                            // try a move
                            MovingObject mo = ((WoFMInteractive)Interactive.Instance).GetPlayerIO().gameObject.GetComponent<MovingObject>();
                            mo.AttemptMove<Blocker>(x, y);
                        }
                    }
                }
            }
        }
        #region ROOM METHODS
        /// <summary>
        /// Sets the flag indicating whether a room has been visited.
        /// </summary>
        /// <param name="roomId">the room number</param>
        /// <param name="flag">the flag</param>
        public void SetRoomVisited(int roomId, bool flag)
        {
            roomsVisited[roomId] = flag;
        }
        /// <summary>
        /// Determines if a room has been visited.
        /// </summary>
        /// <param name="roomId">the room's id</param>
        /// <returns>true if the room was visited, false otherwise</returns>
        public bool WasRoomVisited(int roomId)
        {
            bool was = false;
            roomsVisited.TryGetValue(roomId, out was);
            return was;
        }
        #endregion
        public void CheckIOMoveIntoTile(WoFMInteractiveObject io, Vector2 destination)
        {
            if (io.HasIOFlag(IoGlobals.IO_01_PC))
            {
                // set PC moved into room
                WoFMTile tile = WorldController.Instance.GetTileAt(destination);
                SetRoomVisited(tile.RoomId, true);
            }
            // check for triggers
            foreach (Transform child in GameController.Instance.triggerHolder)
            {
                print("checking trigger " + child.gameObject.name);
                //child is your child transform
                WoFMInteractiveObject tio = child.gameObject.GetComponent<WoFMInteractiveObject>();
                Vector2 tioPos = new Vector2(tio.Script.GetLocalFloatVariableValue("x"), tio.Script.GetLocalFloatVariableValue("y"));
                print(tioPos);
                if (io.LastPositionHeld == tioPos)
                {
                    print("IO just entered " + child.gameObject.name);
                }
            }
        }
    }
}
