/*
 * History:
 *      2021.03.07  CREATED
 *      2021.03.19  ADD function ToJsonNote() and variable IsShown
 *      2021.04.04  ADD enum NoteType; DELETE function ToJsonNote()
 *      2021.10.10  RENAME from GameNote to GameNoteModel
 *      2022.07.25  RENAME from GameNoteModel to NoteDetails
 */

using System.Collections.Generic;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Plutono.Legacy;
using UnityEngine;
using static UnityEditor.PlayerSettings;

namespace Plutono.Song
{
    //class NoteDetail -- Store the data of a note in game.
    [System.Serializable]
    public sealed class NoteDetail
    {
        public uint id;         //the id of this note (start from 1)
        public float pos;       //the position of this note (from -2 to 2, or it won't be shown). float, due to Vector3 accept float only.
        public double size;      //the size of this note (from 0 to 4)
        public double time;      //the time when this note should be touched (start from 0)
        public NoteType type;   //is this note piano(0), slide(1) or blank(2)
        public double duration; //the duration of the hold
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [CanBeNull] public List<GamePianoSound> sounds;
        //slide
        public bool isLink;
        public int prevLink = -1;
        public int nextLink = -1;
        public bool isSliding;
        public double slideStartTime;
        public Vector3 slideStartPos;

        public static object NoteType { get; internal set; }

        public bool IsShown => pos is <= 2.0f and >= -2.0f;     //TRUE if this note should be shown

        public void StartSliding(double slideStartTime, Vector3 fingerPos)
        {
            isSliding = true;
            slideStartPos = fingerPos;
            this.slideStartTime = slideStartTime;
        }
    }

    public enum NoteType
    {
        Piano, Slide, Blank, Vibrate, Swipe
    }

    [System.Serializable]
    public sealed class GamePianoSound
    {
        public double delay; //w
        public double duration; //d
        public short pitch; //p
        public short volume; //v

        public static GamePianoSound FromLegacyPianoSound(LegacyPianoSound sound)
            => new() { delay = sound.w, duration = sound.d, pitch = sound.p, volume = sound.v };
    }
}
