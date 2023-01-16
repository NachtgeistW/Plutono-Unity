using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Plutono.Song;
using Plutono.IO;
using System;
using System.Linq;

public class GamePlayController : Singleton<GamePlayController>
{
    [Header("-Specify in Inspector-")]
    public string StoragePath;
    public int songIndex;
    public int chartIndex;
    public double chartLatency;
    public double songLatency;

    //Load File, delete them after testing
    [HideInInspector] public List<SongDetail> songSourceList;
    private LoadFiles loadFiles;

    //[HideInInspector]
    [Header("-Time-")]
    [SerializeField]
    private double curTime = 0f;
    public double StarOrResumeTime { get; internal set; }

    // Synchronize
    private double lastDspTime = -1;
    private const int SynchronizationWaitingFrames = 1200;
    private int passedFrameBeforeSynchronization = 0;
    
    [Header("-Status-")]
    public List<Note> notesOnScreen;
    public GameStatus Status{ get; internal set; }
    
    public ChartDetail ChartDetail { get; internal set; }
    public SongDetail SongSource { get; internal set; }

    public int lastApperanceNoteIndex = 0;
    private const double GenerationWaitingTime = 1.0;
    public double passedTimeBeforeGeneration = 0;

    [Header("-Audio-")]
    public AudioSource audioSource;
    public double playStartTime;

    private void OnEnable()
    {
        EventHandler.GamePauseEvent += OnGamePauseEvent;
        EventHandler.GameResumeEvent += OnGameResumeEvent;
        EventHandler.GameRestartEvent += OnGameRestartEvent;
        EventHandler.BeforeSceneLoadedEvent += OnBeforeSceneLoadedEvent;
    }

    private void OnDisable()
    {
        EventHandler.GamePauseEvent -= OnGamePauseEvent;
        EventHandler.GameResumeEvent -= OnGameResumeEvent;
        EventHandler.GameRestartEvent -= OnGameRestartEvent;
        EventHandler.BeforeSceneLoadedEvent -= OnBeforeSceneLoadedEvent;
    }

    private void Awake()
    {
        //Load File, delete them after testing
        loadFiles = new LoadFiles();
        loadFiles.RequestReadPermission();
        loadFiles.Initialize(StoragePath).ForEach(song => songSourceList.Add(new SongDetail(song)));

        //Song source
        SongSource = songSourceList[songIndex];
        //SongSource = songSourceList[2]; //overtune
        ChartDetail = SongSource.ChartDetails[chartIndex];

        //Status
        Status = new GameStatus(this, GameMode.Pluvo)
        {
            ChartPlaySpeed = 5.5f
        };

        //Audio
        audioSource.clip = AudioClipFileManager.Read(SongSource.MusicPath);
    }

    private void Start()
    {
        //Time
        StarOrResumeTime = Time.realtimeSinceStartup;
        playStartTime = StarOrResumeTime + songLatency;

        //Audio
        audioSource.PlayScheduled(AudioSettings.dspTime + playStartTime);
    }

    // Update is called once per frame
    private void Update()
    {
        //Time
        if (!Status.IsPaused)
        {
            curTime += Time.unscaledDeltaTime;
            passedTimeBeforeGeneration -= Time.unscaledDeltaTime;
        }

        //Synchronize

        //Note
        ///Generate notes according to the time
        List<NoteDetail> notesToGenerate = new();
        while (lastApperanceNoteIndex < ChartDetail.noteDetails.Count)
        {
            if (ChartDetail.noteDetails[lastApperanceNoteIndex].time < curTime - StarOrResumeTime - chartLatency)
            {
                notesToGenerate.Add(ChartDetail.noteDetails[lastApperanceNoteIndex]);
                lastApperanceNoteIndex++;
            }
            else
                break;
        }
        EventHandler.CallInstantiateNote(notesToGenerate, notesOnScreen);
        passedTimeBeforeGeneration = GenerationWaitingTime;

        ///Recycle Miss Note
        foreach (var note in notesOnScreen.ToList())
        {
            if (note.transform.position.z == 0)
            {
                EventHandler.CallMissNoteEvent(notesOnScreen, note, curTime, Status);
                Status.Judge(note._details, NoteGrade.Miss, 0);
            }
        }
        
        ///Judge Note
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var note = SearchForBestNote(curTime);
            EventHandler.CallHitNoteEvent(notesOnScreen, note, curTime, Status);
            
            var grade = NoteGradeJudgment.JudgeNoteGrade(note._details, curTime, Status.Mode);
            Status.Judge(note._details, grade, 0);
        }

        //End Game
        if (Status.ClearCount == ChartDetail.noteDetails.Count)
        {
            Status.IsCompleted = true;
            //EventHandler.CallGameClearEvent();
            EventHandler.CallTransitionEvent("Result");
        }
        
    }

    /// <summary>
    /// Search for the best note when player hit the screen
    /// </summary>
    /// <param name="touchTime">the time when player touches the screen</param>
    Note SearchForBestNote(double touchTime)
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
            var curDeltaTime = Math.Abs(touchTime - curDetectingNote._details.time);
            var bestDeltaTime = Math.Abs(touchTime - bestNote._details.time);
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

    private double GetClosedBestNoteRange(double timeRange)
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

    private void OnGamePauseEvent()
    {
        //If game is clear or failed, do nothing
        if (Status.IsCompleted || Status.IsFailed)
            return;

        //Status
        Status.IsPaused = true;

        //Time
        Time.timeScale = 0;

        //Audio
        audioSource.Pause();
    }

    private void OnGameResumeEvent()
    {
        //TODO: Wait for a few minutes
        //If game is clear or failed, do nothing
        if (Status.IsCompleted || Status.IsFailed)
            return;
        //Status
        Status.IsPaused = false;

        //Time
        Time.timeScale = 1;

        //Audio
        audioSource.Play();
    }

    private void OnGameRestartEvent()
    {
        throw new NotImplementedException();
    }

    private void OnBeforeSceneLoadedEvent()
    {
        //throw new NotImplementedException();
    }
}
