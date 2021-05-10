/*
 * class ResourceManager -- Save and load resources (include charts, audios).
 *
 * Function
 *      void::RequestReadPermission -- request read permission in order to read files.
 *      List<string>::GetAllLocalSongList -- scan specific file paths and detect the chart.
 *      List<string>::InitializeApplication -- call the GetAllLocalSongList with different path by different platform.
 *
 * History
 *      2020.08.12  CREATE.
 *      2021.04.03  ADD InitializeApplication function
 *      2021.04.07  CHANGE the platformPath
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Assets.Scripts.Model.Plutono;
using Model.Deemo;
using Model.Plutono;
using UnityEngine;
using UnityEngine.Android;
using Application = UnityEngine.Application;

namespace Util.FileManager
{
    [Serializable]
    public class ResourceManger
    {
        public void RequestReadPermission()
        {
            if (Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead)) return;
            Debug.Log("RequestReadPermission");
            Permission.RequestUserPermission(Permission.ExternalStorageRead);
        }

        private List<string> GetAllIniPathList(string path)
        {

            if (string.IsNullOrEmpty(path))
            {
                return null;
            }

            var iniPathList = new List<string>();
            var bookPath = Directory.GetDirectories(path, "*", SearchOption.TopDirectoryOnly);
            foreach (var s in bookPath)
            {
                var iniPath = Directory.GetDirectories(s, "*", SearchOption.TopDirectoryOnly);
                iniPathList.AddRange(iniPath);
            }
            return iniPathList;
        }

        public List<PackInfo> InitializeApplication()
        {
            //Resource directory of DeemoDIY 2.2 and 3.2 
            //private static readonly string StoragePath = "/storage/emulated/0/DeemoDIY";
            const string sdCardPath = "/sdcard/DeemoDIY";

            //general
            const string platformPath = "/sdcard/Plutono";

            //Dictionary<string, SongModel> songDictionary = null;
            var songPackList = new List<PackInfo>();
            var iniPathList = new List<string>();
            try
            {
                Log.LogPlatform();
                switch (Application.platform)
                {
                    //TODO::将case分支里面的重复代码抽取出来
                    case RuntimePlatform.WindowsEditor:
                        {
                            if (!Directory.Exists(platformPath))
                                Directory.CreateDirectory(platformPath);
                            if (Directory.Exists("C:\\Users\\night\\Desktop\\G2 Collection_vol.7"))
                            {
                                iniPathList = GetAllIniPathList("C:\\Users\\night\\Desktop\\G2 Collection_vol.7");
                                GameManager.Instance.songPathList = iniPathList;
                            }

                            /*foreach (var iniPath in iniPathList)
                              {
                                songPackList.Add(
                                IniInfo.ReadIniFromPath(Directory.GetFiles(iniPath, "*.ini").Single()).IniToPackInfo(iniPath));
                              }
                            */
                            songPackList.AddRange(iniPathList.Select(iniPath => 
                                IniInfo.ReadIniFromPath(Directory.GetFiles(iniPath, "*.ini").Single()).IniToPackInfo(iniPath)));
                            return songPackList;
                        }
                    case RuntimePlatform.Android:
                        {
                            if (!Directory.Exists(platformPath))
                                Directory.CreateDirectory(platformPath);
                            if (Directory.Exists(sdCardPath))
                            {
                                iniPathList = GetAllIniPathList(sdCardPath);
                                GameManager.Instance.songPathList = iniPathList;
                            }

                            songPackList.AddRange(iniPathList.Select(iniPath => 
                                IniInfo.ReadIniFromPath(Directory.GetFiles(iniPath, "*.ini").Single()).IniToPackInfo(iniPath)));
                            return songPackList;
                        }
                    case RuntimePlatform.IPhonePlayer:
                        {
                            if (!Directory.Exists(platformPath))
                            {
                                Directory.CreateDirectory(platformPath);
                            }
                            return null;
                        }
                    default:
                        throw new NotSupportedException();
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