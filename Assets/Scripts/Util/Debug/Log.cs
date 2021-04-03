using UnityEngine;
using UnityEditor;

public class Log : MonoBehaviour
{
    public static void LogPlatform()
    {
        Debug.Log(Application.platform);
    }
}