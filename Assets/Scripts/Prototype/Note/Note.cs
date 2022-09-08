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

        private void Awake()
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
            }

            FallingDown();
        }

        public void FallingDown()
        {
            gameObject.transform.position = new Vector3(_details.pos * 10, 0, Settings.maximumNoteRange);
            gameObject.transform.DOMoveZ(32, Settings.NoteFallTime(10)).SetEase(Ease.Linear);
        }
    }
}