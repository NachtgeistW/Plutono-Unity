/*
 * class Utility -- Define some utility tools
 *
 * Function
 *      JsonChart::JsonToJChart -- read a single chart from specific json and return a JsonChart
 *
 * History
 *      2021.03.31: ADD JsonToJChart function.
 */

using System.Collections.Generic;
using System.IO;
using Assets.Scripts.Game.Deemo;
using Assets.Scripts.Game.Note;
using Newtonsoft.Json;
using UnityEngine;

namespace Assets.Scripts.Util
{
    public class Utility
    {
        public static JsonChart JsonToJChart(string jsonPath)
        {
            JsonChart jChart;
            using (var r =
                new StreamReader(jsonPath))
            {
                var json = r.ReadToEnd();
                dynamic items = JsonConvert.DeserializeObject(json);
                jChart = new JsonChart();
                uint i = 1;
                if (items == null) return null;
                foreach (var item in items["notes"])
                {
                    JsonNote jNote = new JsonNote()
                    {
                        id = i,
                        pos = item.pos,
                        shift = item.shift,
                        size = item.size,
                        time = item.time,
                        _time = item._time,
                        type = item.type
                    };

                    //process the piano sound
                    if (item.sounds != null)
                        ;
                    i++;
                    jChart.notes.Add(jNote);
                }
            }
            return jChart;
        }
    }
}