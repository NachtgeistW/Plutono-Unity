/*
 * class GameState -- Control the status of the game such as scores, combos and so on.
 *                    This Class is also in charge of judging the note.
 *
 * class NoteJudgment -- Store the info about whether a note has been judge and its grade
 *
 * History
 *      2021.10.16  CREATE.
 *      2021.10.17  ADDED Class NoteJudgment, method GameStatus, Judge.
 */

using System;
using System.Collections.Generic;
using Assets.Scripts.Model.Plutono;
using Controller.Game;
using UnityEngine;

namespace Assets.Scripts.Controller.Game
{
    public enum GameMode
    {
        Stelo,      //"Star", Plutono
        Arbo,       //"Tree", De1
        Floro,      //"Flower", De2
        Persona,    //"Personal", custom judgment
        Ekzerco     //"Exercise", practice mode
    }

    public sealed class GameStatus
    {
        public GameMode Mode { get; set; }
        public int Level { get; set; }
        public int NoteCount { get; set; }
        public bool IsStarted { get; set; }
        public bool IsPlaying { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsFailed { get; set; }

        public int pCount; //perfect
        public int gCount; //good
        public int bCount; //bad
        public int mCount; //miss

        public int ChartPlaySpeed { get; set; }
        public int Combo { get; private set; }
        public int MaxCombo { get; private set; }
        public int BasicScore { get; private set; }
        public int ComboScore { get; private set; }

        public int ClearCount { get; private set; }
        public Dictionary<uint, NoteJudgment> Judgments { get; private set; } = new Dictionary<uint, NoteJudgment>();

        public GameStatus(GamePlayController controller, GameMode mode)
        {
            Mode = mode;
            Level = int.Parse(controller.ChartInfo.level);
            NoteCount = controller.ChartInfo.notes.Count;
            IsStarted = false;
            IsPlaying = false;
            IsCompleted = false;
            IsFailed = false;
            BasicScore = 0;
            ComboScore = 0;
        }
        public void Judge(GameNote note, NoteGrade grade, double error)
        {
            // Status check
            if (IsFailed || IsCompleted) return;
            if (Judgments[note.Model.id].IsJudged)
            {
                Debug.LogWarning($"Trying to judge note {note.Model.id} which is already judged.");
                return;
            }

            ClearCount++;
            Judgments[note.Model.id].IsJudged = true;
            Judgments[note.Model.id].Grade = grade;
            Judgments[note.Model.id].Error = error;

            // Combo
            var miss = grade == NoteGrade.Bad || grade == NoteGrade.Miss;

            if (miss) Combo = 0; else Combo++;
            if (Combo > MaxCombo) MaxCombo = Combo;

            // Score
            switch (grade)
            {
                case NoteGrade.Perfect:
                    pCount++;
                    break;
                case NoteGrade.Good:
                    gCount++;
                    break;
                case NoteGrade.Bad:
                    bCount++;
                    break;
                case NoteGrade.Miss:
                    mCount++;
                    break;
                default:
                    throw new Exception($"Unknown grade on note {note.Model.id}");
            }

            CalculateBasicScore();
            CalculateComboScore(grade);
        }

        public void CalculateBasicScore()
        {
            BasicScore = (int)(0.9 * (1000000 * (pCount + 0.7 * gCount + 0.3 * bCount) / NoteCount));
        }

        public void CalculateComboScore(NoteGrade grade)
        {
            switch (grade)
            {
                case NoteGrade.Perfect:
                    ComboScore += 2048 / Mathf.Min(1024, NoteCount);
                    break;
                case NoteGrade.Good:
                    ComboScore += 1024 / Mathf.Min(1024, NoteCount);
                    break;
                case NoteGrade.Bad:
                case NoteGrade.Miss:
                    ComboScore -= 4096 / Mathf.Min(1024, NoteCount);
                    break;
                default:
                    throw new Exception("unknown grade");
            }
            if (ComboScore < 0) ComboScore = 0; //completely closed
            if (ComboScore > 1024) ComboScore = 1024; //completely opened
        }

        public bool IsJudged(uint noteId) => Judgments[noteId].IsJudged;

        public NoteJudgment GetJudgment(uint noteId) => Judgments[noteId];


    }
    public class NoteJudgment
    {
        public bool IsJudged;
        public NoteGrade Grade;
        public double Error;
    }
}