/*
 * class SongSelectSceneController -- Control the events happened on game.
 *
 * History
 *      2021.04.04  CREATE.
 *      2021.04.15  RENAME to PlayingController.
 */

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Assets.Scripts.Util;
using Lean.Touch;
using Model.Plutono;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Util.FileManager;
using Views;

namespace Controller
{
    public class PlayingController : MonoBehaviour
    {
        public bool IsGamePlaying { get; private set; }

        [Header("-Text-")]

        [SerializeField] private Text textReady;

        [SerializeField] private Text textSongName;
        [SerializeField] private Text textScore;
        [SerializeField] private Text textLevel;
        [SerializeField] private Text textMode;

        [SerializeField] private Text textCombo;
        [SerializeField] private Text textComboCount;

        [SerializeField] private Text textEarly;
        [SerializeField] private Text textLate;

        private PackInfo _packInfo;
        private GameChart _chartInfo;

        [Header("-Note controlling-")]

        //-Note controlling- --Chlorie
        private uint prevNoteID;
        private uint returnNoteID;
        private List<uint> inGameNoteIDs = new List<uint>();
        private List<NoteView> notes = new List<NoteView>();
        [SerializeField] private Transform _noteParentTransform;

        //-Editor settings- --Chlorie
        public int chartPlaySpeed = 10; //Twice the speed of that in the game
        private uint _comboCount;

        public Button buttonMenu;

        //Object Pool
        [SerializeField] private bool collectionChecks = true;
        [SerializeField] private int maxPoolSize = 20;
        private ObjectPool<NoteView> _notePool;

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
        private bool isCompleted = false;
        private bool isFailed = false;

        private int pCount = 0;  //perfect
        private int gCount = 0;  //good
        private int bCount = 0;  //bad
        private int mCount = 0;  //miss
        private int noteCount;
        private int bonus;
        private int score = 0;
        private int xRayScale = 50;
        public const float YRayStart = -0.45f;
        private const float YRayEnd = -0.55f;
        private float StartTime;
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
            _packInfo = GameManager.Instance.packInfo;
            _chartInfo = GameManager.Instance.gameChart;
            noteCount = GameManager.Instance.gameChart.notes.Count;
        }

        void Start()
        {
            //UI
            InitializeUI();

            //Screen
            _notePool = new ObjectPool<NoteView>(OnCreatePooledItem, OnTakeFromPool, OnReturnToPool, OnDestroyPoolObject, collectionChecks, 10, maxPoolSize);
            PlaceNewNote();

            //Judge System
            cam = Camera.main;
            notes.Sort(SortByTime);
            StartTime = Time.time;
            Offset = 0.21f;

            LeanTouch.OnFingerTap += OnFingerTap;

            //Music
            InitializeMusicSource();
            musicSource.Play();
        }

        void Update()
        {
            //TODO:latency adjust system
            var musicSourceTime = musicSource.time;
            timeSlider.value = musicSourceTime - Offset;
            time = musicSourceTime;

            if (Math.Abs(musicSourceTime - musicLength) < 0.01)
            {
                EndGame();
            }

            ClearMissNote();

            score = CalculateJudgmentPoint();
            ChangeScoreText();

            if (_comboCount > 5)
                showCombo();
            else
                hideCombo();
        }

        private void OnDestroy()
        {
            //solve the memory leak problem
            _notePool.Dispose();
        }

        //Game Float
        private void EndGame()
        {
            GameManager.Instance.pCount = pCount;
            GameManager.Instance.gCount = gCount;
            GameManager.Instance.bCount = bCount;
            GameManager.Instance.mCount = mCount;
            GameManager.Instance.score = score;
            GameManager.Instance.bonus = bonus;
            SceneManager.LoadScene("ResultScene");
        }

        //Judge System
        private static int SortByTime(NoteView noteView1, NoteView noteView2)
        {
            if (noteView1._note.time > noteView2._note.time)
                return 1;
            else if (noteView1._note.time < noteView2._note.time)
                return -1;
            else
                return 0;
        }

        private void ClearMissNote()
        {
            if (notes.Count == 0) return;
            var note = notes.First();
            if (!note.IsZEqualsToZero) return;
            note.gameObject.SetActive(false);
            notes.Remove(note);
            _comboCount = 0;
            CalculateBonus(Judgment.Miss);
            mCount++;
        }

        public void OnFingerDown(LeanFinger finger)
        {
            if (isCompleted || isFailed) return;
            var point = cam.ScreenToWorldPoint(finger.ScreenPosition);
        }

        public void OnFingerUp(LeanFinger finger)
        {
            if (isCompleted || isFailed) return;
        }

