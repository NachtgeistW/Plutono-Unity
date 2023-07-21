using System.Collections.Generic;
using Plutono.Song;

namespace Plutono.GamePlay.Notes
{
    public sealed class BackingNote : BaseNote, IPianoSoundPlayable
    {
        public List<GamePianoSound> sounds;
        public void OnPlayPianoSounds()
        {
            throw new System.NotImplementedException();
        }
    }
}