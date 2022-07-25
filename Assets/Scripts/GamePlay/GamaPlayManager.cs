using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamaPlayManager : MonoBehaviour
{
    public Plutono.Song.Note note;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EventHandler.CallHitNoteEvent(note);
        }
    }
}
