using RPGBase.Flyweights;

namespace RPGBase.Singletons
{
    public abstract class Speech
    {
        /// <summary>
        /// the singleton instance.
        /// </summary>
        public static Speech Instance { get; protected set; }
        public abstract int AddSpeech(BaseInteractiveObject io, int mood, string speech, long voixoff);
    }
}
