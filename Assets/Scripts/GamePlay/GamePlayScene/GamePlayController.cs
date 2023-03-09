using System.Collections.Generic;
using UnityEngine;
using Plutono.Song;
using Plutono.IO;
using System;
using System.Linq;
using Lean.Touch;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;

namespace Plutono.GamePlay
{
    public class GamePlayController : Singleton<GamePlayController>
    {
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
            DisableInput();
        }

        private void Awake()
        {
            PlayerSetting = PlayerSettingsManager.Instance.PlayerSettings_Global_SO;
            // System config
            Application.targetFrameRate = 120;
        }

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            //Song source
            songIndex = SongSelectDataTransformer.SelectedSongIndex;
            chartIndex = SongSelectDataTransformer.SelectedChartIndex;
            SongSource = FileManager.Instance.songSourceList[songIndex];
            ChartDetail = SongSource.ChartDetails[chartIndex];

            //Audio
            musicSource.clip = AudioClipFileManager.Read(SongSource.MusicPath);
            musicSource.time = 0;
            curDspTime = AudioSettings.dspTime;
            musicPlayingDelay = 1.0f;
            musicStartTime = curDspTime + musicPlayingDelay;
            musicSource.PlayScheduled(musicStartTime);

            //Status
            GameMode gameMode = SongSelectDataTransformer.GameMode;
            Status = new GameStatus(this, gameMode)
            {
                ChartPlaySpeed = SongSelectDataTransformer.ChartPlaySpeed,
            };

            //Input
            if (gameMode != GameMode.Autoplay)
            {
                EnableInput();
            }

            //Synchronize
            configGlobalChartOffset = PlayerSetting.globalChartOffset;
            configChartMusicOffset = PlayerSetting.chartMusicOffset;

            //Time
            StartOrResumeTime = Time.realtimeSinceStartup;
            CurTime = 0;
            NoteGenerationLeadTime = Settings.NoteFallTime(Status.ChartPlaySpeed);

#if DEBUG
            Debug.Log("StarOrResumeTime: " + StartOrResumeTime + " DspTime: " + curDspTime + " musicStartTime: " + musicStartTime);
            Debug.Log("globalLatency/musicPlayingDelay: " + musicPlayingDelay + " chartMusicOffset: " + configChartMusicOffset + " ConfigChartOffset: " + configGlobalChartOffset);
#endif
        }

        private void Update()
        {
            //Synchronize Time
            if (!Status.IsPaused)
            {
                SynchronizeTime();
            }

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
            DisableInput();

            //Audio
            musicSource.Pause();

            if (Status.Mode != GameMode.Autoplay)
            {
                DisableInput();
            }
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

        /// <summary>
        /// Search for the best note when player hit the screen
        /// </summary>
        /// <param name="touchTime">the time when player touches the screen</param>
        Note SearchForBestNoteOnTime(double touchTime)
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
                GameMode.Arbo or GameMode.Floro => timeRange switch
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
            if (!hitController.IsHittedNote(finger, out Note note)) return;

            var grade = NoteGradeJudgment.JudgeNoteGrade(note._details, CurTime, Status.Mode);
            var result = Status.Judge(note._details, grade);
            if (result == NoteJudgmentResult.Succeeded)
            {
                noteController.OnHitNote(notesOnScreen, note);
                explosionController.OnHitNote(note, grade);
                EventHandler.CallHitNoteEvent(notesOnScreen, note, CurTime, grade);
                Debug.Log("OnFingerDown" + " Finger: " + finger.Index + " Note: " + note._details.id + " Note Type: " + note._details.type + " Note Time: " + note._details.time + " CurTime: " + CurTime);
            }
        }

        private void OnFingerUpdate(LeanFinger finger)
        {
            if (finger.Index == -42) return;
            if (!hitController.IsHittedNote(finger, out Note note)) return;

            if (note._details.type == NoteType.Slide)
            {
                if (note._details.time - CurTime < Settings.SteloMode.perfectDeltaTime)
                {
                    var grade = NoteGradeJudgment.JudgeNoteGrade(note._details, CurTime, Status.Mode);
                    var result = Status.Judge(note._details, grade);
                    if (result == NoteJudgmentResult.Succeeded)
                    {
                        noteController.OnHitNote(notesOnScreen, note);
                        explosionController.OnHitNote(note, grade);
                        EventHandler.CallHitNoteEvent(notesOnScreen, note, CurTime, grade);
                        Debug.Log("OnFingerUpdate" + " Finger: " + finger.Index + " Note: " + note._details.id + " Note Type: " + note._details.type + " Note Time: " + note._details.time + " CurTime: " + CurTime);
                    }
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

