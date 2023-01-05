/*
 * History
 * 2020.03.31  CREATE.
 */

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
#nullable enable
        public uint? LevelEasy;          //the level of easy.
        public uint? LevelNormal;        //the level of normal.
        public uint? LevelHard;          //the level of hard.
        public string? LevelExtra;      //the level of extra. (Can be ASCII)
        public uint? LevelUltra;         //the level of extra. (Used in Deemo 2.2, equal to Extra)
#nullable disable

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
}