using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RPGBase.Singletons
{
    public class RPGTime
    {
        /// <summary>
        /// the singleton instance.
        /// </summary>
        private static RPGTime instance;
        /// <summary>
        /// the instance property
        /// </summary>
        public static RPGTime Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new RPGTime();
                }
                return instance;
            }
        }
        /// <summary>
        /// the actual game time.
        /// </summary>
        private float gameTime = 0;
        /// <summary>
        /// flag indicating whether the timer has been initialized.
        /// </summary>
        private bool timerInit = false;
        /// <summary>
        /// the time the game was paused.
        /// </summary>
        private float timePaused = 0;
        public float TimePaused { get { return timePaused; } }
        /// <summary>
        /// flag indicating whether the game is paused or not.
        /// </summary>
        private bool timerPaused = false;
        public bool TimerPaused { get { return timerPaused; } }
        /// <summary>
        /// the time the timer was started. 
        /// </summary>
        private float timerStart;
        /// <summary>
        /// the total time the game has been spent paused.
        /// </summary>
        private float totalTimePaused = 0f;
        /// <summary>
        /// Hidden constructor.
        /// </summary>
        private RPGTime() { }
        public void ForceTimerReset(float time)
        {
            float tim = CurrentTime;
            totalTimePaused = tim - time;
            gameTime = time;
            timePaused = 0;
            timerPaused = false;
        }
        private float CurrentTime
        {
            get
            {
                InitializeTimer(); // initialize the timer if needed
                float now = Time.realtimeSinceStartup;
                return now - timerStart;
            }
        }
        public float GameTime
        {
            get
            {
                return GetGameTime(false);
            }
        }
        public float GetGameTime(bool usePause)
        {
            float tim = CurrentTime;
            if (timerPaused && usePause)
            {
                gameTime = tim;
            }
            else
            {
                gameTime = tim - totalTimePaused;
            }
            return gameTime;
        }
        /// <summary>
        /// Initializes the game timer.
        /// </summary>
        public void Init()
        {
            InitializeTimer();
            float tim = CurrentTime;
            totalTimePaused = tim;
            gameTime = 0;
            timePaused = 0;
            timerPaused = false;
            // ARX_BEGIN: jycorbel (2010-07-19) - Add external vars for
            // resetting them on ARX_TIME_Init call.
            // Currently when ARX_TIME_Init
            // the substract FrameDiff = FrameTime - LastFrameTime
            // is negative because of resetting totalTimePaused.
            // This solution reinit FrameTime & LastFrameTime to get a min
            // frameDiff = 0 on ARX_TIME_Init.
            // frameStart = LastFrameTime = gameTime;
            // ARX_END: jycorbel (2010-07-19)
        }
        /// <summary>
        /// Hidden method to start the game timer.
        /// </summary>
        private void InitializeTimer()
        {
            if (!timerInit)
            {
                // if the timer hasn't been started, start it
                timerStart = Time.realtimeSinceStartup;
                timerInit = true;
                // Init(); - why call this again?
            }
        }
        /// <summary>
        /// Pauses the game, and sets the time at which the pause started.
        /// </summary>
        public void Pause()
        {
            if (!timerPaused)
            {
                // get the current time
                // store the time the pause started in a field
                timePaused = CurrentTime;
                // set the paused flag
                timerPaused = true;
            }
        }
        /// <summary>
        /// Unpauses the game, and updates the time spent paused.
        /// </summary>
        public void Unpause()
        {
            if (timerPaused)
            {
                // get the current time
                // update the amount of time spent paused
                totalTimePaused += CurrentTime - timePaused;
                // remove the time the pause was started
                timePaused = 0;
                // remove the paused flag
                timerPaused = false;
            }
        }
    }
}
