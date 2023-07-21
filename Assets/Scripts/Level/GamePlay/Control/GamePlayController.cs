using Plutono.IO;
using Plutono.Song;
using System;
using System.Collections.Generic;
using System.Linq;
using Plutono.GamePlay.Control;
using Plutono.GamePlay.Notes;
using Plutono.Level;
using Plutono.Level.GamePlay;
using Plutono.Util;
using UnityEngine;

namespace Plutono.GamePlay
{
    public class GamePlayController : Singleton<GamePlayController>
    {
        private int songIndex;
        private int chartIndex;

        public double CurTime { get; private set; }
        public float StartOrResumeTime { get; internal set; }
        public float ResumeElapsedTime { get; internal set; }
        private double musicPlayingDelay;
        private double lastDspTime = -1;
        private double curDspTime = -1;
        private int ticksBeforeSynchronization = 600;

        private double configChartMusicOffset = 0;
        private float configGlobalChartOffset;

        public bool IsStatusLoaded { get; private set; }
        [field:SerializeField] public GameStatus Status { get; internal set; }

        public ChartDetail ChartDetail { get; internal set; }
        public SongDetail SongSource { get; internal set; }

        [Space(10)]
        [Header("-Audio-")]
        public AudioSource musicSource;
        public double musicStartTime;

        /// Event
        private void OnEnable()
        {
            EventHandler.GameStartEvent += OnGameStartEvent;
            EventHandler.GamePauseEvent += OnGamePauseEvent;
            EventHandler.GameResumeEvent += OnGameResumeEvent;
            EventHandler.GameRestartEvent += OnGameRestartEvent;
            EventHandler.BeforeSceneLoadedEvent += OnBeforeSceneLoadedEvent;

            EventCenter.AddListener<NoteClearEvent<BlankNote>>(OnNoteClear);
            EventCenter.AddListener<NoteClearEvent<PianoNote>>(OnNoteClear);
            EventCenter.AddListener<NoteClearEvent<SlideNote>>(OnNoteClear);
            EventCenter.AddListener<NoteMissEvent<BlankNote>>(OnNoteMiss);
            EventCenter.AddListener<NoteMissEvent<PianoNote>>(OnNoteMiss);
            EventCenter.AddListener<NoteMissEvent<SlideNote>>(OnNoteMiss);
        }

        private void OnDisable()
        {
            EventHandler.GameStartEvent -= OnGameStartEvent;
            EventHandler.GamePauseEvent -= OnGamePauseEvent;
            EventHandler.GameResumeEvent -= OnGameResumeEvent;
            EventHandler.GameRestartEvent -= OnGameRestartEvent;
            EventHandler.BeforeSceneLoadedEvent -= OnBeforeSceneLoadedEvent;
        }

        private void Start()
        {
            //PlayerSetting = PlayerSettingsManager.Instance.PlayerSettings_Global_SO;
            // System config
            Application.targetFrameRate = 120;

            //Song source
            songIndex = SongSelectDataTransformer.SelectedSongIndex;
            chartIndex = SongSelectDataTransformer.SelectedChartIndex;
            SongSource = FileManager.Instance.songSourceList[songIndex];
            ChartDetail = SongSource.chartDetails[chartIndex];

            //Audio
            musicSource.clip = AudioClipFileManager.Read(SongSource.musicPath);
            musicSource.time = 0;

            //Status
            Status = new GameStatus(this, SongSelectDataTransformer.GameMode)
            {
                ChartPlaySpeed = SongSelectDataTransformer.ChartPlaySpeed,
            };
            IsStatusLoaded = true;

            //Input
            if (Status.Mode != GameMode.Autoplay)
            {
                //EnableInput();
            }

            //Synchronize
            configGlobalChartOffset = 0f;
            configChartMusicOffset = 0f;
            //TODO: Player Setting
            //configGlobalChartOffset = PlayerSetting.globalChartOffset;
            //configChartMusicOffset = PlayerSetting.chartMusicOffset;
        }

        private void OnGameStartEvent()
        {
            curDspTime = AudioSettings.dspTime;
            musicPlayingDelay = 1.0f;
            musicStartTime = curDspTime + musicPlayingDelay;
            musicSource.PlayScheduled(musicStartTime);

            //Time
            StartOrResumeTime = Time.realtimeSinceStartup;
            CurTime = 0;

            Status.IsStarted = true;

#if DEBUG
            // Debug.Log("StarOrResumeTime: " + StartOrResumeTime + " DspTime: " + curDspTime + " musicStartTime: " + musicStartTime);
            // Debug.Log("globalLatency/musicPlayingDelay: " + musicPlayingDelay + " chartMusicOffset: " + configChartMusicOffset + " ConfigChartOffset: " + configGlobalChartOffset);
#endif
        }

