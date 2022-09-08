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
    public static event Action<List<Plutono.Song.NoteDetail>, List<Plutono.Song.Note>> InstantiateLevel;
    public static void CallInstantiateLevel(List<Plutono.Song.NoteDetail> noteDetails, List<Plutono.Song.Note> notesOnScreen) 
        => InstantiateLevel?.Invoke(noteDetails, notesOnScreen);

    public static event Action GameStopEvent;
    public static void  CallGameStopEvent() => GameStopEvent?.Invoke();
    
    public static event Action GameResumeEvent;
    public static void  CallGameResumeEvent() => GameResumeEvent?.Invoke();
    
    public static event Action GameClearEvent;
    public static void  CallGameClearEvent() => GameClearEvent?.Invoke();

    public static event Action GameFailEvent;
    public static void  CallGameFailEvent() => GameFailEvent?.Invoke();

    public static event Action<List<Plutono.Song.Note>, Plutono.Song.Note, float, GameStatus> HitNoteEvent;
    public static void CallHitNoteEvent(List<Plutono.Song.Note> notesOnScreen, Plutono.Song.Note note, float curGameTime, GameStatus status) 
        => HitNoteEvent?.Invoke(notesOnScreen, note, curGameTime, status);
    
    public static event Action<Plutono.Song.Note, float, GameStatus> MissNoteEvent;
    public static void CallMissNoteEvent(Plutono.Song.Note note, float curGameTime, GameStatus status) => MissNoteEvent?.Invoke(note, curGameTime, status);
}
