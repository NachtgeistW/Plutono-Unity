using UnityEngine;

namespace Plutono.GamePlay.Notes
{

    public interface INote { }

    public interface IMovable : INote
    {
        void OnMove(float chartPlaySpeed, double curTime);
        bool ShouldBeMiss();
    }

    public interface IPianoSoundPlayable : INote
    {
        void OnPlayPianoSounds();
    }

    /// <summary>
    /// control the tap action
    /// </summary>
    public interface ITapable : INote
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns>Is </returns>
        bool OnTap(Vector2 worldPos, double hitTime, out double deltaTime, out float deltaXPos);
    }

    /// <summary>
    /// control the slide action
    /// </summary>
    // ReSharper disable once IdentifierTypo
    public interface ISlidable : INote
    {
        void OnSlideStart(Vector2 worldPos, double curTime);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="worldPos"></param>
        /// <returns>If the note can be cleared</returns>
        bool UpdateSlide(Vector2 worldPos);
        void OnSlideEnd(double curTime, out double deltaTime, out float deltaXPos);
    }

    public interface IHoldable : INote
    {
        void OnHoldStart(Vector2 worldPos, double curTime);
        bool UpdateHold(Vector2 worldPos);
        void OnHoldEnd();
    }

    // ReSharper disable once IdentifierTypo
    public interface IFlickable : INote
    {
        void OnFlickStart(Vector2 worldPos, double curTime);
        bool UpdateFlick(Vector2 worldPos);
        void OnFlickEnd();
    }
}
