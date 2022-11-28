﻿/*
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
 *      2022.11.23  RENAME to LegancyChartDetail
*/
using System;
using System.Collections.Generic;
using System.IO;

namespace Plutono.Legancy
{
    using Newtonsoft.Json;

    [Serializable]
    public class LegancyChartDetail
    {
        public float speed = 0.0f;
        public List<LegancyNoteDetail> notes = new();
        //public List<Link> links = new();

        /// <summary>
        /// transfer LegancyChartDetail to the NoteDetail list
        /// </summary>
        /// <returns>A list containing transferred NoteDetail</returns>
        public List<Song.NoteDetail> ToNoteDetailList()
        {
            var result = new List<Song.NoteDetail>();
            foreach (var legancyNote in notes)
            {
                var gNote = legancyNote.ToNoteDetail();
                result.Add(gNote);
            }
            return result;
        }

        /// <summary>
        /// transfer Json to LegancyChartDetail
        /// </summary>
        /// <param name="jsonPath">the path of json file</param>
        /// <returns>A transferred LegancyChartDetail</returns>
        public static LegancyChartDetail ToLegancyChartDetail(string jsonPath)
        {
            var settings = new JsonSerializerSettings();
            settings.MetadataPropertyHandling = MetadataPropertyHandling.Ignore;
            var r = new StreamReader(jsonPath);
            var json = r.ReadToEnd();
            var legancyChart = JsonConvert.DeserializeObject<LegancyChartDetail>(json, settings);
            return legancyChart;
        }
    }

}