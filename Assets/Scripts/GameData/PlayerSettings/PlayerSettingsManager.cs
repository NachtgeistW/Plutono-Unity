using System;
using System.IO;
using Plutono.Util;
using UnityEngine;

namespace Plutono
{
    public class PlayerSettingsManager : Singleton<PlayerSettingsManager>
    {
        [field: SerializeField] public GlobalPlayerSettings GSettings { get; protected set; }

        [Serializable]
        public class GlobalPlayerSettings
        {
            //Sudden+ setting
            [field: SerializeField] public bool IsSuddenOn { get; set; }

            //Range(0.1f, 1f)]
            [field: SerializeField] public float SuddenHeight { get; set; }

            //Latency
            //[Range(-3f, 3f)] 
            [field: SerializeField] public float GlobalChartOffset { get; set; }
            //[Range(-3f, 3f)] 
            [field: SerializeField] public float ChartMusicOffset { get; set; }

            //Language
            public Language language;

            //game theme
            //[Range(200, 3125)] 
            [field: SerializeField] public int DOTweenDefaultCapacity { get; set; }
            //[Range(50, 500)] 
            [field: SerializeField] public int NoteObjectpoolMaxSize { get; set; }
            //[Range(10, 100)] 
            [field: SerializeField] public int ExplosionAnimateObjectpoolMaxSize { get; set; }
        }

        public void LoadGlobalPlayerSettingsFromJson()
        {
            var playerSettingsPath = Application.persistentDataPath + "/PlayerSettings_Global.json";
            if (!File.Exists(playerSettingsPath)) return;
            try
            {
                GSettings = JsonFile<GlobalPlayerSettings>.FromPath(playerSettingsPath);
            }
            catch (Exception e)
            {
                Debug.Log("On SaveGlobalPlayerSettingsToJson in PlayerSettingsManager\n" +
                          "Got: " + e);
                Debug.Log(playerSettingsPath);
            }
        }

        public void SaveGlobalPlayerSettingsToJson()
        {
            var playerSettingsPath = Application.persistentDataPath + "/PlayerSettings_Global.json";
            try
            {
                File.WriteAllText(playerSettingsPath, JsonFile<GlobalPlayerSettings>.ToJson(GSettings));
            }
            catch (Exception e)
            {
                Debug.Log("On SaveGlobalPlayerSettingsToJson in PlayerSettingsManager\n" +
                          "Got: " + e);
            }
        }
    }
}