        public void OnFingerTap(LeanFinger finger)
        {
            //if (isCompleted || isFailed) return;
            var point = cam.ScreenPointToRay(finger.ScreenPosition);

            //judge line range
            if (point.direction.y > YRayStart || point.direction.y < YRayEnd)
            {
                Debug.Log("You click at " + point.direction.y + ". Out of judge line.");
                return;
            }

            NoteView note = SearchForBestNote(time);
            //note range
            /*                var x = point.direction.x * 50;
                            if (x > note.rightRange || x < note.leftRange)
                            {
                                Debug.Log("You click at " + point.direction.x * 50 + ". Its left range is "+ note.leftRange + " and its right range is " + note.rightRange + ". Out of note.");
                                return;
                            }
            */
            if (note == null)
                if (isCompleted) return;

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
                Debug.Log("You click at " + time + ", which should be " + note._note.time + ". Perfect.");
                note.gameObject.SetActive(false);
                notes.Remove(note);
                _comboCount++;
                pCount++;
                CalculateBonus(Judgment.Perfect);
                return;
            }

            if (deltaTouchTime < Parameters.GoodDeltaTime)
            {
                Debug.Log("You click at " + time + ", which should be " + note._note.time + ". Good.");
                note.gameObject.SetActive(false);
                notes.Remove(note);
                _comboCount++;
                gCount++;
                return;
            }
            if (deltaTouchTime < Parameters.BadDeltaTime)
            {
                Debug.Log("You click at " + time + ", which should be " + note._note.time + ". Bad.");
                note.gameObject.SetActive(false);
                notes.Remove(note);
                _comboCount = 0;
                bCount++;
                return;
            }
        }

        private float GetClosedBestNoteRange(float timeRange)
        {
            if (timeRange < Parameters.PerfectDeltaTime) return Parameters.PerfectDeltaTime;
            else if (timeRange < Parameters.GoodDeltaTime) return Parameters.GoodDeltaTime;
            else return Parameters.BadDeltaTime;
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

        public int CalculateJudgmentPoint()
        {
            return (int)(0.9 * (1000000 * (pCount + 0.7 * gCount + 0.3 * bCount) / noteCount));
        }
        public int CalculateBonus(Judgment judgment)
        {
            switch (judgment)
            {
                case Judgment.Perfect:
                    bonus += 2048 / Mathf.Min(1024, noteCount);
                    break;
                case Judgment.Good:
                    bonus += 1024 / Mathf.Min(1024, noteCount);
                    break;
                case Judgment.Bad:
                case Judgment.Miss:
                    bonus -= 8192 / Mathf.Min(1024, noteCount);
                    break;
                default:
                    throw new Exception("unknown judgment");
            }

            if (bonus < 0) bonus = 0;       //completely closed
            if (bonus > 1024) bonus = 1024; //completely opened
            return bonus;
        }

        //Music
        private void InitializeMusicSource()
        {
            musicSource.clip = AudioClipFileManager.Read(GameManager.Instance.songPath + "\\music.mp3");
            musicLength = musicSource.clip.length;
            timeSlider.maxValue = musicLength;
            musicSource.time = 0;
        }

        //Note
        private void PlaceNewNote()
        {
            foreach (var note in _chartInfo.notes)
                InitNoteObject(note);
        }

        private void InitNoteObject(GameNote note)
        {
            var newNote = _notePool.Get();
            newNote.SetNoteAppearance(note);
            notes.Add(newNote);
        }

        private NoteView OnCreatePooledItem()
        {
            var newNote = Instantiate(prefabNoteView, _noteParentTransform);
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
        void OnDestroyPoolObject(NoteView note)
        {
            Destroy(note.gameObject);
        }

        //UI
        private void InitializeUI()
        {
            textSongName.text = _packInfo.songName;
            textScore.text = Convert.ToString(0);
            textLevel.text = "Lv." + _chartInfo.level;
            _comboCount = 0;

            hideCombo();
            hideEarly();
            hideLate();
        }

        public void hideCombo()
        {
            textCombo.enabled = false;
            textComboCount.enabled = false;
            textComboCount.text = Convert.ToString(_comboCount);
        }

        public void showCombo()
        {
            textCombo.enabled = true;
            textComboCount.enabled = true;
            textComboCount.text = Convert.ToString(_comboCount);
        }

        public void hideEarly()
        {
            textEarly.enabled = false;
        }

        public void showEarly()
        {
            textEarly.enabled = true;
        }

        public void hideLate()
        {
            textLate.enabled = false;
        }

        public void showLate()
        {
            textLate.enabled = true;
        }
        private void ChangeScoreText()
        {
            textScore.text = Convert.ToString(score);
        }
    }
}
