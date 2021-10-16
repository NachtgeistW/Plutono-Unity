/*
 * class PlayingController -- Control the events happened on game.
 *
 * History
 *      2021.04.04  CREATE.
 *      2021.04.15  RENAME to PlayingController.
 */

using System;
using System.Collections.Generic;
using System.Linq;
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

namespace Controller
{
    public class PlayingController : MonoBehaviour
    {
        public bool IsGamePlaying { get; private set; }

        //UI
        public UIController uiController;

        private PackInfo packInfo;
        private GameChartModel chartInfo;

        [Header("-Note controlling-")]

        //-Note controlling- --Chlorie
        private readonly List<NoteView> notes = new List<NoteView>();
        [SerializeField] private Transform noteParentTransform;

        //-Editor settings- --Chlorie
        public int chartPlaySpeed = 10; //Twice the speed of that in the game
        private uint comboCount = 0;

        public Button buttonMenu;

        //Object Pool
        [SerializeField] private bool collectionChecks = true;
        [SerializeField] private int maxPoolSize = 20;
        private ObjectPool<NoteView> notePool;

        [Tooltip("(放prefab不是script！)note prefab。")]
        [SerializeField] private NoteView prefabNoteView;

        public Slider timeSlider;

        //-Sounds-
        public AudioSource musicSource;

        //Music Playing
        public float musicLength; // In seconds
        private bool musicPlayState;
        private bool isMusicEnds;

        //Judge System
        private bool isCompleted;
        private readonly bool isFailed = false;

        private int pCount; //perfect
        private int gCount; //good
        private int bCount; //bad
        private int mCount; //miss
        private int noteCount;
        private int comboScore;
        private int basicScore;
        public const float YRayStart = -0.40f;
        private const float YRayEnd = -0.60f;
        private float startTime;
        private float time;
        public float Offset { get; set; }

        private Camera cam;

        public enum Judgment
        {
            Perfect,
            Good,
            Bad,
            Miss
        }

        // Start is called before the first frame update
        private void Awake()
        {
            Application.targetFrameRate = 120;

            GameManager.Instance.playingController = this;
            packInfo = GameManager.Instance.packInfo;
            chartInfo = GameManager.Instance.gameChart;
            noteCount = GameManager.Instance.gameChart.notes.Count;
        }

        private void Start()
        {
            //UI
            uiController.InitializeUI(packInfo.songName, chartInfo.level);

            //Screen
            notePool = new ObjectPool<NoteView>(OnCreatePooledItem, OnTakeFromPool, OnReturnToPool,
                OnDestroyPoolObject, collectionChecks, 10, maxPoolSize);
            PlaceNewNote();

            //Judge System
            cam = Camera.main;
            notes.Sort(SortByTime);

            LeanTouch.OnFingerTap += OnFingerTap;

            //Music
            InitializeMusicSource();
            var nowDspTime = AudioSettings.dspTime;
            musicSource.PlayScheduled(nowDspTime + 1);
            startTime = -1f;
        }

        private void Update()
        {
            //TODO:latency adjust system
            startTime += Time.unscaledDeltaTime;
            var musicSourceTime = startTime;
            timeSlider.value = musicSourceTime - Offset;
            time = musicSourceTime;

            if (musicSourceTime > musicLength) EndGame();

            if (notes.Count == 0)
                isCompleted = true;
            ClearMissNote();

            basicScore = CalculateBasicScore();
            uiController.ChangeScoreText(basicScore);

            if (comboCount > 5)
                uiController.ShowCombo(comboCount);
            else
                uiController.HideCombo();

            uiController.HideEarly();
            uiController.HideLate();
        }

        private void OnDestroy()
        {
            //solve the memory leak problem
            notePool.Dispose();
        }

        //Game Float
        private void EndGame()
        {
            GameManager.Instance.pCount = pCount;
            GameManager.Instance.gCount = gCount;
            GameManager.Instance.bCount = bCount;
            GameManager.Instance.mCount = mCount;
            GameManager.Instance.score = basicScore;
            GameManager.Instance.bonus = comboScore;
            SceneManager.LoadScene("ResultScene");
        }

