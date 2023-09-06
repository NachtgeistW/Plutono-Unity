/* 
 * History:
 *      2022.07.22  CREATED
 */

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
        [SerializeField] SpriteRenderer blankSpriteRenderer;

        public void PlayAnimation(NoteGrade noteGrade, float noteSize)
        {
            waveSpriteRenderer.color = Color.black;
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
            explosionAnim.SetBool("IsHit", false);
        }

        //public void ForceStopAnimation()
        //{
        //    //Force stop DOTween animation, or it would has effect on objectpool releasing
        //    waveSpriteRenderer.DOKill();
        //    lightSpriteRenderer.DOKill();
        //    circleSpriteRenderer.DOKill();
        //}

    }
}

