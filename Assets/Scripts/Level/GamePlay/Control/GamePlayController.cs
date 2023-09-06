using System;
using Plutono.GamePlay;
using Plutono.IO;
using Plutono.Song;
using Plutono.GamePlay.Notes;
using Plutono.Util;
using UnityEngine;

namespace Plutono.Level.GamePlay
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

        #region UnityEvent

        /// Event
        private void OnEnable()
        {
            EventHandler.BeforeSceneLoadedEvent += OnBeforeSceneLoadedEvent;
            EventCenter.AddListener<GamePrepareEvent>(_ => { OnGamePrepare(); });
            EventCenter.AddListener<GameStartEvent>(OnGameStart);
            EventCenter.AddListener<GameResumeEvent>(OnGameResume);
            EventCenter.AddListener<GamePauseEvent>(OnGamePause);

            EventCenter.AddListener<NoteClearEvent<BlankNote>>(OnNoteClear);
            EventCenter.AddListener<NoteClearEvent<PianoNote>>(OnNoteClear);
            EventCenter.AddListener<NoteClearEvent<SlideNote>>(OnNoteClear);
            EventCenter.AddListener<NoteMissEvent<BlankNote>>(OnNoteMiss);
            EventCenter.AddListener<NoteMissEvent<PianoNote>>(OnNoteMiss);
            EventCenter.AddListener<NoteMissEvent<SlideNote>>(OnNoteMiss);
        }

        private void OnDisable()
        {
            EventHandler.BeforeSceneLoadedEvent -= OnBeforeSceneLoadedEvent;
            EventCenter.RemoveListener<GamePrepareEvent>(_ => { OnGamePrepare(); });
            EventCenter.RemoveListener<GameStartEvent>(OnGameStart);
            EventCenter.RemoveListener<GameResumeEvent>(OnGameResume);
            EventCenter.RemoveListener<GamePauseEvent>(OnGamePause);

            EventCenter.RemoveListener<NoteClearEvent<BlankNote>>(OnNoteClear);
            EventCenter.RemoveListener<NoteClearEvent<PianoNote>>(OnNoteClear);
            EventCenter.RemoveListener<NoteClearEvent<SlideNote>>(OnNoteClear);
            EventCenter.RemoveListener<NoteMissEvent<BlankNote>>(OnNoteMiss);
            EventCenter.RemoveListener<NoteMissEvent<PianoNote>>(OnNoteMiss);
            EventCenter.RemoveListener<NoteMissEvent<SlideNote>>(OnNoteMiss);
        }

        protected override void Awake()
        {
            base.Awake();

            //TODO: Player Setting
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

            //Synchronize
            //TODO: Player Setting
            configGlobalChartOffset = 0f;
            configChartMusicOffset = 0f;
        }

        private void Start()
        {
            OnGamePrepare();
        }

        private void Update()
        {
            if (!Status.IsStarted)
                return;

            //Synchronize Time
            if (!Status.IsPaused)
                SynchronizeTime();

            //End Game
            //if (Status.ClearCount == ChartDetail.noteDetails.Count)
            if (musicSource.clip.length - musicSource.time <= 0.05f)
            {
                Status.IsCompleted = true;
                EventCenter.Broadcast(new GameClearEvent());
                EventHandler.CallTransitionEvent("Result");
            }
        }

        #endregion

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
                Debug.Log("--SynchronizeTime--\nStarOrResumeTime: " + StartOrResumeTime + " DspTime: " + curDspTime
                    + " CurTime: " + CurTime + " musicTime: " + musicSource.time);
#endif
            }
            else
            {
                CurTime += Time.unscaledDeltaTime;
            }
        }
        
        private void OnGamePrepare()
        {
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
        }

        private void OnGameStart(GameStartEvent evt)
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
            Debug.Log($"StarOrResumeTime: {StartOrResumeTime} DspTime: {curDspTime} musicStartTime: {musicStartTime}\n" +
                      $"globalLatency/musicPlayingDelay: {musicPlayingDelay} chartMusicOffset: {configChartMusicOffset} ConfigChartOffset: {configGlobalChartOffset}"
                      );
#endif
        }

        private void OnGamePause(GamePauseEvent evt)
        {
            Debug.Log("GamePlayControl OnGamePause");

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

        private void OnGameResume(GameResumeEvent evt)
        {
            Debug.Log("GamePlayControl OnGameResume");
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
#if DEBUG
            Debug.Log("GamePlayControl On NoteClearEvent\n" +
                      $"Note: {evt.Note.id} Time: {evt.Note.time} CurTime: {CurTime} NoteJudge: {Status.GetJudgment(evt.Note.id).Grade}");
#endif
        }

        private void OnNoteClear(NoteClearEvent<SlideNote> evt)
        {
            Status.CalculateScore(evt.Note.id, Status.Mode == GameMode.Autoplay ? NoteGrade.Perfect : evt.Grade);
#if DEBUG
            Debug.Log("GamePlayControl On NoteClearEvent\n" +
                      $"Note: {evt.Note.id} Time: {evt.Note.time} CurTime: {CurTime} NoteJudge: {Status.GetJudgment(evt.Note.id).Grade}");
#endif
        }

        private void OnNoteMiss(NoteMissEvent<BlankNote> evt)
        {
            Status.CalculateScore(evt.Note.id, NoteGrade.Miss);
        }

        private void OnNoteMiss(NoteMissEvent<PianoNote> evt)
        {
            Status.CalculateScore(evt.Note.id, NoteGrade.Miss);
#if DEBUG
            Debug.Log("GamePlayControl On NoteMissEvent\n" +
                      $"Note: {evt.Note.id} Time: {evt.Note.time} CurTime: {CurTime} NoteJudge: {Status.GetJudgment(evt.Note.id).Grade}");
#endif
        }

        private void OnNoteMiss(NoteMissEvent<SlideNote> evt)
        {
            Status.CalculateScore(evt.Note.id, NoteGrade.Miss);
#if DEBUG
            Debug.Log("GamePlayControl On NoteMissEvent\n" +
                      $"Note: {evt.Note.id} Time: {evt.Note.time} CurTime: {CurTime} NoteJudge: {Status.GetJudgment(evt.Note.id).Grade}");
#endif
        }
    }
}