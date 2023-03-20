namespace Plutono.GamePlay
{
    public static class ResultDataTransformer
    {
        public static GameMode Mode { get; set; }
        public static int PCount { get; set; } //perfect
        public static int GCount { get; set; } //good
        public static int BCount { get; set; } //bad
        public static int MCount { get; set; } //miss
        public static int BasicScore { get; set; }
        public static int ComboScore { get; set; }
    }
}