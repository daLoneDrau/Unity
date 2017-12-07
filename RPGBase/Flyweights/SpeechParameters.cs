using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPGBase.Flyweights
{
    public struct SpeechParameters
    {
        public static int ANGRY = 16;
        public static int HAPPY = 8;
        public static int KEEP_SPEECH = 64;
        public static int NO_TEXT = 1;
        public static int OFF_VOICE = 32;
        public static int PLAYER = 4;
        public static int SIDE_L = 4096;
        public static int SIDE_R = 8192;
        public static int SPEECH_CCCLISTENER_L = 1024;
        public static int SPEECH_CCCLISTENER_R = 2048;
        public static int SPEECH_CCCTALKER_L = 256;
        public static int SPEECH_CCCTALKER_R = 512;
        public static int UNBREAKABLE = 2;
        public static int ZOOM_SPEECH = 128;
        private long flags;
        public bool KillAllSpeech { get; set; }
        public string SpeechName { get; set; }
        /**
         * Creates a new instance of {@link SpeechParameters}.
         * @param initParams the inital parameters, such as 'KILLALL', or 'H'
         * @param spName the name of the speech being given
         */
        public SpeechParameters(string initParams, string spName)
        {
            SpeechName = spName;
            flags = 0;
            KillAllSpeech = false;
            if (initParams != null
                    && initParams.Length > 0)
            {
                String[] split = initParams.Split(' ');
                for (int i = split.Length - 1; i >= 0; i--)
                {
                    if (string.Equals(split[i], "KILLALL", StringComparison.OrdinalIgnoreCase))
                    {
                        KillAllSpeech = true;
                    }
                    if (string.Equals(split[i], "T", StringComparison.OrdinalIgnoreCase))
                    {
                        AddFlag(SpeechParameters.NO_TEXT);
                    }
                    if (string.Equals(split[i], "U", StringComparison.OrdinalIgnoreCase))
                    {
                        AddFlag(SpeechParameters.UNBREAKABLE);
                    }
                    if (string.Equals(split[i], "P", StringComparison.OrdinalIgnoreCase))
                    {
                        AddFlag(SpeechParameters.PLAYER);
                    }
                    if (string.Equals(split[i], "H", StringComparison.OrdinalIgnoreCase))
                    {
                        AddFlag(SpeechParameters.HAPPY);
                    }
                    if (string.Equals(split[i], "A", StringComparison.OrdinalIgnoreCase))
                    {
                        AddFlag(SpeechParameters.ANGRY);
                    }
                    if (string.Equals(split[i], "O", StringComparison.OrdinalIgnoreCase))
                    {
                        AddFlag(SpeechParameters.OFF_VOICE);
                    }
                    if (string.Equals(split[i], "KEEP", StringComparison.OrdinalIgnoreCase))
                    {
                        AddFlag(SpeechParameters.KEEP_SPEECH);
                    }
                    if (string.Equals(split[i], "ZOOM", StringComparison.OrdinalIgnoreCase))
                    {
                        AddFlag(SpeechParameters.ZOOM_SPEECH);
                    }
                    if (string.Equals(split[i], "CCCTALKER_L", StringComparison.OrdinalIgnoreCase))
                    {
                        AddFlag(SpeechParameters.SPEECH_CCCTALKER_L);
                    }
                    if (string.Equals(split[i], "CCCTALKER_R", StringComparison.OrdinalIgnoreCase))
                    {
                        AddFlag(SpeechParameters.SPEECH_CCCTALKER_R);
                    }
                    if (string.Equals(split[i], "CCCLISTENER_L", StringComparison.OrdinalIgnoreCase))
                    {
                        AddFlag(SpeechParameters.SPEECH_CCCLISTENER_L);
                    }
                    if (string.Equals(split[i], "CCCLISTENER_R", StringComparison.OrdinalIgnoreCase))
                    {
                        AddFlag(SpeechParameters.SPEECH_CCCLISTENER_R);
                    }
                    if (string.Equals(split[i], "SIDE_L", StringComparison.OrdinalIgnoreCase))
                    {
                        AddFlag(SpeechParameters.SIDE_L);
                    }
                    if (string.Equals(split[i], "SIDE_R", StringComparison.OrdinalIgnoreCase))
                    {
                        AddFlag(SpeechParameters.SIDE_R);
                    }
                }
            }
        }
        /// <summary>
        /// Adds a flag.
        /// </summary>
        /// <param name="flag">the flag</param>
        public void AddFlag(long flag)
        {
            if (flag == ZOOM_SPEECH)
            {
                RemoveFlag(SPEECH_CCCTALKER_L);
                RemoveFlag(SPEECH_CCCTALKER_R);
                RemoveFlag(SPEECH_CCCLISTENER_L);
                RemoveFlag(SPEECH_CCCLISTENER_R);
                RemoveFlag(SIDE_L);
                RemoveFlag(SIDE_R);
            }
            else if (flag == SPEECH_CCCTALKER_L)
            {
                RemoveFlag(ZOOM_SPEECH);
                RemoveFlag(SPEECH_CCCTALKER_R);
                RemoveFlag(SPEECH_CCCLISTENER_L);
                RemoveFlag(SPEECH_CCCLISTENER_R);
                RemoveFlag(SIDE_L);
                RemoveFlag(SIDE_R);
            }
            else if (flag == SPEECH_CCCTALKER_R)
            {
                RemoveFlag(ZOOM_SPEECH);
                RemoveFlag(SPEECH_CCCTALKER_L);
                RemoveFlag(SPEECH_CCCLISTENER_L);
                RemoveFlag(SPEECH_CCCLISTENER_R);
                RemoveFlag(SIDE_L);
                RemoveFlag(SIDE_R);
            }
            else if (flag == SPEECH_CCCLISTENER_L)
            {
                RemoveFlag(ZOOM_SPEECH);
                RemoveFlag(SPEECH_CCCTALKER_L);
                RemoveFlag(SPEECH_CCCTALKER_R);
                RemoveFlag(SPEECH_CCCLISTENER_R);
                RemoveFlag(SIDE_L);
                RemoveFlag(SIDE_R);
            }
            else if (flag == SPEECH_CCCLISTENER_R)
            {
                RemoveFlag(ZOOM_SPEECH);
                RemoveFlag(SPEECH_CCCTALKER_L);
                RemoveFlag(SPEECH_CCCTALKER_R);
                RemoveFlag(SPEECH_CCCLISTENER_L);
                RemoveFlag(SIDE_L);
                RemoveFlag(SIDE_R);
            }
            else if (flag == SIDE_L)
            {
                RemoveFlag(ZOOM_SPEECH);
                RemoveFlag(SPEECH_CCCTALKER_L);
                RemoveFlag(SPEECH_CCCTALKER_R);
                RemoveFlag(SPEECH_CCCLISTENER_L);
                RemoveFlag(SPEECH_CCCLISTENER_R);
                RemoveFlag(SIDE_R);
            }
            else if (flag == SIDE_R)
            {
                RemoveFlag(ZOOM_SPEECH);
                RemoveFlag(SPEECH_CCCTALKER_L);
                RemoveFlag(SPEECH_CCCTALKER_R);
                RemoveFlag(SPEECH_CCCLISTENER_L);
                RemoveFlag(SPEECH_CCCLISTENER_R);
                RemoveFlag(SIDE_L);
            }
            flags |= flag;
        }
        /// <summary>
        /// Determines if the <see cref="BaseInteractiveObject"/> has a specific flag.
        /// </summary>
        /// <param name="flag">the flag</param>
        /// <returns>true if the <see cref="BaseInteractiveObject"/> has the flag; false otherwise</returns>
        public bool HasFlag(long flag)
        {
            return (flags & flag) == flag;
        }
        /// Removes a flag.
        /// </summary>
        /// <param name="flag">the flag</param>
        public void RemoveFlag(long flag)
        {
            flags &= ~flag;
        }
    }
}
