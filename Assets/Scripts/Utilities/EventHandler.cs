using System;

public static class EventHandler
{
    public static event Action<string> TransitionEvent;
    public static void CallTransitionEvent(string sceneName) => TransitionEvent?.Invoke(sceneName);


    public static event Action BeforeSceneLoadedEvent;
    public static void CallBeforeSceneLoadedEvent() => BeforeSceneLoadedEvent?.Invoke();
    public static event Action AfterSceneLoadedEvent;
    public static void CallAfterSceneLoadedEvent() => AfterSceneLoadedEvent?.Invoke();

    //GamePlay Scene Event
    public static event Action<Plutono.Song.ChartDetail> InstantiateLevel;
    public static void CallInstantiateLevel(Plutono.Song.ChartDetail chartDetails) => InstantiateLevel?.Invoke(chartDetails);
    
    public static event Action GameStopEvent;
    public static void  CallGameStopEvent() => GameStopEvent?.Invoke();
    
    public static event Action GameResumeEvent;
    public static void  CallGameResumeEvent() => GameResumeEvent?.Invoke();
    
    public static event Action GameClearEvent;
    public static void  CallGameClearEvent() => GameClearEvent?.Invoke();

    public static event Action GameFailEvent;
    public static void  CallGameFailEvent() => GameFailEvent?.Invoke();

    public static event Action<Plutono.Song.Note, float, GameStatus> HitNoteEvent;
    public static void CallHitNoteEvent(Plutono.Song.Note note, float curGameTime, GameStatus status) => HitNoteEvent?.Invoke(note, curGameTime, status);
}
