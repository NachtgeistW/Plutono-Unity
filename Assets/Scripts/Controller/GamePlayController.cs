/*
 * class GamePlayController -- Control the events happened on game.
 *
 * History
 *      2021.04.04  CREATE.
 *      2021.04.15  RENAME to PlayingController.
 *      2021.10.17  MOVE CalculateBasicScore and ComboScore to Class GameStatus.
 *      2021.11.04  RENAME to GamePlayController.
    */

using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Controller.Game;
using Assets.Scripts.Model.Plutono;
using Assets.Scripts.Util;
using Controller.Game;
using Lean.Touch;
using Model.Plutono;
using Models.IO;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Views;

namespace Assets.Scripts.Controller
{
    public class GamePlayController : MonoBehaviour
    {
        private Camera cam;

        public GameStatus Status { get; set; }

        //UI
        [Header("-UI-")]
        public UIController UiController;

        public PackInfo PackInfo { get; set; }
        public GameChartModel ChartInfo { get; set; }

        [Header("-Note controlling-")]
        //-Note controlling- --Chlorie
        [SerializeField] private Transform noteParentTransform;
        [SerializeField] private NoteSpawner NoteSpawner;
        private List<BlankNote> BlankNotes = new();
        private List<PianoNote> PianoNotes = new();
        private List<SlideNote> SlideNotes = new();

        //-Editor settings- --Chlorie
        public int chartPlaySpeed = 10; //Twice the speed of that in the game

        public Button buttonMenu;

        //Object Pool
        private ObjectPool<GameNote> notePool;

        //-Sounds-
        public AudioSource musicSource;

        //Music Playing
        public float MusicLength { get; set; } // In seconds
        private bool musicPlayState;
        private bool isMusicEnds;

        //Judge System
        public const float YRayStart = -0.40f;
        private const float YRayEnd = -0.60f;

        //Synchronize
        public bool ResynchronizeChartOnNextFrame { get; set; }
        public float MusicStartedTime { get; set; }
        public float StartOrResumeTime { get; set; }
        public float Time { get; set; }
        public float MusicOffset { get; set; }
        public float Latency { get; set; }

        private void Awake()
        {
            GameManager.Instance.playingController = this;

            PackInfo = GameManager.Instance.packInfo;
            ChartInfo = GameManager.Instance.gameChart;
            Application.targetFrameRate = 120;
        }

        private void Start()
        {

            //Game Status
            Time = 0;
            Status = new GameStatus(this, GameMode.Arbo)
            {
                ChartPlaySpeed = 12
            };
            StartOrResumeTime = UnityEngine.Time.realtimeSinceStartup;

            //Screen
            //Judge System
            cam = Camera.main;
            //notes.Sort(SortByTime);
            //LeanTouch.OnFingerTap += OnFingerTap;
            NoteSpawner.PlaceNewNote(BlankNotes, PianoNotes, SlideNotes, ChartInfo);

            //Music
            InitializeMusicSource();
            var nowDspTime = AudioSettings.dspTime;
            musicSource.PlayScheduled(nowDspTime + 1);

            //UI
            UiController.InitializeUi(PackInfo.songName, ChartInfo.level, MusicLength);
        }

        private void Update()
        {
            //TODO:latency adjust system

            if (Time > MusicLength) EndGame();
            UiController.OnGameUpdate(Time, Status);

/*            if (notes.Count == 0)
                isCompleted = true;
*/            //ClearMissNote();

            SynchronizeMusic();
        }

        private void OnDestroy()
        {
            //solve the memory leak problem
            //notePool.Dispose();
        }

        //Game Float
        private void EndGame()
        {
            GameManager.Instance.pCount = Status.pCount;
            GameManager.Instance.gCount = Status.gCount;
            GameManager.Instance.bCount = Status.bCount;
            GameManager.Instance.mCount = Status.mCount;
            GameManager.Instance.score = Status.BasicScore;
            GameManager.Instance.bonus = Status.ComboScore;
            SceneManager.LoadScene("ResultScene");
        }

