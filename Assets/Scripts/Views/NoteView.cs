namespace Assets.Scripts.Views
{
    using Model.Plutono;

    using UnityEngine;

    using Util;

    public class NoteView : MonoBehaviour
    {
        public uint id;
        private float time;
        [SerializeField] protected SpriteRenderer noteSpriteRenderer;
        [SerializeField] protected SpriteRenderer frameSpriteRenderer;
        [SerializeField] protected SpriteRenderer waveSpriteRenderer;
        [SerializeField] protected SpriteRenderer lightSpriteRenderer;
        [SerializeField] protected SpriteRenderer circleSpriteRenderer;
        [SerializeField] protected Sprite circleSprite;
        [SerializeField] protected Sprite waveSprite;
        [SerializeField] protected Sprite lightSprite;
        [SerializeField] protected Sprite[] noteEffectFrames;
        protected const float NoteEffectScale = 8.5f;
        protected Color waveColor;
        protected bool IsInViewableRange { get; set; }
        protected bool IsNoteShouldBeClear { get; set; }

        protected float MaxXPos;
        public float TouchableLeftRange;
        public float TouchableRightRange;

        private void OnGameUpdate(float noteTime, float curGameTime, int chartPlaySpeed)
        {
            time = curGameTime;
            UpdatePosition(noteTime, chartPlaySpeed);
        }

        public virtual void SetNoteAppearance(float noteSize, float notePos) { }

        //TODO:只让少量的note移动，而不是全部一起
        public void UpdatePosition(float noteTime, int chartPlaySpeed)
        {
            var z = Parameters.maximumNoteRange /
                Parameters.NoteFallTime(chartPlaySpeed) * (noteTime - time);
            if (IsInViewableRange)
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
            gameObject.transform.localPosition = new Vector3(MaxXPos, 0.0f, z);
        }

        private void UpdateLight(float noteSize, float noteTime)
        {
            var deltaTime = time - noteTime;
            if (deltaTime >= 0.0f && deltaTime <= Parameters.lightIncTime)
            {
                var rate = deltaTime / Parameters.lightIncTime;
                var height = rate * Parameters.lightHeight;
                lightSpriteRenderer.transform.localScale = noteSize * new Vector3(Parameters.lightSize, height, height);
                lightSpriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, rate);
            }
            else if (deltaTime > Parameters.lightIncTime && deltaTime <= Parameters.lightIncTime + Parameters.lightDecTime)
            {
                var rate = (1 - (deltaTime - Parameters.lightIncTime) / Parameters.lightDecTime);
                var height = rate * Parameters.lightHeight;
                lightSpriteRenderer.transform.localScale = noteSize * new Vector3(Parameters.lightSize, height, height);
                lightSpriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, rate);
            }
            else
            {
                lightSpriteRenderer.transform.localScale = Vector3.zero;
                lightSpriteRenderer.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
            }
        }

        private void UpdateEffectFrame(float noteSize, float noteTime)
        {
            var frame = Mathf.FloorToInt((time - noteTime) / Parameters.frameSpeed);
            if (frame >= 15 || !IsInViewableRange)
            {
                frameSpriteRenderer.sprite = null;
                noteSpriteRenderer.sprite = null;
            }
            else if (frame >= 0)
            {
                noteSpriteRenderer.sprite = null;
                frameSpriteRenderer.sprite = noteEffectFrames[frame];
                frameSpriteRenderer.transform.localScale = NoteEffectScale * new Vector3(noteSize, 1.0f, 1.0f);
            }
        }
    }
}