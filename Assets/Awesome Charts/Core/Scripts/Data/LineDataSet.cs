using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AwesomeCharts {
    [System.Serializable]
    public class LineDataSet : DataSet<LineEntry> {

        [SerializeField]
        private float lineThickness = Defaults.CHART_LINE_THICKNESS;
        [SerializeField]
        private Color lineColor = Defaults.CHART_LINE_COLOR;
        [SerializeField]
        private Color fillColor = Defaults.CHART_BACKGROUND_COLOR;
        [SerializeField]
        private Texture fillTexture;
        [SerializeField]
        private bool useBezier = false;

        public float LineThickness {
            get { return lineThickness; }
            set { lineThickness = value; }
        }

        public Color LineColor {
            get { return lineColor; }
            set { lineColor = value; }
        }

        public Color FillColor {
            get { return fillColor; }
            set { fillColor = value; }
        }

        public Texture FillTexture {
            get { return fillTexture; }
            set { fillTexture = value; }
        }

        public bool UseBezier {
            get { return useBezier; }
            set { useBezier = value; }
        }

        public LineDataSet () : this ("") { }

        public LineDataSet (string title) : base (title) { }

        public LineDataSet (string title, List<LineEntry> entries) : base (title, entries) { }

        public float GetMaxPosition () {
            if (Entries == null || Entries.Count == 0)
                return 0;
            else if (Entries.Count == 1)
                return Mathf.Max (0f, Entries[0].Position);

            List<LineEntry> sortedEntries = Entries.OrderByDescending (a => a.Position).ToList ();

            return sortedEntries[0].Position;
        }

        public float GetMinPosition () {
            if (Entries == null || Entries.Count == 0)
                return 0;
            else if (Entries.Count == 1)
                return Mathf.Min (0f, Entries[0].Position);

            List<LineEntry> sortedEntries = Entries.OrderBy (a => a.Position).ToList ();

            return sortedEntries[0].Position;
        }

        public float PositionDelta () {
            float result = 0;
            List<LineEntry> sortedEntries = GetSortedEntries ();
            if (Entries.Count > 1) {
                result = sortedEntries[sortedEntries.Count - 1].Position - sortedEntries[0].Position;
            }

            return result;
        }

        public override List<LineEntry> GetSortedEntries () {
            return Entries
                .OrderBy (entry => entry.Position)
                .ToList ();
        }
    }
}