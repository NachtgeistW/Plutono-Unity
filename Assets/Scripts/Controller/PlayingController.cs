/*
 * class SongSelectSceneController -- Control the events happened on game.
 *
 * History
 *      2021.04.04  CREATE.
 *      2021.04.15  RENAME to PlayingController.
 */

using System;
using System.Collections.Generic;
using System.IO;
using Assets.Scripts.Model.Plutono;
using Assets.Scripts.Util;
using Assets.Scripts.Views;
using Controller.Game;
using Model.Plutono;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;
using Views;

namespace Controller
{
    public class PlayingController : MonoBehaviour
    {
        public bool IsGamePlaying { get; private set; }

        [SerializeField] private Text textSongName;
        [SerializeField] private Text textScore;
        [SerializeField] private Text textLevel;
        [SerializeField] private Text textMode;

        [SerializeField] private uint _comboCount;
        [SerializeField] private Text textCombo;
        [SerializeField] private Text textComboCount;

        [SerializeField] private Text textEarly;
        [SerializeField] private Text textLate;

        [SerializeField] private PackInfo _packInfo;
        [SerializeField] private GameChart _chartInfo;

        //-Note controlling- --Chlorie
        [SerializeField] private uint prevNoteID;
        [SerializeField] private uint returnNoteID;
        [SerializeField] private List<uint> inGameNoteIDs = new List<uint>();
        [SerializeField] private List<NoteView> notes = new List<NoteView>();
        [SerializeField] private Transform _noteParentTransform;

        //-Editor settings- --Chlorie
        public int chartPlaySpeed = 10; //Twice the speed of that in the game

        //Object Pool
        [SerializeField] private bool collectionChecks = true;
        [SerializeField] private int maxPoolSize = 20;
        private ObjectPool<NoteView> notePool;

        [Header("(放prefab不是script！)note prefab。")]
        [SerializeField] private NoteView prefabNoteView;

        public Slider timeSlider;

        //-Sounds-
        public AudioSource musicSource;

        //Music Playing
        public float musicLength; // In seconds
        private bool musicPlayState;
        private bool isMusicEnds;

        // Start is called before the first frame update
        private void Awake()
        {
            _packInfo = GameManager.Instance.packInfo;
            _chartInfo = GameManager.Instance.gameChart;
            GameManager.Instance.playingController = this;
        }

        void Start()
        {
            InitializeUI();
            notePool = new ObjectPool<NoteView>(OnCreatePooledItem, OnTakeFromPool, OnReturnToPool, OnDestroyPoolObject, collectionChecks, 10, maxPoolSize);
            PlaceNewNote();
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        private void OnDestroy()
        {
            //solve the memory leak problem
            notePool.Dispose();
        }

        //Note
        private void PlaceNewNote()
        {
            foreach (var note in _chartInfo.notes)
            {
                InitNoteObject(note);
            }
        }

        private void InitNoteObject(GameNote note)
        {
            var newNote = notePool.Get();
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
        }

        public void showCombo()
        {
            textCombo.enabled = true;
            textComboCount.enabled = true;
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
            /*            if (_chartInfo.notes.Count <= 1) return;
                        int ttl = inGameNoteIDs[_chartInfo.notes.Count - 1] + 1, cur = 0;
                        if (prevNoteID > 0) cur = inGameNoteIDs[prevNoteID] + 1;
                        var score = (int)((800000.0 * cur * (ttl - 1) + 200000.0 * cur * (cur - 1)) / (ttl * (ttl - 1)));
                        int intPart = score / 10000, decPart = (score / 100) % 100;
                        var str = "" + intPart;
                        if (decPart >= 10) str += "." + decPart + " %"; else str += ".0" + decPart + " %";
                        this.textScore.text = str;
            */
        }


/*        // Called when an item is returned to the pool using Release
        public void OnReturnToNote(NoteView noteView)
        {
            GameObject noteObject = noteView.gameObject;
            noteObject.SetActive(false);
            noteObject.transform.localPosition = Vector3.zero;
            if (returnNoteID < noteView.id)
                returnNoteID = noteView.id;
            notePool.ReturnObject(noteView);
            notes.Remove(noteView);
        }
*/    }
}
