/* 
 * History:
 *      2022.07.22  CREATED
 */

using System.Collections.Generic;
using UnityEngine;


namespace Plutono.Song
{
    //class Explosion -- Control the note explosion effect on hitting.
    public class Explosion : MonoBehaviour
    {
        Animator explosionAnim;
        [SerializeField]Material material;

        public void PlayAnimation(NoteGrade noteGrade)
        {
            switch (noteGrade)
            {
                case NoteGrade.Perfect:
                    material.SetColor("_TintColor", Color.white);
                    material.SetColor("_GlowColor", Settings.perfectLightColor);
                    break;
                case NoteGrade.Good:
                    material.color = Color.green;
                    break;
                case NoteGrade.Bad:
                    material.color = Color.blue;
                    break;
                case NoteGrade.Miss:
                    material.color = Color.red;
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
            //explosionAnim.Update(0f);
            explosionAnim.SetBool("IsHit", false);
        }
    }
}

