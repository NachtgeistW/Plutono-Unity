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
        [Tooltip("//Resource directory of DeemoDIY 2.2 and 3.2")]
        public string StoragePath;

        public void RequestReadPermission()
        {
            if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead))
            {
                Debug.Log("RequestReadPermission");
                Permission.RequestUserPermission(Permission.ExternalStorageRead);
            }

            Permission.RequestUserPermission("android.permission.INTERNET");
        }

        public List<SongInfo> Initialize()
        {
            Log.LogPlatform();

            if (string.IsNullOrEmpty(StoragePath))
                StoragePath = Application.platform switch
                {
                    RuntimePlatform.Android =>
                       StoragePath = "/storage/emulated/0/DeemoDIY",
                    _ => StoragePath = Application.persistentDataPath,
                };

            if (Directory.Exists(StoragePath))
            {
                return Directory.GetDirectories(StoragePath) // get song packs
                    .SelectMany(
                        packPath => Directory.GetDirectories(packPath)
                        .Select(songPath => Directory.GetFiles(songPath, "*.ini"))
                        .SelectMany(iniPaths => iniPaths.Select(ini => SongIniInfo.ReadIniFromPath(ini).IniToSongInfo(packPath)))
                    ) // get song info
                    .ToList();
            }
            else
            {
                Debug.LogWarning("Storage path not found");
                return new();
            }
        }
    }
}