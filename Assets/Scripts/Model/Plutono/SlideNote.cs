/*
 * class PianoNote -- 存储游戏中SlideNote的数据、渲染与判定逻辑
 *
 * History
 *      2021.12.22  CREATE.
 */
using Model.Plutono;

namespace Assets.Scripts.Model.Plutono
{
    using Views;

    public class SlideNote : GameNote
    {
        public SlideNoteView SlideNoteView { get; set; }

        protected override void OnGameUpdate()
        {
            SlideNoteView.UpdatePosition(Model.time, Controller.chartPlaySpeed);
        }

        public void PlayPianoSound()
        {

        }

        public void OnFingerTap()
        {

        }

        protected override NoteRenderer CreateRenderer()
        {
            int a = 0;
            return new NoteRenderer();
        }
    }
}