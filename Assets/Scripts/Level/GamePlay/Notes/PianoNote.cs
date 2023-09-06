using System.Collections.Generic;
using Plutono.Level.GamePlay;
using Plutono.Song;
using Plutono.Util;
using UnityEngine;

namespace Plutono.GamePlay.Notes
{
    public class PianoNote : BaseNote, IMovable, IPianoSoundPlayable, ITapable
    {
        public List<GamePianoSound> sounds;

        public PianoNote(uint id, float pos, double time, float size) : base(id, pos, time, size) { }

        public PianoNote(uint id, float pos, double time, float size, List<GamePianoSound> sounds) : base(id, pos, time, size)
        {
            this.sounds = sounds;
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

        public bool ShouldBeMiss() => transform.position.z <= -10;

        public void OnPlayPianoSounds()
        {
            throw new System.NotImplementedException();
        }

        public bool OnTap(Vector2 worldPos, double hitTime, out double deltaTime, out float deltaXPos)
        {
            deltaTime = double.MaxValue;
            deltaXPos = float.MaxValue;

            if (IsClear) return false;
            return IsHit(worldPos.x, out deltaXPos, hitTime, out deltaTime);
        }
    }
}