        //Judge System
        private static int SortByTime(NoteView noteView1, NoteView noteView2)
        {
            if (noteView1._note.time > noteView2._note.time)
                return 1;
            if (noteView1._note.time < noteView2._note.time)
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

            var note = SearchForBestNote(time);
            if (note == null)
                if (isCompleted)
                    return;

            //too early
            if (note._note.time > time + Parameters.BadDeltaTime)
            {
                Debug.Log("You click at " + time + ", which should be " + note._note.time + ". You are too early.");
                return;
            }

            //miss, remove, it shouldn't be there now
            /*if (note._note.time < time - Parameters.BadDeltaTime)
            {
                Debug.Log("You click at " + time + ", which should be " + note._note.time + ". You miss it.");
                note.gameObject.SetActive(false);
                notes.Remove(note);
                _comboCount = 0;
                mCount++;
                return;
            }*/
            var deltaTouchTime = Mathf.Abs(note._note.time - time);
            if (deltaTouchTime < Parameters.PerfectDeltaTime)
            {
                note.gameObject.SetActive(false);
                notes.Remove(note);
                comboCount++;
                pCount++;
                CalculateComboScore(Judgment.Perfect);
                Debug.Log("You click at " + time + ", which should be " + note._note.time + ". Perfect.");
                return;
            }

            if (deltaTouchTime < Parameters.GoodDeltaTime)
            {
                note.gameObject.SetActive(false);
                notes.Remove(note);
                comboCount++;
                gCount++;
                CalculateComboScore(Judgment.Good);
                if (note._note.time - time < 0)
                    uiController.ShowEarly();
                else
                    uiController.ShowLate();
                Debug.Log("You click at " + time + ", which should be " + note._note.time + ". Good.");
                return;
            }

            if (deltaTouchTime < Parameters.BadDeltaTime)
            {
                note.gameObject.SetActive(false);
                notes.Remove(note);
                comboCount = 0;
                bCount++;
                CalculateComboScore(Judgment.Bad);
                if (note._note.time - time < 0)
                    uiController.ShowEarly();
                else
                    uiController.ShowLate();
                Debug.Log("You click at " + time + ", which should be " + note._note.time + ". Bad.");
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
/*                var x = point.direction.x * 50;
                if (x > note.touchableRightRange || x < note.touchableLeftRange)
                {
                    Debug.Log("You click at " + point.direction.x * 50 + ". Its left range is "+ note.touchableLeftRange + " and its right range is " + note.touchableRightRange + ". Out of note.");
                    return;
                }
*/
                var curDeltaTime = Mathf.Abs(curNote._note.time - touchTime);
                var bestDeltaTime = Mathf.Abs(bestNote._note.time - touchTime);
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

        public int CalculateBasicScore()
        {
            return (int) (0.9 * (1000000 * (pCount + 0.7 * gCount + 0.3 * bCount) / noteCount));
        }

        public int CalculateComboScore(Judgment judgment)
        {
            switch (judgment)
            {
                case Judgment.Perfect:
                    comboScore += 2048 / Mathf.Min(1024, noteCount);
                    break;
                case Judgment.Good:
                    comboScore += 1024 / Mathf.Min(1024, noteCount);
                    break;
                case Judgment.Bad:
                case Judgment.Miss:
                    comboScore -= 4096 / Mathf.Min(1024, noteCount);
                    break;
                default:
                    throw new Exception("unknown judgment");
            }

            if (comboScore < 0) comboScore = 0; //completely closed
            if (comboScore > 1024) comboScore = 1024; //completely opened
            return comboScore;
        }

        //Music
        private void InitializeMusicSource()
        {
            var filePath = "file://" + GameManager.Instance.songPath + "/music.mp3";

            musicSource.clip = AudioClipFileManager.Read(filePath);
            musicLength = musicSource.clip.length;
            timeSlider.maxValue = musicLength;
            musicSource.time = 0;
        }

        //Note
        private void PlaceNewNote()
        {
            foreach (var note in chartInfo.notes)
                InitNoteObject(note);
        }

        private void InitNoteObject(GameNoteModel note)
        {
            var newNote = notePool.Get();
            newNote.SetNoteAppearance(note);
            notes.Add(newNote);
        }

        private NoteView OnCreatePooledItem()
        {
            var newNote = Instantiate(prefabNoteView, noteParentTransform);
            newNote.gameObject.SetActive(false);
            return newNote;
        }

        // Called when an item is returned to the pool using Release
        private void OnReturnToPool(NoteView note)
        {
            note.gameObject.SetActive(false);
        }

        // Called when an item is taken from the pool using Get
        private void OnTakeFromPool(NoteView note)
        {
            note.gameObject.SetActive(true);
        }

        // If the pool capacity is reached then any items returned will be destroyed.
        // We can control what the destroy behavior does, here we destroy the GameObject.
        private void OnDestroyPoolObject(NoteView note)
        {
            Destroy(note.gameObject);
        }
    }
}