/*
 * History:
 *      2022.07.25  CREATED
 */
using UnityEngine;
using DG.Tweening;

namespace Plutono.Song
{
    //class Note -- GameObject on the scene.
    public class Note : MonoBehaviour
    {
        public NoteDetail _details;

        [SerializeField] private SpriteRenderer _blankSpriteRenderer;
        [SerializeField] private SpriteRenderer _pianoSpriteRenderer;
        [SerializeField] private SpriteRenderer _slideSpriteRenderer;

        private void Start()
        {
            SetSpriteRenderer();
        }

        public void FallingDown()
        {
            //Force the note to back to the original position where it begins to fall down
            gameObject.transform.position = new Vector3((float)(_details.pos * 10), 0, Settings.maximumNoteRange);
            gameObject.transform.DOMoveZ(0, Settings.NoteFallTime(10)).SetEase(Ease.Linear);
        }

        public void SetSpriteRenderer()
        {
            _blankSpriteRenderer.gameObject.SetActive(false);
            _pianoSpriteRenderer.gameObject.SetActive(false);
            _slideSpriteRenderer.gameObject.SetActive(false);
            if (_details.IsShown)
            {
                switch (_details.type)
                {
                    case NoteType.Blank:
                        _blankSpriteRenderer.gameObject.SetActive(true);
                        break;
                    case NoteType.Piano:
                        _pianoSpriteRenderer.gameObject.SetActive(true);
                        break;
                    case NoteType.Slide:
                        _slideSpriteRenderer.gameObject.SetActive(true);
                        break;
                    case NoteType.Swipe:
                        _slideSpriteRenderer.gameObject.SetActive(true);
                        break;
                }
            }
        }
    }
}