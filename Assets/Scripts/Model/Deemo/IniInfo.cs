/*
 * class IniInfo -- Store the information from a ini file.
 *
 *      This class includes the song name, artist, chart designer and level info.
 *      songName: string, song name.
 *      artist: string, the composer of this song.
 *      chartDesigner: string, the chart designer of this song.
 *      levelEasy: uint, the level of easy.
 *      levelNormal: uint, the level of normal.
 *      levelHard: uint, the level of hard.
 *      levelExtra: string, the level of extra. (Can be ASCII)
 *      levelUltra: uint, the level of extra. (Used in Deemo 2.2, equal to Extra)
 *      Note: if level = 0 / = null, the chart of this level is not exist.
 *            the rest data in ini wouldn't be convert.
 *
 * Function
 *      IniInfo::ReadIniConfig -- read ini info from a ini file.
 *      PackInfo::IniToPackInfo -- convert ini to a config file used in Plutono.
 *
 * History
 *      2020.03.31  CREATE.
 */

using Assets.Scripts.Model.Plutono;
using Assets.Scripts.Util;
using IniParser;
using UnityEngine;

namespace Assets.Scripts.Model.Deemo
{
    [System.Serializable]
    public class IniInfo
    {
        public string songName = "";
        public string artist = "";
        public string chartDesigner = "";
        public uint levelEasy = 0;
        public uint levelNormal = 0;
        public uint levelHard = 0;
        public string levelExtra = "";
        public uint levelUltra = 0;

        public IniInfo ReadIniConfig(string iniPath)
        {
            var parser = new FileIniDataParser();
            //TODO: 判定ini路径是否合法
            var data = parser.ReadFile(iniPath);
            var info = new IniInfo
            {
                songName = data["Song"]["Name"],
                artist = data["Song"]["Artist"],
                chartDesigner = data["Song"]["Noter"]
            };
            if (data["Song"]["Easy"] != null)
                info.levelEasy = uint.Parse(data["Song"]["Easy"]);
            if (data["Song"]["Normal"] != null)
                info.levelNormal = uint.Parse(data["Song"]["Normal"]);
            if (data["Song"]["Hard"] != null)
                info.levelHard = uint.Parse(data["Song"]["Hard"]);
            if (data["Song"]["Extra"] != null)
                info.levelExtra = data["Song"]["Extra"];
            if (data["Song"]["Ultra"] != null)
                info.levelUltra = uint.Parse(data["Song"]["Ultra"]);
            return info;
        }
        public PackInfo IniToPackInfo(string iniDocumentPath)
        {
            //TODO::将if分支里面的重复代码抽取出来
            var packInfo = new PackInfo { songName = songName, composer = artist };
            if (levelEasy != 0)
            {
                var jChart = JsonChart.JsonToJChart(iniDocumentPath + "/easy.json");
                var gChart = jChart.ToGameChart();
                gChart.level = levelEasy.ToString();
                gChart.chartDesigner = chartDesigner;
                packInfo.charts.Add(gChart);
            }
            if (levelNormal != 0)
            {
                var jChart = JsonChart.JsonToJChart(iniDocumentPath + "/normal.json");
                var gChart = jChart.ToGameChart();
                gChart.level = levelNormal.ToString();
                gChart.chartDesigner = chartDesigner;
                packInfo.charts.Add(gChart);
            }
            if (levelHard != 0)
            {
                var jChart = JsonChart.JsonToJChart(iniDocumentPath + "/hard.json");
                var gChart = jChart.ToGameChart();
                gChart.level = levelHard.ToString();
                gChart.chartDesigner = chartDesigner;
                packInfo.charts.Add(gChart);
            }
            if (levelExtra != "")
            {
                var jChart = JsonChart.JsonToJChart(iniDocumentPath + "/extra.json");
                var gChart = jChart.ToGameChart();
                gChart.level = levelExtra;
                gChart.chartDesigner = chartDesigner;
                packInfo.charts.Add(gChart);
            }
            if (levelUltra != 0)
            {
                var jChart = JsonChart.JsonToJChart(iniDocumentPath + "/ultra.json");
                var gChart = jChart.ToGameChart();
                gChart.level = levelUltra.ToString();
                gChart.chartDesigner = chartDesigner;
                packInfo.charts.Add(gChart);
            }
            Debug.Log(packInfo.songName);
            return packInfo;
        }
    }
}