using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Plutono.Song;

public class GamaPlayManager : MonoBehaviour
{
    private float time = 0f;
    
    public Note note;
    public ChartDetails chartDetails;
    // Start is called before the first frame update
    void Start()
    {
        chartDetails.notes.Add(new NoteDetails
        {
            id = 1,
            pos = 0,
            size = 1.2f,
            time = 1,
            type = NoteType.Blank,
        });
        chartDetails.notes.Add(new NoteDetails
        {
            id = 2,
            pos = 1,
            size = 1.2f,
            time = 1,
            type = NoteType.Slide,
        });
        chartDetails.notes.Add(new NoteDetails
        {
            id = 3,
            pos = -1,
            size = 1.2f,
            time = 1,
            type = NoteType.Piano,
        });
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.G))
        {
            EventHandler.CallInstantiateLevel(chartDetails);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EventHandler.CallHitNoteEvent(note);
        }
    }
}
