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
using UnityEditor;
using UnityEngine;
using UnityEngine.Android;

using Application = UnityEngine.Application;

namespace Plutono.IO
{
    /// <summary>
    /// class LoadFiles -- Save and load resources (include charts, audios).
    /// </summary>
    public class LoadFiles
    {
        /// <summary>
        /// void::RequestReadPermission -- request read permission in order to read files.
        /// </summary>
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

        /// <summary>
        /// scan specific file paths and detect the chart.
        /// </summary>
        /// <param name="StoragePath"></param>
        /// <returns></returns>
        public List<Legacy.LegacySongDetail> LoadSongData(string StoragePath)
        {
#if UNITY_EDITOR
            if (string.IsNullOrEmpty(StoragePath))
            {
                StoragePath = Application.persistentDataPath;
            }
#else
            StoragePath = Application.platform switch
            {
                //RuntimePlatform.Android =>
                //    StoragePath = "/storage/emulated/0/DeemoDIY",
                _ => StoragePath = Application.persistentDataPath,
            };
            Debug.Log(Directory.Exists(StoragePath) + " " + StoragePath);
#endif
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

        public void LoadPlayerSettingsFromJson()
        {
            var playerSettingsPath = Application.persistentDataPath + "/PlayerSettings_Global.json";
            if (File.Exists(playerSettingsPath))
            {
                JsonUtility.FromJsonOverwrite(File.ReadAllText(playerSettingsPath), PlayerSettingsManager.Instance.PlayerSettings_Global_SO);
            }
        }

        public void SavePlayerSettingsToJson()
        {
            string json = JsonUtility.ToJson(PlayerSettingsManager.Instance.PlayerSettings_Global_SO);
            File.WriteAllText(Application.persistentDataPath + "/PlayerSettings_Global.json", json);
        }
    }
}