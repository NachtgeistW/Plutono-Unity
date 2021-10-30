/*
 * class JsonChartModel -- Store the information of a single chart from json.
 *
 *      This class includes the speed and note property.
 *      speed: float, is always 0.0f (It is in official charts, but no one knows what this means...)
 *      notes: List<JsonNoteModel>, stores the information of the notes in a chart
 *
 * History:
 *      2021.03.29  COPY from Deenote; ADD ToGameChart function
 *      2021.04.04  MOVE JsonToJChart to class JsonChartModel
 *      2021.10.10  RENAME from JsonChart to JsonChartModel
*/

using System;
using System.Collections.Generic;
using System.IO;
using Assets.Scripts.Model.Plutono;
using Model.Deemo;
using Model.Plutono;
using Newtonsoft.Json;

namespace Assets.Scripts.Model.Deemo
{
    [Serializable]
    public class JsonChartModel
    {
        public float speed = 0.0f;
        public List<JsonNoteModel> notes = new List<JsonNoteModel>();
        //public List<Link> links = new();

        /// <summary>
        /// transfer JsonChartModel to the note list of GameChart
        /// </summary>
        /// <returns>A GameChart note list</returns>
        public List<GameNoteModel> ToGameChartNoteList()
        {
            var gChartNoteList = new List<GameNoteModel>();
            foreach (var jNote in notes)
            {
                gChartNoteList.Add(jNote.ToGameNote());
            }
            return gChartNoteList;
        }

        /// <summary>
        /// transfer Json to JsonChartModel
        /// </summary>
        /// <param name="jsonPath">the path of json file</param>
        /// <returns>A tranferred JsonChartModel</returns>
        public static JsonChartModel JsonToJsonChart(string jsonPath)
        {
            var settings = new JsonSerializerSettings();
            settings.MetadataPropertyHandling = MetadataPropertyHandling.Ignore;
            var r = new StreamReader(jsonPath);
            var json = r.ReadToEnd();
            var jChart = JsonConvert.DeserializeObject<JsonChartModel>(json, settings);
            return jChart;
        }
    }

}