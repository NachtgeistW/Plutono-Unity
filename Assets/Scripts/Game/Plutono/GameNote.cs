/*
 * class GameNote -- Store the information of a note in game.
 *
 *      This class includes the type, position, size, time, piano sounds property.
 *      id: uint, the id of this note (start from 1)
 *      type: uint, is this note black(0), yellow(1), white(2)
 *      pos: float, the position of this note (from -2 to 2, or it won't be shown)
 *      size: float, the size of this note (from 0 to 4)
 *      time: float, the time when this note should be touched (start from 0)
 *      IsShown: bool, TRUE if this note should be shown
 *
 * Function:
 *
 *
 * History:
 *      2021.03.07  CREATED
 *      2021.03.19  ADD function ToJsonNote() and variable IsShown
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace Assets.Scripts.Game.Note
{
    [System.Serializable]
    public class GameNote
    {
        public uint id;
        public float pos;
        public float size;
        public float time;
        public uint type;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [CanBeNull] public List<GamePianoSound> sounds;

        public bool IsShown => pos <= 2.0f && pos >= -2.0f;

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

    }
}
