/*
 * class PianoNote -- 存储游戏中PianoNote的数据、渲染与判定逻辑
 *
 * History
 *      2021.12.22  CREATE.
 */

using Model.Plutono;

namespace Assets.Scripts.Model.Plutono
{
    using Views;

    public class PianoNote : GameNote
    {
        public PianoNoteView PianoNoteView { get; set; }

        protected override void OnGameUpdate()
        {
            PianoNoteView.UpdatePosition(Model.time, Controller.chartPlaySpeed);
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