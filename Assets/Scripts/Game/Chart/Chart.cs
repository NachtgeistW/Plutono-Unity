/*
 * class Chart -- Store the information of a single chart.
 *
 *      This class includes the speed, difficulty, level, beats and notes property.
 *
 * History
 *     2020.7.29 Copy from Deenote.
 *     2020.9.03 COPY from Deenote(Refactor) and EDIT.
 */
using JetBrains.Annotations;

namespace Assets.Scripts.Game.Chart
{
    public sealed class Chart
    {
        public float speed;
        [CanBeNull] public JsonNote[] notes;
        [CanBeNull] public System.Collections.Generic.List<JsonLink> links;
    }
}