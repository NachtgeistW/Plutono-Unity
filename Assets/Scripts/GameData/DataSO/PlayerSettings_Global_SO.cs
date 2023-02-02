using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "Plutono/PlayerSettings", order = 0)]
public class PlayerSettings_Global_SO : ScriptableObject
{
    //Sudden+ setting
    public bool isSuddenOn;
    [Range(0f, 1f)]
    public float suddenHeight;

    //Latency
    [Range(-3f, 3f)] public float globalChartOffset;
    [Range(-3f, 3f)] public float chartMusicOffset;

    //Language
    public Language language;

    //game theme

    [Header("-Advance-")]
    public int DOTweenDefaultCapacity;
    public int NoteObjectpoolMaxSize;
    public int ExplosionAnimateObjectpoolMaxSize;
}