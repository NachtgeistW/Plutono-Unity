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

    //latency
    public float musicOffset;
    public float touchOffset;

    //language
    public Language language;

    //game theme
}