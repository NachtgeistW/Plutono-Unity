/*
 * class Chart -- Store the information of a single chart.
 *
 *      This class includes the speed, difficulty, level, beats and notes property.
 *
 * History
 *      2020.7.29 Copy from Deenote.
 */
using System.Collections.Generic;

namespace Assets.Scripts.Game.Chart
{
    [System.Serializable]
    public class Chart
    {
        private readonly float speed = 0;
        private readonly int difficulty = 0;
        private readonly string level = "";
        private readonly List<float> beats = new List<float>(); // For quantifying the note
        private readonly List<Note> notes = new List<Note>();
    }
}