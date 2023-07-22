using Plutono.GamePlay.Notes;
using Plutono.Song;
using Plutono.Util;

namespace Plutono.Level.GamePlay
{
    public struct NoteClearEvent<TNote> : IEvent
        where TNote: BaseNote
    {
        public TNote Note;
        public NoteGrade Grade;
        public float DeltaXPos;
    }

    public struct NoteMissEvent<TNote> : IEvent
        where TNote : BaseNote
    {
        public TNote Note;
    }

    public struct GamePrepareEvent : IEvent { }
    public struct GameStartEvent : IEvent { }
    public struct GamePauseEvent : IEvent { }
    public struct GameResumeEvent : IEvent { }
    public struct GameFailEvent : IEvent { }
    public struct GameClearEvent : IEvent { }
}