using System.Collections.Generic;
using UnityEngine;
using Plutono.Song;
using System;
using Plutono.GamePlay;
using System.Linq;
using Plutono.GamePlay.Notes;
using Plutono.Level.GamePlay;

/*
 * 控制游戏的整体状态（注意，只存数据，不存具体的gameObject）
 * 存储该盘游戏的模式、是否正在游玩、已判定的note等
 */
public sealed class GameStatus
{
    public GameMode Mode { get; set; }
    public int NoteCount { get; set; }
    public bool IsStarted { get; set; }
    public bool IsPaused { get; set; }
    public bool IsCompleted { get; set; }
    public bool IsFailed { get; set; }

    public int pCount; //perfect
    public int gCount; //good
    public int bCount; //bad
    public int mCount; //miss

    public float ChartPlaySpeed { get; set; }
    public int Combo { get; set; }
    public int MaxCombo { get; private set; }
    public int BasicScore { get; private set; }
    public int ComboScore { get; private set; }

    public int ClearCount { get; private set; }
    public Dictionary<uint, NoteJudgmentStatus> Judgments { get; private set; } = new();

    public GameStatus(GamePlayController controller, GameMode mode)
    {
        Mode = mode;
        NoteCount = controller.ChartDetail.noteDetails.Count(note => note.IsShown);
        IsStarted = false;
        IsPaused = false;
        IsCompleted = false;
        IsFailed = false;
        BasicScore = 0;
        ComboScore = 0;
        mCount = NoteCount;
    }

    public void Reset()
    {
        IsStarted = false;
        IsPaused = false;
        IsCompleted = false;
        IsFailed = false;
        BasicScore = 0;
        ComboScore = 0;
        Combo = 0;
        MaxCombo = 0;
        ClearCount = 0;
        Judgments.Clear();
        mCount = NoteCount;

    }

    /// <summary>
    /// 当note判定完毕后调用，存储这个note的判定并计算分数
    /// </summary>
    /// <param name="noteDetail"></param>
    /// <param name="grade"></param>
    public CalculateResult CalculateScore(uint noteId, NoteGrade grade)
    {
        // Status check
        if (IsFailed || IsCompleted) return CalculateResult.GameEnded;
        //if (noteDetail.IsShown == false) return CalculateResult.NoteNotShown;
        if (grade == NoteGrade.None) return CalculateResult.NoteNotFound;

        Judgments.TryGetValue(noteId, out var noteJudgmentCheck);
        if (noteJudgmentCheck is { IsJudged: true })
        {
            Debug.Log("ID: " + noteId + " Judgment: " + noteJudgmentCheck.IsJudged);
            Debug.LogWarning($"Trying to judge note {noteId} which is already judged.");
            return CalculateResult.HasBeenJudged;
        }

        ClearCount++;
        Judgments.Add(noteId, new NoteJudgmentStatus
        {
            Grade = grade,
            IsJudged = true
        });

        // Score
        switch (grade)
        {
            case NoteGrade.Perfect:
                pCount++;
                mCount--;
                break;
            case NoteGrade.Good:
                gCount++;
                mCount--;
                break;
            case NoteGrade.Bad:
                bCount++;
                mCount--;
                break;
            case NoteGrade.Miss:
                break;
            default:
                throw new Exception($"Unknown grade on note {noteId}");
        }

        // Combo
        var miss = grade is NoteGrade.Bad or NoteGrade.Miss;

        if (miss) Combo = 0; else Combo++;
        if (Combo > MaxCombo) MaxCombo = Combo;

        CalculateBasicScore();
        CalculateComboScore(grade);
        return CalculateResult.Succeeded;
    }

    private void CalculateBasicScore()
    {
        BasicScore = (int)(0.9 * (1000000 * (pCount + 0.7 * gCount + 0.3 * bCount) / NoteCount));
    }

    private void CalculateComboScore(NoteGrade grade)
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

    private bool IsJudged(uint noteId) => Judgments[noteId].IsJudged;

    private NoteJudgmentStatus GetJudgment(uint noteId) => Judgments[noteId];
}
public class NoteJudgmentStatus
{
    public bool IsJudged;
    public NoteGrade Grade;
    public double Error;
    private bool v;
}

public enum CalculateResult
{
    Succeeded = 0,
    // 成功。

    HasBeenJudged = -1,
    // Note已被判定。

    NoteNotFound = -2,
    // 找不到Note。

    GameEnded = -3,
    // 游戏已经结束。

    NoteNotShown = -4,
    // 无需显示Note。
}