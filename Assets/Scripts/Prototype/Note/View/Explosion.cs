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
        private void OnEnable()
        {
            EventHandler.HitNoteEvent += OnHitNoteEvent;
        }

        private void OnDisable()
        {
            EventHandler.HitNoteEvent -= OnHitNoteEvent;
        }

        private void OnHitNoteEvent(List<Note> notesOnScreen, Note note, double curGameTime, GameStatus status)
        {
            //The Animation Event that be fired after playing explosion animation 
            var explosionAnimEvent = new AnimationEvent
            {
                functionName = nameof(ExecuteAfterNoteAnimate),
                objectReferenceParameter = note,
                //The time waiting for the animation to finish
                time = Settings.noteAnimationPlayingTime
            };

            explosionAnim = note.GetComponent<Animator>();
            var clip = explosionAnim.runtimeAnimatorController.animationClips[0];
            clip.AddEvent(explosionAnimEvent);
            explosionAnim.SetBool("IsHit", true);
        }

        // the function to be called as an event
        private void ExecuteAfterNoteAnimate(Note note)
        {
            explosionAnim.Rebind();
            //explosionAnim.Update(0f);
            explosionAnim.SetBool("IsHit", false);
            EventHandler.CallExecuteActionAfterNoteAnimate(note);
        }
    }
}

