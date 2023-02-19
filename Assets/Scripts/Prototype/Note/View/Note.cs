/*
 * History:
 *      2022.07.25  CREATED
 */
using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using System.Security.Cryptography;
using System;

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

        public void UpdatePosition(double curTime, float chartPlaySpeed)
        {
            //Force the note to back to the original position where it begins to fall down
            //gameObject.transform.position = new Vector3((float)(_details.pos * Settings.perspectiveHorizontalScale), 0, Settings.maximumNoteRange);
            //gameObject.transform.DOMoveZ(0, Settings.NoteFallTime(5.5f)).SetEase(Ease.Linear);
            var z = (float)(Settings.maximumNoteRange / Settings.NoteFallTime(chartPlaySpeed) * (_details.time - curTime));
            gameObject.transform.position = new Vector3((float)(_details.pos * Settings.perspectiveHorizontalScale), 0, z);
        }

        public void ForceStopAnimation()
        {
            //Force stop DOTween animation, or it would has effect on objectpool releasing
            gameObject.transform.DOKill();
        }

        public void SetProperties()
        {
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
    }
}