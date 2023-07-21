using Plutono.GamePlay.Notes;
using Plutono.Level.GamePlay;
using Plutono.Song;
using Plutono.Util;

namespace Plutono.GamePlay.Control
{
    public class BlankNoteSpawner : NoteSpawner<BlankNote>
    {
        private void OnEnable()
        {
            EventCenter.AddListener<NoteClearEvent<BlankNote>>(OnNoteClear);
            EventCenter.AddListener<NoteMissEvent<BlankNote>>(OnNoteMiss);
        }

        private void OnDisable()
        {
            EventCenter.RemoveAllListener<NoteClearEvent<BlankNote>>();
            EventCenter.RemoveAllListener<NoteMissEvent<BlankNote>>();
        }
    }
}