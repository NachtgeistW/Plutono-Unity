/*
 * History
 * 2020.03.31  CREATE.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Plutono.Song;
using UnityEngine;
using Plutono.Util;

namespace Plutono.Legacy
{

    /// <summary>
    /// Store the information from a ini file.
    [System.Serializable]
    public class IniDetail
    {
        public string SongName;        //song name.
        public string Artist;          //the composer of this song.
        public string ChartDesigner;   //the chart designer of this song.
        public uint? LevelEasy;          //the level of easy.
        public uint? LevelNormal;        //the level of normal.
        public uint? LevelHard;          //the level of hard.
        public string? LevelExtra;      //the level of extra. (Can be ASCII)
        public uint? LevelUltra;         //the level of extra. (Used in Deemo 2.2, equal to Extra)

        //  Note: if level = 0 / = null, the chart of this level is not exist.
        //        the rest data in ini wouldn't be convert.

        /// <summary>
        /// read ini info from a ini file.
        /// </summary>
        /// <param name="iniFilePath">string, the path of the ini file</param>
        /// <returns>a initialized IniInfo class</returns>
        public static IniDetail ReadIniFromPath(string iniFilePath)
        {
            //TODO: 判定ini路径是否合法
            var data = IniFile.FromPath(iniFilePath).GetSection("Song");
            var info = new IniDetail
            {
                SongName = data["Name"],
                Artist = data["Artist"],
                ChartDesigner = data["Noter"]
            };
            if (data["Easy"] != null)
                info.LevelEasy = uint.Parse(data["Easy"]);
            if (data["Normal"] != null)
                info.LevelNormal = uint.Parse(data["Normal"]);
            if (data["Hard"] != null)
                info.LevelHard = uint.Parse(data["Hard"]);
            if (data["Extra"] != null)
                info.LevelExtra = data["Extra"];
            if (data["Ultra"] != null)
                info.LevelUltra = uint.Parse(data["Ultra"]);
            return info;
        }
    }

    [System.Serializable]
    public class SongInfo
    {
        public readonly IniDetail IniInfo;
        public readonly List<ChartDetail> Charts = new();
        public Sprite? Cover;
        public string MusicPath;

        public SongInfo(string iniPath)
        {
            var iniFolderPath = Path.GetDirectoryName(iniPath);

            IniInfo = IniDetail.ReadIniFromPath(iniPath);
            MusicPath = Path.Combine(iniFolderPath, "music.mp3");

            // convert ini to a config file used in Plutono.
            Func<string, string?, ChartDetail?> loadChart = (difficulty, level) =>
            {
                if (level is null) return null;

                var path = Path.Combine(iniFolderPath, $"{difficulty}.json");
                ChartDetail chartDetail = new()
                {
                    level = level,
                    chartDesigner = IniInfo.ChartDesigner
                };
                IEnumerable<NoteDetail> loadJson(string path) =>
                    LegacyChartDetail.ToLegacyChartDetail(path).notes.Select(n => n.ToNoteDetail());
                    
                if (File.Exists(path)) chartDetail.noteDetails = loadJson(path).ToList();
                else
                {
                    var arr = path.ToCharArray();
                    arr[0] = char.ToUpper(arr[0]);
                    path = arr.ToString();

                    if (File.Exists(path)) chartDetail.noteDetails = loadJson(path).ToList();
                    else
                    {
                        Debug.LogWarning($"Level Easy in Song {IniInfo.SongName}, Level {difficulty} has defined a difficulty but doesn't provide a chart.");
                        return null;
                    }
                }

                return chartDetail;
            };

            {
                if (loadChart("easy", IniInfo.LevelEasy?.ToString()) is { } chart)
                    Charts.Add(chart);
            }
            {
                if (loadChart("normal", IniInfo.LevelNormal?.ToString()) is { } chart)
                    Charts.Add(chart);
            }
            {
                if (loadChart("hard", IniInfo.LevelHard?.ToString()) is { } chart)
                    Charts.Add(chart);
            }
            {
                if (loadChart("extra", IniInfo.LevelUltra?.ToString()) is { } chart)
                    Charts.Add(chart);
            }
            {
                if (loadChart("ultra", IniInfo.LevelUltra?.ToString()) is { } chart)
                    Charts.Add(chart);
            }

            var coverPath = Path.Combine(iniFolderPath, "cover.png");
            if (File.Exists(coverPath) && LoadTexture(coverPath) is { } spriteTexture)
                Cover = Sprite.Create(
                    spriteTexture,
                    new Rect(0, 0, spriteTexture.width, spriteTexture.height),
                    new Vector2(0, 0),
                    100.0f,
                    0,
                    SpriteMeshType.Tight
                );

            Debug.Log(IniInfo.SongName);
        }

        /// <summary>
        /// Load a PNG or JPG file from disk to a Texture2D
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns>The Texture2D. Returns null if load fails</returns>
        static Texture2D? LoadTexture(string filePath)
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