/* 
 * class PianoNoteView -- Store the appearance of a piano note in game.
 *
 *      This class includes the id, type, position, size, time, piano sounds property.
 *      id: uint, the id of this note (start from 1)
 *      time: float, the current game time when the player hits the note
 *      spriteRenderer and sprite: the sprites of a piano note and the renderer used to render them
 *      IsInViewableRange: bool, judging note can be seen or not according to the game logic of Deemo
 *
 * History:
 *      2021.12.20  CREATED
 */

namespace Assets.Scripts.Views
{
    using Model.Plutono;

    using UnityEngine;

    using Util;

    public class PianoNoteView : NoteView
    {
        [SerializeField] private Sprite pianoNoteSprite;
        private const float PianoNoteScale = 4.5f;

        public override void SetNoteAppearance(float noteSize, float notePos)
        {
            noteSpriteRenderer.sprite = pianoNoteSprite;
            noteSpriteRenderer.transform.localScale = PianoNoteScale * new Vector3(noteSize, 1.0f, 1.0f);
            waveColor = Color.black;

            if (noteSize > 2.0f || noteSize < -2.0f)
            {
                IsInViewableRange = false;
                circleSpriteRenderer.sprite = null;
                waveSpriteRenderer.sprite = null;
                lightSpriteRenderer.sprite = null;
            }
            else
            {
                IsInViewableRange = true;
                circleSpriteRenderer.sprite = circleSprite;
                waveSpriteRenderer.transform.localScale = Vector3.zero;
                waveSpriteRenderer.sprite = waveSprite;
                lightSpriteRenderer.sprite = lightSprite;
            }
            noteSpriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
            gameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

            MaxXPos = Parameters.maximumNoteWidth * notePos;
            TouchableLeftRange = Parameters.maximumNoteWidth * (notePos - 0.5f * noteSize);
            TouchableRightRange = Parameters.maximumNoteWidth * (notePos + 0.5f * noteSize);
        }
    }
}