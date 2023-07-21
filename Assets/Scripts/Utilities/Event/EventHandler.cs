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
}
