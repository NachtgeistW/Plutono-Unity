/*
 * class JsonChart -- Store the information of a single chart from json.
 *
 *      This class includes the speed and note property.
 *      speed: float, is always 0.0f (It is in official charts, but no one knows what this means...)
 *      notes: List<JsonNote>, stores the information of the notes in a chart
 *
 * Function:
 *      GameChart::ToGameChart -- transfer JsonChart to GameChart
 *      JsonChart::JsonToJChart -- transfer Json to JsonChart
 *
 * History:
 *      2021.03.29  COPY from Deenote; ADD ToGameChart function
 *      2021.04.04  MOVE JsonToJChart to class JsonChart
*/

using System;
using System.Collections.Generic;
using System.IO;
using Assets.Scripts.Model.Plutono;
using Newtonsoft.Json;

namespace Assets.Scripts.Model.Deemo
{
    [Serializable]
    public class JsonChart
    {
        public float speed = 0.0f;
        public List<JsonNote> notes = new List<JsonNote>();
        //public List<Link> links = new();

        public GameChart ToGameChart()
        {
            var gChart = new GameChart();
            foreach (var jNote in notes)
            {
                gChart.notes.Add(jNote.ToGameNote());
            }
            return gChart;
        }

        public static JsonChart JsonToJChart(string jsonPath)
        {
            var r = new StreamReader(jsonPath);
            var json = r.ReadToEnd();
            var jChart = JsonConvert.DeserializeObject<JsonChart>(json);
            return jChart;
        }
    }

}