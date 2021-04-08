/*
 * class GameManager -- manager the global variable and function
 *
 * History:
 *  2021.04.07 CREATE
 */
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Controller;
using Assets.Scripts.Model.Plutono;
using Assets.Scripts.Util;
using Assets.Scripts.Views;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    //list for storing song information
    public List<PackInfo> songPackList;

    //Welcome scene
    private WelcomeController _welcomeController;

    //Song Select Scene
    private SongSelectController _songSelectController;
    public uint songIndex;

    //Chart Select Scene
    private ChartSelectController _chartSelectController;
    public uint chartIndex;

    //Game Playing Scene
    private InGameController _gameController;

}
