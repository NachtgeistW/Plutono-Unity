using UnityEngine;

public static class Settings
{
    //Note displaying
    public const float noteReturnTime = 5.0f;
    public const float frameSpeed = 0.025f;
    public const float circleSize = 1.0f;
    public const float circleTime = 0.5f;
    public const float waveSize = 4.0f;
    public const float waveHeight = 6.0f;
    public const float lightSize = 25.0f;
    public const float lightHeight = 40.0f;
    public const float waveIncTime = 0.05f;
    public const float waveDecTime = 0.5f;
    public const float lightIncTime = 8.0f / 60;
    public const float lightDecTime = 40.0f / 60;
    public static Color linkLineColor = new(1.0f, 0.913f, 0.529f);
    public const float minAlphaDif = 0.05f;

    public const float maximumNoteRange = 240.0f;
    public static float NoteFallTime(float chartPlaySpeed) //Maybe I'll change this into something more precise later...
    {
        var speed = chartPlaySpeed switch
        {
            0.5f => 10.0f,
            1.0f => 8.5f,
            1.5f => 7.0f,
            2.0f => 5.7f,
            2.5f => 4.6f,
            3.0f => 3.8f,
            3.5f => 3.2f,
            4.0f => 2.7f,
            4.5f => 2.2f,
            5.0f => 1.7f,
            5.5f => 1.3f,
            6.0f => 0.9f,
            6.5f => 0.6f,
            7.0f => 0.45f,
            7.5f => 0.35f,
            8.0f => 0.3f,
            8.5f => 0.25f,
            9.0f => 0.2f,
            9.5f => 0.15f,
            _ => 1.7f,
        };
        return speed / 180.0f * maximumNoteRange;
    }
    public static float perspectiveHorizontalScale = 7.5f;

    /// <summary>
    /// Settings related to Arbo(Deemo1) Mode
    /// </summary>
    public static class ArboMode
    {
        public const float perfectDeltaTime = 0.5f;
        public const float goodDeltaTime = 0.7f;
        public const float badDeltaTime = 1.0f;
    }

    /// <summary>
    /// Settings related to Stelo(Plutono) Mode
    /// </summary>
    public static class SteloMode
    {
        public const float perfectDeltaTime = 0.35f;
        public const float goodDeltaTime = 0.7f;
        public const float badDeltaTime = 1.0f;
    }
}
