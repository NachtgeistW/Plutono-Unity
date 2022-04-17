/*
 * class IniInfo --
 *
 *
 * Function
 IniInfo::ReadIniFromPath --
 PackInfo::IniToPackInfo --
 *
 * History
 2020.03.31  CREATE.
 */

using System;
using System.IO;

using Assets.Scripts.Model.Deemo;
using Assets.Scripts.Model.Plutono;
using Assets.Scripts.Util;

using Model.Plutono;

using UnityEngine;

namespace Model.Deemo
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Assets.Scripts.Model.Deemo;
    using Assets.Scripts.Model.Plutono;
    using Assets.Scripts.Util;

    using Plutono;

    using UnityEngine;

    /// <summary>
    /// Store the information from a ini file.
    public class SongIniInfo
    {
        public string songName = "";        //song name.
        public string artist = "";          //the composer of this song.
        public string chartDesigner = "";   //the chart designer of this song.
        public uint levelEasy;          //the level of easy.
        public uint levelNormal;        //the level of normal.
        public uint levelHard;          //the level of hard.
        public string levelExtra = "";      //the level of extra. (Can be ASCII)
        public uint levelUltra;         //the level of extra. (Used in Deemo 2.2, equal to Extra)

        //  Note: if level = 0 / = null, the chart of this level is not exist.
        //        the rest data in ini wouldn't be convert.

        /// <summary>
        /// read ini info from a ini file.
        /// </summary>
        /// <param name="iniFilePath"></param>
        /// <returns>a initialized IniInfo class</returns>
        public static SongIniInfo ReadIniFromPath(string iniFilePath)
        {
            //TODO: 判定ini路径是否合法
            var data = IniFile.FromPath(iniFilePath).GetSection("Song");
            var info = new SongIniInfo
            {
                songName = data["Name"],
                artist = data["Artist"],
                chartDesigner = data["Noter"]
            };
            if (data["Easy"] != null)
                info.levelEasy = uint.Parse(data["Easy"]);
            if (data["Normal"] != null)
                info.levelNormal = uint.Parse(data["Normal"]);
            if (data["Hard"] != null)
                info.levelHard = uint.Parse(data["Hard"]);
            if (data["Extra"] != null)
                info.levelExtra = data["Extra"];
            if (data["Ultra"] != null)
                info.levelUltra = uint.Parse(data["Ultra"]);
            return info;
        }

        /// <summary>
        /// convert ini to a config file used in Plutono.
        /// </summary>
        /// <param name="iniDocumentPath">string, the path of the ini file</param>
        /// <returns>A completed PackInfo, including a GameChart and their level and designer respectively</returns>
        public SongInfo IniToSongInfo(string iniDocumentPath)
        {
            var packInfo = new SongInfo { SongName = songName, Composer = artist, MusicPath = Path.Combine(iniDocumentPath, "music.mp3") };

            Func<string, GameChartModel> loadChart = difficulty =>
            {
                var path = Path.Combine(iniDocumentPath, $"{difficulty}.json");
                GameChartModel gChart = new()
                {
                    level = levelNormal.ToString(),
                    chartDesigner = chartDesigner
                };
                IEnumerable<GameNoteModel> loadJson(string path) =>
                    JsonChartModel.JsonToJsonChart(path).ToGameChartNoteList();

                if (File.Exists(path)) gChart.notes = loadJson(path).ToList();
                else
                {
                    var arr = path.ToCharArray();
                    arr[0] = char.ToUpper(arr[0]);
                    path = arr.ToString();

                    if (File.Exists(path)) gChart.notes = loadJson(path).ToList();
                    else
                    {
                        Debug.LogWarning($"Level Easy in Song {songName}, Level {difficulty} has defined a difficulty but doesn't provide a chart.");
                        return null;
                    }
                }


                return gChart;
            };

            if (levelEasy != 0 && loadChart("easy") is { } chart)
                packInfo.Charts.Add(chart);

            if (levelNormal != 0 && (chart = loadChart("normal")) is { })
                packInfo.Charts.Add(chart);

            if (levelHard != 0 && (chart = loadChart("hard")) is { })
                packInfo.Charts.Add(chart);

            if (string.IsNullOrEmpty(levelExtra) && (chart = loadChart("hard")) is { })
                packInfo.Charts.Add(chart);

            if (levelUltra != 0 && (chart = loadChart("ultra")) is { })
                packInfo.Charts.Add(chart);

            if (File.Exists(iniDocumentPath + "/cover.png") &&
                LoadTexture(iniDocumentPath + "/cover.png") is { } spriteTexture)
                packInfo.Cover = Sprite.Create(
                    spriteTexture,
                    new Rect(0, 0, spriteTexture.width, spriteTexture.height),
                    new Vector2(0, 0),
                    100.0f,
                    0,
                    SpriteMeshType.Tight
                );

            Log.LogStr(packInfo.SongName);
            return packInfo;
        }

        /// <summary>
        /// Load a PNG or JPG file from disk to a Texture2D
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns>The Texture2D. Returns null if load fails</returns>
        public static Texture2D? LoadTexture(string filePath)
        {
            if (File.Exists(filePath))
            {
                Texture2D tex2D = new(0, 0);           // Create new "empty" texture
                if (tex2D.LoadImage(File.ReadAllBytes(filePath)))           // Load the imagedata into the texture (size is set automatically)
                    return tex2D;                 // If data = readable -> return texture
            }

            return null;                     // Return null if load failed
        }
    }
}