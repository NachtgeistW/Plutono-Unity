/*
 * class GameManager -- manager the global variable and function
 *
 * History:
 *  2021.04.07 CREATE
 */

using System.Collections.Generic;
using Assets.Scripts.Model.Plutono;
using Controller;
using Model.Plutono;
using UnityEngine;
using Util;

public class GameManager : MonoSingleton<GameManager>
{
    //list for storing song information
    [HideInInspector] public List<PackInfo> songPackList;
    [HideInInspector] public List<string> songPathList;

    //Welcome scene
    [HideInInspector] private WelcomeController _welcomeController;

    //Song Select Scene
    [HideInInspector] private SongSelectController _songSelectController;
    [HideInInspector] public PackInfo packInfo;
    [HideInInspector] public string songPath;

    //Chart Select Scene
    [HideInInspector] private ChartSelectController _chartSelectController;
    [HideInInspector] public GameChartModel gameChart;

    //Game Playing Scene
    [HideInInspector] public PlayingController playingController;
    [HideInInspector] public int pCount;  //perfect
    [HideInInspector] public int gCount;  //good
    [HideInInspector] public int bCount;  //bad
    [HideInInspector] public int mCount;  //miss
    [HideInInspector] public int score;   //score
    [HideInInspector] public int bonus;   //bonus


}
