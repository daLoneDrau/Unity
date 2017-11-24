using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Flyweights
{
    class SpeechParameters
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
        private bool killAllSpeech;
        private char[] speechName;
        /**
         * Creates a new instance of {@link SpeechParameters}.
         * @param initParams the inital parameters, such as 'KILLALL', or 'H'
         * @param spName the name of the speech being given
         */
        public SpeechParameters( String initParams,  String spName)
        {
            setSpeechName(spName);
            if (initParams != null
                    && initParams.Length() > 0)
            {
                String[] split = initParams.Split(" ");
                for (int i = split.Length - 1; i >= 0; i--)
                {
                    if (split[i].equalsIgnoreCase("KILLALL"))
                    {
                        killAllSpeech = true;
                    }
                    if (split[i].equalsIgnoreCase("T"))
                    {
                        AddFlag(SpeechParameters.NO_TEXT);
                    }
                    if (split[i].equalsIgnoreCase("U"))
                    {
                        AddFlag(SpeechParameters.UNBREAKABLE);
                    }
                    if (split[i].equalsIgnoreCase("P"))
                    {
                        AddFlag(SpeechParameters.PLAYER);
                    }
                    if (split[i].equalsIgnoreCase("H"))
                    {
                        AddFlag(SpeechParameters.HAPPY);
                    }
                    if (split[i].equalsIgnoreCase("A"))
                    {
                        AddFlag(SpeechParameters.ANGRY);
                    }
                    if (split[i].equalsIgnoreCase("O"))
                    {
                        AddFlag(SpeechParameters.OFF_VOICE);
                    }
                    if (split[i].equalsIgnoreCase("KEEP"))
                    {
                        AddFlag(SpeechParameters.KEEP_SPEECH);
                    }
                    if (split[i].equalsIgnoreCase("ZOOM"))
                    {
                        AddFlag(SpeechParameters.ZOOM_SPEECH);
                    }
                    if (split[i].equalsIgnoreCase("CCCTALKER_L"))
                    {
                        AddFlag(SpeechParameters.SPEECH_CCCTALKER_L);
                    }
                    if (split[i].equalsIgnoreCase("CCCTALKER_R"))
                    {
                        AddFlag(SpeechParameters.SPEECH_CCCTALKER_R);
                    }
                    if (split[i].equalsIgnoreCase("CCCLISTENER_L"))
                    {
                        AddFlag(SpeechParameters.SPEECH_CCCLISTENER_L);
                    }
                    if (split[i].equalsIgnoreCase("CCCLISTENER_R"))
                    {
                        AddFlag(SpeechParameters.SPEECH_CCCLISTENER_R);
                    }
                    if (split[i].equalsIgnoreCase("SIDE_L"))
                    {
                        AddFlag(SpeechParameters.SIDE_L);
                    }
                    if (split[i].equalsIgnoreCase("SIDE_R"))
                    {
                        AddFlag(SpeechParameters.SIDE_R);
                    }
                }
            }
        }
        /**
         * Adds a flag.
         * @param flag the flag
         */
        public void AddFlag( long flag)
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
        /**
         * @return the speechName
         */
        public String getSpeechName()
        {
            String s = null;
            if (speechName != null)
            {
                s = new String(speechName);
            }
            return s;
        }
        /**
         * Determines if the {@link BaseInteractiveObject} has a specific flag.
         * @param flag the flag
         * @return true if the {@link BaseInteractiveObject} has the flag; false
         *         otherwise
         */
        public  bool HasFlag( long flag)
        {
            return (flags & flag) == flag;
        }
        /**
         * @return the killAllSpeech
         */
        public bool isKillAllSpeech()
        {
            return killAllSpeech;
        }
        /**
         * Removes a flag.
         * @param flag the flag
         */
        public  void RemoveFlag( long flag)
        {
            flags &= ~flag;
        }
        /**
         * @param flag the killAllSpeech to set
         */
        public void setKillAllSpeech(bool flag)
        {
            killAllSpeech = flag;
        }
        /**
         * @param val the value to set
         */
        public void setSpeechName(String val)
        {
            if (val != null)
            {
                speechName = val;
            }
        }
    }
}
