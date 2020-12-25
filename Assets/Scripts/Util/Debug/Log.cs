﻿using UnityEngine;
using UnityEditor;

public class Log : MonoBehaviour
{
    public static void LogAndroid()
    {
        Debug.Log("Android");
    }
    public static void LogiOS()
    {
        Debug.Log("iOS");
    }
    public static void LogPlatform()
    {
        Debug.Log(Application.platform);
    }
}