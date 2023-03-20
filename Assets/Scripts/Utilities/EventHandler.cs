using System;
using System.Collections.Generic;

public static class EventHandler
{
    public static event Action<string> TransitionEvent;
    public static void CallTransitionEvent(string sceneName) => TransitionEvent?.Invoke(sceneName);


    public static event Action BeforeSceneLoadedEvent;
    public static void CallBeforeSceneLoadedEvent() => BeforeSceneLoadedEvent?.Invoke();
    public static event Action AfterSceneLoadedEvent;
    public static void CallAfterSceneLoadedEvent() => AfterSceneLoadedEvent?.Invoke();

    //GamePlay Scene Event
    public static event Action<List<Plutono.Song.NoteDetail>, List<Plutono.Song.Note>> InstantiateNoteEvent;
    public static void CallInstantiateNote(List<Plutono.Song.NoteDetail> noteDetails, List<Plutono.Song.Note> notesOnScreen) 
        => InstantiateNoteEvent?.Invoke(noteDetails, notesOnScreen);

    public static event Action GameStartEvent;
    public static void CallGameStartEvent() => GameStartEvent?.Invoke();
    
    public static event Action GamePauseEvent;
    public static void CallGamePauseEvent() => GamePauseEvent?.Invoke();
    
    public static event Action GameResumeEvent;
    public static void  CallGameResumeEvent() => GameResumeEvent?.Invoke();
    
    public static event Action GameRestartEvent;
    public static void CallGameRestartEvent() => GameRestartEvent?.Invoke();
    
    public static event Action GameClearEvent;
    public static void  CallGameClearEvent() => GameClearEvent?.Invoke();

    public static event Action GameFailEvent;
    public static void  CallGameFailEvent() => GameFailEvent?.Invoke();

    public static event Action<List<Plutono.Song.Note>, Plutono.Song.Note, double, Plutono.Song.NoteGrade> HitNoteEvent;
    public static void CallHitNoteEvent(List<Plutono.Song.Note> notesOnScreen, Plutono.Song.Note note, double curGameTime, Plutono.Song.NoteGrade noteGrade) 
        => HitNoteEvent?.Invoke(notesOnScreen, note, curGameTime, noteGrade);
    public static event Action<List<Plutono.Song.Note>, Plutono.Song.Note, double, Plutono.Song.NoteGrade> MissNoteEvent;
    public static void CallMissNoteEvent(List<Plutono.Song.Note> notesOnScreen, Plutono.Song.Note note, double curGameTime, Plutono.Song.NoteGrade noteGrade) 
        => MissNoteEvent?.Invoke(notesOnScreen, note, curGameTime, noteGrade);
}
