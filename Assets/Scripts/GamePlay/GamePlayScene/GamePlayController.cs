using Lean.Touch;
using Plutono.IO;
using Plutono.Song;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Plutono.GamePlay
{
    public class GamePlayController : Singleton<GamePlayController>
    {
        public Camera camera;

        [Header("-Specify in Inspector-")]
        public string StoragePath;
        public int songIndex;
        public int chartIndex;

        //[HideInInspector]
        //[Header("-Time and Synchronize-")]
        [SerializeField] public double CurTime { get; private set; }
        public float StartOrResumeTime { get; internal set; }
        public float ResumeElapsedTime { get; internal set; }
        private double musicPlayingDelay;
        private double lastDspTime = -1;
        private double curDspTime = -1;
        private double NoteGenerationLeadTime;
        private int ticksBeforeSynchronization = 600;

        private double configChartMusicOffset = 0;
        private float configGlobalChartOffset;

        [Header("-Status-")]
        public List<Note> notesOnScreen;
        public GameStatus Status { get; internal set; }
        public PlayerSettings_Global_SO PlayerSetting { get; private set; }
        public bool IsStatusLoaded { get; private set; } = false;

        [Header("-Note and Chart-")]
        [SerializeField] private NoteController noteController;
        [SerializeField] private ExplosionController explosionController;
        [SerializeField] private HitController hitController;
        public ChartDetail ChartDetail { get; internal set; }
        public SongDetail SongSource { get; internal set; }

        public int lastApperanceNoteIndex = 0;

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
        }

        private void OnDisable()
        {
            EventHandler.GameStartEvent -= OnGameStartEvent;
            EventHandler.GamePauseEvent -= OnGamePauseEvent;
            EventHandler.GameResumeEvent -= OnGameResumeEvent;
            EventHandler.GameRestartEvent -= OnGameRestartEvent;
            EventHandler.BeforeSceneLoadedEvent -= OnBeforeSceneLoadedEvent;
            DisableInput();
        }

        private void Start()
        {
            PlayerSetting = PlayerSettingsManager.Instance.PlayerSettings_Global_SO;
            // System config
            Application.targetFrameRate = 120;

            //Song source
            songIndex = SongSelectDataTransformer.SelectedSongIndex;
            chartIndex = SongSelectDataTransformer.SelectedChartIndex;
            SongSource = FileManager.Instance.songSourceList[songIndex];
            ChartDetail = SongSource.ChartDetails[chartIndex];

            //Audio
            musicSource.clip = AudioClipFileManager.Read(SongSource.MusicPath);
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
                EnableInput();
            }

            //Synchronize
            configGlobalChartOffset = PlayerSetting.globalChartOffset;
            configChartMusicOffset = PlayerSetting.chartMusicOffset;
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
            NoteGenerationLeadTime = Settings.NoteFallTime(Status.ChartPlaySpeed);

            Status.IsStarted = true;

#if DEBUG
            Debug.Log("StarOrResumeTime: " + StartOrResumeTime + " DspTime: " + curDspTime + " musicStartTime: " + musicStartTime);
            Debug.Log("globalLatency/musicPlayingDelay: " + musicPlayingDelay + " chartMusicOffset: " + configChartMusicOffset + " ConfigChartOffset: " + configGlobalChartOffset);
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
            ///Generate notes according to the time
            GenerateNote();
            foreach (var note in notesOnScreen)
            {
                note.UpdatePosition(Status.ChartPlaySpeed, CurTime);
            }

            if (Status.Mode == GameMode.Autoplay)
            {
                foreach (var note in notesOnScreen.ToList())
                {
                    if (note.transform.position.z <= Settings.judgeLinePosition)
                    {
#if DEBUG
                        if (note._details.id % 50 == 1)
                        {
                            Debug.Log("noteId: " + note._details.id + " noteTime: " + note._details.time + " CurTime: " + CurTime + " musicTime: " + musicSource.time);
                        }
#endif
                        Status.Judge(note._details, NoteGrade.Perfect);
                        noteController.OnHitNote(notesOnScreen, note);
                        explosionController.OnHitNote(note, NoteGrade.Perfect);
                        EventHandler.CallHitNoteEvent(notesOnScreen, note, CurTime, NoteGrade.Perfect);
                    }
                }
            }
            else
            {
                RecycleMissNote(CurTime);
            }

            //End Game
            //if (Status.ClearCount == ChartDetail.noteDetails.Count)
            if (musicSource.clip.length - musicSource.time <= 0.5f)
            {
                Status.IsCompleted = true;
                EventHandler.CallGameClearEvent();
                EventHandler.CallTransitionEvent("Result");
            }
        }


        private void GenerateNote()
        {
            List<NoteDetail> notesToGenerate = new();
            while (lastApperanceNoteIndex < ChartDetail.noteDetails.Count)
            {
                //添加生成的提前量
                var nextNoteTime = ChartDetail.noteDetails[lastApperanceNoteIndex].time;
                if (nextNoteTime - (CurTime + NoteGenerationLeadTime) < 0.01
                    || CurTime + NoteGenerationLeadTime >= nextNoteTime)
                {
                    notesToGenerate.Add(ChartDetail.noteDetails[lastApperanceNoteIndex]);
                    lastApperanceNoteIndex++;
                }
                else
                    break;
            }
            noteController.InstantiateNote(notesToGenerate, notesOnScreen);
        }

        private void SynchronizeTime()
        {
            ticksBeforeSynchronization--;
            ResumeElapsedTime = Time.realtimeSinceStartup - StartOrResumeTime;
            curDspTime = AudioSettings.dspTime;
            // Sync: every 600 ticks (=10 seconds) and every tick within the first 0.5 seconds after start/resume
            if ((ticksBeforeSynchronization <= 0 || ResumeElapsedTime < 0.5f)
                && lastDspTime != curDspTime)
            {
                ticksBeforeSynchronization = 600;
                lastDspTime = curDspTime;
                CurTime = (float)curDspTime - musicStartTime - configGlobalChartOffset + configChartMusicOffset;
#if DEBUG
                Debug.Log("--SynchronizeTime--");
                Debug.Log("StarOrResumeTime: " + StartOrResumeTime + " DspTime: " + curDspTime
                    + " CurTime: " + CurTime + " musicTime: " + musicSource.time);
#endif
            }
            else
            {
                CurTime += Time.unscaledDeltaTime;
            }
        }

        ///Recycle Miss Note
        private void RecycleMissNote(double CurTime)
        {
            foreach (var note in notesOnScreen.ToList())
            {
                if (note.transform.position.z <= Settings.recycleNotePosition)
                {
                    Status.Judge(note._details, NoteGrade.Miss);
                    noteController.OnMissNote(notesOnScreen, note);
                    EventHandler.CallMissNoteEvent(notesOnScreen, note, CurTime, NoteGrade.Miss);
#if DEBUG
                    Debug.Log("--OnNoteMiss--");
                    Debug.Log("CurTime: " + CurTime +" Note: " + note._details.id + " Time: " + note._details.time + " Pos: " + note._details.pos 
                    + " Judge Size: " + (note._details.size < 1.2 ? 2.4 : note._details.size * 2));
#endif                    
                }
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
                DisableInput();
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
            notesOnScreen.Clear();
            ResultDataTransformer.BasicScore = Status.BasicScore;
            ResultDataTransformer.ComboScore = Status.ComboScore;
            ResultDataTransformer.PCount = Status.pCount;
            ResultDataTransformer.GCount = Status.gCount;
            ResultDataTransformer.BCount = Status.bCount;
            ResultDataTransformer.MCount = Status.mCount;
        }

        //Input
        public void EnableInput()
        {
            LeanTouch.OnFingerDown += OnFingerDown;
            LeanTouch.OnFingerUpdate += OnFingerUpdate;
            LeanTouch.OnFingerUp += OnFingerUp;
        }

        public void DisableInput()
        {
            LeanTouch.OnFingerDown -= OnFingerDown;
            LeanTouch.OnFingerUpdate -= OnFingerUpdate;
            LeanTouch.OnFingerUp -= OnFingerUp;
        }

        protected void OnFingerDown(LeanFinger finger)
        {            
            if (!hitController.TryHitNote(finger, CurTime, out Note note)) return;

            var grade = NoteGradeJudgment.Judge(note._details, CurTime, Status.Mode);
            var result = Status.Judge(note._details, grade);
            if (result == NoteJudgmentResult.Succeeded)
            {
                noteController.OnHitNote(notesOnScreen, note);
                explosionController.OnHitNote(note, grade);
                EventHandler.CallHitNoteEvent(notesOnScreen, note, CurTime, grade);
#if DEBUG
                if (grade < NoteGrade.Perfect)
                {
                    var pos = camera.ScreenToWorldPoint(new Vector3(finger.ScreenPosition.x, finger.ScreenPosition.y, camera.nearClipPlane));
                    Debug.Log("--OnFingerDown--");
                    Debug.Log("Finger: " + finger.Index + " ScreenPos:" + finger.ScreenPosition + " Pos:" + pos);
                    Debug.Log("CurTime: " + CurTime +" Note: " + note._details.id + " Time: " + note._details.time + " Pos: " + note._details.pos 
                    + " Judge Size: " + (note._details.size < 1.2 ? 2.4 : note._details.size * 2));
                }
#endif
            }
        }

        private void OnFingerUpdate(LeanFinger finger)
        {
            if (finger.Index == -42) return;
            if (!hitController.TryHitNote(finger, CurTime, out Note note)) return;

            if (note._details.type != NoteType.Slide)
                return;
            if (note._details.time - CurTime < Settings.SteloMode.perfectDeltaTime)
            {
                var grade = NoteGradeJudgment.Judge(note._details, CurTime, Status.Mode);
                var result = Status.Judge(note._details, grade);
                if (result == NoteJudgmentResult.Succeeded)
                {
                    noteController.OnHitNote(notesOnScreen, note);
                    explosionController.OnHitNote(note, grade);
                    EventHandler.CallHitNoteEvent(notesOnScreen, note, CurTime, grade);
#if DEBUG
                    if (grade < NoteGrade.Perfect)
                    {
                        var pos = camera.ScreenToWorldPoint(new Vector3(finger.ScreenPosition.x, finger.ScreenPosition.y, camera.nearClipPlane));
                        Debug.Log("--OnFingerUpdate--");
                        Debug.Log("Finger: " + finger.Index + " ScreenPos:" + finger.ScreenPosition + " Pos:" + pos);
                        Debug.Log("CurTime: " + CurTime +" Note: " + note._details.id + " Time: " + note._details.time + " Pos: " + note._details.pos 
                        + " Judge Size: " + (note._details.size < 1.2 ? 2.4 : note._details.size * 2));
                    }
#endif
                }
            }
        }

        private void OnFingerUp(LeanFinger finger)
        {

        }
    }


    public class GameEvent : UnityEvent<GamePlayController>
    {
    }

    public class NoteEvent : UnityEvent<GamePlayController, Note>
    {
    }

    //public class NoteJudgeEvent : UnityEvent<GamePlayController, Note, JudgeData>
    //{
    //}
}

