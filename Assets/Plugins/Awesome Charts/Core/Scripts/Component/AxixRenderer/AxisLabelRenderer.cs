using System.Collections.Generic;
using UnityEngine;

namespace AwesomeCharts {
    [RequireComponent (typeof (RectTransform))]
    [ExecuteInEditMode]
    public class AxisLabelRenderer : ACMonoBehaviour {

        [SerializeField]
        private ChartLabel objectPrefab;
        [SerializeField]
        private AxisLabelConfig labelsConfig;
        [SerializeField]
        private List<AxisLabelRendererExtry> entries;

        private ReusableObjectsPool objectsPool;

        private bool labelsDirty;

        void Awake () {
            if (entries == null)
                entries = new List<AxisLabelRendererExtry> ();

            if (labelsConfig == null)
                labelsConfig = new AxisLabelConfig ();

            objectsPool = new ReusableObjectsPool ();
            objectsPool.Parent = transform;
            objectsPool.DefaultObjectPrefabPath = "prefabs/TextAxisLabelPrefab";

            labelsDirty = true;
        }

        public ChartLabel ObjectPrefab {
            set { objectPrefab = value; }
            get { return objectPrefab; }
        }

        public AxisLabelConfig LabelsConfig {
            set {
                labelsConfig = value;
                labelsDirty = true;
            }
            get { return labelsConfig; }
        }

        public List<AxisLabelRendererExtry> Entries {
            set {
                entries = value;
                labelsDirty = true;
            }
            get { return entries; }
        }

        private Vector2 GetSize () {
            return GetComponent<RectTransform> ().sizeDelta;
        }

        private int GetEntriesCount () {
            return Entries != null? Entries.Count : 0;
        }

        private ChartLabel GetLabelAt (int index) {
            return objectsPool.GetReusableObject (index).GetComponent<ChartLabel> ();
        }

        public void Reload () {
            Update ();
        }

        void OnValidate () {
            if (objectsPool == null)
                return;

            labelsDirty = true;
        }

        protected override void Update () {
            base.Update ();

            UpdateLabelsPool ();

            if (objectsPool.IsDirty () || labelsDirty) {
                UpdateLabels (GetSize ());
            }

            labelsDirty = false;
        }

        private void UpdateLabelsPool () {
            objectsPool.Parent = transform;
            objectsPool.ObjectPrefab = ObjectPrefab != null? ObjectPrefab.gameObject : null;
            objectsPool.PoolSize = GetEntriesCount ();
            objectsPool.Update ();
        }

        private void UpdateLabels (Vector2 rendererSize) {
            int index = 0;
            Entries.ForEach (labelEntry => {
                UpdateLabel (GetLabelAt (index), LabelsConfig, labelEntry, rendererSize);
                index++;
            });
        }

        private void UpdateLabel (ChartLabel label,
            AxisLabelConfig config,
            AxisLabelRendererExtry entry,
            Vector2 rendererSize) {

            label.SetLabelColor (config.LabelColor);
            label.SetLabelTextSize (config.LabelSize);
            label.SetLabelTextAlignment (GetLabelAlignment (entry));
            label.SetLabelText (entry.Text);
            label.GetComponent<RectTransform> ().pivot = GetLabelPivot (entry);
            label.transform.localPosition = CreateLabelPositionForEntry (entry, rendererSize, config.LabelMargin);
        }

        private Vector2 CreateLabelPositionForEntry (AxisLabelRendererExtry entry, Vector2 rendererSize, float margin) {
            if (entry.Orientation == AxisOrientation.HORIZONTAL) {
                switch (entry.Gravity) {
                    case AxisLabelGravity.START:
                        return new Vector2 (entry.PositionOnAxis, -margin);
                    case AxisLabelGravity.END:
                        return new Vector2 (entry.PositionOnAxis, rendererSize.y + margin);
                }
            } else {
                switch (entry.Gravity) {
                    case AxisLabelGravity.START:
                        return new Vector2 (-margin, entry.PositionOnAxis);
                    case AxisLabelGravity.END:
                        return new Vector2 (rendererSize.x + margin, entry.PositionOnAxis);
                }
            }

            return Vector2.zero;
        }

        private TextAnchor GetLabelAlignment (AxisLabelRendererExtry entry) {
            if (entry.Orientation == AxisOrientation.HORIZONTAL) {
                switch (entry.Gravity) {
                    case AxisLabelGravity.START:
                        return TextAnchor.UpperCenter;
                    case AxisLabelGravity.END:
                        return TextAnchor.LowerCenter;
                }
            } else {
                switch (entry.Gravity) {
                    case AxisLabelGravity.START:
                        return TextAnchor.MiddleRight;
                    case AxisLabelGravity.END:
                        return TextAnchor.MiddleLeft;
                }
            }

            return TextAnchor.MiddleCenter;
        }

        private Vector2 GetLabelPivot (AxisLabelRendererExtry entry) {
            if (entry.Orientation == AxisOrientation.HORIZONTAL) {
                switch (entry.Gravity) {
                    case AxisLabelGravity.START:
                        return PivotValue.TOP_MIDDLE;
                    case AxisLabelGravity.END:
                        return PivotValue.BOTTOM_MIDDLE;
                }
            } else {
                switch (entry.Gravity) {
                    case AxisLabelGravity.START:
                        return PivotValue.MIDDLE_RIGHT;
                    case AxisLabelGravity.END:
                        return PivotValue.MIDDLE_LEFT;
                }
            }

            return PivotValue.CENTER;
        }
    }
}