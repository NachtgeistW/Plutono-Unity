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
        //private static readonly string StoragePath = "/storage/emulated/0/DeemoDIY";
        private static readonly string SdCardPath = "/sdcard/DeemoDIY";

        //general
        private static readonly string PlatformPath = Application.persistentDataPath + "/Plutono";

        Dictionary<string, SongModel> songDictionary = null;

        public void RequestReadPermission()
        {
            if (Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead)) return;
            Debug.Log("RequestReadPermission");
            Permission.RequestUserPermission(Permission.ExternalStorageRead);
        }

        private string[] GetAllLocalSongList(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }

            var d = Directory.GetDirectories(path, "*", SearchOption.TopDirectoryOnly);
            foreach (var variable in d)
            {
                Debug.Log(variable);
            }
            return Directory.GetDirectories(path, "*", SearchOption.TopDirectoryOnly); ;
        }

        public string[] InitializeApplication()
        {
            string[] vs = null;
            try
            {
                Log.LogPlatform();
                switch (Application.platform)
                {
                    case RuntimePlatform.Android:
                    {
                        if (!Directory.Exists(PlatformPath))
                            Directory.CreateDirectory(PlatformPath);
/*                        if (Directory.Exists(StoragePath))
                        {
                            vs = GetAllLocalSongList(StoragePath);
                        }
*/                        
                        if (Directory.Exists(SdCardPath))
                        {
                            vs = GetAllLocalSongList(SdCardPath);
                        }
                        return vs;
                    }
                    case RuntimePlatform.IPhonePlayer:
                    {
                        if (!Directory.Exists(PlatformPath))
                        {
                            Directory.CreateDirectory(PlatformPath);
                        }
                        return GetAllLocalSongList(PlatformPath);
                    }
                    default:
                        return null;
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
                return null;
            }
        }
    }
}