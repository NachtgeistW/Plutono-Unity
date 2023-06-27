/*
 * History
 * 2020.03.31  CREATE.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Plutono.Song;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEngine;
using UnityEngine.Serialization;

namespace Plutono.Legacy
{
#nullable enable
    [Serializable]
    public class LegacySongDetail
    {
        public readonly IniDetail IniInfo;
        public readonly List<ChartDetail> ChartDetails = new();
        [FormerlySerializedAs("Cover")] public Sprite? cover;
        [FormerlySerializedAs("MusicPath")] public string musicPath;

        public LegacySongDetail(string iniPath)
        {
            var iniFolderPath = Path.GetDirectoryName(iniPath);

            IniInfo = IniDetail.ReadIniFromPath(iniPath);
            musicPath = Path.Combine(iniFolderPath, "music.mp3");

            // convert ini to a config file used in Plutono.
            // Also convert legacyChart to chartDetail.
            ChartDetail? LoadChart(string difficulty, string? level)
            {
                if (level is null) return null;

                var path = Path.Combine(iniFolderPath, $"{difficulty}.json");
                ChartDetail chartDetail = new()
                {
                    level = level,
                    chartDesigner = IniInfo.ChartDesigner
                };

                static IEnumerable<NoteDetail> LoadJson(string path) =>
                    LegacyChartDetail.JsonToLegacyChartDetail(path).ToNoteDetailList();

                if (File.Exists(path))
                {
                    chartDetail.noteDetails = LoadJson(path).OrderBy(n => n.time).ToList();
                    var chartContent = File.ReadAllText(path);
                    var id = IniInfo.SongName + IniInfo.Artist + difficulty + IniInfo.ChartDesigner + chartContent;
                    chartDetail.id = Md5HashString(id);
                }
                else
                {
                    var arr = path.ToCharArray();
                    arr[0] = char.ToUpper(arr[0]);
                    path = arr.ToString();

                    if (File.Exists(path))
                    {
                        chartDetail.noteDetails = LoadJson(path).OrderBy(n => n.time).ToList();
                        var chartContent = File.ReadAllText(path);
                        var id = IniInfo.SongName + IniInfo.Artist + difficulty + IniInfo.ChartDesigner + chartContent;
                        chartDetail.id = Md5HashString(id);
                    }
                    else
                    {
                        Debug.LogWarning($"Level {difficulty} in Song {IniInfo.SongName} has defined a difficulty but doesn't provide a chart.");
                        return null;
                    }
                }

                return chartDetail;
            }

            {
                if (LoadChart("easy", IniInfo.LevelEasy?.ToString()) is { } chart)
                    ChartDetails.Add(chart);
            }
            {
                if (LoadChart("normal", IniInfo.LevelNormal?.ToString()) is { } chart)
                    ChartDetails.Add(chart);
            }
            {
                if (LoadChart("hard", IniInfo.LevelHard?.ToString()) is { } chart)
                    ChartDetails.Add(chart);
            }
            {
                if (LoadChart("extra", IniInfo.LevelUltra?.ToString()) is { } chart)
                    ChartDetails.Add(chart);
            }
            {
                if (LoadChart("ultra", IniInfo.LevelUltra?.ToString()) is { } chart)
                    ChartDetails.Add(chart);
            }

            try
            {
                var coverPath = Path.Combine(iniFolderPath ?? throw new InvalidOperationException(), "cover.png");
                if (File.Exists(coverPath) && Util.Texture.LoadTexture(coverPath) is { } spriteTexture)
                    cover = Sprite.Create(
                        spriteTexture,
                        new Rect(0, 0, spriteTexture.width, spriteTexture.height),
                        new Vector2(0, 0),
                        100.0f,
                        0,
                        SpriteMeshType.Tight
                    );
            }
            catch (Exception)
            {
                Debug.LogError($"Failed to load cover for {IniInfo.SongName}.");
            }
        }

        public static string Md5HashString(string value)
        {
            var sb = new StringBuilder();

            using (var hash = MD5.Create())
            {
                var enc = Encoding.UTF8;
                var result = hash.ComputeHash(enc.GetBytes(value));

                foreach (var b in result)
                    sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }
    }
}