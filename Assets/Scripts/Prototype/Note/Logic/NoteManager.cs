using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plutono.Song
{
    public class NoteManager : MonoBehaviour
    {
        public Note notePrefab;
        public Transform noteParent;
        
        private void OnEnable()
        {
            EventHandler.InstantiateLevel += OnInstantiateLevel;
            EventHandler.HitNoteEvent += OnHitNoteEvent;
        }

        private void OnDisable()
        {
            EventHandler.InstantiateLevel -= OnInstantiateLevel;
            EventHandler.HitNoteEvent -= OnHitNoteEvent;
        }

        private void OnInstantiateLevel(ChartDetails chartDetails)
        {
            foreach (var note in chartDetails.notes)
            {
                notePrefab._details = note;
                Instantiate(notePrefab,new Vector3(note.pos * 10, 0, note.time * Settings.maximumNoteRange), Quaternion.identity, noteParent);
            }
        }
        private void OnHitNoteEvent(Note note)
        {
            Destroy(note);
        }

    }
}
