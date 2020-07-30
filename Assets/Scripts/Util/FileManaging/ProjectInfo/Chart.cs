/*
 * class Chart -- Store the information of a single chart.
 *
 *      This class includes the speed, difficulty, level, beats and notes property.
 *
 * class Note -- Store the information of a note.
 *
 *      This class includes the position, size, time, shift, piano sounds and "is a link" property.
 *      If it is in a link, it also use prevLink and nextLink information.
 *
 * class PianoSound -- Store the information of piano sound on a note.
 *
 *      This class includes the delay, duration, pitch and volume property.
 *
 * History
 *      2020.7.29 Copy from Deenote.
 */

using System.Collections.Generic;

namespace Assets.Scripts.Util
{
    [System.Serializable]
    public class Chart
    {
        public float speed = 0;
        public int difficulty = 0;
        public string level = "";
        public List<float> beats = new List<float>(); // For quantifying the note
        public List<Note> notes = new List<Note>();
    }

    [System.Serializable]
    public class Note
    {
        public float position = 0.0f; //pos
        public float size = 0.0f; //size
        public float time = 0.0f; //_time
        public float shift = 0.0f; //shift
        public List<PianoSound> sounds = new List<PianoSound>(); //sounds
        public bool isLink = false; //Whether the note is a link note
        //The two variables below are not used when isLink=false
        public int prevLink = -1; //Index of previous link note in the same link, -1 means current note is the first
        public int nextLink = -1; //Index of next link note in the same link, -1 means current note is the last
    }

    [System.Serializable]
    public class PianoSound //Saves info of a piano sound
    {
        public float delay = 0.0f; //w
        public float duration = 0.0f; //d
        public int pitch = 0; //p
        public int volume = 0; //v
    }
}