using UnityEngine;
using UnityEditor;

public class Log : MonoBehaviour
{
    public static void LogPlatform()
    {
        Debug.Log(Application.platform);
    }

    public static void LogStr(string str)
    {
        if (Application.isEditor)
        {
            Debug.Log(str);
        }
    }
}