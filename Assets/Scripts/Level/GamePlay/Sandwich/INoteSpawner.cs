using Plutono.GamePlay.Notes;
using Plutono.Song;

namespace Plutono.Level.GamePlay
{
    public interface INoteSpawner<out TNote> where TNote : BaseNote
    {
        public TNote SpawnNotes(NoteDetail noteDetail);
    }
}