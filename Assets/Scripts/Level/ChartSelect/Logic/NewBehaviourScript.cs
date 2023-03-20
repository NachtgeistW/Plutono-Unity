using Plutono.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AwesomeCharts;

public class NewBehaviourScript : MonoBehaviour
{
    MusicBarCaculator MusicBarCaculator = new MusicBarCaculator();
    public BarChart barChart;
    // Start is called before the first frame update
    void Start()
    {
        var noteDetail = FileManager.Instance.songSourceList[0].ChartDetails[0].noteDetails;
        var musicSource = AudioClipFileManager.Read(FileManager.Instance.songSourceList[0].MusicPath);

        MusicBarCaculator.calculate(musicSource.length, noteDetail);
        BarDataSet set = new();
        for (int i = 0; i < MusicBarCaculator.maxBar - 1; i++)
        {
            set.AddEntry(new BarEntry(i, MusicBarCaculator.GetBar(i)));
        }
        barChart.GetChartData().DataSets.Add(set);
        
        BarChartConfig config = new BarChartConfig
        {
            BarWidth = 40,
            BarSpacing = 20,
            InnerBarSpacing = 10,
            SizingMethod = BarSizingMethod.SIZE_TO_FIT
        };
        barChart.Config = config;
        
        barChart.SetDirty();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
