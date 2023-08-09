using System;
using System.Collections.Generic;
using Plutono.Level.GamePlay;
using Plutono.Song;
using Plutono.Util;
using UnityEngine;

namespace Plutono.GamePlay.Notes
{
    public sealed class SlideNote : BaseNote, IMovable, ITapable, ISlidable
    {
        public List<GamePianoSound> sounds;

        public int prevLink = -1;
        public int nextLink = -1;
        public bool isSliding;
        public double slideStartTime;
        public Vector2 slideStartPos;
        private Vector2 moved;

        public SlideNote(uint id, float pos, double time, float size) : base(id, pos, time, size)
        {
            type = NoteType.Slide;
        }

        public SlideNote(uint id, float pos, double time, float size, int prevLink, int nextLink) : base(id, pos, time, size)
        {
            type = NoteType.Slide;
            this.prevLink = prevLink;
            this.nextLink = nextLink;
        }

        private void OnEnable()
        {
            transform.position = new Vector3(pos, 0, Settings.maximumNoteRange);

            SpriteRenderer.size = new Vector2((float)size * 1.42f, 0.57f);
        }

        public void OnMove(float chartPlaySpeed, double curTime)
        {
            var z = (float)(Settings.maximumNoteRange / Settings.NoteFallTime(chartPlaySpeed) * (time - curTime));
            transform.position = new Vector3(pos, 0, z);
        }

        public bool ShouldBeMiss(GameMode mode)
        {

        }

        public bool OnTap(Vector2 worldPos, double hitTime, out double deltaTime, out float deltaXPos)
        {
            deltaTime = double.MaxValue;
            deltaXPos = float.MaxValue;

            if (IsClear) return false;
            return IsHit(worldPos.x, out deltaXPos, hitTime, out deltaTime);
        }

        public void OnSlideStart(Vector2 worldPos, double curTime)
        {
            if (isSliding || IsClear) 
                return;

            if (!IsHit(worldPos.x, out _, curTime, out _)) return;
            isSliding = true;
            slideStartTime = curTime;
            slideStartPos = worldPos;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="worldPos"></param>
        /// <returns>Can be cleared</returns>
        public bool UpdateSlide(Vector2 worldPos)
        {
            var noteJudgingSize = size < 1.2 ? 0.6 : size / 2;
            if (!IsHit (worldPos.x, out _, time, out _)) 
                return true;
            moved = worldPos - slideStartPos;
            return Mathf.Abs(moved.x) >= noteJudgingSize / 2;
        }

        public void OnSlideEnd(double curTime, out double deltaTime, out float deltaXPos)
        {
            deltaTime = curTime - slideStartTime;
            deltaXPos = moved.x - slideStartPos.x;
        }

        public void PlayPianoSounds()
        {
            throw new System.NotImplementedException();
        }
    }
}