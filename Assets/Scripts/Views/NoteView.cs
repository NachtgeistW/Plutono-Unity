using Assets.Scripts.Model.Plutono;
using Assets.Scripts.Util;
using UnityEngine;

namespace Views
{
    public class NoteView : MonoBehaviour
    {
        public uint id;
        private GameNote _note;                     //note实例
        private float time;                         //？
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
        private bool inViewableRage = false;

        private float _x;

        private void Update()
        {
            time = GameManager.Instance.playingController.timeSlider.value;
            UpdatePosition();
        }

        public void SetNoteAppearance(GameNote note)
        {
            _note = note;
            id = note.id;
            if (note.sounds == null)
            {
                noteSpriteRenderer.sprite = blankNoteSprite;
                noteSpriteRenderer.transform.localScale = BlankNoteScale * new Vector3(note.size, 1.0f, 1.0f);
                waveColor = Color.black;
            }
            else
            {
                noteSpriteRenderer.sprite = pianoNoteSprite;
                noteSpriteRenderer.transform.localScale = PianoNoteScale * new Vector3(note.size, 1.0f, 1.0f);
                waveColor = Color.black;
            }

            if (note.pos > 2.0f || note.pos < -2.0f)
            {
                inViewableRage = false;
                circleSpriteRenderer.sprite = null;
                waveSpriteRenderer.sprite = null;
                lightSpriteRenderer.sprite = null;
            }
            else
            {
                inViewableRage = true;
                circleSpriteRenderer.sprite = circleSprite;
                waveSpriteRenderer.transform.localScale = Vector3.zero;
                waveSpriteRenderer.sprite = waveSprite;
                lightSpriteRenderer.sprite = lightSprite;
            }
            //noteSpriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
            noteSpriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
            gameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            
            _x = Parameters.maximumNoteWidth * _note.pos;
        }

        public void UpdatePosition()
        {
            var z = Parameters.maximumNoteRange /
                Parameters.NoteFallTime(GameManager.Instance.playingController.chartPlaySpeed) * (_note.time - time);
            if (inViewableRage)
            {
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
                //TODO:Is this step unnecessary? Because on the beginning of generation it is transparency already
            }
            else
                noteSpriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);

            if (z <= 0.0f)
                z = 0.0f;
            else
                frameSpriteRenderer.sprite = null;

            gameObject.transform.localPosition = new Vector3(_x, 0.0f, z);
        }
    }
}