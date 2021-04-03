/*
 * class Utility -- Define some utility tools
 *
 * Function
 *      JsonChart::JsonToJChart -- read a single chart from specific json and return a JsonChart
 *
 * History
 *      2021.03.31: ADD JsonToJChart function.
 *      2021.04.03: REWRITE JsonToJChart function with reflection.
 */

using System.IO;
using Assets.Scripts.Game.Deemo;
using Newtonsoft.Json;

namespace Assets.Scripts.Util
{
    public class Utility
    {
        public static JsonChart JsonToJChart(string jsonPath)
        {
            var r = new StreamReader(jsonPath);
            var json = r.ReadToEnd();
            var jChart = JsonConvert.DeserializeObject<JsonChart>(json);
            return jChart;
        }
    }
}