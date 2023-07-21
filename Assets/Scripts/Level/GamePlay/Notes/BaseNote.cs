using System;
using Plutono.Song;
using UnityEngine;

namespace Plutono.GamePlay.Notes
{
    public abstract class BaseNote : MonoBehaviour
    {
        [field: SerializeField] public SpriteRenderer SpriteRenderer { get; set; }

        [Space(10)]
        public uint id;         //the id of this note (start from 1)
        public float pos;       //the position of this note (from -2 to 2, or it won't be shown). float, due to Vector3 accept float only.
        public double size;      //the size of this note (from 0 to 4)
        public double time;      //the time when this note should be touched (start from 0)
        public NoteType type;   //is this note piano(0), slide(1) or blank(2)

        public bool IsClear { get; set; }    //is this note cleared

        protected BaseNote(uint id, float pos, double time, double size)
        {
            this.id = id;
            this.pos = pos;
            this.time = time;
            this.size = size;
            IsClear = false;
        }

        protected BaseNote() { }

        public void SetData(uint id, float pos, double time, double size)
        {
            this.id = id;
            this.pos = pos;
            this.time = time;
            this.size = size;
            IsClear = false;
        }

        public virtual void SetData(NoteDetail noteDetail)
        {
            id = noteDetail.id;
            pos = noteDetail.pos;
            time = noteDetail.time;
            size = noteDetail.size;
        }

        /// <summary>
        /// Check if the note is hit within the range.
        /// </summary>
        /// <param name="xPos">the x position of player's finger on world position</param>
        /// <param name="deltaXPos">the distance between the hit pos and note pos</param>
        /// <param name="curTime"></param>
        /// <param name="deltaTime"></param>
        /// <returns>true if hit or is in Autoplay mode</returns>
        public bool IsHit(float xPos, out float deltaXPos, double curTime, out double deltaTime)
        {
            var noteJudgingSize = size < 1.2 ? 1.2 : size;
            var noteDeltaXPos = Mathf.Abs(xPos - pos);
            if (noteDeltaXPos <= noteJudgingSize)
            {
                deltaXPos = noteDeltaXPos;
                deltaTime = Math.Abs(curTime - time);
                return true;
            }
            else
            {
                deltaXPos = float.MaxValue;
                deltaTime = double.MaxValue;
                return false;
            }
        }
    }
}