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
}
