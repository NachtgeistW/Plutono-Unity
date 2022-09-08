using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "Plutono/PlayerSettings", order = 0)]
public class PlayerSettings_Global_SO : ScriptableObject
{
    //Sudden+ setting
    public bool isSuddenOn;
    [Range(0f, 1f)]
    public float suddenHeight;

    //Latency
    [Range(-3f, 3f)]
    public float musicOffset;
    [Range(-0.3f, 3f)]
    public float touchOffset;

    //Language
    public Language language;

    //game theme
}