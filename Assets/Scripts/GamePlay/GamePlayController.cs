using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Plutono.Song;
using System;

public class GamePlayController : MonoBehaviour
{
    //[HideInInspector]
    public float curTime = 0f;
    public float StarOrResumeTime { get; set; }
    
    public List<Note> notesOnScreen;
    public GameStatus Status;
    public ChartDetail ChartDetail { get; set; }

    System.Random rnd;

    private void Awake()
    {
        // rnd = new System.Random();
        // ChartDetail = new ChartDetail
        // {
        //     level = "10 结月缘"
        // };
        // ChartDetail.noteDetails.Add(new NoteDetail
        // {
        //     id = 1,
        //     pos = rnd.Next(-2, 2),
        //     size = 1.2f,
        //     time = 1,
        //     type = NoteType.Blank,
        // });
        // ChartDetail.noteDetails.Add(new NoteDetail
        // {
        //     id = 2,
        //     pos = rnd.Next(-2, 2),
        //     size = 1.2f,
        //     time = 2,
        //     type = NoteType.Slide,
        // });
        // ChartDetail.noteDetails.Add(new NoteDetail
        // {
        //     id = 3,
        //     pos = rnd.Next(-2, 2),
        //     size = 1.2f,
        //     time = 3,
        //     type = NoteType.Piano,
        // });

        Status = new GameStatus(this, GameMode.Pluvo);
    }

    private void Start()
    {
        StarOrResumeTime = Time.realtimeSinceStartup;
    }
    // Update is called once per frame
    void Update()
    {
        curTime += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.G))
        {
            ChartDetail.noteDetails[0].pos = rnd.Next(-2, 2);
            ChartDetail.noteDetails[1].pos = rnd.Next(-2, 2);
            ChartDetail.noteDetails[2].pos = rnd.Next(-2, 2);

            EventHandler.CallInstantiateLevel(ChartDetail.noteDetails, notesOnScreen);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var note = SearchForBestNote(curTime);
            EventHandler.CallHitNoteEvent(notesOnScreen, note, curTime, Status);
        }
    }

    /// <summary>
    /// Search for the best note when player hit the screen
    /// </summary>
    /// <param name="touchTime">the time when player touches the screen</param>
    Note SearchForBestNote(float touchTime)
    {
        if (notesOnScreen.Count == 0)
        {
            return null;
        }
        Note bestNote = null;
        foreach (var curDetectingNote in notesOnScreen)
        {
            if (bestNote == null)
                bestNote = curDetectingNote;
            var curDeltaTime = Mathf.Abs(touchTime - curDetectingNote._details.time);
            var bestDeltaTime = Mathf.Abs(touchTime - bestNote._details.time);
            if (bestDeltaTime > curDeltaTime)
            {
                bestNote = curDetectingNote;
                continue;
            }
            if (curDeltaTime > GetClosedBestNoteRange(bestDeltaTime))
                return bestNote;
        }
        //Bestnote not found, return null
        return null;
    }

    private float GetClosedBestNoteRange(float timeRange)
    {
        return Status.Mode switch
        {
            GameMode.Stelo => timeRange switch
            {
                <= Settings.SteloMode.perfectDeltaTime => Settings.SteloMode.perfectDeltaTime,
                <= Settings.SteloMode.goodDeltaTime => Settings.SteloMode.goodDeltaTime,
                _ => Settings.SteloMode.badDeltaTime
            },
            GameMode.Arbo or GameMode.Pluvo => timeRange switch
            {
                <= Settings.ArboMode.perfectDeltaTime => Settings.ArboMode.perfectDeltaTime,
                <= Settings.ArboMode.goodDeltaTime => Settings.ArboMode.goodDeltaTime,
                _ => Settings.ArboMode.badDeltaTime
            },
            //TODO: Finish other mode.
            GameMode.Persona => throw new NotImplementedException(),
            GameMode.Ekzerco => throw new NotImplementedException(),
            _ => throw new NotImplementedException(),
        };
    }
}
