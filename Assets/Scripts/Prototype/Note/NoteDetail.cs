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

namespace Plutono.Song
{
    //class NoteDetail -- Store the data of a note in game.
    [System.Serializable]
    public sealed class NoteDetail
    {
        public uint id;         //the id of this note (start from 1)
        public float pos;       //the position of this note (from -2 to 2, or it won't be shown)
        public float size;      //the size of this note (from 0 to 4)
        public float time;      //the time when this note should be touched (start from 0)
        public NoteType type;   //is this note piano(0), slide(1) or blank(2)
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [CanBeNull] public List<GamePianoSound> sounds;
        //slide
        public bool isLink = false;
        public int prevLink = -1;
        public int nextLink = -1;

        public static object NoteType { get; internal set; }

        public bool IsShown => pos <= 2.0f && pos >= -2.0f;     //TRUE if this note should be shown

    }
    public enum NoteType
    {
        Piano, Slide, Blank
    }

    [System.Serializable]
    public sealed class GamePianoSound
    {
        public float delay; //w
        public float duration; //d
        public short pitch; //p
        public short volume; //v

        public static GamePianoSound FromLegacyPianoSound(LegacyPianoSound sound)
            => new() { delay = sound.w, duration = sound.d, pitch = sound.p, volume = sound.v };
    }
}
