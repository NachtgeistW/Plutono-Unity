/* 
 * History:
 *      2022.07.22  CREATED
 */

using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;


namespace Plutono.Song
{
    //class Explosion -- Control the note explosion effect on hitting.
    public class Explosion : MonoBehaviour
    {
        Animator explosionAnim;
        [SerializeField] SpriteRenderer waveSpriteRenderer;
        [SerializeField] SpriteRenderer lightSpriteRenderer;
        [SerializeField] SpriteRenderer circleSpriteRenderer;

        public void PlayAnimation(NoteGrade noteGrade, float noteSize)
        {
            waveSpriteRenderer.transform.localScale = new Vector3(noteSize, 0, 1);
            lightSpriteRenderer.transform.localScale = new Vector3(noteSize, 1, 1);
            switch (noteGrade)
            {
                case NoteGrade.Perfect:
                    lightSpriteRenderer.material.color = Settings.perfectLightColor;
                    break;
                case NoteGrade.Good:
                    lightSpriteRenderer.material.color = Color.green;
                    break;
                case NoteGrade.Bad:
                    lightSpriteRenderer.material.color = Color.blue;
                    break;
                case NoteGrade.Miss:
                    lightSpriteRenderer.material.color = Color.red;
                    break;
                case NoteGrade.None:
                default:
                    break;
            }

            //Tween waveTransform = waveSpriteRenderer.transform.DOScaleY(1, 0.3f).SetEase(Ease.Linear);
            //Tween waveColor = waveSpriteRenderer.DOColor(new Color(0, 0, 0, 1), 0.3f).SetEase(Ease.Linear);
            //waveTransform.WaitForCompletion();
            //waveColor.WaitForCompletion();
            //waveSpriteRenderer.transform.DOScaleY(1, 0.7f);
            //waveSpriteRenderer.DOColor(new Color(0, 0, 0, 0), 0.7f).SetEase(Ease.Linear);

            //lightSpriteRenderer.transform.DOScale(new Vector3(noteSize * 2, 4, 1), 0.5f).SetEase(Ease.Linear);
            //lightSpriteRenderer.DOColor(new Color(0, 0, 0, 1), 0.5f).SetEase(Ease.Linear);
            //lightSpriteRenderer.transform.DOScale(new Vector3(1, 1, 1), 0.5f).SetDelay(0.5f);
            //lightSpriteRenderer.DOColor(new Color(0, 0, 0, 0), 0.5f).SetEase(Ease.Linear).SetDelay(0.5f);

            //circleSpriteRenderer.transform.DOScale(new Vector3(1, 1, 1), 0.15f).SetEase(Ease.Linear);
            //circleSpriteRenderer.DOColor(new Color(0, 0, 0, 0), 0.15f).SetEase(Ease.Linear);

            //The Animation Event that be fired after playing explosion animation 
            var explosionAnimEvent = new AnimationEvent
            {
                functionName = nameof(ExecuteAfterAnimate),
                //objectReferenceParameter = note,
                //The time waiting for the animation to finish
                time = Settings.noteAnimationPlayingTime
            };

            explosionAnim = gameObject.GetComponent<Animator>();
            var clip = explosionAnim.runtimeAnimatorController.animationClips[0];
            clip.AddEvent(explosionAnimEvent);
            explosionAnim.SetBool("IsHit", true);
        }

        // the function to be called as an event
        private void ExecuteAfterAnimate()
        {
            explosionAnim.Rebind();
            //explosionAnim.Update(0f);
            explosionAnim.SetBool("IsHit", false);
        }

        public void ForceStopAnimation()
        {
            //Force stop DOTween animation, or it would has effect on objectpool releasing
            waveSpriteRenderer.DOKill();
            lightSpriteRenderer.DOKill();
            circleSpriteRenderer.DOKill();
        }

    }
}

