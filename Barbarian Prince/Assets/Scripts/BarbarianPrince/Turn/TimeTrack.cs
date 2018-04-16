using Assets.Scripts.BarbarianPrince.Constants;
using Assets.Scripts.BarbarianPrince.UI.Controllers;
using RPGBase.Pooled;
using RPGBase.Singletons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.BarbarianPrince.Turn
{
    public class TimeTrack
    {
        /// <summary>
        /// the singleton instance.
        /// </summary>
        private static TimeTrack instance;
        /// <summary>
        /// Gets the one and only instance of <see "TimeTrack"></see>.
        /// </summary>
        /// <returns><see "TimeTrack"></returns>
        public static TimeTrack Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TimeTrack();
                }
                return instance;
            }
        }
        /// <summary>
        /// the current day.
        /// </summary>
        public Day Day { get; private set; }
        /// <summary>
        /// the current time phase.
        /// </summary>
        public TimePhase Time { get; private set; }
        /// <summary>
        /// the current week.
        /// </summary>
        public Week Week { get; private set; }
        /// <summary>
        /// Hidden constructor.
        /// </summary>
        private TimeTrack()
        {
            Time = TimePhase.PHASE_00_PRE_ACTION;
            Day = Day.ONE;
            Week = Week.ONE;
        }
        /// <summary>
        /// Advances the day by one.
        /// </summary>
        public void NextDay()
        {
            int current = Day.Value + 1;
            if (current > Day.MAX)
            {
                current = 1;
                NextWeek();
            }
            Day = Day.ValueOf(current);
        }
        /// <summary>
        /// Advances the time by one period.
        /// </summary>
        public void NextPhase()
        {
            Time = Time.Advance();
            if (Time.Equals(TimePhase.PHASE_00_PRE_ACTION))
            {
                NextDay();
            }
            Script.Instance.SendMsgToAllIO(BPGlobals.SM_300_TIME_CHANGE, null);
            GameController.Instance.UpdateTimeTrack(ToUiString());
        }
        /// <summary>
        /// Advances the week by one.
        /// </summary>
        private void NextWeek()
        {
            int current = Week.Value + 1;
            if (current > Week.TEN.Value)
            {
                // do something to end the game
                // BPController.stopGame();
            }
            else
            {
                Week = Week.ValueOf(current);
            }
        }
        /// <summary>
        /// Resets the <see "TimeTrack></see>.
        /// </summary>
        public void Reset()
        {
            Time = TimePhase.PHASE_00_PRE_ACTION;
            Day = Day.ONE;
            Week = Week.ONE;
        }
        /// <summary>
        /// Gets the current time converted for display to the UI.
        /// </summary>
        /// <returns></returns>
        public string ToUiString()
        {
            String s = "";
            PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
            sb.Append("Day ");
            sb.Append(Day.Value);
            sb.Append(" Week ");
            sb.Append(Week.Value);
            sb.Append(" - ");
            sb.Append(Time.Title);
            s = sb.ToString();
            sb.ReturnToPool();
            sb = null;
            return s;
        }
        public override string ToString()
        {
            String s = "";
            PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
            sb.Append(Day.Adjective);
            sb.Append(" Day ");
            sb.Append(Week.Adjective);
            sb.Append(" Week ");
            sb.Append(" - ");
            sb.Append(Time.Title);
            s = sb.ToString();
            sb.ReturnToPool();
            sb = null;
            return s;
        }
    }
}
