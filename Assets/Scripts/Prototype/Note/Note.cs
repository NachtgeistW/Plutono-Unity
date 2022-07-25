using UnityEngine;

namespace Plutono.Song
{
    //GameObject on the scene
    public class Note : MonoBehaviour
    {
        public NoteDetails _details;

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
        }
    }
}