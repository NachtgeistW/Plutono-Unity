/*
 * class GameState -- Control the status of the game such as scores, combos and so on.
 *
 * History
 *      2021.10.16  CREATE.
 */

namespace Controller.Game
{
    public sealed class GameStatus
    {
        public GameMode Mode { get; set; }
        public uint Level { get; set; }

        public uint ChartPlaySpeed { get; set; }
        public uint ComboCount { get; private set; }

    }

    public enum GameMode
    { 
        Stelo,  //"Star", Plutono
        Arbo,   //"Tree", De1
        Floro,  //"Flower", De2
        Persona,  //"Personal", custom judgment
        Ekzerco     //"Exercise", practice mode
    }
}