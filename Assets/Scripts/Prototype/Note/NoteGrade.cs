using System;
using UnityEngine;

namespace Plutono.Song
{
    public enum NoteGrade
    {
        Perfect,
        Good,
        Bad,
        Miss,
        None
    }
    public static class NoteGradeJudgment
    {
        public static NoteGrade JudgeNoteGrade(NoteDetail noteDetail, float curTime, GameMode mode)
        {
            var time = Mathf.Abs(noteDetail.time - curTime);
            return mode switch
            {
                GameMode.Stelo => time switch
                {
                    <= 0.35f => NoteGrade.Perfect,
                    <= 0.5f => NoteGrade.Good,
                    <= 0.7f => NoteGrade.Bad,
                    _ => NoteGrade.Miss
                },
                GameMode.Arbo or GameMode.Floro => time switch
                {
                    <= 0.5f => NoteGrade.Perfect,
                    <= 0.7f => NoteGrade.Good,
                    <= 1.0f => NoteGrade.Bad,
                    _ => NoteGrade.Miss
                },
                //TODO: Finish
                GameMode.Persona => throw new NotImplementedException(),
                GameMode.Ekzerco => throw new NotImplementedException(),
                _ => throw new NotImplementedException(),
            };
        }

        public static double GetScoreWeight(this NoteGrade grade)
        {
            return grade switch
            {
                NoteGrade.Perfect => 1.0,
                NoteGrade.Good => 0.7,
                NoteGrade.Bad => 0.3,
                _ => 0,
            };
        }
    }
}
