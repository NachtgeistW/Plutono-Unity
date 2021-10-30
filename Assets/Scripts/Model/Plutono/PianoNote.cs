using Model.Plutono;

namespace Assets.Scripts.Model.Plutono
{
    public class PianoNote : GameNote
    {
        public void PlayPianoSound()
        {

        }

        public void OnFingerTap()
        {

        }

        protected override NoteRenderer CreateRenderer()
        {
            int a = 0;
            return new NoteRenderer();
        }
    }
}