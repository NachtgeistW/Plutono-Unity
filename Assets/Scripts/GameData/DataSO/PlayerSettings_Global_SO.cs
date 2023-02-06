using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "Plutono/PlayerSettings", order = 0)]
public class PlayerSettings_Global_SO : ScriptableObject
{
    //Sudden+ setting
    public bool isSuddenOn;
    [Range(0.1f, 1f)] public float suddenHeight;

    //Latency
    [Range(-3f, 3f)] public float globalChartOffset;
    [Range(-3f, 3f)] public float chartMusicOffset;

    //Language
    public Language language;

    //game theme

    [Header("-Advance-")]
    [Range(200, 3125)] public int DOTweenDefaultCapacity;
    [Range(50, 500)] public int NoteObjectpoolMaxSize;
    [Range(10, 100)] public int ExplosionAnimateObjectpoolMaxSize;
}