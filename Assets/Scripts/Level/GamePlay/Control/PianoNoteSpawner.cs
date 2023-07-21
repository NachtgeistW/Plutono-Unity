using Plutono.GamePlay.Notes;
using Plutono.Level.GamePlay;
using Plutono.Song;
using Plutono.Util;

namespace Plutono.GamePlay.Control
{
    public class PianoNoteSpawner : NoteSpawner<PianoNote>
    {
        private void OnEnable()
        {
            EventCenter.AddListener<NoteClearEvent<PianoNote>>(OnNoteClear);
            EventCenter.AddListener<NoteMissEvent<PianoNote>>(OnNoteMiss);
        }

        private void OnDisable()
        {
            EventCenter.RemoveAllListener<NoteClearEvent<PianoNote>>();
            EventCenter.RemoveAllListener<NoteMissEvent<PianoNote>>();
        }
    }
}