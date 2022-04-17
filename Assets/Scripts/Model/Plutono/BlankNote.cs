/*
 * class PianoNote -- 存储游戏中BlankNote的数据、渲染与判定逻辑
 *
 * History
 *      2021.12.22  CREATE.
 */
using Model.Plutono;

namespace Assets.Scripts.Model.Plutono
{
    using Views;

    public class BlankNote : GameNote
    {
        public BlankNoteView BlankNoteView { get; set; }

        protected override void OnGameUpdate()
        {
            BlankNoteView.UpdatePosition(Model.time, Controller.chartPlaySpeed);
        }

        public void OnFingerTap()
        {

        }

        protected override NoteRenderer CreateRenderer()
        {
            return new NoteRenderer();
        }
    }
}