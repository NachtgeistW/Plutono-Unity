using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "Plutono/PlayerSettings", order = 0)]
public class PlayerSettings_SO : ScriptableObject 
{
    //Sudden+
    public bool isSuddenOn;
    [Range(0, 1)]
    public float suddenHeight;

    //Latency
    [Range(-3f, 3f)]
    public float audioOffset;
    [Range(-0.3f, 3f)]
    public float touchOffset;

    //Language
    public Language language;
}