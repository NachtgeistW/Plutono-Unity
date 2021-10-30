using Assets.Scripts.Model.Plutono;
using Assets.Scripts.Util;
using UnityEngine;

namespace Assets.Scripts.Views
{
    public class NoteView : MonoBehaviour
    {
        public uint id;
        private float time;
        [SerializeField] private SpriteRenderer noteSpriteRenderer;
        [SerializeField] private SpriteRenderer frameSpriteRenderer;
        [SerializeField] private SpriteRenderer waveSpriteRenderer;
        [SerializeField] private SpriteRenderer lightSpriteRenderer;
        [SerializeField] private SpriteRenderer circleSpriteRenderer;
        [SerializeField] private Sprite pianoNoteSprite;
        [SerializeField] private Sprite blankNoteSprite;
        [SerializeField] private Sprite slideNoteSprite;
        [SerializeField] private Sprite circleSprite;
        [SerializeField] private Sprite waveSprite;
        [SerializeField] private Sprite lightSprite;
        [SerializeField] private Sprite[] noteEffectFrames;
        private const float PianoNoteScale = 7.0f;
        private const float BlankNoteScale = 4.5f;
        private float slideNoteScale = 4.5f;
        private float noteEffectScale = 8.5f;
        private Color waveColor;
        public bool InViewableRage { get; set; }
        public bool IsNoteShouldBeClear { get; set; }

        private float x;
        public float touchableLeftRange;
        public float touchableRightRange;

        private void OnGameUpdate(GameNoteModel model, float curGameTime, int chartPlaySpeed)
        {
            time = curGameTime;
            UpdatePosition(model, chartPlaySpeed);
        }

        public void SetNoteAppearance(GameNoteModel model)
        {
            if (model.type == GameNoteModel.NoteType.Blank)
            {
                noteSpriteRenderer.sprite = blankNoteSprite;
                noteSpriteRenderer.transform.localScale = BlankNoteScale * new Vector3(model.size, 1.0f, 1.0f);
                waveColor = Color.black;
            }
            else
            {
                noteSpriteRenderer.sprite = pianoNoteSprite;
                noteSpriteRenderer.transform.localScale = BlankNoteScale * new Vector3(model.size, 1.0f, 1.0f);
                waveColor = Color.black;
            }

            if (model.pos > 2.0f || model.pos < -2.0f)
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

            x = Parameters.maximumNoteWidth * model.pos;
            touchableLeftRange = Parameters.maximumNoteWidth * (model.pos - 0.5f * model.size);
            touchableRightRange = Parameters.maximumNoteWidth * (model.pos + 0.5f * model.size);
        }

        //TODO:只让少量的note移动，而不是全部一起
        public void UpdatePosition(GameNoteModel note, int chartPlaySpeed)
        {
            var z = Parameters.maximumNoteRange /
                Parameters.NoteFallTime(chartPlaySpeed) * (note.time - time);
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

        private void UpdateLight(GameNoteModel model)
        {
            var deltaTime = time - model.time;
            if (deltaTime >= 0.0f && deltaTime <= Parameters.lightIncTime)
            {
                var rate = deltaTime / Parameters.lightIncTime;
                var height = rate * Parameters.lightHeight;
                lightSpriteRenderer.transform.localScale = model.size * new Vector3(Parameters.lightSize, height, height);
                lightSpriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, rate);
            }
            else if (deltaTime > Parameters.lightIncTime && deltaTime <= Parameters.lightIncTime + Parameters.lightDecTime)
            {
                var rate = (1 - (deltaTime - Parameters.lightIncTime) / Parameters.lightDecTime);
                var height = rate * Parameters.lightHeight;
                lightSpriteRenderer.transform.localScale = model.size * new Vector3(Parameters.lightSize, height, height);
                lightSpriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, rate);
            }
            else
            {
                lightSpriteRenderer.transform.localScale = Vector3.zero;
                lightSpriteRenderer.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
            }
        }

        private void UpdateEffectFrame(GameNoteModel model)
        {
            var frame = Mathf.FloorToInt((time - model.time) / Parameters.frameSpeed);
            if (frame >= 15 || !InViewableRage)
            {
                frameSpriteRenderer.sprite = null;
                noteSpriteRenderer.sprite = null;
            }
            else if (frame >= 0)
            {
                noteSpriteRenderer.sprite = null;
                frameSpriteRenderer.sprite = noteEffectFrames[frame];
                frameSpriteRenderer.transform.localScale = noteEffectScale * new Vector3(model.size, 1.0f, 1.0f);
            }
        }
    }
}