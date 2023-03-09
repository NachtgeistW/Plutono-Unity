public static class Settings
{
    //Note displaying
    public const float noteAnimationPlayingTime = 0.25f;
    public const float noteReturnTime = 5.0f;
    public const float frameSpeed = 0.025f;
    public const float circleSize = 1.0f;
    public const float circleTime = 0.5f;
    public const float waveSize = 4.0f;
    public const float waveHeight = 6.0f;
    public const float lightWidth = 25.0f;
    public const float lightHeight = 40.0f;
    public const float lightMaxScale = 40.0f;
    public const float lightIncTime = 8.0f / 60;
    public const float lightDecTime = 40.0f / 60;
    public const float waveIncTime = 0.05f;
    public const float waveDecTime = 0.5f;
    public static UnityEngine.Color linkLineColor = new(1.0f, 0.913f, 0.529f);
    public static UnityEngine.Color perfectLightColor = new(1.0f, 0.568f, 0f);
    public static UnityEngine.Color goodLightColor = new(1.0f, 0.913f, 0.529f);
    public static UnityEngine.Color badLightColor = new(1.0f, 0.913f, 0.529f);
    public const float minAlphaDif = 0.05f;

    public const float maximumNoteRange = 30f;
    public const float judgeLinePosition = 0f;
    public const float recycleNotePosition = -10f;
    public const float falldownSpeedRevision = 3f;

    public static float NoteFallTime(float chartPlaySpeed)
    {
        return (maximumNoteRange - judgeLinePosition) / (chartPlaySpeed * falldownSpeedRevision);
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
