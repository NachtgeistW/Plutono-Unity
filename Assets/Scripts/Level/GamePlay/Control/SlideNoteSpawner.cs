using Plutono.GamePlay.Notes;
using Plutono.Level.GamePlay;
using Plutono.Song;
using Plutono.Util;

namespace Plutono.GamePlay.Control
{
    public class SlideNoteSpawner : NoteSpawner<SlideNote>
    {
        private void OnEnable()
        {
            EventCenter.AddListener<NoteClearEvent<SlideNote>>(OnNoteClear);
            EventCenter.AddListener<NoteMissEvent<SlideNote>>(OnNoteMiss);
        }

        private void OnDisable()
        {
            EventCenter.RemoveAllListener<NoteClearEvent<BlankNote>>();
            EventCenter.RemoveAllListener<NoteMissEvent<BlankNote>>();
        }
    }
}