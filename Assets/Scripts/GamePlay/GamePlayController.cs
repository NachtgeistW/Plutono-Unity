using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Plutono.Song;

public class GamePlayController : MonoBehaviour
{
    //[HideInInspector]
    public float time = 0f;
    
    public Note note;
    public GameStatus Status { get; set; }
    public ChartDetail ChartDetail { get; set; }


    void Awake()
    {
        ChartDetail = new ChartDetail
        {
            level = "10 Ω·‘¬‘µ"
        };
        ChartDetail.noteDetails.Add(new NoteDetail
        {
            id = 1,
            pos = 0,
            size = 1.2f,
            time = 1,
            type = NoteType.Blank,
        });
        ChartDetail.noteDetails.Add(new NoteDetail
        {
            id = 2,
            pos = 1,
            size = 1.2f,
            time = 1,
            type = NoteType.Slide,
        });
        ChartDetail.noteDetails.Add(new NoteDetail
        {
            id = 3,
            pos = -1,
            size = 1.2f,
            time = 1,
            type = NoteType.Piano,
        });
        
        Status = new GameStatus(this, GameMode.Floro);
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.G))
        {
            EventHandler.CallInstantiateLevel(ChartDetail);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EventHandler.CallHitNoteEvent(note, time, Status);
        }
    }
}
