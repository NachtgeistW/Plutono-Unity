/*
 * class GameNoteModel -- Store the data of a note in game.
 *
 *      This class includes the type, position, size, time, piano sounds property.
 *      id: uint, the id of this note (start from 1)
 *      type: NoteType, is this note black(0), yellow(1), white(2)
 *      pos: float, the position of this note (from -2 to 2, or it won't be shown)
 *      size: float, the size of this note (from 0 to 4)
 *      time: float, the time when this note should be touched (start from 0)
 *      IsShown: bool, TRUE if this note should be shown
 *
 * History:
 *      2021.03.07  CREATED
 *      2021.03.19  ADD function ToJsonNote() and variable IsShown
 *      2021.04.04  ADD enum NoteType; DELETE function ToJsonNote()
 *      2021.10.10  RENAME from GameNote to GameNoteModel
 */

using System.Collections.Generic;
using JetBrains.Annotations;
using Model.Deemo;
using Newtonsoft.Json;

namespace Assets.Scripts.Model.Plutono
{
    using Views;

    [System.Serializable]
    public class GameNoteModel
    {
        public uint id;
        public float pos;
        public float size;
        public float time;
        public NoteType type;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [CanBeNull] public List<GamePianoSound> sounds;
        public bool IsShown => pos <= 2.0f && pos >= -2.0f;

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

            public static GamePianoSound FromJsonPianoSound(JsonPianoSound sound)
                => new GamePianoSound() { delay = sound.w, duration = sound.d, pitch = sound.p, volume = sound.v };
        }

        public BlankNote ToBlankNote()
        {
            var blankNote = new BlankNote()
            {
                Model = this,
                BlankNoteView = new BlankNoteView()
            };
            return blankNote;
        }
        public PianoNote ToPianoNote()
        {
            var pianoNote = new PianoNote
            {
                Model = this,
                PianoNoteView = new PianoNoteView()
            };
            return pianoNote;
        }
        public SlideNote ToSlideNote()
        {
            var slideNote = new SlideNote
            {
                Model = this,
                SlideNoteView = new SlideNoteView()
            };
            return slideNote;
        }
    }
}
