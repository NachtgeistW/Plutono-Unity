using System;

namespace Plutono.Song
{
    public enum NoteGrade
    {
        None,
        Miss,
        Bad,
        Good,
        Perfect
    }
    
    public static class NoteGradeJudgment
    {
        public static NoteGrade JudgeNoteGrade(NoteDetail noteDetail, double curTime, GameMode mode)
        {
            var time = Math.Abs(noteDetail.time - curTime);
            return mode switch
            {
                GameMode.Stelo => time switch
                {
                    <= Settings.SteloMode.perfectDeltaTime => NoteGrade.Perfect,
                    <= Settings.SteloMode.goodDeltaTime => NoteGrade.Good,
                    <= Settings.SteloMode.badDeltaTime => NoteGrade.Bad,
                    _ => NoteGrade.Miss
                },
                GameMode.Arbo or GameMode.Floro => time switch
                {
                    <= Settings.ArboMode.perfectDeltaTime => NoteGrade.Perfect,
                    <= Settings.ArboMode.goodDeltaTime => NoteGrade.Good,
                    <= Settings.ArboMode.badDeltaTime => NoteGrade.Bad,
                    _ => NoteGrade.Miss
                },
                //TODO: Finish other mode.
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
