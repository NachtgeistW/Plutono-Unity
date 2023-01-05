using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Plutono.Song;
using Plutono.IO;
using System;
using System.Linq;

public class GamePlayController : Singleton<GamePlayController>
{
    //Load File, delete them after testing
    public string StoragePath;
    public List<Plutono.Song.SongDetail> songSourceList;
    private LoadFiles loadFiles;

    //[HideInInspector]
    [Header("-Time-")]
    public float curTime = 0f;
    public float StarOrResumeTime { get; set; }

    // Synchronize
    private double lastDspTime = -1;
    private const int SynchronizationWaitingFrames = 1200;
    public int passedFrameBeforeSynchronization = 0;
    
    [Header("-Status-")]
    public List<Note> notesOnScreen;
    public GameStatus Status{ get; private set; }
    
    public ChartDetail ChartDetail { get; set; }
    public SongDetail SongSource { get; set; }
    
    public ChartDetail tempChartDetail;
    
    public int lastApperanceNoteIndex = 0;
    private const float GenerationWaitingTime = 1.0f;
    public float passedTimeBeforeGeneration = 0;

    [Header("-Audio-")]
    public AudioSource audioSource;
    public float playLatency;
    public float playStartTime;


    private void Awake()
    {
        //Load File, delete them after testing
        loadFiles = new LoadFiles();
        loadFiles.RequestReadPermission();
        loadFiles.Initialize(StoragePath).ForEach(song => songSourceList.Add(new SongDetail(song)));

        //Song source
        SongSource = songSourceList[2];
        ChartDetail = SongSource.ChartDetails[0];

        tempChartDetail = ChartDetail;

        //Status
        Status = new GameStatus(this, GameMode.Pluvo)
        {
            ChartPlaySpeed = 5.5f
        };

        //Audio
        audioSource.clip = AudioClipFileManager.Read(SongSource.MusicPath);

        //Time
        StarOrResumeTime = Time.realtimeSinceStartup;
        playStartTime = StarOrResumeTime + playLatency;
    }

    private void Start()
    {
        //Audio
        audioSource.PlayScheduled(playStartTime);
        
        //EventHandler.CallInstantiateNote(ChartDetail.noteDetails, noteObjectsOnScreen);
    }

    // Update is called once per frame
    void Update()
    {
        //Time
        curTime += Time.unscaledDeltaTime;
        passedTimeBeforeGeneration -= Time.unscaledDeltaTime;

        //Note
        //Generate notes according to the time
        List<NoteDetail> notesToGenerate = new();
        while (lastApperanceNoteIndex < tempChartDetail.noteDetails.Count)
        {
            if (tempChartDetail.noteDetails[lastApperanceNoteIndex].time < curTime - playStartTime)
            {
                notesToGenerate.Add(tempChartDetail.noteDetails[lastApperanceNoteIndex]);
                lastApperanceNoteIndex++;
            }
            else
                break;
        }
        EventHandler.CallInstantiateNote(notesToGenerate, notesOnScreen);
        passedTimeBeforeGeneration = GenerationWaitingTime;

        //Recycle Miss Note
        foreach (var note in notesOnScreen.ToList())
        {
            if (note.transform.position.z == 0)
            {
                EventHandler.CallMissNoteEvent(notesOnScreen, note, curTime, Status);
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var note = SearchForBestNote(curTime);
            EventHandler.CallHitNoteEvent(notesOnScreen, note, curTime, Status);
            
            var grade = NoteGradeJudgment.JudgeNoteGrade(note._details, curTime, Status.Mode);
            Status.Judge(note._details, grade, 0);
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