        private void Update()
        {
            if (!Status.IsStarted)
                return;

            //Synchronize Time
            if (!Status.IsPaused)
                SynchronizeTime();

            //Note

//            if (Status.Mode == GameMode.Autoplay)
//            {
//                foreach (var note in notesOnScreen.ToList())
//                {
//                    if (note.transform.position.z <= Settings.judgeLinePosition)
//                    {
//#if DEBUG
//                        if (note.details.id % 50 == 1)
//                        {
//                            Debug.Log("noteId: " + note.details.id + " noteTime: " + note.details.time + " CurTime: " + CurTime + " musicTime: " + musicSource.time);
//                        }
//#endif
//                        Status.CalculateScore(note.details, NoteGrade.Perfect);
//                        BlankNoteSpawnerClient.OnNoteClear(notesOnScreen, note);
//                        explosionController.OnHitNote(note, NoteGrade.Perfect);
//                        EventHandler.CallHitNoteEvent(notesOnScreen, note, CurTime, NoteGrade.Perfect);
//                    }
//                }
//            }

            //End Game
            //if (Status.ClearCount == ChartDetail.noteDetails.Count)
            if (musicSource.clip.length - musicSource.time <= 0.5f)
            {
                Status.IsCompleted = true;
                EventHandler.CallGameClearEvent();
                EventHandler.CallTransitionEvent("Result");
            }
        }


        private void SynchronizeTime()
        {
            ticksBeforeSynchronization--;
            ResumeElapsedTime = Time.realtimeSinceStartup - StartOrResumeTime;
            curDspTime = AudioSettings.dspTime;
            // Sync: every 600 ticks (=10 seconds) and every tick within the first 0.5 seconds after start/resume
            if ((ticksBeforeSynchronization <= 0 || ResumeElapsedTime < 0.5f)
                && Math.Abs(lastDspTime - curDspTime) > 0.001)
            {
                ticksBeforeSynchronization = 600;
                lastDspTime = curDspTime;
                CurTime = (float)curDspTime - musicStartTime - configGlobalChartOffset + configChartMusicOffset;
#if DEBUG
                // Debug.Log("--SynchronizeTime--");
                // Debug.Log("StarOrResumeTime: " + StartOrResumeTime + " DspTime: " + curDspTime
                //     + " CurTime: " + CurTime + " musicTime: " + musicSource.time);
#endif
            }
            else
            {
                CurTime += Time.unscaledDeltaTime;
            }
        }

        private void OnGamePauseEvent()
        {
            //If game is clear or failed, do nothing
            if (Status.IsCompleted || Status.IsFailed)
                return;

            //Time
            Time.timeScale = 0;

            //Status
            Status.IsPaused = true;

            //Hit
            if (Status.Mode != GameMode.Autoplay)
            {
                //DisableInput();
            }

            //Audio
            musicSource.Pause();

        }

        private void OnGameResumeEvent()
        {
            //If game is clear or failed, do nothing
            if (Status.IsCompleted || Status.IsFailed)
                return;

            //Status
            Status.IsPaused = false;

            //Time
            StartOrResumeTime = Time.realtimeSinceStartup;
            Time.timeScale = 1;

            //Audio
            musicSource.Play();
        }

        private void OnGameRestartEvent()
        {
            throw new NotImplementedException();
        }

        private void OnBeforeSceneLoadedEvent()
        {
            ResultDataTransformer.BasicScore = Status.BasicScore;
            ResultDataTransformer.ComboScore = Status.ComboScore;
            ResultDataTransformer.PCount = Status.pCount;
            ResultDataTransformer.GCount = Status.gCount;
            ResultDataTransformer.BCount = Status.bCount;
            ResultDataTransformer.MCount = Status.mCount;
        }

        private void OnNoteClear(NoteClearEvent<BlankNote> evt)
        {
            Status.CalculateScore(evt.Note.id, Status.Mode == GameMode.Autoplay ? NoteGrade.Perfect : evt.Grade);
        }
        private void OnNoteClear(NoteClearEvent<PianoNote> evt)
        {
            Status.CalculateScore(evt.Note.id, Status.Mode == GameMode.Autoplay ? NoteGrade.Perfect : evt.Grade);
        }
        private void OnNoteClear(NoteClearEvent<SlideNote> evt)
        {
            Status.CalculateScore(evt.Note.id, Status.Mode == GameMode.Autoplay ? NoteGrade.Perfect : evt.Grade);
        }

        private void OnNoteMiss(NoteMissEvent<BlankNote> evt)
        {
            Status.CalculateScore(evt.Note.id, NoteGrade.Miss);
        }

        private void OnNoteMiss(NoteMissEvent<PianoNote> evt)
        {
            Debug.Log("GamePlayControl OnNoteMiss noteId: " + evt.Note.id);

            Status.CalculateScore(evt.Note.id, NoteGrade.Miss);
        }

        private void OnNoteMiss(NoteMissEvent<SlideNote> evt)
        {
            Status.CalculateScore(evt.Note.id, NoteGrade.Miss);
        }
    }
}