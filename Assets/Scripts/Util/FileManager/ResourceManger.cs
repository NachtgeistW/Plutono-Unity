/*
 * class ResourceManager -- Save and load resources (include charts, audios).
 *
 * Function
 *      void::RequestReadPermission -- request read permission in order to read files.
 *      string[]::GetAllLocalSongList -- scan specific file paths and detect the chart.
 *      string[]::InitializeApplication -- call the GetAllLocalSongList with different path by different platform.
 *
 * History
 *      2020.8.12 CREATE.
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Assets.Scripts.Game.Deemo;
using Assets.Scripts.Game.Plutono;
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
        public void RequestReadPermission()
        {
            if (Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead)) return;
            Debug.Log("RequestReadPermission");
            Permission.RequestUserPermission(Permission.ExternalStorageRead);
        }

        private List<string> GetAllLocalSongList(string path)
        {
            var songPathList = new List<string>();

            if (string.IsNullOrEmpty(path))
            {
                return null;
            }

            var bookPath = Directory.GetDirectories(path, "*", SearchOption.TopDirectoryOnly);
            foreach (var s in bookPath)
            {
                var songPath = Directory.GetDirectories(s, "*", SearchOption.TopDirectoryOnly);
                songPathList.AddRange(songPath);
            }
            return songPathList;
        }

        public List<string> InitializeApplication()
        {
            //Resource directory of DeemoDIY 2.2 and 3.2 
            //private static readonly string StoragePath = "/storage/emulated/0/DeemoDIY";
            const string sdCardPath = "/sdcard/DeemoDIY";

            //general
            string platformPath = Application.persistentDataPath + "/Plutono";

            Dictionary<string, SongModel> songDictionary = null;

            var songPathList = new List<string>();
            var songPackList = new List<PackInfo>();
            try
            {
                Log.LogPlatform();
                switch (Application.platform)
                {
                    case RuntimePlatform.WindowsEditor:
                        {
                            if (!Directory.Exists(platformPath))
                                Directory.CreateDirectory(platformPath);
                            if (Directory.Exists("C:\\Users\\night\\Desktop\\G2 Collection_vol.7"))
                            {
                                songPathList = GetAllLocalSongList("C:\\Users\\night\\Desktop\\G2 Collection_vol.7");
                            }
                            foreach (var filePath in songPathList)
                            {
                                var iniInfo = new IniInfo();
                                var iniPathArray = Directory.GetFiles(filePath, "*.ini");
                                foreach (var iniPath in iniPathArray)
                                {
                                    iniInfo = iniInfo.ReadIniConfig(iniPath);
                                }
                                songPackList.Add(iniInfo.IniToPackInfo(filePath));
                            }
                            return songPathList;
                        }
                    case RuntimePlatform.Android:
                        {
                            if (!Directory.Exists(platformPath))
                                Directory.CreateDirectory(platformPath);
                            if (Directory.Exists(sdCardPath))
                            {
                                songPathList = GetAllLocalSongList(sdCardPath);
                            }
                            foreach (var filePath in songPathList)
                            {
                                var iniInfo = new IniInfo();
                                var iniPathArray = Directory.GetFiles(filePath, "*.ini");
                                foreach (var iniPath in iniPathArray)
                                {
                                    iniInfo = iniInfo.ReadIniConfig(iniPath);
                                }
                                songPackList.Add(iniInfo.IniToPackInfo(filePath));
                            }
                            return songPathList;
                        }
                    case RuntimePlatform.IPhonePlayer:
                        {
                            if (!Directory.Exists(platformPath))
                            {
                                Directory.CreateDirectory(platformPath);
                            }
                            return GetAllLocalSongList(platformPath);
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