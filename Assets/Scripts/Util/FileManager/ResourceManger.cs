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
using Assets.Scripts.Game.Song;
using UnityEngine;
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
        private static readonly string _platformPath = Application.persistentDataPath + "Charon";
        private static readonly string _unityEditorPath = Application.dataPath + "Charon";


        Dictionary<string, SongModel> songDictionary = null;

        private string[] GetAllLocalSongList(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }
            return Directory.GetDirectories(path, "*", SearchOption.AllDirectories);
        }

        public string[] InitializeApplication()
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                Log.LogAndroid();
                if (!Directory.Exists(_storagePath) &&
                    !Directory.Exists(_sdCardPath) &&
                    !Directory.Exists(_platformPath))
                {

                    Directory.CreateDirectory(_platformPath);
                }

                return GetAllLocalSongList(_storagePath);
            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                Log.LogiOS();
                if (!Directory.Exists(_platformPath))
                {
                    Directory.CreateDirectory(_platformPath);
                }
                return GetAllLocalSongList(_storagePath);

            }
            else if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                if (!Directory.Exists(_unityEditorPath))
                {
                    Directory.CreateDirectory(_unityEditorPath);
                }
                return GetAllLocalSongList(_storagePath);
            }
            else
                return null;
        }
    }
}