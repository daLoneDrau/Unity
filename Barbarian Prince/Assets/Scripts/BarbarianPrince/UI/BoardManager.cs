using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.BarbarianPrince.UI
{
    public class BoardManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject grassBig;
        [SerializeField]
        private GameObject grassSmall;
        // the number of rows in the game board
        public int rows;
        // the number of columns in the game board.
        public int columns;
        /// <summary>
        /// array to hold board tiles.
        /// </summary>
        public GameObject[] tiles;
        /// <summary>
        /// Transform to hold all our game objects, so they don't clog up the hierarchy.
        /// </summary>
        private Transform tileHolder;
        /// <summary>
        /// list to keep track of all board positions and whether an object has been spawned there.
        /// </summary>
        private List<Vector3> gridPositions = new List<Vector3>();
        /// <summary>
        /// Initializes the board positions.
        /// </summary>
        void InitializeList()
        {
            gridPositions.Clear();
            for (int x = columns - 1; x >= 0; x--)
            {
                for (int y = rows - 1; y >= 0; y--)
                {
                    gridPositions.Add(new Vector3(x, y, 0f));
                }
            }
        }
        /// <summary>
        /// Sets up the board's floor.
        /// </summary>
        void BoardSetup()
        {
            tileHolder = new GameObject("Board").transform;
            RenderHex();
        }
        private void RenderHex()
        {
            // REMEMBER - SPRITE WAS SET TO COVER 32px per unit, so a 16x16 pixel would cover from 0,0 to 0.5,0.5 if positioned at 0.25,0.25
            // GET HEX TYPE
            // v1
            // render 32x32 image of hex type in middle. covers -0.5,-0.5 to .5,.5, with middle at 0,0
            GameObject toInstantiate = grassBig;
            GameObject instance = Instantiate(
                toInstantiate, // original object
                new Vector3(0, 0, 0),// z set to zero because working in 2d
                Quaternion.identity // no rotation
                ) as GameObject; // cast it to GameObject
            instance.transform.SetParent(tileHolder); // set new tile as child of tile holder
            // render North-side transitions
            // north side covers -.5,.5 to .5,1
            toInstantiate = grassSmall;
            // 1st 16x16 hex cover -.5,.5 to 0,1, with middle at -0.25, .75
            instance = Instantiate(
                toInstantiate, // original object
                new Vector3(-.25f, .75f, 0),// z set to zero because working in 2d
                Quaternion.identity // no rotation
                ) as GameObject; // cast it to GameObject
            instance.transform.SetParent(tileHolder); // set new tile as child of tile holder
            // 2nd 16x16 hex cover 0,.5 to .5,1, with middle at 0.25, .75
            instance = Instantiate(
                toInstantiate, // original object
                new Vector3(0.25f, .75f, 0),// z set to zero because working in 2d
                Quaternion.identity // no rotation
                ) as GameObject; // cast it to GameObject
            instance.transform.SetParent(tileHolder); // set new tile as child of tile holder
        }

        /// <summary>
        /// Called by the <see cref="GameMananger"/> when it's time to create the board.
        /// </summary>
        public void SetupScene()
        {
            BoardSetup();
        }
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
