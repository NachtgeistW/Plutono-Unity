/*
 * class ResourceManager -- Save and load resources (include charts, audios).
 *
 *      This class has below function:
 *
 * History
 *      2020.8.12 Created.
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Assets.Scripts.Game.Song;
using UnityEditor;
using UnityEngine;
using UnityEngine.Android;
using Application = UnityEngine.Application;

namespace Assets.Scripts.Util.FileManager
{
    [Serializable]
    public class ResourceManger : MonoBehaviour
    {
        //Resource directory of DeemoDIY 2.2 and 3.2 
        private static readonly string _storagePath = "/storage/emulated/0/DeemoDIY";
        private static readonly string _sdCardPath = "/sdcard/DeemoDIY";

        //general
        private static readonly string _platformPath = Application.persistentDataPath + "/Charon";

        Dictionary<string, SongModel> songDictionary = null;

        public void RequestReadPermission()
        {
            Debug.Log("RequestReadPermission");
            if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead))
            {
                Permission.RequestUserPermission(Permission.ExternalStorageRead);
            }
        }

        private string[] GetAllLocalSongList(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }
            Debug.Log(Directory.GetFiles(path, "*.ini", SearchOption.AllDirectories));
            return Directory.GetDirectories(path, "*", SearchOption.AllDirectories); ;
        }

        public string[] InitializeApplication()
        {
            string[] vs = null;
            try
            {
                Log.LogPlatform();
                if (Application.platform == RuntimePlatform.Android)
                {
                    if (!Directory.Exists(_platformPath))
                        Directory.CreateDirectory(_platformPath);
                    if (Directory.Exists(_storagePath))
                    {
                        vs = GetAllLocalSongList(_storagePath);
                        foreach (var item in vs)
                        {
                            Debug.Log(item);
                        }
                    }
                    if (Directory.Exists(_sdCardPath))
                    {
                        var temp = GetAllLocalSongList(_sdCardPath);
                    }
                    return vs;
                }
                else if (Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    if (!Directory.Exists(_platformPath))
                    {
                        Directory.CreateDirectory(_platformPath);
                    }
                    return GetAllLocalSongList(_storagePath);

                }
                else
                    return null;
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
                return null;
            }
        }
    }
}