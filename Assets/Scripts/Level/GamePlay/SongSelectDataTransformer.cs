namespace Plutono.GamePlay
{
    public static class SongSelectDataTransformer
    {
        public static int SelectedSongIndex { get; set; }
        public static int SelectedChartIndex { get; set; }

        public static void Reset()
        {
            SelectedSongIndex = 0;
            SelectedChartIndex = 0;
        }

        public static GameMode GameMode { get; set; } = GameMode.Stelo;
        public static float ChartPlaySpeed { get; set; } = 5.5f;
    }
}
