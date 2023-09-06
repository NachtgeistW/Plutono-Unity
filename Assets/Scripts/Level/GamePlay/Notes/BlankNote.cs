using Plutono.Level.GamePlay;
using Plutono.Util;
using UnityEngine;

namespace Plutono.GamePlay.Notes
{
    public class BlankNote : BaseNote, IMovable, ITapable
    {
        public BlankNote(uint id, float pos, double time, float size) : base(id, pos, time, size) { }

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

        public bool ShouldBeMiss() => transform.position.z <= -10;

        public bool OnTap(Vector2 worldPos, double hitTime, out double deltaTime, out float deltaXPos)
        {
            deltaTime = double.MaxValue;
            deltaXPos = float.MaxValue;

            if (IsClear) return false;
            return IsHit(worldPos.x, out deltaXPos, hitTime, out deltaTime); ;
        }

    }
}