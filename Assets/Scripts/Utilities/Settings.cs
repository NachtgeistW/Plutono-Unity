using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings
{
    //Note displaying
    public static float noteReturnTime = 5.0f;
    public static float frameSpeed = 0.025f;
    public static float circleSize = 1.0f;
    public static float circleTime = 0.5f;
    public static float waveSize = 4.0f;
    public static float waveHeight = 6.0f;
    public static float lightSize = 25.0f;
    public static float lightHeight = 40.0f;
    public static float waveIncTime = 0.05f;
    public static float waveDecTime = 0.5f;
    public static float lightIncTime = 8.0f / 60;
    public static float lightDecTime = 40.0f / 60;
    public static Color linkLineColor = new Color(1.0f, 233 / 255.0f, 135 / 255.0f);
    public static float minAlphaDif = 0.05f;

    public static float maximumNoteRange = 240.0f;
    public static float NoteFallTime(int chartPlaySpeed) //Maybe I'll change this into something more precise later...
    {
        float speed;
        switch (chartPlaySpeed)
        {
            case 1: speed = 10.0f; break;
            case 2: speed = 8.5f; break;
            case 3: speed = 7.0f; break;
            case 4: speed = 5.7f; break;
            case 5: speed = 4.6f; break;
            case 6: speed = 3.8f; break;
            case 7: speed = 3.2f; break;
            case 8: speed = 2.7f; break;
            case 9: speed = 2.2f; break;
            case 10: speed = 1.7f; break;
            case 11: speed = 1.3f; break;
            case 12: speed = 0.9f; break;
            case 13: speed = 0.6f; break;
            case 14: speed = 0.45f; break;
            case 15: speed = 0.35f; break;
            case 16: speed = 0.3f; break;
            case 17: speed = 0.25f; break;
            case 18: speed = 0.2f; break;
            case 19: speed = 0.15f; break;
            default: speed = 1.7f; break;
        }
        return speed / 180.0f * maximumNoteRange;
    }

}
