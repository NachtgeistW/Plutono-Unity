using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AwesomeCharts {
    [System.Serializable]
    public class BarDataSet : DataSet<BarEntry> {

        [SerializeField]
        private List<Color> barColors = new List<Color>();

        public List<Color> BarColors {
            get { return barColors; }
            set { barColors = value; }
        }

        public BarDataSet() : this("") { }

        public BarDataSet(string title) : base(title) { }

        public BarDataSet(string title, List<BarEntry> entries) : base(title, entries) { }

        public long GetMaxPosition() {
            if (Entries == null || Entries.Count == 0)
                return 0;

            List<BarEntry> sortedEntries = Entries.OrderByDescending(a => a.Position).ToList();

            return sortedEntries[0].Position;
        }

        public long GetMinPosition() {
            if (Entries == null || Entries.Count == 0)
                return 0;

            List<BarEntry> sortedEntries = Entries.OrderBy(a => a.Position).ToList();

            return sortedEntries[0].Position;
        }

        public long PositionDelta() {
            long result = 0;
            List<BarEntry> sortedEntries = GetSortedEntries();
            if (Entries.Count > 1) {
                result = sortedEntries[sortedEntries.Count - 1].Position - sortedEntries[0].Position;
            }

            return result;
        }

        public Color GetColorForIndex(int index) {
            if (BarColors == null || BarColors.Count == 0)
                return Defaults.BAR_LINE_COLOR;

            return BarColors[index % BarColors.Count];
        }

        public override List<BarEntry> GetSortedEntries() {
            return Entries
                .OrderBy(entry => entry.Position)
                .ToList();
        }
    }
}