using Assets.Scripts.Model.Plutono;
using Assets.Scripts.Util;
using Model.Plutono;
using UnityEngine;

namespace Views
{
    //TODO: 将note实例拆出去
    public class NoteView : MonoBehaviour
    {
        public uint id;
        public GameNoteModel _note;                     //note实例
        private float _time;                         //？
        public SpriteRenderer noteSpriteRenderer;
        public SpriteRenderer frameSpriteRenderer;
        public SpriteRenderer waveSpriteRenderer;
        public SpriteRenderer lightSpriteRenderer;
        public SpriteRenderer circleSpriteRenderer;
        public Sprite pianoNoteSprite;
        public Sprite blankNoteSprite;
        public Sprite slideNoteSprite;
        public Sprite circleSprite;
        public Sprite waveSprite;
        public Sprite lightSprite;
        public Sprite[] noteEffectFrames;
        private const float PianoNoteScale = 7.0f;
        private const float BlankNoteScale = 4.5f;
        private float _slideNoteScale = 4.5f;
        private float noteEffectScale = 8.5f;
        private Color waveColor;
        public bool InViewableRage { get; set; }
        public bool IsNoteShouldBeClear { get; set; }

        private float x;
        public float touchableLeftRange;
        public float touchableRightRange;

        private void Update()
        {
            _time = GameManager.Instance.playingController.timeSlider.value;
            UpdatePosition();
        }

        public void SetNoteAppearance(GameNoteModel note)
        {
            _note = note;
            id = note.id;
            if (note.type == GameNoteModel.NoteType.Blank)
            {
                noteSpriteRenderer.sprite = blankNoteSprite;
                noteSpriteRenderer.transform.localScale = BlankNoteScale * new Vector3(note.size, 1.0f, 1.0f);
                waveColor = Color.black;
            }
            else
            {
                noteSpriteRenderer.sprite = pianoNoteSprite;
                noteSpriteRenderer.transform.localScale = BlankNoteScale * new Vector3(note.size, 1.0f, 1.0f);
                waveColor = Color.black;
            }

            if (note.pos > 2.0f || note.pos < -2.0f)
            {
                InViewableRage = false;
                circleSpriteRenderer.sprite = null;
                waveSpriteRenderer.sprite = null;
                lightSpriteRenderer.sprite = null;
            }
            else
            {
                InViewableRage = true;
                circleSpriteRenderer.sprite = circleSprite;
                waveSpriteRenderer.transform.localScale = Vector3.zero;
                waveSpriteRenderer.sprite = waveSprite;
                lightSpriteRenderer.sprite = lightSprite;
            }
            noteSpriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
            gameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

            x = Parameters.maximumNoteWidth * _note.pos;
            touchableLeftRange = Parameters.maximumNoteWidth * (_note.pos - 0.5f * _note.size);
            touchableRightRange = Parameters.maximumNoteWidth * (_note.pos + 0.5f * _note.size);
        }

        //TODO:只让少量的note移动，而不是全部一起
        public void UpdatePosition()
        {
            var z = Parameters.maximumNoteRange /
                Parameters.NoteFallTime(GameManager.Instance.playingController.chartPlaySpeed) * (_note.time - _time);
            if (InViewableRage)
            {
                IsNoteShouldBeClear = false;
                //根据note位移改变透明度达到渐显效果
                if (z < Parameters.maximumNoteRange && z >= Parameters.alpha1NoteRange)
                {
                    var alpha = (Parameters.maximumNoteRange - z) /
                                (Parameters.maximumNoteRange - Parameters.alpha1NoteRange);
                    noteSpriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, alpha);
                }
                else if (z <= Parameters.alpha1NoteRange)
                    noteSpriteRenderer.color = Color.white;
                else
                    noteSpriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
            }
            else
                noteSpriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);

            if (z <= -10.0f)
            {
                z = -10.0f;
                IsNoteShouldBeClear = true;
            }
            else
                frameSpriteRenderer.sprite = null;

            //UpdateLight();
            //UpdateEffectFrame();
            gameObject.transform.localPosition = new Vector3(x, 0.0f, z);
        }

        private void UpdateLight()
        {
            var dTime= _time - _note.time;
            if (dTime >= 0.0f && dTime <= Parameters.lightIncTime)
            {
                float rate = dTime / Parameters.lightIncTime;
                float height = rate * Parameters.lightHeight;
                lightSpriteRenderer.transform.localScale = _note.size * new Vector3(Parameters.lightSize, height, height);
                lightSpriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, rate);
            }
            else if (dTime > Parameters.lightIncTime && dTime <= Parameters.lightIncTime + Parameters.lightDecTime)
            {
                float rate = (1 - (dTime - Parameters.lightIncTime) / Parameters.lightDecTime);
                float height = rate * Parameters.lightHeight;
                lightSpriteRenderer.transform.localScale = _note.size * new Vector3(Parameters.lightSize, height, height);
                lightSpriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, rate);
            }
            else
            {
                lightSpriteRenderer.transform.localScale = Vector3.zero;
                lightSpriteRenderer.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
            }
        }

        private void UpdateEffectFrame()
        {
            var frame = Mathf.FloorToInt((_time - _note.time) / Parameters.frameSpeed);
            if (frame >= 15 || !InViewableRage) 
            { 
                frameSpriteRenderer.sprite = null;
                noteSpriteRenderer.sprite = null;
            }
            else if (frame >= 0)
            {
                noteSpriteRenderer.sprite = null;
                frameSpriteRenderer.sprite = noteEffectFrames[frame];
                frameSpriteRenderer.transform.localScale = noteEffectScale * new Vector3(_note.size, 1.0f, 1.0f);
            }
        }
    }
}