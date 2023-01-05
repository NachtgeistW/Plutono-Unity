// /*
//  * Function
//  *      List<string>::InitializeApplication -- call the GetAllLocalSongList with different path by different platform.
//  *
//  * History
//  *      2020.08.12  CREATE.
//  *      2021.04.03  ADD InitializeApplication function
//  *      2021.04.07  CHANGE the platformPath
//  */

using System.Collections.Generic;
using System.IO;
using System.Linq;

using UnityEngine;
using UnityEngine.Android;

using Application = UnityEngine.Application;

namespace Plutono.IO
{
    //class LoadFiles -- Save and load resources (include charts, audios).
    public class LoadFiles
    {
        //void::RequestReadPermission -- request read permission in order to read files.
        [Tooltip("//Resource directory of DeemoDIY 2.2 and 3.2")]
        public void RequestReadPermission()
        {
            if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead))
            {
                Debug.Log("RequestReadPermission");
                Permission.RequestUserPermission(Permission.ExternalStorageRead);
            }

            Permission.RequestUserPermission("android.permission.INTERNET");
        }

        //List<Legacy.LegacySongDetail> Initialize -- scan specific file paths and detect the chart.
        public List<Legacy.LegacySongDetail> Initialize(string StoragePath)
        {
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
                            .Select(
                                songPath => Directory.GetFiles(songPath, "*.ini"))
                            .SelectMany(iniPaths => iniPaths.Select(ini => new Legacy.LegacySongDetail(ini)))
                    ) // get song detail
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