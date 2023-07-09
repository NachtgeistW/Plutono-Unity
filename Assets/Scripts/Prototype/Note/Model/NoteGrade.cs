using System;

namespace Plutono.Song
{
    public enum NoteGrade
    {
        Miss,
        None,
        Bad,
        Good,
        Perfect
    }
    
    public static class NoteGradeJudgment
    {
        public static NoteGrade Judge(NoteDetail noteDetail, double curTime, GameMode mode)
        {
            var interval = Math.Abs(noteDetail.time - curTime);
            return GetNoteGrade(interval, mode);
        }

        public static NoteGrade Judge(double interval, GameMode mode)
        {
            return GetNoteGrade(interval, mode);
        }

        private static NoteGrade GetNoteGrade(double interval, GameMode mode)
        {
            return mode switch
            {
                GameMode.Stelo => interval switch
                {
                    <= Settings.SteloMode.perfectDeltaTime => NoteGrade.Perfect,
                    <= Settings.SteloMode.goodDeltaTime => NoteGrade.Good,
                    <= Settings.SteloMode.badDeltaTime => NoteGrade.Bad,
                    _ => NoteGrade.Miss
                },
                GameMode.Arbo or GameMode.Floro => interval switch
                {
                    <= Settings.ArboMode.perfectDeltaTime => NoteGrade.Perfect,
                    <= Settings.ArboMode.goodDeltaTime => NoteGrade.Good,
                    <= Settings.ArboMode.badDeltaTime => NoteGrade.Bad,
                    _ => NoteGrade.Miss
                },
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
