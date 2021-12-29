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

using System.IO;

using Assets.Scripts.Model.Deemo;
using Assets.Scripts.Model.Plutono;
using Assets.Scripts.Util;

using Model.Plutono;

using UnityEngine;

namespace Model.Deemo
{
    /// <summary>
    /// Store the information from a ini file.
    public class IniInfo
    {
        public string songName = "";        //song name.
        public string artist = "";          //the composer of this song.
        public string chartDesigner = "";   //the chart designer of this song.
        public uint levelEasy = 0;          //the level of easy.
        public uint levelNormal = 0;        //the level of normal.
        public uint levelHard = 0;          //the level of hard.
        public string levelExtra = "";      //the level of extra. (Can be ASCII)
        public uint levelUltra = 0;         //the level of extra. (Used in Deemo 2.2, equal to Extra)

        //  Note: if level = 0 / = null, the chart of this level is not exist.
        //        the rest data in ini wouldn't be convert.

        /// <summary>
        /// read ini info from a ini file.
        /// </summary>
        /// <param name="iniFilePath"></param>
        /// <returns>a initialized IniInfo class</returns>
        public static IniInfo ReadIniFromPath(string iniFilePath)
        {
            //TODO: 判定ini路径是否合法
            var data = IniFile.FromPath(iniFilePath).GetSection("Song");
            var info = new IniInfo
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
        public PackInfo IniToPackInfo(string iniDocumentPath)
        {
            //TODO::将if分支里面的重复代码抽取出来
            try
            {

            }
            catch (System.Exception)
            {

                throw;
            }
            var packInfo = new PackInfo { songName = songName, composer = artist };
            if (levelEasy != 0)
            {
                GameChartModel gChart = new GameChartModel();
                gChart.level = levelEasy.ToString();
                gChart.chartDesigner = chartDesigner;

                var filePathUpper = iniDocumentPath + "/Easy.json";
                var filePathLower = iniDocumentPath + "/easy.json";
                if (File.Exists(filePathLower))
                    gChart.notes = JsonChartModel.JsonToJsonChart(filePathLower).ToGameChartNoteList();
                else if (File.Exists(filePathUpper))
                    gChart.notes = JsonChartModel.JsonToJsonChart(filePathUpper).ToGameChartNoteList();
                else
                    throw new System.Exception("Level Easy in Song " + songName + ", Level" + levelEasy.ToString() + "has defined a difficulty but doesn't provide a chart.");

                packInfo.charts.Add(gChart);
            }
            if (levelNormal != 0)
            {
                GameChartModel gChart = new GameChartModel();
                gChart.level = levelNormal.ToString();
                gChart.chartDesigner = chartDesigner;

                var filePathUpper = iniDocumentPath + "/Normal.json";
                var filePathLower = iniDocumentPath + "/normal.json";
                if (File.Exists(filePathLower))
                    gChart.notes = JsonChartModel.JsonToJsonChart(filePathLower).ToGameChartNoteList();
                else if (File.Exists(filePathUpper))
                    gChart.notes = JsonChartModel.JsonToJsonChart(filePathUpper).ToGameChartNoteList();
                else
                    throw new System.Exception("Level Easy in Song " + songName + ", Level" + levelNormal.ToString() + "has defined a difficulty but doesn't provide a chart.");

                packInfo.charts.Add(gChart);
            }
            if (levelHard != 0)
            {
                GameChartModel gChart = new GameChartModel();
                gChart.level = levelHard.ToString();
                gChart.chartDesigner = chartDesigner;

                var filePathUpper = iniDocumentPath + "/Hard.json";
                var filePathLower = iniDocumentPath + "/hard.json";
                if (File.Exists(filePathLower))
                    gChart.notes = JsonChartModel.JsonToJsonChart(filePathLower).ToGameChartNoteList();
                else if (File.Exists(filePathUpper))
                    gChart.notes = JsonChartModel.JsonToJsonChart(filePathUpper).ToGameChartNoteList();
                else
                    throw new System.Exception("Level Hard in Song " + songName + ", Level" + levelHard.ToString() + "has defined a difficulty but doesn't provide a chart.");

                packInfo.charts.Add(gChart);
            }
            if (levelExtra != "")
            {
                GameChartModel gChart = new GameChartModel();
                gChart.level = levelExtra.ToString();
                gChart.chartDesigner = chartDesigner;

                var filePathUpper = iniDocumentPath + "/Extra.json";
                var filePathLower = iniDocumentPath + "/extra.json";
                if (File.Exists(filePathLower))
                    gChart.notes = JsonChartModel.JsonToJsonChart(filePathLower).ToGameChartNoteList();
                else if (File.Exists(filePathUpper))
                    gChart.notes = JsonChartModel.JsonToJsonChart(filePathUpper).ToGameChartNoteList();
                else
                    throw new System.Exception("Level Extra in Song " + songName + ", Level" + levelExtra.ToString() + "has defined a difficulty but doesn't provide a chart.");

                packInfo.charts.Add(gChart);
            }
            if (levelUltra != 0)
            {
                GameChartModel gChart = new GameChartModel();
                gChart.level = levelExtra.ToString();
                gChart.chartDesigner = chartDesigner;

                var filePathUpper = iniDocumentPath + "/Ultra.json";
                var filePathLower = iniDocumentPath + "/ultra.json";
                if (File.Exists(filePathLower))
                    gChart.notes = JsonChartModel.JsonToJsonChart(filePathLower).ToGameChartNoteList();
                else if (File.Exists(filePathUpper))
                    gChart.notes = JsonChartModel.JsonToJsonChart(filePathUpper).ToGameChartNoteList();
                else
                    throw new System.Exception("Level Ultra in Song " + songName + ", Level" + levelUltra.ToString() + "has defined a difficulty but doesn't provide a chart.");

                packInfo.charts.Add(gChart);
            }

            if (File.Exists(iniDocumentPath + "/cover.png"))
            {
                var PixelsPerUnit = 100.0f;
                SpriteMeshType spriteType = SpriteMeshType.Tight;
                Texture2D SpriteTexture = LoadTexture(iniDocumentPath + "/cover.png");
                packInfo.cover = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0, 0), PixelsPerUnit, 0, spriteType);
            }
            Log.LogStr(packInfo.songName);
            return packInfo;
        }

        /// <summary>
        /// Load a PNG or JPG file from disk to a Texture2D
        /// </summary>
        /// <param name="FilePath"></param>
        /// <returns>The Texture2D. Returns null if load fails</returns>
        public static Texture2D LoadTexture(string FilePath)
        {
            Texture2D Tex2D;
            byte[] FileData;

            if (File.Exists(FilePath))
            {
                FileData = File.ReadAllBytes(FilePath);
                Tex2D = new Texture2D(2, 2);           // Create new "empty" texture
                if (Tex2D.LoadImage(FileData))           // Load the imagedata into the texture (size is set automatically)
                    return Tex2D;                 // If data = readable -> return texture
            }
            return null;                     // Return null if load failed
        }
    }
}