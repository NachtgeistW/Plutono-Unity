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
using Util;

public class GameManager : MonoSingleton<GameManager>
{
    //list for storing song information
    public List<PackInfo> songPackList;
    public List<string> songPathList;

    //Welcome scene
    private WelcomeController _welcomeController;

    //Song Select Scene
    private SongSelectController _songSelectController;
    public PackInfo packInfo;
    public string songPath;

    //Chart Select Scene
    private ChartSelectController _chartSelectController;
    public GameChart gameChart;

    //Game Playing Scene
    public PlayingController playingController;

}
