/*
 * History
 *      2023.02.07  CREATED
 */

using Plutono.Song;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MusicBarCaculator
{
    public static readonly int maxBar = 120;
    static readonly int maxPointOnBar = 8;
    MusicBar[] musicBars =  new MusicBar[maxBar];

    public int GetBar(int point)
    {
        return musicBars[point].PointOnBar;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="musicLength">the length of music source clip</param>
    public void calculate(double musicLength, List<NoteDetail> noteDetails)
    {
        var musicLenghtPerBar = musicLength / maxBar;
        int lastNoteIndex = 0;
        int i = 0;
        try
        {
            for (i = 0; i < maxBar - 1; i++)
            {
                //FIXME
                if (lastNoteIndex < noteDetails.Count - 1)
                {
                    int num = 0;
                    while (noteDetails[lastNoteIndex].time >= musicLenghtPerBar * i &&
                        noteDetails[lastNoteIndex].time < musicLenghtPerBar * (i + 1))
                    {
                        num++;
                        lastNoteIndex++;
                    }
                    musicBars[i] = new MusicBar(i, num);
                }
                else
                {
                    musicBars[i] = new MusicBar(i, 0);
                }
            }
            //int num = 0;
            //for (lastNoteIndex = 0; lastNoteIndex < noteDetails.Count; lastNoteIndex++)
            //{
            //    if (i >= 120) return;
            //    if (noteDetails[lastNoteIndex].time >= musicLenghtPerBar * i)
            //    {
            //        musicBars[i] = new MusicBar(i, num);
            //        i++;
            //        num = 0;
            //    }
            //    num++;
            //}
        }
        catch (Exception)
        {
            Debug.Log("LastNoteIndex:" + lastNoteIndex + " i:" + i);
            throw;
        }
    }
}

public class MusicBar
{
    public int Bar { get; set; }
    public int PointOnBar { get; set; }

    public MusicBar(int bar, int pointOnBar)
    {
        Bar = bar;
        PointOnBar = pointOnBar;
    }

}

public class MusicBarCaculatorTest : MonoBehaviour
{
    private MusicBarCaculator musicBarCaculator;

    private void Start()
    {
        musicBarCaculator = new MusicBarCaculator();
        Debug.Log(musicBarCaculator.GetBar(0));
        Debug.Log(musicBarCaculator.GetBar(1));
        Debug.Log(musicBarCaculator.GetBar(8));
        Debug.Log(musicBarCaculator.GetBar(9));
        Debug.Log(musicBarCaculator.GetBar(16));
        Debug.Log(musicBarCaculator.GetBar(17));
    }
}