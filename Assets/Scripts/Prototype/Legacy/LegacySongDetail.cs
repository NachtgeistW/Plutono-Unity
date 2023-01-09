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

namespace Plutono.Legacy
{
#nullable enable
    [System.Serializable]
    public class LegacySongDetail
    {
        public readonly IniDetail IniInfo;
        public readonly List<ChartDetail> ChartDetails = new();
        public Sprite? Cover;
        public string MusicPath;

        public LegacySongDetail(string iniPath)
        {
            var iniFolderPath = Path.GetDirectoryName(iniPath);

            IniInfo = IniDetail.ReadIniFromPath(iniPath);
            MusicPath = Path.Combine(iniFolderPath, "music.mp3");

            // convert ini to a config file used in Plutono.
            // Also convert legacyChart to chartDetail.
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
                    LegacyChartDetail.JsonToLegacyChartDetail(path).ToNoteDetailList();
                
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
                    ChartDetails.Add(chart);
            }
            {
                if (loadChart("normal", IniInfo.LevelNormal?.ToString()) is { } chart)
                    ChartDetails.Add(chart);
            }
            {
                if (loadChart("hard", IniInfo.LevelHard?.ToString()) is { } chart)
                    ChartDetails.Add(chart);
            }
            {
                if (loadChart("extra", IniInfo.LevelUltra?.ToString()) is { } chart)
                    ChartDetails.Add(chart);
            }
            {
                if (loadChart("ultra", IniInfo.LevelUltra?.ToString()) is { } chart)
                    ChartDetails.Add(chart);
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
                if (tex2D.LoadImage(File.ReadAllBytes(filePath)))           // Load the image data into the texture (size is set automatically)
                    return tex2D;                 // If data = readable -> return texture
            }

            return null;                     // Return null if load failed
        }
    }
#nullable disable
}