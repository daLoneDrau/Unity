using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.BarbarianPrince.UI
{
    public class BoardManager : MonoBehaviour
    {
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
            // loop through and instantiate game objects
            GameObject toInstantiate = null;
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
