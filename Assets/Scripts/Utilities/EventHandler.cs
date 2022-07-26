using System;

public static class EventHandler
{
    public static event Action<string> TransitionEvent;
    public static void CallTransitionEvent(string sceneName)
    {
        TransitionEvent?.Invoke(sceneName);
    }

    //Note Event
    public static event Action<Plutono.Song.Note> HitNoteEvent;
    public static void CallHitNoteEvent(Plutono.Song.Note note)
    {
        HitNoteEvent?.Invoke(note);
    }

    public static event Action<Plutono.Song.ChartDetails> InstantiateLevel;
    public static void CallInstantiateLevel(Plutono.Song.ChartDetails chartDetails)
    {
        InstantiateLevel?.Invoke(chartDetails);
    }
}
