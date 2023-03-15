/*
 * History:
 *      2022.07.25  CREATED
 */
using System;
using UnityEngine;

namespace Plutono.Song
{
    //class Note -- GameObject on the scene.
    public class Note : MonoBehaviour
    {
        public NoteDetail _details;

        [SerializeField] private SpriteRenderer _blankSpriteRenderer;
        [SerializeField] private SpriteRenderer _pianoSpriteRenderer;
        [SerializeField] private SpriteRenderer _slideSpriteRenderer;
        [SerializeField] private BoxCollider _boxCollider;

        private void Start()
        {
            SetProperties();
        }

        public void UpdatePosition(float chartPlaySpeed, double curTime)
        {
            var z = (float)(Settings.maximumNoteRange / Settings.NoteFallTime(chartPlaySpeed) * (_details.time - curTime));
            transform.position = new Vector3(_details.pos, 0, z);
        }

        public void SetProperties()
        {
            //Set position
            transform.position = new Vector3(_details.pos, 0, Settings.maximumNoteRange);

            //Set renderer
            _blankSpriteRenderer.gameObject.SetActive(false);
            _pianoSpriteRenderer.gameObject.SetActive(false);
            _slideSpriteRenderer.gameObject.SetActive(false);
            if (_details.IsShown)
            {
                switch (_details.type)
                {
                    case NoteType.Blank:
                        _blankSpriteRenderer.gameObject.SetActive(true);
                        _blankSpriteRenderer.transform.localScale =
                            new Vector3((float)_details.size, 1, 1);
                        break;
                    case NoteType.Piano:
                        _pianoSpriteRenderer.gameObject.SetActive(true);
                        _pianoSpriteRenderer.transform.localScale =
                            new Vector3((float)_details.size, 1, 1);
                        break;
                    case NoteType.Slide:
                        _slideSpriteRenderer.gameObject.SetActive(true);
                        _slideSpriteRenderer.transform.localScale =
                            new Vector3((float)_details.size, 1, 1);
                        break;
                    case NoteType.Swipe:
                        _slideSpriteRenderer.gameObject.SetActive(true);
                        _slideSpriteRenderer.transform.localScale =
                            new Vector3((float)_details.size, 1, 1);
                        break;
                }
            }

            //Set collider box size
            if (_details.size > 1)
                _boxCollider.size = new Vector3((float)_details.size * 2, 1, 1);
            else
                _boxCollider.size = new Vector3(2, 1, 1);
        }

        /// <summary>
        /// Check if the note is hitted by player.
        /// </summary>
        /// <param name="xPos">the x positon of player's finger on world position</param>
        /// <param name="hitTime">the time when player touches the screen</param>
        /// <param name="mode">Current mode of game</param>
        /// <returns>true if hitted or is in Autoplay mode</returns>
        /// <exception cref="NotImplementedException"></exception>
        public NoteGrade IsHitted(float xPos, double hitTime, GameMode mode)
        {
            if (mode == GameMode.Autoplay) return NoteGrade.Perfect;
            if (Mathf.Abs(xPos - _details.pos) <= _details.size)
            {
                var hitTreshold = hitTime - _details.time;
                return mode switch
                {
                    GameMode.Stelo => hitTreshold switch
                    {
                        <= Settings.SteloMode.perfectDeltaTime => NoteGrade.Perfect,
                        <= Settings.SteloMode.goodDeltaTime => NoteGrade.Good,
                        _ => NoteGrade.Bad
                    },
                    GameMode.Arbo or GameMode.Floro => hitTreshold switch
                    {
                        <= Settings.ArboMode.perfectDeltaTime => NoteGrade.Perfect,
                        <= Settings.ArboMode.goodDeltaTime => NoteGrade.Good,
                        _ => NoteGrade.Bad
                    },
                    //TODO: Finish other mode.
                    GameMode.Persona => throw new NotImplementedException(),
                    GameMode.Ekzerco => throw new NotImplementedException(),
                    _ => throw new NotImplementedException(),
                };
            }
            return NoteGrade.None;
        }
    }
}