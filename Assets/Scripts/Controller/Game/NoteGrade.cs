/*
 * class NoteGrade -- Store the grade of note.
 *
 * History
 *      2021.10.16  CREATE.
 */

namespace Controller.Game
{
    public enum NoteGrade
    {
        Perfect,
        Good,
        Bad,
        Miss
    }
    public static class NoteGradeJudgment
    {
        public static double GetScoreWeight(this NoteGrade grade)
        {
            switch (grade)
            {
                case NoteGrade.Perfect:
                    return 1.0;
                case NoteGrade.Good:
                    return 0.7;
                case NoteGrade.Bad:
                    return 0.3;
                case NoteGrade.Miss:
                    return 0;
                default:
                    return 0f;
            }
        }
    }
}