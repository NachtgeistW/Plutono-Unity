/*
 * class GameNote -- 存储游戏中单个note的数据、渲染与判定逻辑
 *
 * History
 *      2021.10.17  CREATE.
 */

using System;
using Assets.Scripts.Controller;
using Assets.Scripts.Controller.Game;
using Assets.Scripts.Views;
using Controller;
using Controller.Game;
using Model.Plutono;
using UnityEngine;
using Views;

namespace Assets.Scripts.Model.Plutono
{
    public abstract class GameNote : MonoBehaviour
    {
        public PlayingController Controller { get; private set; }
        public bool IsInitialized { get; private set; }

        public GameNoteModel Model { get; set; }
        public NoteView NoteView{ get; set; }
        public NoteRenderer Renderer;
        public float JudgmentOffset { get; set; }

        public float MissThreshold { get; set; }
        public bool IsCleared { get; set; }

        protected virtual void Init(PlayingController controller)
        {
            if (IsInitialized) return;
            IsInitialized = true;
            Controller = controller;
            JudgmentOffset = 0f;
        }

        public virtual void SetData(int noteID)
        {
            
        }

        protected virtual void OnGameUpdate()
        {
            NoteView.UpdatePosition(Model, Controller.chartPlaySpeed);
        }

        protected virtual void OnTouch(Vector2 screenPos)
        {
            if (!Controller.Status.IsPlaying)
                return;
            var grade = JudgeNoteGrade();
            if (grade != NoteGrade.None) ClearNote(grade);
            if (ShouldMiss())
            {
                ClearNote(NoteGrade.Miss);
            }
        }

        public virtual bool ShouldMiss()
        {
            return Controller.Time - (Model.time + JudgmentOffset) > MissThreshold;
        }

        protected virtual void ClearNote(NoteGrade grade)
        {
            if (IsCleared) return;
            IsCleared = true;
            Controller.Status.Judge(this, grade, 0);
        }

        /// <summary>
        /// 根据模式设定判定区间，以及判定note
        /// </summary>
        /// <returns>该note应得的判定</returns>
        protected virtual NoteGrade JudgeNoteGrade()
        {
            var deltaTime = Controller.Time - Model.time + JudgmentOffset;
            var grade = NoteGrade.None;

            if (Controller.Status.Mode == GameMode.Arbo || Controller.Status.Mode == GameMode.Floro)
            {
                if (deltaTime < -0.300f) grade = NoteGrade.Miss;
                deltaTime = Math.Abs(deltaTime);
                if (deltaTime < 0.300f) grade = NoteGrade.Bad;
                if (deltaTime < 0.070f) grade = NoteGrade.Good;
                if (deltaTime < 0.050f) grade = NoteGrade.Perfect;
            }
            else
            {
                if (deltaTime < -0.070f) grade = NoteGrade.Miss;
                deltaTime = Math.Abs(deltaTime);
                if (deltaTime < 0.070f) grade = NoteGrade.Bad;
                if (deltaTime < 0.050f) grade = NoteGrade.Good;
                if (deltaTime < 0.035f) grade = NoteGrade.Perfect;
            }

            return grade;
        }

        // UI
        protected abstract NoteRenderer CreateRenderer();
    }
}