        // Synchronize
        private double lastDspTime = -1;
        private const int SynchronizationWaitingFrames = 1200;
        private int passedFrameBeforeSynchronization = SynchronizationWaitingFrames;
        private void SynchronizeMusic()
        {
            passedFrameBeforeSynchronization--;
            var resumeElapsedTime = UnityEngine.Time.realtimeSinceStartup - StartOrResumeTime;
            var curDspTime = AudioSettings.dspTime;
            if (ResynchronizeChartOnNextFrame || passedFrameBeforeSynchronization <= 0 || resumeElapsedTime < 0.5f && Math.Abs(lastDspTime - curDspTime) > 0)
            {
                Time = Time + MusicOffset - Latency - MusicStartedTime;
                lastDspTime = curDspTime;
                ResynchronizeChartOnNextFrame = false;
                passedFrameBeforeSynchronization = SynchronizationWaitingFrames;
            }
            else
            {
                Time += UnityEngine.Time.unscaledDeltaTime;
            }
        }
/*
        //Judge System
        private static int SortByTime(NoteView noteView1, NoteView noteView2)
        {
            if (noteView1.note.time > noteView2.note.time)
                return 1;
            if (noteView1.note.time < noteView2.note.time)
                return -1;
            return 0;
        }

        
        private void ClearMissNote()
        {
            if (notes.Count == 0) return;
            var note = notes.First();
            if (!note.IsNoteShouldBeClear) return;
            note.gameObject.SetActive(false);
            notes.Remove(note);
            comboCount = 0;
            CalculateComboScore(Judgment.Miss);
            mCount++;
            uiController.ShowLate();
        }

        public void OnFingerTap(LeanFinger finger)
        {
            if (isCompleted || isFailed) return;
            var point = cam.ScreenPointToRay(finger.ScreenPosition);

            //judge line range
            if (point.direction.y > YRayStart || point.direction.y < YRayEnd)
            {
                Debug.Log("You click at " + point.direction.y + ". Out of judge line.");
                return;
            }

            var note = SearchForBestNote(Time);
            if (note == null)
                if (isCompleted)
                    return;

            //too early
            if (note.note.time > time + Parameters.BadDeltaTime)
            {
                Debug.Log("You click at " + time + ", which should be " + note.note.time + ". You are too early.");
                return;
            }

            //miss, remove, it shouldn't be there now
            if (note._note.time < time - Parameters.BadDeltaTime)
            {
                Debug.Log("You click at " + time + ", which should be " + note._note.time + ". You miss it.");
                note.gameObject.SetActive(false);
                notes.Remove(note);
                _comboCount = 0;
                mCount++;
                return;
            }
            var deltaTouchTime = Mathf.Abs(note.note.time - time);
            if (deltaTouchTime < Parameters.PerfectDeltaTime)
            {
                note.gameObject.SetActive(false);
                notes.Remove(note);
                comboCount++;
                pCount++;
                CalculateComboScore(Judgment.Perfect);
                Debug.Log("You click at " + time + ", which should be " + note.note.time + ". Perfect.");
                return;
            }

            if (deltaTouchTime < Parameters.GoodDeltaTime)
            {
                note.gameObject.SetActive(false);
                notes.Remove(note);
                comboCount++;
                gCount++;
                CalculateComboScore(Judgment.Good);
                if (note.note.time - time < 0)
                    uiController.ShowEarly();
                else
                    uiController.ShowLate();
                Debug.Log("You click at " + time + ", which should be " + note.note.time + ". Good.");
                return;
            }

            if (deltaTouchTime < Parameters.BadDeltaTime)
            {
                note.gameObject.SetActive(false);
                notes.Remove(note);
                comboCount = 0;
                bCount++;
                CalculateComboScore(Judgment.Bad);
                if (note.note.time - time < 0)
                    uiController.ShowEarly();
                else
                    uiController.ShowLate();
                Debug.Log("You click at " + time + ", which should be " + note.note.time + ". Bad.");
            }
        }

        private static float GetClosedBestNoteRange(float timeRange)
        {
            if (timeRange < Parameters.PerfectDeltaTime) return Parameters.PerfectDeltaTime;
            return timeRange < Parameters.GoodDeltaTime ? Parameters.GoodDeltaTime : Parameters.BadDeltaTime;
        }

        private NoteView SearchForBestNote(float touchTime)
        {
            if (notes.Count == 0)
                return null;

            NoteView bestNote = null;
            foreach (var curNote in notes)
            {
                if (bestNote == null)
                    bestNote = notes.First();
                //note range
                                var x = point.direction.x * 50;
                                if (x > note.touchableRightRange || x < note.touchableLeftRange)
                                {
                                    Debug.Log("You click at " + point.direction.x * 50 + ". Its left range is "+ note.touchableLeftRange + " and its right range is " + note.touchableRightRange + ". Out of note.");
                                    return;
                                }
                
                var curDeltaTime = Mathf.Abs(curNote.note.time - touchTime);
                var bestDeltaTime = Mathf.Abs(bestNote.note.time - touchTime);
                if (curDeltaTime < bestDeltaTime)
                {
                    bestNote = curNote;
                    continue;
                }

                if (curDeltaTime > GetClosedBestNoteRange(bestDeltaTime))
                    return bestNote;
            }

            //没找到，应该为miss
            return null;
        }
*/
        //Music
        private void InitializeMusicSource()
        {
            var filePath = "file://" + GameManager.Instance.songPath + "/music.mp3";

            musicSource.clip = AudioClipFileManager.Read(filePath);
            MusicLength = musicSource.clip.length;
            musicSource.time = 0;
        }
    }
}